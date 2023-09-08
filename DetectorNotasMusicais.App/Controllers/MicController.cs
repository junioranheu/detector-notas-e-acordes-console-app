using DetectorNotasMusicais.App.Models;
using NAudio.CoreAudioApi;
using static DetectorNotasMusicais.App.Utils.Fixtures.Void;

namespace DetectorNotasMusicais.App.Controllers
{
    public sealed class MicController
    {
        public static int DetectarDispositivo()
        {
            ListarDispositivos();
            return 0;
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
    }
}