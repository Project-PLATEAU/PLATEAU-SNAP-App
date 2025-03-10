using Cysharp.Threading.Tasks;
using iShape.Geometry;
using Unity.Collections;
using iShape.Mesh2d;
using iShape.Triangulation.Shape.Delaunay;
using Unity.Mathematics;
using UnityEngine;
using System.Linq;
using System.Threading;

namespace Synesthesias.Snap.Runtime
{
    /// <summary>
    /// iShapeを使用してメッシュを生成するModel
    /// </summary>
    public class ShapeMeshFactoryModel : IMeshFactoryModel
    {
        private readonly ShapeValidatorModel validatorModel;
        private readonly VectorCalculatorModel calculatorModel;
        private readonly PlainShapeFactoryModel plainShapeFactory;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ShapeMeshFactoryModel(
            ShapeValidatorModel validatorModel,
            VectorCalculatorModel calculatorModel,
            PlainShapeFactoryModel plainShapeFactory)
        {
            this.validatorModel = validatorModel;
            this.calculatorModel = calculatorModel;
            this.plainShapeFactory = plainShapeFactory;
        }

        /// <summary>
        /// メッシュを生成する
        /// </summary>
        public async UniTask<Mesh> CreateAsync(
            Vector3[] hull,
            Vector3[][] holes,
            CancellationToken cancellationToken)
        {
            if (hull.Length < 1)
            {
                Debug.LogWarning("頂点が1つもありません");
                return null;
            }

            await UniTask.DelayFrame(2, cancellationToken: cancellationToken);

            // 頂点の重複がある時，クラッシュするのを防止する．
            if (await validatorModel.IsOverlappingVerticesAsync(
                    hull: hull,
                    holes: holes,
                    cancellationToken))
            {
                Debug.LogWarning("頂点の重複が検出された為、処理を中断しました");
                return null;
            }

            // メッシュを生成する用の頂点座標を設定
            var rotationAxisY = calculatorModel.GetRotationAxisY(hull);

            if (float.IsNaN(rotationAxisY))
            {
                Debug.LogWarning("生成できないメッシュです");
                return null;
            }

            var rotatedHullVertices = calculatorModel.GetRotatedVertices(hull, rotationAxisY);
            var iGeom = IntGeom.DefGeom;
            await UniTask.DelayFrame(1, cancellationToken: cancellationToken);

            var vector2Hull = calculatorModel.GetHullVertices2d(rotatedHullVertices);
            var vector2Holes = calculatorModel.GetHolesVertices2d(holes);
            var hoge = validatorModel.IsCounterClockwise(vector2Hull);

            await UniTask.DelayFrame(2, cancellationToken: cancellationToken);

            // vector2Hullの頂点座標が反時計回りに格納されている時，クラッシュするのを防止する．
            // 3D->2Dへの変換が間違っていた場合，以下の警告が出る．
            if (validatorModel.IsCounterClockwise(vector2Hull))
            {
                Debug.LogWarning("頂点が反時計まわりに格納されています");
                return null;
            }

            var pShape = plainShapeFactory.CreatePlainShape(
                hull: vector2Hull,
                holes: vector2Holes,
                iGeom: iGeom,
                Allocator.Persistent);

            await UniTask.DelayFrame(1, cancellationToken: cancellationToken);
            var extraPoints = new NativeArray<IntVector>(0, Allocator.Persistent);
            await UniTask.DelayFrame(1, cancellationToken: cancellationToken);

            var minX = hull.Min(v => v.x);
            var maxX = hull.Max(v => v.x);
            var minY = hull.Min(v => v.y);
            var maxY = hull.Max(v => v.y);

            await UniTask.DelayFrame(1, cancellationToken: cancellationToken);

            var width = maxX - minX;
            var height = maxY - minY;
            var maxDimension = math.max(width, height);
            var maxEdgeValue = maxDimension * 0.5F; // 2分の1の長さ
            await UniTask.DelayFrame(1, cancellationToken: cancellationToken);

            maxEdgeValue = math.clamp(maxEdgeValue, 0.5F, 10F);
            var maxEdge = iGeom.Int(maxEdgeValue);
            await UniTask.DelayFrame(1, cancellationToken: cancellationToken);

            var delaunay = pShape.Delaunay(
                maxEdge: maxEdge,
                extraPoints: extraPoints,
                allocator: Allocator.Persistent);

            pShape.Dispose();

            await UniTask.DelayFrame(1, cancellationToken: cancellationToken);

            var totalArea = width * height;
            var maxAreaValue = totalArea * 0.5F; // 2分の1の面積
            maxAreaValue = math.clamp(maxAreaValue, 0.5F, 50F);
            var maxArea = iGeom.Int(maxAreaValue);

            delaunay.Tessellate(iGeom, maxArea);

            await UniTask.DelayFrame(1, cancellationToken: cancellationToken);
            extraPoints.Dispose();
            await UniTask.DelayFrame(1, cancellationToken: cancellationToken);

            var triangles = delaunay.Indices(Allocator.Persistent);
            await UniTask.DelayFrame(1, cancellationToken: cancellationToken);

            var vertices = delaunay.Vertices(Allocator.Persistent, iGeom, 0);
            await UniTask.DelayFrame(1, cancellationToken: cancellationToken);

            delaunay.Dispose();
            await UniTask.DelayFrame(1, cancellationToken: cancellationToken);

            // set each triangle as a separate mesh

            var subVertices = new NativeArray<float3>(3, Allocator.Persistent);
            await UniTask.DelayFrame(1, cancellationToken: cancellationToken);

            var subIndices = new NativeArray<int>(new[] { 0, 1, 2 }, Allocator.Persistent);
            await UniTask.DelayFrame(1, cancellationToken: cancellationToken);

            var colorMesh = new NativeColorMesh(triangles.Length, Allocator.Persistent);
            await UniTask.DelayFrame(1, cancellationToken: cancellationToken);

            for (int i = 0; i < triangles.Length; i += 3)
            {
                for (int j = 0; j < 3; j += 1)
                {
                    // 処理負荷を軽減するために1フレーム待機
                    await UniTask.DelayFrame(1, cancellationToken: cancellationToken);

                    var v = vertices[triangles[i + j]];
                    subVertices[j] = new float3(v.x, v.y, v.z);
                }

                var subMesh = new StaticPrimitiveMesh(subVertices, subIndices, Allocator.Persistent);
                await UniTask.DelayFrame(1, cancellationToken: cancellationToken);

                colorMesh.AddAndDispose(subMesh, Color.black);
            }

            await UniTask.DelayFrame(1, cancellationToken: cancellationToken);
            subIndices.Dispose();

            await UniTask.DelayFrame(1, cancellationToken: cancellationToken);
            subVertices.Dispose();

            await UniTask.DelayFrame(1, cancellationToken: cancellationToken);
            vertices.Dispose();

            await UniTask.DelayFrame(1, cancellationToken: cancellationToken);
            triangles.Dispose();

            await UniTask.DelayFrame(1, cancellationToken: cancellationToken);

            var mesh = new Mesh();
            mesh.MarkDynamic();
            colorMesh.FillAndDispose(mesh);

            await UniTask.DelayFrame(1, cancellationToken: cancellationToken);

            // verticesに渡す頂点を作成
            var invertRotationMatrix = calculatorModel.GetInvertRotationMatrix(rotationAxisY);

            var restoredVertices = calculatorModel.GetRestoredVertices(
                mesh.vertices,
                invertRotationMatrix);

            mesh.vertices = restoredVertices;
            mesh.RecalculateBounds();
            mesh.RecalculateNormals();

            return mesh;
        }
    }
}