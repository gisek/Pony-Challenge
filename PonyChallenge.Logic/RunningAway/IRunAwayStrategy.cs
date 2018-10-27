using CSharpFunctionalExtensions;

namespace PonyChallenge.Logic.RunningAway
{
    public interface IRunAwayStrategy
    {
        Maybe<GraphMove> Run(Maze maze);
    }
}