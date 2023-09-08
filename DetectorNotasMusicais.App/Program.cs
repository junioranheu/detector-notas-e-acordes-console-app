using DetectorNotasMusicais.App.Controllers;

Console.WriteLine("Iniciando o projeto");

int dispositivoId = MicController.DetectarDispositivo();
AudioController.DetectarAudio(dispositivoId);