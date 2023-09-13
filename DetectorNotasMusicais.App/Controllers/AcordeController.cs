using DetectorNotasMusicais.App.Enums;
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
        #endregion

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
            ExibirMensagemInicial(opcaoDeteccaoEnum: null);
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
                ExibirMensagemInicial(opcaoDeteccaoEnum: OpcoesDeteccaoEnum.Acordes);
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
                    var (isErro, strRetorno) = MapearAcorde(listaNotasSemDuplicatas);

                    if (isErro)
                    {
                        ExibirMensagemErro(msg: $"\n{strRetorno}", isLimparConsole: true);
                        return;
                    }

                    Console.WriteLine($"Lista de notas tocadas: {string.Join(", ", listaNotasSemDuplicatas)}.\nAcorde tocado: {strRetorno}");
                    listaNotas.Clear();
                    qtdLoopsParaLimparListaNotas = 0;
                }
            }
        }

        private static (bool isErro, string strRetorno) MapearAcorde(List<string> listaNotas)
        {
            if (listaNotas.Count < 3)
            {
                return (isErro: true, strRetorno: "O acorde precisa ter pelo menos 3 notas.");
            }

            foreach (var item in dicionarioAcordes)
            {
                List<string>? listaNotasNoAcorde = item.Value;

                if (VerificarNotasFazemParteDeUmAcordeValido(listaNotas, listaNotasNoAcorde))
                {
                    return (isErro: false, strRetorno: item.Key);
                }
            }

            return (isErro: true, strRetorno: "Acorde desconhecido.");
        }

        private static bool VerificarNotasFazemParteDeUmAcordeValido(List<string> listaNotas, List<string> notasAcorde)
        {
            foreach (var nota in notasAcorde)
            {
                if (!listaNotas.Contains(nota))
                {
                    return false;
                }
            }

            return true;
        }
    }
}