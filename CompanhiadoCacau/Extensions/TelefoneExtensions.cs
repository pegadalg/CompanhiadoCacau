namespace CompanhiadoCacau.Extensions
{
    public static class TelefoneExtensions
    {
        // Método de extensão para formatar o telefone
        public static string FormatarTelefone(this string telefone)
        {
            if (string.IsNullOrWhiteSpace(telefone) || telefone.Length != 11)
            {
                return telefone;  // Retorna o telefone original se não for válido
            }

            return string.Format("({0}) {1}-{2}",
                telefone.Substring(0, 2),
                telefone.Substring(2, 5),
                telefone.Substring(7, 4));
        }
    }
}
