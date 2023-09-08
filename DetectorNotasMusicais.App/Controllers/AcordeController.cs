using DetectorNotasMusicais.App.Utils.Bases;
using NAudio.Wave;
using static DetectorNotasMusicais.App.Utils.Fixtures.Get;
using static DetectorNotasMusicais.App.Utils.Fixtures.Void;

namespace DetectorNotasMusicais.App.Controllers
{
    public sealed class AcordeController:AudioBase
    {
        static readonly List<string> listaNotas = new();
        static int qtdLoopsParaLimparListaNotas = 0;
 
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
            // Converter o áudio de bytes para array de float;
            float[] audioBuffer = new float[bytesLidos / 2];
            float maxAmplitude = 0;

            for (int i = 0; i < bytesLidos / 2; i++)
            {
                audioBuffer[i] = BitConverter.ToInt16(buffer, i * 2) / 32768f;
                maxAmplitude = Math.Max(maxAmplitude, Math.Abs(audioBuffer[i]));
            }

            // Detectar a frequência;
            float frequencia = DetectarFrequencia(audioBuffer, taxaAmostragem_kHz);

            // É necesário verificar se o ambiente está em silêncio para exibir novas atualizações;
            bool isProvavelmenteSilencio = maxAmplitude < fatorLimiarDeSilencio;
            // Console.WriteLine($"isProvavelmenteSilencio: {isProvavelmenteSilencio} | maxAmplitude: {maxAmplitude} | fatorLimiarDeSilencio: {fatorLimiarDeSilencio}");

            if (!isProvavelmenteSilencio)
            {
                qtdLoopsParaLimparListaNotas = 0;

                // Mapear a nota com base na frequência encontrada;
                string nota = MapearNota(frequencia);
                listaNotas.Add(nota);

                // Exibir nota;
                ExibirMensagemInicial();
                ExibirMensagemFinalizacao();

                Console.Write("Nota: ");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write($"{nota}{(isExibirFrequencia ? $" — {frequencia}" : string.Empty)}\n");
                Console.ResetColor();
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

                    Console.WriteLine($"LISTA DE NOTAS: {string.Join(", ", listaNotasSemDuplicatas)}");
                    listaNotas.Clear();
                    qtdLoopsParaLimparListaNotas = 0;
                }
            }
        }
    }
}