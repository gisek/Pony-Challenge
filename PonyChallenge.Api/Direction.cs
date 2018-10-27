using Newtonsoft.Json;

namespace PonyChallenge.Api
{
    public class Direction
    {
        [JsonProperty("direction")]
        public string DirectionName { get; }

        private Direction(string directionName)
        {
            DirectionName = directionName;
        }

        public static Direction North { get; } = new Direction("north");
        public static Direction South { get; } = new Direction("south");
        public static Direction East { get; } = new Direction("east");
        public static Direction West { get; } = new Direction("west");
    }
}