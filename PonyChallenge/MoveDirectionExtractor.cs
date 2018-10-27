using System.Linq;
using CSharpFunctionalExtensions;
using MoreLinq;
using PonyChallenge.Api;
using PonyChallenge.Logic;

namespace PonyChallenge
{
    public class MoveDirectionExtractor
    {
        public Direction GetDirectionToMove(
            Maybe<GraphMove> maybeFromTo,
            Maze maze)
        {
            var (from, to) = maybeFromTo.HasValue
                ? (maybeFromTo.Value.FromNode, maybeFromTo.Value.ToNode)
                : (maze.PonyPosition, maze.Graph[maze.PonyPosition].Shuffle().First()); // we have to choose some move, as API doesn't allow us to pass on a move

            var difference = from - to;

            if (difference == 1)
            {
                return Direction.West;
            }

            if (difference == -1)
            {
                return Direction.East;
            }

            if (difference > 1)
            {
                return Direction.North;
            }
            else
            {
                return Direction.South;
            }
        }
    }
}