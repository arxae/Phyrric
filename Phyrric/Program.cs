namespace Phyrric
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var game = new PhyrricGame())
            {
                game.Run();
            }
        }
    }
}
