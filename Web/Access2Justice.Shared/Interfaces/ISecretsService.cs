using System.Threading.Tasks;

namespace Access2Justice.Shared.Interfaces
{
    public interface ISecretsService
    {
        Task<string> GetSecretAsync(string secretName);
        string GetSecret(string secretName);
    }
}
