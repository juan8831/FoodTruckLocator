using Newtonsoft.Json;

namespace RedfinProject.Models
{
    /// <summary>
    /// Represents JSON object retrived from API
    /// </summary>
    class FoodTruck
    {
        [JsonProperty(PropertyName = "location")]
        public string Location { get; set; }

        [JsonProperty(PropertyName = "applicant")]
        public string Applicant { get; set; }
    }
}
