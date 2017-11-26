using System.Collections.Generic;
using Caliel.Base64Diff.Domain.Similarity;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Caliel.Base64Diff.Api.v1.Models {
    public struct DiffViewModel {
        /// <summary>
        /// Similarity of two sides
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public ArraySimilarities Similarity { get; set; }

        /// <summary>
        /// Differences when both sides has same size
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IReadOnlyCollection<ArrayDifference> Diffs { get; set; }
    }
}