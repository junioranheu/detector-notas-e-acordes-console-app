using NAudio.Wave;
using static DetectorNotasMusicais.App.Utils.Fixtures.Void;

namespace DetectorNotasMusicais.App.Controllers
{
    public sealed class AudioController
    {
        #region variaveis_globais
        static readonly int taxaAmostragem_kHz = 44100; // Taxa de amostragem (Sampling rate);
        static readonly string[] listaNotasMusicais = { "A", "A#", "B", "C", "C#", "D", "D#", "E", "F", "F#", "G", "G#" };
        static readonly float refFrequencia = 440.0f; // Frequência de referência para a nota A4 (440 Hz), utilizada para encontrar a distância em semitons de outras notas;
        static readonly int refSemitonsPorOitava = 12; // Número de semitons por oitava;
        static readonly int minFrequenciaHz = 30; // Frequência mínima esperada em Hz;
        static readonly int maxFrequenciaHz = 1000; // Frequência máxima esperada em Hz
        const bool isExibirFrequencia = true; // Exibe ou esconde a frequência no output;
        #endregion;

        public static void DetectarAudio(int dispositivoId)
        {
            // Instanciar um novo objeto para a captura de áudio do microfone do usuário;
            WaveInEvent mic = new()
            {
                DeviceNumber = dispositivoId,
                WaveFormat = new WaveFormat(taxaAmostragem_kHz, 16, 1), // Mono, 44.1 kHz;
                BufferMilliseconds = 450, // "Delay" para capturar áudio;
                NumberOfBuffers = 3
            };

            // Event handler para dados de áudio recebidos;
            mic.DataAvailable += (sender, e) => HandleDetectarAudio(taxaAmostragem_kHz, e.Buffer, e.BytesRecorded);

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

        #region metodos_auxiliares
        private static void HandleDetectarAudio(int taxaAmostragem_kHz, byte[] buffer, int bytesLidos)
        {
            // Converter o áudio de bytes para array de float;
            float[] audioBuffer = new float[bytesLidos / 2];

            for (int i = 0; i < bytesLidos / 2; i++)
            {
                audioBuffer[i] = BitConverter.ToInt16(buffer, i * 2) / 32768f;
            }

            // Detectar a frequência;
            float frequencia = DetectarFrequencia(audioBuffer, taxaAmostragem_kHz);

            // É necesário verificar se o ambiente está em silêncio para exibir novas atualizações;
            bool isProvavelmenteSilencio = IsProvavelmenteSilencio(frequencia);

            if (!isProvavelmenteSilencio)
            {
                // Mapear a nota com base na frequência encontrada;
                string nota = MapearNota(frequencia);

                ExibirMensagemInicial();
                ExibirMensagemFinalizacao();

                Console.Write("Nota: ");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write($"{nota}{(isExibirFrequencia ? $" — {frequencia}" : string.Empty)}\n");
                Console.ResetColor();
            }
        }

        private static float DetectarFrequencia(float[] audioBuffer, int taxaAmostragem_kHz)
        {
            float melhorFrequenciaEncontrada = 0, melhorCorrelacaoEncontrada = 0;

            for (int lag = minFrequenciaHz; lag <= maxFrequenciaHz; lag++)
            {
                float correlacaoAtual = 0;

                for (int i = 0; i < audioBuffer.Length - lag; i++)
                {
                    correlacaoAtual += audioBuffer[i] * audioBuffer[i + lag];
                }

                if (correlacaoAtual > melhorCorrelacaoEncontrada)
                {
                    melhorCorrelacaoEncontrada = correlacaoAtual;
                    melhorFrequenciaEncontrada = (float)taxaAmostragem_kHz / lag;
                }
            }


            return melhorFrequenciaEncontrada;
        }

        private static string MapearNota(float frequencia)
        {
            // Calcular o número de semitons distantes da nota de referência;
            float distanciaEmSemitonsDaNotaDeRef = 12 * (float)Math.Log2(frequencia / refFrequencia);

            // Calcular o index de notas;
            int index = (int)Math.Round(distanciaEmSemitonsDaNotaDeRef) % refSemitonsPorOitava;

            // Certificar de que o índice da nota é positivo;
            if (index < 0)
            {
                index += refSemitonsPorOitava;
            }

            // Determinar o número da oitava;
            int oitava = 4 + (int)Math.Floor(distanciaEmSemitonsDaNotaDeRef / refSemitonsPorOitava);

            // Construir o nome completo da nota, incluindo a oitava;
            string nota = $"{listaNotasMusicais[index]}{oitava}";

            return nota;
        }

        private static bool IsProvavelmenteSilencio(float frequencia)
        {
            const int minAceitavel = 75;

            bool isFrequenciaIgualAoMinimoAceitavel1 = frequencia == (float)taxaAmostragem_kHz / minFrequenciaHz;
            bool isFrequenciaMenorAoMinimoAceitavel2 = frequencia < minAceitavel;

            return isFrequenciaIgualAoMinimoAceitavel1 || isFrequenciaMenorAoMinimoAceitavel2;
        }
        #endregion;
    }
}