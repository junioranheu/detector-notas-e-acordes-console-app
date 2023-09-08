﻿using MathNet.Numerics;
using MathNet.Numerics.IntegralTransforms;
using NAudio.Wave;
using static DetectorNotasMusicais.App.Utils.Fixtures.Void;

namespace DetectorNotasMusicais.App.Controllers
{
    public sealed class AcordeController
    {
        #region variaveis_globais
        private const int taxaAmostragem_kHz = 44100; // Taxa de amostragem (Sampling rate);
        static readonly string[] listaNotasMusicais = { "A", "A#", "B", "C", "C#", "D", "D#", "E", "F", "F#", "G", "G#" };
        private const float refFrequencia = 440.0f; // Frequência de referência para a nota A4 (440 Hz), utilizada para encontrar a distância em semitons de outras notas;
        private const int refSemitonsPorOitava = 12; // Número de semitons por oitava;
        private const float fatorLimiarDeSilencio = 0.01f; // Define o que é provavelmente silêncio ou não;
        private const bool isExibirFrequencia = false; // Exibe ou esconde a frequência no output;

        static readonly List<string> listaNotas = new();
        static int qtdLoopsParaLimparListaNotas = 0;
        #endregion;

        public static void DetectarAudio(int dispositivoId)
        {
            // Instanciar um novo objeto para a captura de áudio do microfone do usuário;
            WaveInEvent mic = new()
            {
                DeviceNumber = dispositivoId,
                WaveFormat = new WaveFormat(taxaAmostragem_kHz, 16, 1), // Mono, 44.1 kHz;
                BufferMilliseconds = 200, // "Delay" para capturar áudio;
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

        #region metodos_auxiliares
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
                qtdLoopsParaLimparListaNotas++;
                // Console.WriteLine($"qtdLoopsParaLimparListaNotas: {qtdLoopsParaLimparListaNotas}");

                if (qtdLoopsParaLimparListaNotas > 10)
                {
                    Console.WriteLine($"LISTA DE NOTAS: {string.Join(", ", listaNotas)}");
                    listaNotas.Clear();
                    qtdLoopsParaLimparListaNotas = 0;
                }
            }
        }

        private static float DetectarFrequencia(float[] audioBuffer, int taxaAmostragem_kHz)
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

        private static int EncontrarPicoMaximo(Complex32[] data)
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
        #endregion;
    }
}