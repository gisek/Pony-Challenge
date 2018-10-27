using System;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using PonyChallenge.Api;
using PonyChallenge.Logic;
using PonyChallenge.Logic.RunningAway;

namespace PonyChallenge
{
    public class MazeWalker
    {
        public event EventHandler WeLost;
        public event EventHandler WeWon;
        public event EventHandler<PonyMovedEventArgs> PonyMoved;

        private readonly PonyApi _ponyApi;
        private readonly IRunAwayStrategy _runAwayStrategy;
        private readonly MoveDirectionExtractor _moveDirectionExtractor;

        public MazeWalker(PonyApi ponyApi, IRunAwayStrategy runAwayStrategy, MoveDirectionExtractor moveDirectionExtractor)
        {
            _ponyApi = ponyApi;
            _runAwayStrategy = runAwayStrategy;
            _moveDirectionExtractor = moveDirectionExtractor;
        }

        public async Task Run(CreateNewMazeCommand createNewMazeCommand)
        {
            const string activeState = "active";

            var mazeId = await _ponyApi.CreateNewMaze(createNewMazeCommand);

            var state = activeState;
            while (state == activeState)
            {
                var maze = await GetMaze(mazeId);

                var bestMove = maze.FindBestMove(maze);
                var directionToMove = _moveDirectionExtractor.GetDirectionToMove(bestMove, maze);

                state = await _ponyApi.Move(mazeId, directionToMove);
                var map = await _ponyApi.Preview(mazeId);

                PonyMoved?.Invoke(this, new PonyMovedEventArgs(
                    maze.ShortestPathToEndpoint.Value.Select(path => path.Count),
                    maze.ShortestPathToDomokun.Value.Count,
                    map));
            }

            if (state == "over")
            {
                WeLost?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                WeWon?.Invoke(this, EventArgs.Empty);
            }
        }

        private async Task<Maze> GetMaze(string mazeId)
        {
            var mazeDto = await _ponyApi.GetMaze(mazeId);
            var mazeGraph = mazeDto.ConvertToGraph();
            var maze = new Maze(_runAwayStrategy, mazeGraph, mazeDto.PonyPosition, mazeDto.DomokunPosition, mazeDto.EndPointPosition);
            return maze;
        }
    }
}