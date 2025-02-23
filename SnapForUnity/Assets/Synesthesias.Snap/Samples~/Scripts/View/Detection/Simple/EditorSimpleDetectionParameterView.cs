using UnityEngine;

namespace Synesthesias.Snap.Sample
{
    /// <summary>
    /// 建物検出画面のパラメータのView(簡易メッシュ版 - エディタ用)
    /// </summary>
    public class EditorSimpleDetectionParameterView : MonoBehaviour, IEditorDetectionParameterModel
    {
        [SerializeField] private double fromLatitude;
        [SerializeField] private double fromLongitude;
        [SerializeField] private double fromAltitude;
        [SerializeField] private double toLatitude;
        [SerializeField] private double toLongitude;
        [SerializeField] private double toAltitude;
        [SerializeField] private double maxDistance;
        [SerializeField] private Camera camera;

        public double FromLatitude
            => fromLatitude;

        public double FromLongitude
            => fromLongitude;

        public double FromAltitude
            => fromAltitude;

        public double ToLatitude
            => toLatitude;

        public double ToLongitude
            => toLongitude;

        public double ToAltitude
            => toAltitude;

        public double Roll
            => camera.transform.rotation.eulerAngles.z;

        public double MaxDistance
            => maxDistance;

        public double FieldOfView
            => camera.fieldOfView;

        public Quaternion EunRotation
            => camera.transform.rotation;
    }
}