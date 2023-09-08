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
    }
}