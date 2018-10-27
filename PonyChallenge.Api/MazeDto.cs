using System.Collections.Generic;
using System.Linq;
using MoreLinq;
using Newtonsoft.Json;

namespace PonyChallenge.Api
{
    public class MazeDto
    {
        [JsonProperty("pony")]
        private int[] PonyPositionInternal { get; set; }

        [JsonProperty("domokun")]
        private int[] DomokunPositionInternal { get; set; }

        [JsonProperty("end-point")]
        private int[] EndPointPositionInternal { get; set; }

        [JsonProperty("data")]
        private List<List<string>> WallsInternal { get; set; }

        [JsonProperty("size")]
        private List<int> SizesInternal { get; set; }

        // Encapsulate nasty API
        private int Width => SizesInternal[0];

        // Encapsulate nasty API
        private int Height => SizesInternal[1];

        // Encapsulate nasty API
        public int DomokunPosition => DomokunPositionInternal.Single();
        
        // Encapsulate nasty API
        public int PonyPosition => PonyPositionInternal.Single();

        // Encapsulate nasty API
        public int EndPointPosition => EndPointPositionInternal.Single();

        public IReadOnlyList<IReadOnlyList<int>> ConvertToGraph()
        {
            const string north = "north";
            const string west = "west";

            var mazeGraph = Enumerable.Range(0, Height * Width).Select(i => new List<int>()).ToList();

            var wallsIn2D = WallsInternal.Batch(Width).Select(enumerable => enumerable.ToArray()).ToArray();

            for (int row = 0; row < Height; row++)
            {
                for (int col = 0; col < Width; col++)
                {
                    var currentNode = row * Width + col;

                    if (!wallsIn2D[row][col].Contains(north) && row > 0)
                    {
                        var previousRow = row - 1;
                        var neighborNode = previousRow * Width + col;
                        mazeGraph[currentNode].Add(neighborNode);
                        mazeGraph[neighborNode].Add(currentNode);
                    }

                    if (!wallsIn2D[row][col].Contains(west) && col > 0)
                    {
                        var previousCol = col - 1;
                        var neighborNode = row * Width + previousCol;
                        mazeGraph[currentNode].Add(neighborNode);
                        mazeGraph[neighborNode].Add(currentNode);
                    }
                }
            }

            return mazeGraph;
        }
    }
}