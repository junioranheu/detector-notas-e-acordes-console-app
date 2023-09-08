using MathNet.Numerics;
using MathNet.Numerics.IntegralTransforms;

namespace DetectorNotasMusicais.App.Utils.Bases
{
    public class AudioBase
    {
        internal const int taxaAmostragem_kHz = 44100; // Taxa de amostragem (Sampling rate);
        internal static string[] listaNotasMusicais = { "A", "A#", "B", "C", "C#", "D", "D#", "E", "F", "F#", "G", "G#" };
        internal const float refFrequencia = 440.0f; // Frequência de referência para a nota A4 (440 Hz), utilizada para encontrar a distância em semitons de outras notas;
        internal const int refSemitonsPorOitava = 12; // Número de semitons por oitava;
        internal const float fatorLimiarDeSilencio = 0.06f; // Define o que é provavelmente silêncio ou não;
        internal const bool isExibirFrequencia = false; // Exibe ou esconde a frequência no output;

        internal static float DetectarFrequencia(float[] audioBuffer, int taxaAmostragem_kHz)
        {
            // FFT utilizando MathNet.Numerics;
            Complex32[] fft = new Complex32[audioBuffer.Length];

            for (int i = 0; i < audioBuffer.Length; i++)
            {
                fft[i] = new Complex32(audioBuffer[i], 0);
            }

            Fourier.Forward(fft, FourierOptions.NoScaling);

            // Detectar o pico de magnute em dados complexos (FFT); 
            int pico = EncontrarPicoMaximo(fft);

            // Calcula a frequência;
            float frequencia = pico * taxaAmostragem_kHz / fft.Length;

            return frequencia;
        }

        internal static int EncontrarPicoMaximo(Complex32[] data)
        {
            float maxMagnitude = 0;
            int maxIndex = 0;

            for (int i = 0; i < data.Length / 2; i++)
            {
                float magnitude = data[i].Magnitude;

                if (magnitude > maxMagnitude)
                {
                    maxMagnitude = magnitude;
                    maxIndex = i;
                }
            }

            return maxIndex;
        }

        internal static (float frequencia, bool isProvavelmenteSilencio) CalcularFrequencia_E_VerificarIsProvavelmentSilencio(int taxaAmostragem_kHz, byte[] buffer, int bytesLidos)
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

            return (frequencia, isProvavelmenteSilencio);
        }

        internal static string MapearNota(float frequencia)
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
    }
}