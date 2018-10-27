namespace PonyChallenge.Api
{
    public class Pony
    {
        private readonly string _name;

        private Pony(string name)
        {
            _name = name;
        }

        public static implicit operator string(Pony pony)
        {
            return pony._name;
        }

        public override string ToString()
        {
            return _name;
        }

        public static Pony Spike { get; } = new Pony("Spike");
        public static Pony RainbowDash { get; } = new Pony("Rainbow Dash");
        public static Pony Rarity { get; } = new Pony("Rarity");
        public static Pony Applejack { get; } = new Pony("Applejack");
        public static Pony Fluttershy { get; } = new Pony("Fluttershy");
        public static Pony PinkiePie { get; } = new Pony("Pinkie Pie");
    }
}