using Synesthesias.Snap.Runtime;
using System.Collections.Generic;
using System.Linq;

namespace Synesthesias.PLATEAU.Snap.Generated.Model
{
    public partial class Surface : ISurfaceModel
    {
        public string GmlId
        {
            get => Gmlid;
            set => Gmlid = value;
        }

        public List<List<List<double>>> GetUniqueCoordinates()
            => Coordinates
                .Select(coordinates => coordinates
                    .Skip(1)
                    .Reverse()
                    .ToList())
                .ToList();
    }
}