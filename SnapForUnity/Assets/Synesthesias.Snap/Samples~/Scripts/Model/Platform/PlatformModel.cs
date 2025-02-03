namespace Synesthesias.Snap.Sample
{
    public class PlatformModel
    {
        public bool IsSupportedMobileDevice()
        {
#if UNITY_EDITOR
            return false;
#elif UNITY_ANDROID || UNITY_IOS
            return true;
#else
            return false;
#endif
        }
    }
}