/*
 * PLATEAU.Snap.Server
 *
 * No description provided (generated by Openapi Generator https://github.com/openapitools/openapi-generator)
 *
 * The version of the OpenAPI document: 1.0
 * Generated by: https://github.com/openapitools/openapi-generator.git
 */


using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using FileParameter = Synesthesias.PLATEAU.Snap.Generated.Client.FileParameter;
using OpenAPIDateConverter = Synesthesias.PLATEAU.Snap.Generated.Client.OpenAPIDateConverter;

namespace Synesthesias.PLATEAU.Snap.Generated.Model
{
    /// <summary>
    /// VisibleSurfacesResponse
    /// </summary>
    [DataContract(Name = "VisibleSurfacesResponse")]
    public partial class VisibleSurfacesResponse
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VisibleSurfacesResponse" /> class.
        /// </summary>
        /// <param name="surfaces">surfaces.</param>
        public VisibleSurfacesResponse(List<Surface> surfaces = default(List<Surface>))
        {
            this.Surfaces = surfaces;
        }

        /// <summary>
        /// Gets or Sets Surfaces
        /// </summary>
        [DataMember(Name = "surfaces", EmitDefaultValue = true)]
        public List<Surface> Surfaces { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("class VisibleSurfacesResponse {\n");
            sb.Append("  Surfaces: ").Append(Surfaces).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }

        /// <summary>
        /// Returns the JSON string presentation of the object
        /// </summary>
        /// <returns>JSON string presentation of the object</returns>
        public virtual string ToJson()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this, Newtonsoft.Json.Formatting.Indented);
        }

    }

}
