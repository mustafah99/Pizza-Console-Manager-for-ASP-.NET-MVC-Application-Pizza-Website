using Newtonsoft.Json;

namespace PizzaManager.Domain
{
    public sealed class Token
    {
        [JsonProperty(PropertyName = "access_token")]
        public string Value { get; set; }
    }
}