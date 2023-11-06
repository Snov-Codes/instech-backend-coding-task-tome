using Newtonsoft.Json;

namespace Domain.Entities
{
    public class Cover : BaseEntity
    {
        [JsonProperty(PropertyName = "startDate")]
        public DateOnly StartDate { get; set; }

        [JsonProperty(PropertyName = "endDate")]
        public DateOnly EndDate { get; set; }

        [JsonProperty(PropertyName = "claimType")]
        public CoverType Type { get; set; }

        [JsonProperty(PropertyName = "premium")]
        public decimal Premium { get; set; }
    }

    public enum CoverType
    {
        Yacht = 0,
        PassengerShip = 1,
        ContainerShip = 2,
        BulkCarrier = 3,
        Tanker = 4
    }
}
