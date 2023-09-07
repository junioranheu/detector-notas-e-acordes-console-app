using NAudio.CoreAudioApi;

namespace DetectorNotasMusicais.App.Controllers
{
    public sealed class MicController
    {
        public static int DetectarDispositivo()
        {
            ListarDispositivos();
            return 0;
        }

        static void ListarDispositivos()
        {
            MMDeviceEnumerator enumerador = new();
            MMDeviceCollection? listaDispositivos = enumerador.EnumerateAudioEndPoints(DataFlow.Capture, DeviceState.Active);

            if (!listaDispositivos.Any())
            {
                Console.ForegroundColor = ConsoleColor.Red;
                throw new ApplicationException("Nenhum microfone foi encontrado para continuar o processo.");
            }

            Console.WriteLine("List of Active Microphones:");
            foreach (var device in listaDispositivos)
            {
                Console.WriteLine($"Device Name: {device.FriendlyName}");
                Console.WriteLine($"Device ID: {device.ID}");
                Console.WriteLine($"Device State: {device.State}");
                Console.WriteLine();
            }
        }
    }
}