using System.Collections.Generic;

namespace Synesthesias.Snap.Runtime
{
    public class MeshManager
    {
        private static Dictionary<string, MeshModel> meshModels = new ();

        public static Dictionary<string, MeshModel> MeshModels { get { return meshModels; } set { meshModels = value; }}
    }
}