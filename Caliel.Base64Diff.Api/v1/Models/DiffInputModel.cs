using System;
using System.Text.RegularExpressions;

namespace Caliel.Base64Diff.Api.v1.Models {
    public struct DiffInputModel {
        private static readonly Regex RegEx = new Regex(@"^([A-Za-z0-9+/]{4})*([A-Za-z0-9+/]{4}|[A-Za-z0-9+/]{3}=|[A-Za-z0-9+/]{2}==)$", RegexOptions.Compiled);

        public string Data { get; set; }

        public bool IsValid() {
            var isEmpty = string.IsNullOrWhiteSpace(Data);
            if (isEmpty) {
                return false;
            }

            if (RegEx.IsMatch(Data) == false) {
                return false;
            }

            return true;
        }

        public byte[] GetBytes() => string.IsNullOrWhiteSpace(Data)
            ? throw new FormatException()
            : Convert.FromBase64String(Data);
    }
}