using DetectorNotasMusicais.App.Enums;
using static DetectorNotasMusicais.App.Utils.Fixtures.Get;

namespace DetectorNotasMusicais.App.Utils.Fixtures
{
    public sealed class Void
    {
        public static void ExibirMensagemInicial(OpcoesDeteccaoEnum? opcaoDeteccaoEnum)
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

            if (opcaoDeteccaoEnum is not null)
            {
                Console.WriteLine($"\n{ObterDescricaoEnum(OpcoesDeteccaoEnum.Notas)}");
            }
        }

        public static (OpcoesDeteccaoEnum? opcaoDeteccaoEnum, bool isErroOpcaoDeteccaoEnum) QuestionarOpcaoDeteccao(bool isExibirMensagemInicial)
        {
            try
            {
                if (isExibirMensagemInicial)
                {
                    Console.WriteLine("\nVocê quer utilizar:");
                }

                Console.WriteLine($"{(isExibirMensagemInicial ? "" : "\n")}#1 — {ObterDescricaoEnum(OpcoesDeteccaoEnum.Notas)}");
                Console.WriteLine($"#2 — {ObterDescricaoEnum(OpcoesDeteccaoEnum.Acordes)}");
                Console.Write("\n#");

                int? input = Convert.ToInt32(Console.ReadLine());

                if (Enum.IsDefined(typeof(OpcoesDeteccaoEnum), input))
                {
                    OpcoesDeteccaoEnum opcaoDeteccaoEnum = (OpcoesDeteccaoEnum)input;
                    return (opcaoDeteccaoEnum, isErroOpcaoDeteccaoEnum: false);
                }

                return (opcaoDeteccaoEnum: null, isErroOpcaoDeteccaoEnum: true);
            }
            catch (Exception)
            {
                return (opcaoDeteccaoEnum: null, isErroOpcaoDeteccaoEnum: true);
            }
        }

        public static void ExibirMensagemFinalizacao()
        {
            Console.WriteLine("\nPressione qualquer tecla para finalizar o programa. 👋\n");
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
                ExibirMensagemInicial(opcaoDeteccaoEnum: null);
            }

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(msg);
            Console.ResetColor();
        }
    }
}