using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GithubActions.Shared.Extensions
{
    public static class HttpContextExtensions
    {
        public static async Task<string> TryGetPrettyPrintJson(this HttpContent httpContent)
        {
            var rawJson = await httpContent.ReadAsStringAsync();

            return rawJson.TryGetPrettyPrintJson();
        }

        public static string TryGetPrettyPrintJson(this string rawJson)
        {
            try
            {
                return JToken.Parse(rawJson).ToString(Formatting.Indented);
            }
            catch
            {
                return rawJson;
            }
        }

        public static async Task<string> TryGetPrettyPrintXml(this HttpContent httpContent)
        {
            var rawXml = await httpContent.ReadAsStringAsync();

            return rawXml.TryGetPrettyPrintXml();
        }

        private static string TryGetPrettyPrintXml(this string rawXml)
        {
            try
            {
                return XDocument.Parse(rawXml).ToString();
            }
            catch
            {
                return rawXml;
            }
        }
    }
}