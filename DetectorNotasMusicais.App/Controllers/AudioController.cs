using NAudio.Wave;

namespace DetectorNotasMusicais.App.Controllers
{
    public sealed class AudioController
    {
        public static void DetectarAudio()
        {
            const int taxaAmostragem_kHz = 44100;

            // Instancia um novo objeto para a captura de áudio do microfone do usuário;
            WaveInEvent mic = new()
            {
                DeviceNumber = 0, // Identificação do dispositivo;
                WaveFormat = new WaveFormat(taxaAmostragem_kHz, 1) // Mono, 44.1 kHz;
            };

            // Event handler para dados de áudio recebidos;
            mic.DataAvailable += (sender, e) => ProcessarAudio(taxaAmostragem_kHz, e.Buffer, e.BytesRecorded);

            // Inicia a captura de áudio;
            mic.StartRecording();

            Console.WriteLine("Pressione qualquer tecla para finalizar o processo.");
            Console.ReadKey();

            // Finalizar o processo;
            mic.StopRecording();
            mic.Dispose();
        }

        private static void ProcessarAudio(int taxaAmostragem_kHz, byte[] buffer, int bytesLidos)
        {
            // Converte o áudio de bytes para array de float;
            float[] audioBuffer = new float[bytesLidos / 2];

            for (int i = 0; i < bytesLidos / 2; i++)
            {
                audioBuffer[i] = BitConverter.ToInt16(buffer, i * 2) / 32768f;
            }

            // Detecte a frequência;
            float frequencia = DetectarFrequencia(audioBuffer, taxaAmostragem_kHz);

            // Mapear a nota com base na frequência encontrada;
            string nota = MapearNotaPeloTom(frequencia);

            Console.WriteLine($"Nota: {nota} | Frequência: {frequencia}");
        }

        private static float DetectarFrequencia(float[] audioBuffer, int taxaAmostragem_kHz)
        {
            int minFrequenciaHz = 30; // Frequência mínima esperada em Hz;
            int maxFrequenciaHz = 1000; // Frequência máxima esperada em Hz

            float melhorFrequenciaEncontrada = 0;
            float melhorCorrelacaoEncontrada = 0;

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

        private static string MapearNotaPeloTom(float tom)
        {
            const float refFrequencia = 440.0f; // Define a frequência de referência para A4 (440 Hz)
            const int refSemitonsPorOitava = 12; // Define o número de semitons por oitava;

            // Calcula o número de semitons distantes da nota de referência;
            float distanciaEmSemitonsDaNotaDeRef = 12 * (float)Math.Log2(tom / refFrequencia);

            // Definir lista de notas musicais;
            string[] listaNotasMusicais = { "A", "A#", "B", "C", "C#", "D", "D#", "E", "F", "F#", "G", "G#" };

            // Calcule o index de notas;
            int index = (int)Math.Round(distanciaEmSemitonsDaNotaDeRef) % refSemitonsPorOitava;

            // Certifique-se de que o índice da nota seja positivo;
            if (index < 0)
            {
                index += refSemitonsPorOitava;
            }

            // Determine o número da oitava;
            int oitava = 4 + (int)Math.Floor(distanciaEmSemitonsDaNotaDeRef / refSemitonsPorOitava);

            // Construa o nome completo da nota, incluindo a oitava;
            string nota = $"{listaNotasMusicais[index]}{oitava}";

            return nota;
        }
    }
}