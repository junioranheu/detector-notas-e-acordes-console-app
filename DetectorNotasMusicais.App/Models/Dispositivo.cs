using NAudio.CoreAudioApi;

namespace DetectorNotasMusicais.App.Models
{
    public sealed class Dispositivo
    {
        public int DispositivoId { get; set; }

        public string Nome { get; set; }  = string.Empty;

        public string Codigo { get; set; } = string.Empty;

        public DeviceState Status { get; set; }
    }
}