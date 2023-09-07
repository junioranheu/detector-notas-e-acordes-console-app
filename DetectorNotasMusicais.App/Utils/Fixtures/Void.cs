namespace DetectorNotasMusicais.App.Utils.Fixtures
{
    public sealed class Void
    {
        public static void EstourarExcecao(string ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(ex);
            Console.ResetColor();
            Environment.Exit(0);
        }
    }
}