using DetectorNotasMusicais.App.Controllers;
using DetectorNotasMusicais.App.Enums;
using System.Text;
using static DetectorNotasMusicais.App.Utils.Fixtures.Void;

#region config_e_variaveis
Console.OutputEncoding = Encoding.UTF8;

bool isExibirMensagemInicial = true, isErroOpcaoDeteccaoEnum;
OpcoesDeteccaoEnum? opcaoDeteccaoEnum;
#endregion

#region program
ExibirMensagemInicial(opcaoDeteccaoEnum: null);
int dispositivoId = MicController.DetectarDispositivo();

do
{
    (opcaoDeteccaoEnum, isErroOpcaoDeteccaoEnum) = QuestionarOpcaoDeteccao(isExibirMensagemInicial);
    ExibirMensagemInicial(opcaoDeteccaoEnum);

    if (opcaoDeteccaoEnum == OpcoesDeteccaoEnum.Notas)
    {
        NotaController.DetectarAudio(dispositivoId);
    }
    else if (opcaoDeteccaoEnum == OpcoesDeteccaoEnum.Acordes)
    {
        AcordeController.DetectarAudio(dispositivoId);
    }
    else
    {
        isExibirMensagemInicial = false;
        ExibirMensagemErro("\nVocê deve selecionar uma das opções disponíveis abaixo:", isLimparConsole: true);
    }
} while (isErroOpcaoDeteccaoEnum);
#endregion;