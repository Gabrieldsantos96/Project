namespace Project.Shared.Validations;
public static class RefreshTokenValidator
{
    public static bool IsValidRefreshToken(string refreshToken)
    {
        if (string.IsNullOrEmpty(refreshToken))
        {
            return false;
        }

        // Verifica o comprimento esperado (32 bytes em Base64 resultam em 44 caracteres)
        if (refreshToken.Length != 44)
        {
            return false;
        }

        // Verifica se a string é um Base64 válido
        try
        {
            // Tenta decodificar a string Base64
            byte[] decodedBytes = Convert.FromBase64String(refreshToken);

            // Verifica se o tamanho do array decodificado é exatamente 32 bytes
            if (decodedBytes.Length != 32)
            {
                return false;
            }

            // Verifica se os caracteres são válidos para Base64
            return refreshToken.All(c => c >= 'A' && c <= 'Z' ||
                                        c >= 'a' && c <= 'z' ||
                                        c >= '0' && c <= '9' ||
                                        c == '+' || c == '/' || c == '=');
        }
        catch (FormatException)
        {
            // Se a decodificação falhar, a string não é um Base64 válido
            return false;
        }
    }
}
