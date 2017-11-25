using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
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
        public ActionResult Left(string id, [FromBody] InputModel inputModel) {
            if (inputModel.IsValid() == false) {
                return BadRequest(id);
            }

            LeftData[id] = inputModel.GetBytes();

            return Ok(id);
        }

        [HttpPost]
        [Route("right")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public ActionResult Right(string id, [FromBody]InputModel inputModel) {
            if (inputModel.IsValid() == false) {
                return BadRequest(id);
            }

            RightData[id] = inputModel.GetBytes();

            return Ok(id);
        }

        [HttpGet]
        [Route("")]
        [ProducesResponseType(typeof(ViewModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.InternalServerError)]
        public ActionResult Get(string id) {
            if (LeftData.ContainsKey(id) == false || RightData.ContainsKey(id) == false) {
                return NotFound(id);
            }

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

        public struct InputModel {
            public string Data { get; set; }

            public bool IsValid() {
                var isEmpty = string.IsNullOrWhiteSpace(Data);
                if (isEmpty) {
                    return false;
                }

                var regEx = new Regex(@"^([A-Za-z0-9+/]{4})*([A-Za-z0-9+/]{4}|[A-Za-z0-9+/]{3}=|[A-Za-z0-9+/]{2}==)$", RegexOptions.Compiled);
                if (regEx.IsMatch(Data) == false) {
                    return false;
                }

                return true;
            }

            public byte[] GetBytes() => Convert.FromBase64String(Data);
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