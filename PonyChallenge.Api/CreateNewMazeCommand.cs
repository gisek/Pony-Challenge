using Newtonsoft.Json;

namespace PonyChallenge.Api
{
    public class CreateNewMazeCommand
    {
        public CreateNewMazeCommand(uint width, uint height, uint difficulty, Pony pony)
        {
            Height = height;
            Width = width;
            Difficulty = difficulty;
            Pony = pony;
        }

        [JsonProperty("maze-width")]
        public uint Width { get; }
        [JsonProperty("maze-height")]
        public uint Height { get; }
        [JsonProperty("difficulty")]
        public uint Difficulty { get; }
        [JsonProperty("maze-player-name")]
        public string Pony { get; }
    }
}