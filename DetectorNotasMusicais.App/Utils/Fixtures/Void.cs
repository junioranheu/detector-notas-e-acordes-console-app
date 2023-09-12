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

        public static void ExibirMensagemFinalizacao()
        {
            Console.WriteLine("\nPressione qualquer tecla para finalizar o programa. 🎶\n");
        }

        public static void EstourarExcecao(string ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\n{ex}");
            Console.ResetColor();
            Environment.Exit(0);
        }

        public static void ExibirMensagemNota(string nota, bool isExibirFrequencia, float frequencia)
        {
            Console.Write("Nota: ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write($"{nota}{(isExibirFrequencia ? $" — {frequencia}" : string.Empty)}\n");
            Console.ResetColor();
        }

        public static void ExibirMensagemErro(string msg, bool isLimparConsole)
        {
            if (isLimparConsole)
            {
                Console.Clear();
                ExibirMensagemInicial();
            }

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(msg);
            Console.ResetColor();
        }
    }
}