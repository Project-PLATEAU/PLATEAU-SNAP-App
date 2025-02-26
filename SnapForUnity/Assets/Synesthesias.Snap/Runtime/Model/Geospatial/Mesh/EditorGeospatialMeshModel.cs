using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

namespace Synesthesias.Snap.Runtime
{
    /// <summary>
    /// メッシュ(簡易版)のModel(エディタ)
    /// Hullのみ対応(Holeは無視される)
    /// </summary>
    public class EditorGeospatialMeshModel : IGeospatialMeshModel
    {
        private readonly MeshModel meshModel;
        private readonly IGeospatialMathModel geospatialMathModel;
        private readonly ITriangulationModel editorTriangulationModel;
        private readonly GeospatialMainLoopState mainLoopState = new();
        

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public EditorGeospatialMeshModel(
            MeshModel meshModel,
            IGeospatialMathModel geospatialMathModel,
            ITriangulationModel editorTriangulation)
        {
            this.meshModel = meshModel;
            this.geospatialMathModel = geospatialMathModel;
            this.editorTriangulationModel = editorTriangulation;
            mainLoopState.SetStateType(GeospatialMainLoopStateType.Ready);
        }

        /// <summary>
        /// メッシュの生成
        /// </summary>
        public async UniTask<GeospatialMeshResult> CreateMeshAsync(
            Camera camera,
            ISurfaceModel surface,
            Quaternion eunRotation,
            CancellationToken cancellationToken)
        {
            if (surface?.Coordinates == null || surface.Coordinates.Count < 1)
            {
                return new GeospatialMeshResult(
                    mainLoopState: mainLoopState,
                    accuracyState: GeospatialAccuracyState.HighAccuracy,
                    GeospatialMeshResultType.EmptyCoordinate);
            }

            var hullCoordinates = surface.Coordinates[0];
            var holesCoordinates = surface.Coordinates.Skip(1);

            var originPose = CreatePose(
                coordinates: hullCoordinates[0],
                eunRotation: eunRotation);

            var hullVertices = CreateVertices(
                originPosition: originPose.position,
                coordinates: hullCoordinates,
                eunRotation: eunRotation);
            
            var holesVertices = holesCoordinates.Select(
                coordinates => CreateVertices(
                    originPosition: originPose.position,
                    coordinates: coordinates,
                    eunRotation: eunRotation
                )).ToArray();

            // if (!meshModel.TryCreateFanTriangles(
            //         cameraPosition: camera.transform.position,
            //         vertices: vertices,
            //         results: out var triangles))
            // {
            //     return new GeospatialMeshResult(
            //         mainLoopState: mainLoopState,
            //         accuracyState: GeospatialAccuracyState.HighAccuracy,
            //         resultType: GeospatialMeshResultType.InsufficientVertices);
            // }

            // var triangles = editorTriangulationModel.GetTriangles(hullVertices);

            // var anchorObject = new GameObject(
            //     name: "EditorAnchor") { transform = { position = originPose.position } };
            var anchorObject = new GameObject(
                name: "EditorAnchor") { transform = { position = Vector3.zero } };

            var mesh = editorTriangulationModel.GetMesh(
                camera: camera,
                hullVertices: hullVertices, 
                holesVertices: holesVertices);

            return new GeospatialMeshResult(
                mainLoopState: mainLoopState,
                accuracyState: GeospatialAccuracyState.HighAccuracy,
                resultType: GeospatialMeshResultType.Success,
                anchorTransform: anchorObject.transform,
                mesh: mesh);
        }

        private Vector3[] CreateVertices(
            Vector3 originPosition,
            List<List<double>> coordinates,
            Quaternion eunRotation)
        {
            // 座標の数だけPoseを作成
            var poses = coordinates
                .Skip(1)
                .Select(coordinate =>
                {
                    var pose = CreatePose(coordinate, eunRotation);
                    return pose;
                }).ToArray();

            var vertices = poses
                .Select(pose => pose.position - originPosition)
                .ToArray();

            var results = new[] { Vector3.zero }
                .Concat(vertices)
                .SkipLast(1) //Vector3.zeroが2個ある時にメッシュ生成できないため
                .ToArray();

            return results;
        }

        private Pose CreatePose(
            List<double> coordinates,
            Quaternion eunRotation)
        {
            var geospatialPose = geospatialMathModel.CreateGeospatialPose(
                latitude: coordinates[1],
                longitude: coordinates[0],
                altitude: coordinates[2],
                eunRotation: eunRotation);

            var pose = geospatialMathModel.CreatePose(geospatialPose);
            return pose;
        }
    }
}