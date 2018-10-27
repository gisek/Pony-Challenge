using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CSharpFunctionalExtensions;
using PonyChallenge.Logic.RunningAway;

namespace PonyChallenge.Logic
{
    public class Maze
    {
        private readonly IRunAwayStrategy _runAwayStrategy;
        public int PonyPosition { get; }
        public int DomokunPosition { get; }
        public int EndPointPosition { get; }
        public ReadOnlyCollection<ReadOnlyCollection<int>> Graph { get; }

        public readonly Lazy<Maybe<IReadOnlyList<int>>> ShortestPathAvoidingDomokun;
        public readonly Lazy<Maybe<IReadOnlyList<int>>> ShortestPathToEndpoint;
        public readonly Lazy<IReadOnlyList<int>> ShortestPathToDomokun;

        public Maze(
            IRunAwayStrategy runAwayStrategy,
            IEnumerable<IEnumerable<int>> graph,
            int ponyPosition,
            int domokunPosition,
            int endPointPosition)
        {
            _runAwayStrategy = runAwayStrategy;
            PonyPosition = ponyPosition;
            DomokunPosition = domokunPosition;
            EndPointPosition = endPointPosition;

            // Materialize and recreate collections to disable breaking the encapsulation by casting back to List
            Graph = new ReadOnlyCollection<ReadOnlyCollection<int>>(graph
                .Select(node => new ReadOnlyCollection<int>(node.ToList())).ToList());

            ShortestPathAvoidingDomokun = new Lazy<Maybe<IReadOnlyList<int>>>(() =>
                GetShortestPath(PonyPosition, EndPointPosition, new HashSet<int>(new[] { DomokunPosition })));

            ShortestPathToEndpoint = new Lazy<Maybe<IReadOnlyList<int>>>(() =>
                GetShortestPath(PonyPosition, EndPointPosition, new HashSet<int>()));

            ShortestPathToDomokun = new Lazy<IReadOnlyList<int>>(() =>
                GetShortestPath(PonyPosition, DomokunPosition, new HashSet<int>()).Value);
        }

        public Maybe<GraphMove> FindBestMove(
            Maze maze)
        {
            var move = maze.ShortestPathAvoidingDomokun.Value.HasValue
                ? maze.ShortestPathAvoidingDomokun.Value.Select(
                    path => new GraphMove(fromNode: path[0], toNode: path[1]))
                : _runAwayStrategy.Run(maze);

            return move;
        }

        private Maybe<IReadOnlyList<int>> GetShortestPath(int from, int to, ISet<int> nodesToAvoid)
        {
            var alreadyVisited = new HashSet<int>();
            var queue = new Queue<int>();
            var predecessorsMap = Enumerable.Repeat((int?)null, Graph.Count).ToList();

            queue.Enqueue(from);
            alreadyVisited.Add(from);

            // run BFS on the graph
            while (queue.Peek() != to)
            {
                var current = queue.Dequeue();
                foreach (var neighbor in Graph[current].Where(node => !alreadyVisited.Contains(node) && !nodesToAvoid.Contains(node)))
                {
                    queue.Enqueue(neighbor);
                    predecessorsMap[neighbor] = current;
                    alreadyVisited.Add(neighbor);
                }

                if (!queue.Any())
                {
                    return Maybe<IReadOnlyList<int>>.None;
                }
            }

            IEnumerable<int> GetReversedPath()
            {
                int? current = to;
                while (!(current is null))
                {
                    yield return current.Value;
                    current = predecessorsMap[current.Value];
                }
            }

            return GetReversedPath().Reverse().ToList();
        }
    }
}