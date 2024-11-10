namespace CompanhiadoCacau.Extensions
{
    public static class CpfExtensions
    {
        // Método de extensão para formatar o CPF
        public static string FormatarCpf(this string cpf)
        {
            if (string.IsNullOrWhiteSpace(cpf) || cpf.Length != 11)
            {
                return cpf;  // Retorna o CPF original se não for válido
            }

            return string.Format("{0}.{1}.{2}-{3}",
                cpf.Substring(0, 3),
                cpf.Substring(3, 3),
                cpf.Substring(6, 3),
                cpf.Substring(9, 2));
        }
    }
}
