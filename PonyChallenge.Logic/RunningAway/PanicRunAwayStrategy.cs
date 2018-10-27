using System.Linq;
using CSharpFunctionalExtensions;
using MoreLinq;
using PonyChallenge.Common;

namespace PonyChallenge.Logic.RunningAway
{
    public class PanicRunAwayStrategy : IRunAwayStrategy
    {
        public Maybe<GraphMove> Run(Maze maze)
        {
            var domokunDirection = maze.ShortestPathToDomokun.Value[1];
            var destinationNode = maze.Graph[maze.PonyPosition]
                .Except(domokunDirection.ToEnumerable()) // try to avoid running towards Domokun
                .Shuffle()
                .Concat(domokunDirection.ToEnumerable()) // fallback if no other move is possible
                .First();

            return Maybe<GraphMove>.From(new GraphMove(maze.PonyPosition, destinationNode));
        }
    }
}