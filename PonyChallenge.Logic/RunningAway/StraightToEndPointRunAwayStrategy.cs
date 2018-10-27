using CSharpFunctionalExtensions;

namespace PonyChallenge.Logic.RunningAway
{
    public class StraightToEndPointRunAwayStrategy : IRunAwayStrategy
    {
        public Maybe<GraphMove> Run(Maze maze)
        {
            return maze.ShortestPathToEndpoint.Value.Select(path => new GraphMove(fromNode: path[0], toNode: path[1]));
        }
    }
}