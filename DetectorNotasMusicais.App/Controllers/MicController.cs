using DetectorNotasMusicais.App.Models;
using NAudio.CoreAudioApi;
using static DetectorNotasMusicais.App.Utils.Fixtures.Void;

namespace DetectorNotasMusicais.App.Controllers
{
    public sealed class MicController
    {
        public static int DetectarDispositivo()
        {
            List<Dispositivo> listaDispositivos = ListarDispositivos();
            int dispositivoId = SelecionarDispositivo(listaDispositivos);

            return dispositivoId;
        }

        private static List<Dispositivo> ListarDispositivos()
        {
            MMDeviceEnumerator enumerador = new();
            MMDeviceCollection? enumeradorDispositivos = enumerador.EnumerateAudioEndPoints(DataFlow.Capture, DeviceState.Active);

            if (!enumeradorDispositivos.Any())
            {
                EstourarExcecao("Nenhum dispositivo foi encontrado para continuar o processo.\nCertifique-se que ao menos um microfone está conectado e tente novamente.");
            }

            List<Dispositivo> listaDispositivos = new();

            int i = 1;
            foreach (var device in enumeradorDispositivos)
            {
                Dispositivo dispositivo = new()
                {
                    DispositivoId = i,
                    Codigo = device.ID,
                    Nome = device.FriendlyName,
                    Status = device.State
                };

                listaDispositivos.Add(dispositivo);
                i++;
            }

            return listaDispositivos;
        }

        private static int SelecionarDispositivo(List<Dispositivo> listaDispositivos)
        {
            Console.WriteLine("\nEscolha um dos dispositivos disponíveis abaixo:");

            foreach (var item in listaDispositivos)
            {
                Console.WriteLine($"🎤 #{item.DispositivoId} — {item.Nome}.");
            }

            int opcaoMax = listaDispositivos.Select(x => x.DispositivoId).Max();
            int dispositivoId = 0;

            do
            {
                Console.Write("\nDispositivo: #");
                string? input = Console.ReadLine();
                bool isErroTryParse = false;

                if (!int.TryParse(input, out dispositivoId))
                {
                    isErroTryParse = true;
                    ExibirMensagemErro(msg: "\nValor inserido é inválido. Tente novamente. ❌", isLimparConsole: false);
                }

                if (dispositivoId < 1 || dispositivoId > opcaoMax)
                {
                    string msgUnicoDispositivo = $"{(isErroTryParse ? "" : "\n")}Há apenas 1 dispositivo válido. Escolha o número #1, por favor.";
                    string msgMultiplosDispositivos = $"{(isErroTryParse ? "" : "\n")}Escolha um dispositivo válido, do #1 ao #{opcaoMax}, por favor.";

                    ExibirMensagemErro(msg: opcaoMax > 1 ? msgMultiplosDispositivos : msgUnicoDispositivo, isLimparConsole: false);
                }
            } while (dispositivoId < 1 || dispositivoId > opcaoMax);

            // Workaround: o dispositivo começa com 0, portanto é necessário subtrair 1 do escolhido pelo usuário;
            dispositivoId -= 1;

            return dispositivoId;
        }
    }
}