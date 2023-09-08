using DetectorNotasMusicais.App.Controllers;
using System.Text;
using static DetectorNotasMusicais.App.Utils.Fixtures.Void;

Console.OutputEncoding = Encoding.UTF8;

ExibirMensagemInicial();
int dispositivoId = MicController.DetectarDispositivo();

ExibirMensagemInicial();
AudioController.DetectarAudio(dispositivoId);