using DetectorNotasMusicais.App.Controllers;

#region bem_vindo;
Console.ForegroundColor = ConsoleColor.Cyan;
Console.WriteLine(@"     _          _                
    / \   _ __ | |__   ___ _   _ 
   / _ \ | '_ \| '_ \ / _ \ | | |
  / ___ \| | | | | | |  __/ |_| |
 /_/   \_\_| |_|_| |_|\___|\__,_|
                                 ");

Console.WriteLine("Projeto criado por @junioranheu (com ajuda do ChatGPT 3.5)");
Console.ResetColor();
#endregion;

int dispositivoId = MicController.DetectarDispositivo();
AudioController.DetectarAudio(dispositivoId);