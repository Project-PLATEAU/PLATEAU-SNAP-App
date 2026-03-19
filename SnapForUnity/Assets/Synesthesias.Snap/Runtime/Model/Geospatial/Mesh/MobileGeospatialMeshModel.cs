using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEngine.XR.ARSubsystems;

namespace Synesthesias.Snap.Runtime
{
    /// <summary>
    /// メッシュ(簡易版)のModel(携帯端末)
    /// Hullのみ対応(Holeは無視される)
    /// </summary>
    public class MobileGeospatialMeshModel : IDisposable, IGeospatialMeshModel
    {
        private readonly List<GeospatialAnchorResult> anchorResults = new();
        private readonly IMeshFactoryModel meshFactoryModel;
        private readonly GeospatialAccuracyModel accuracyModel;
        private readonly GeospatialAnchorModel geospatialAnchorModel;
        private readonly IGeospatialMathModel geospatialMathModel;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MobileGeospatialMeshModel(
            IMeshFactoryModel meshFactoryModel,
            GeospatialAccuracyModel accuracyModel,
            GeospatialAnchorModel geospatialAnchorModel,
            IGeospatialMathModel geospatialMathModel)
        {
            this.meshFactoryModel = meshFactoryModel;
            this.accuracyModel = accuracyModel;
            this.geospatialAnchorModel = geospatialAnchorModel;
            this.geospatialMathModel = geospatialMathModel;
        }

        /// <summary>
        /// 破棄
        /// </summary>
        public void Dispose()
        {
            ClearAllAnchors();
        }
       /// <summary>
        /// 全てのアンカーを明示的にクリア
        /// </summary>
        public void ClearAllAnchors()
        {
            try
            {
                ClearAnchors(anchorResults);
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"[MobileGeospatialMeshModel] アンカーのクリア中にエラーが発生しました: {ex.Message}");
            }
        }
        
        /// <summary>
        /// メッシュの生成
        /// </summary>
        public async UniTask<GeospatialMeshResult> CreateMeshAsync(
            ISurfaceModel surface,
            Quaternion eunRotation,
            CancellationToken cancellationToken)
        {
            var accuracyResult = accuracyModel.GetAccuracy();

            if (!accuracyResult.IsSuccess)
            {
                return new GeospatialMeshResult(
                    mainLoopState: accuracyResult.MainLoopState,
                    accuracyState: accuracyResult.AccuracyState);
            }

            // 面の頂点座標のリストを取得   
            var coordinates = surface?.GetUniqueCoordinates();

            if (coordinates == null || coordinates.Count < 1)
            {
                return new GeospatialMeshResult(
                    mainLoopState: accuracyResult.MainLoopState,
                    accuracyState: accuracyResult.AccuracyState,
                    resultType: GeospatialMeshResultType.EmptyCoordinate);
            }

            var hullCoordinates = coordinates[0]; // Hullのみ対応(Holeは無視)

            // 単一アンカー方式：最初の頂点にのみアンカーを作成
            if (!TryCreateOriginAnchor(
                    firstCoordinate: hullCoordinates[0],
                    eunRotation: eunRotation,
                    result: out var originAnchorResult))
            {
                return new GeospatialMeshResult(
                    mainLoopState: accuracyResult.MainLoopState,
                    accuracyState: accuracyResult.AccuracyState,
                    resultType: GeospatialMeshResultType.AnchorCreationFailed);
            }

            anchorResults.Add(originAnchorResult);

            // originアンカーのTracking待機
            var originAnchor = originAnchorResult.Anchor;

            await UniTask.WaitUntil(
                () =>
                {
                    if (originAnchor.trackingState != TrackingState.Tracking)
                    {
                        return false;
                    }

                    if (!IsValidPosition(originAnchor.transform.position))
                    {
                        return false;
                    }

                    return true;
                },
                cancellationToken: cancellationToken);

            // 単一アンカー方式：全頂点を同一フレーム内で経緯度から変換
            var vertices = ConvertCoordinatesToVertices(
                hullCoordinates: hullCoordinates,
                originAnchorTransform: originAnchor.transform,
                eunRotation: eunRotation);
          
            var mesh = await meshFactoryModel.CreateAsync(
                hull: vertices,
                holes: null,
                cancellationToken: cancellationToken);

            var result = new GeospatialMeshResult(
                mainLoopState: accuracyResult.MainLoopState,
                accuracyState: accuracyResult.AccuracyState,
                resultType: GeospatialMeshResultType.Success,
                anchorTransform: originAnchor.transform,
                mesh: mesh,
                hullVertices: vertices,
                holesVertices: null);

            return result;
        }
        
        private bool IsValidPosition(Vector3 position)
        {
            return position != Vector3.zero &&
                   !float.IsNaN(position.x) && !float.IsNaN(position.y) && !float.IsNaN(position.z) &&
                   !float.IsInfinity(position.x) && !float.IsInfinity(position.y) && !float.IsInfinity(position.z);
        }

        /// <summary>
        /// 原点アンカーを作成する（単一アンカー方式）
        /// </summary>
        /// <param name="firstCoordinate">最初の頂点の経緯度座標</param>
        /// <param name="eunRotation">EUN回転</param>
        /// <param name="result">作成されたアンカーの結果</param>
        /// <returns>成功した場合true、失敗した場合false</returns>
        private bool TryCreateOriginAnchor(
            List<double> firstCoordinate,
            Quaternion eunRotation,
            out GeospatialAnchorResult result)
        {
            var geospatialVector = SurfaceConverter.ToGeospatialVector(firstCoordinate);

            result = geospatialAnchorModel.CreateAnchor(
                latitude: geospatialVector.Latitude,
                longitude: geospatialVector.Longitude,
                altitude: geospatialVector.Altitude,
                eunRotation: eunRotation);

            return result.IsSuccess;
        }

        /// <summary>
        /// 経緯度座標配列を頂点座標配列に変換する（単一アンカー方式）
        /// 全頂点を同一フレーム内で変換することで、AR座標系のずれを防ぐ
        /// </summary>
        /// <param name="hullCoordinates">Hull頂点の経緯度座標配列</param>
        /// <param name="originAnchorTransform">原点アンカーのTransform</param>
        /// <param name="eunRotation">EUN回転</param>
        /// <returns>原点アンカーのローカル座標系での頂点配列</returns>
        private Vector3[] ConvertCoordinatesToVertices(
            List<List<double>> hullCoordinates,
            Transform originAnchorTransform,
            Quaternion eunRotation)
        {
            var vertices = new Vector3[hullCoordinates.Count];

            for (var i = 0; i < hullCoordinates.Count; i++)
            {
                var coordinate = hullCoordinates[i];
                var geospatialVector = SurfaceConverter.ToGeospatialVector(coordinate);

                // 経緯度をGeospatialPoseに変換
                var geospatialPose = geospatialMathModel.CreateGeospatialPose(
                    latitude: geospatialVector.Latitude,
                    longitude: geospatialVector.Longitude,
                    altitude: geospatialVector.Altitude,
                    eunRotation: eunRotation);

                // ワールド座標に変換
                var worldPosition = geospatialMathModel.GetVector3(geospatialPose);

                // originAnchorのローカル座標系に変換
                vertices[i] = originAnchorTransform.InverseTransformPoint(worldPosition);
            }

            return vertices;
        }
        /// <summary>
        /// アンカーをクリアする
        /// </summary>
        /// <param name="anchorResults">アンカーのリスト</param>
        private static void ClearAnchors(List<GeospatialAnchorResult> anchorResults)
        {
            foreach (var anchorResult in anchorResults
                         .Where(anchorResult => anchorResult?.Anchor != null && anchorResult.Anchor.gameObject != null))
            {
                UnityEngine.Object.Destroy(anchorResult.Anchor.gameObject);
            }

            anchorResults.Clear();
        }
    }
}