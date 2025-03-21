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
    /// BuildingImageResponse
    /// </summary>
    [DataContract(Name = "BuildingImageResponse")]
    public partial class BuildingImageResponse
    {

        /// <summary>
        /// Gets or Sets Status
        /// </summary>
        [DataMember(Name = "status", EmitDefaultValue = false)]
        public StatusType? Status { get; set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="BuildingImageResponse" /> class.
        /// </summary>
        /// <param name="status">status.</param>
        /// <param name="id">id.</param>
        /// <param name="message">message.</param>
        public BuildingImageResponse(StatusType? status = default(StatusType?), long? id = default(long?), string message = default(string))
        {
            this.Status = status;
            this.Id = id;
            this.Message = message;
        }

        /// <summary>
        /// Gets or Sets Id
        /// </summary>
        [DataMember(Name = "id", EmitDefaultValue = true)]
        public long? Id { get; set; }

        /// <summary>
        /// Gets or Sets Message
        /// </summary>
        [DataMember(Name = "message", EmitDefaultValue = true)]
        public string Message { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("class BuildingImageResponse {\n");
            sb.Append("  Status: ").Append(Status).Append("\n");
            sb.Append("  Id: ").Append(Id).Append("\n");
            sb.Append("  Message: ").Append(Message).Append("\n");
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
