using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite.Internal.UrlMatches;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Caliel.Base64Diff.Api.v1.Controllers {
    [Produces("application/json")]
    [Route("v1/diff/{id}")]
    public class DiffController : Controller {

        private static readonly Dictionary<string, byte[]> LeftData = new Dictionary<string, byte[]>();
        private static readonly Dictionary<string, byte[]> RightData = new Dictionary<string, byte[]>();

        [HttpPost]
        [Route("left")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.InternalServerError)]
        public ActionResult Left(string id, [FromBody] InputModel inputModel) {
            LeftData[id] = inputModel.Bytes;

            return Ok(id);
        }

        [HttpPost]
        [Route("right")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.InternalServerError)]
        public ActionResult Right(string id, [FromBody]InputModel inputModel) {
            RightData[id] = inputModel.Bytes;

            return Ok(id);
        }

        [HttpGet]
        [Route("")]
        [ProducesResponseType(typeof(ViewModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.InternalServerError)]
        public ActionResult Get(string id) {
            var left = LeftData[id];
            var right = RightData[id];


            if (left.Length != right.Length) {
                return Ok(new ViewModel {
                    Situation = Situations.NotEqualSize
                });
            }

            if (left.SequenceEqual(right)) {
                return Ok(new ViewModel {
                    Situation = Situations.AreEquals
                });
            }

            return Ok(new ViewModel {
                Situation = Situations.SameSize,
                Diffs = MonteDiffs(left, right).ToArray(),
            });
        }

        private static IEnumerable<OffsetLength> MonteDiffs(byte[] left, byte[] right) {
            var offset = 0;
            while (offset < left.Length) {
                if (left[offset] == right[offset]) {
                    offset += 1;
                    continue;
                }

                var length = 1;
                var idx = offset + 1;
                while (idx < left.Length) {
                    if (left[idx] == right[idx]) {
                        yield return new OffsetLength {
                            Offset = offset,
                            Length = length
                        };

                        offset = idx;
                        break;
                    }

                    length += 1;
                    idx += 1;
                }
            }
        }


        public enum Situations { AreEquals, NotEqualSize, SameSize }

        public class InputModel {
            public string Data { get; set; }

            public byte[] Bytes => Data == null ? null : Convert.FromBase64String(Data);
        }

        public struct ViewModel {
            [JsonConverter(typeof(StringEnumConverter))]
            public Situations Situation { get; set; }

            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public IReadOnlyCollection<OffsetLength> Diffs { get; set; }
        }

        public struct OffsetLength {
            public int Offset { get; set; }
            public int Length { get; set; }
        }
    }
}