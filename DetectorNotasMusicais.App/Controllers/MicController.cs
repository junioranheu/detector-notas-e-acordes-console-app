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

        static List<Dispositivo> ListarDispositivos()
        {
            MMDeviceEnumerator enumerador = new();
            MMDeviceCollection? enumeradorDispositivos = enumerador.EnumerateAudioEndPoints(DataFlow.Capture, DeviceState.Active);

            if (!enumeradorDispositivos.Any())
            {
                EstourarExcecao("Nenhum microfone foi encontrado para continuar o processo.");
            }

            List<Dispositivo> listaDispositivos = new();

            foreach (var device in enumeradorDispositivos)
            {
                Dispositivo dispositivo = new()
                {
                    Id = xxxx,
                    Codigo = device.ID,
                    Nome = device.FriendlyName,
                    Status = device.State
                };

                listaDispositivos.Add(dispositivo);
            }

            return listaDispositivos;
        }
    }
}