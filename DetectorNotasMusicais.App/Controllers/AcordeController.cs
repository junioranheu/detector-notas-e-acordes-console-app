using DetectorNotasMusicais.App.Utils.Bases;
using NAudio.Wave;
using static DetectorNotasMusicais.App.Utils.Fixtures.Get;
using static DetectorNotasMusicais.App.Utils.Fixtures.Void;

namespace DetectorNotasMusicais.App.Controllers
{
    public sealed class AcordeController : AudioBase
    {
        #region variaveis
        static readonly List<string> listaNotas = new();
        static int qtdLoopsParaLimparListaNotas = 0;

        static readonly Dictionary<string, List<string>> dicionarioAcordes = new()
        {
            ["C"] = new List<string> { "C", "E", "G" },
            ["D"] = new List<string> { "D", "F#", "A" },
            ["E"] = new List<string> { "E", "G#", "B" },
            ["F"] = new List<string> { "F", "A", "C" },
            ["G"] = new List<string> { "G", "B", "D" },
            ["A"] = new List<string> { "A", "C#", "E" },
            ["B"] = new List<string> { "B", "D#", "F#" }
        };
        #endregion;

        public static void DetectarAudio(int dispositivoId)
        {
            // Instanciar um novo objeto para a captura de áudio do microfone do usuário;
            WaveInEvent mic = new()
            {
                DeviceNumber = dispositivoId,
                WaveFormat = new WaveFormat(taxaAmostragem_kHz, 16, 1), // Mono, 44.1 kHz;
                BufferMilliseconds = 150, // "Delay" para capturar áudio;
                NumberOfBuffers = 3
            };

            // Event handler para dados de áudio recebidos;
            mic.DataAvailable += (sender, e) => HandleDetectarAcorde(taxaAmostragem_kHz, e.Buffer, e.BytesRecorded);

            // Iniciar a captura de áudio;
            mic.StartRecording();

            // Finalizar o processo;
            ExibirMensagemFinalizacao();
            Console.ReadKey();
            mic.StopRecording();
            mic.Dispose();
            ExibirMensagemInicial();
            Console.WriteLine("\nAdeus! 👋");
        }

        private static void HandleDetectarAcorde(int taxaAmostragem_kHz, byte[] buffer, int bytesLidos)
        {
            var (frequencia, isProvavelmenteSilencio) = CalcularFrequencia_E_VerificarIsProvavelmentSilencio(taxaAmostragem_kHz, buffer, bytesLidos);

            if (!isProvavelmenteSilencio)
            {
                qtdLoopsParaLimparListaNotas = 0;

                // Mapear a nota com base na frequência encontrada;
                string nota = MapearNota(frequencia);
                listaNotas.Add(nota);

                // Exibir nota;
                ExibirMensagemInicial();
                ExibirMensagemFinalizacao();
                ExibirMensagemNota(nota, isExibirFrequencia, frequencia);
            }
            else
            {
                const int qtdMaxLoopsParaLimparListaNotas = 10;
                qtdLoopsParaLimparListaNotas++;
                // Console.WriteLine($"qtdLoopsParaLimparListaNotas: {qtdLoopsParaLimparListaNotas}");

                if (qtdLoopsParaLimparListaNotas > qtdMaxLoopsParaLimparListaNotas)
                {
                    List<string> listaNotasSemOitava = ObterTonsSemOitava(listaNotas);
                    List<string> listaNotasSemDuplicatas = ObterArrayDistinct(listaNotasSemOitava);
                    string acorde = MapearAcorde(listaNotasSemOitava);

                    Console.WriteLine($"Lista de notas tocadas: {string.Join(", ", listaNotasSemDuplicatas)}\nAcorde tocado: {acorde}");
                    listaNotas.Clear();
                    qtdLoopsParaLimparListaNotas = 0;
                }
            }
        }

        private static string MapearAcorde(List<string> listaNotas)
        {
            foreach (var acorde in dicionarioAcordes)
            {
                bool isAcordeEmQuestao = true;

                foreach (var nota in listaNotas)
                {
                    if (!acorde.Value.Contains(nota))
                    {
                        isAcordeEmQuestao = false;
                        break;
                    }
                }

                if (isAcordeEmQuestao)
                {
                    return acorde.Key;
                }
            }

            return "Acorde desconhecido.";
        }
    }
}