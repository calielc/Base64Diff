using System.Collections.Generic;
using Caliel.Base64Diff.Api.Bussiness;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Caliel.Base64Diff.Api.v1.Models {
    public struct DiffViewModel {
        [JsonConverter(typeof(StringEnumConverter))]
        public ArraySimilarities Similarity { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IReadOnlyCollection<ArrayDifference> Diffs { get; set; }
    }
}