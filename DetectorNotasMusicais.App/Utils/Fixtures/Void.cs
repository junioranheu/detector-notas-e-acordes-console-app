namespace DetectorNotasMusicais.App.Utils.Fixtures
{
    public sealed class Void
    {
        public static void ExibirMensagemInicial()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(@"     _          _                
    / \   _ __ | |__   ___ _   _ 
   / _ \ | '_ \| '_ \ / _ \ | | |
  / ___ \| | | | | | |  __/ |_| |
 /_/   \_\_| |_|_| |_|\___|\__,_|
                                 ");

            Console.WriteLine("Projeto criado por @junioranheu (com ajuda do ChatGPT 3.5). 🤠");
            Console.ResetColor();
        }

        public static void EstourarExcecao(string ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\n{ex}");
            Console.ResetColor();
            Environment.Exit(0);
        }
    }
}