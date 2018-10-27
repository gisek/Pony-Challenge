namespace PonyChallenge.Logic
{
    public class GraphMove
    {
        public GraphMove(int fromNode, int toNode)
        {
            FromNode = fromNode;
            ToNode = toNode;
        }

        public int FromNode { get; }
        public int ToNode { get; }
    }
}
