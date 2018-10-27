using CSharpFunctionalExtensions;

namespace PonyChallenge.Logic
{
    public class PonyMovedEventArgs
    {
        public PonyMovedEventArgs(Maybe<int> distanceFromEndPoint, Maybe<int> distanceFromDomokun, string board)
        {
            DistanceFromEndPoint = distanceFromEndPoint;
            DistanceFromDomokun = distanceFromDomokun;
            Board = board;
        }

        public Maybe<int> DistanceFromEndPoint { get; }
        public Maybe<int> DistanceFromDomokun { get; }
        public string Board { get; }
    }
}