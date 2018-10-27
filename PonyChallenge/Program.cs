using System;
using System.Linq;
using System.Threading.Tasks;
using MoreLinq.Extensions;
using PonyChallenge.Api;
using PonyChallenge.Logic.RunningAway;

namespace PonyChallenge
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            Console.SetWindowSize(120, 60);

            var api = new PonyApi();

            var runAwayStrategy = new IRunAwayStrategy[]
                {
                    new StraightToEndPointRunAwayStrategy(),
                    new PanicRunAwayStrategy()
                }
                .RandomSubset(1)
                .Single(); // Should be replaced with some AI

            var moveDirectionExtractor = new MoveDirectionExtractor();
            var walker = new MazeWalker(api, runAwayStrategy, moveDirectionExtractor);

            walker.WeLost += (sender, eventArgs) => { Console.WriteLine("We lost :("); };
            walker.WeWon += (sender, eventArgs) => { Console.WriteLine("We won!"); };
            walker.PonyMoved += (sender, eventArgs) =>
            {
                Console.Clear();
                Console.WriteLine($"Distance to the end-point: {eventArgs.DistanceFromEndPoint}");
                Console.WriteLine($"Distance to the Domokun  : {eventArgs.DistanceFromDomokun}");
                Console.WriteLine(eventArgs.Board);
            };

            await walker.Run(new CreateNewMazeCommand(25, 25, 10, Pony.PinkiePie));

            Console.ReadLine();
        }
    }
}