using Redis.OM.Modeling;

namespace ScalablePathTest.Models
{
    public class UserRedis
    {

        [RedisIdField]
        public Ulid Id { get; set; }
        [Indexed]
        public string? Name { get; set; }
        [Indexed]
        public string? DOB { get; set; }
        [Indexed]
        public string? LeadSource { get; set; }
    }
}
