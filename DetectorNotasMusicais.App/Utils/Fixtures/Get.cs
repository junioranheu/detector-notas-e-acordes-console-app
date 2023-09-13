using System.ComponentModel;
using System.Reflection;

namespace DetectorNotasMusicais.App.Utils.Fixtures
{
    public sealed class Get
    {
        public static List<string> ObterTonsSemOitava(List<string> listaNotas)
        {
            List<string> listaNotasSemOitavas = new();

            foreach (var item in listaNotas)
            {
                string notaSemOitava = new(item.Where(c => c != '-' && (c < '0' || c > '9')).ToArray());
                listaNotasSemOitavas.Add(notaSemOitava);
            }

            return listaNotasSemOitavas;
        }

        public static List<string> ObterArrayDistinct(List<string> listaNotas)
        {
           return listaNotas.Distinct().ToList(); 
        }

        /// <summary>
        /// Pegar a descrição de um enum;
        /// https://stackoverflow.com/questions/50433909/get-string-name-from-enum-in-c-sharp;
        /// </summary>
        public static string ObterDescricaoEnum(Enum enumVal)
        {
            MemberInfo[] memInfo = enumVal.GetType().GetMember(enumVal.ToString());
            DescriptionAttribute? attribute = CustomAttributeExtensions.GetCustomAttribute<DescriptionAttribute>(memInfo[0]);

            return attribute!.Description;
        }
    }
}