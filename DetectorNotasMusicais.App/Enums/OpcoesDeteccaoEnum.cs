using System.ComponentModel;

namespace DetectorNotasMusicais.App.Enums
{
    public enum OpcoesDeteccaoEnum
    {
        [Description("Detector de notas. 🎵")]
        Notas = 1,

        [Description("Detector de acordes. 🎶")]
        Acordes = 2
    }
}