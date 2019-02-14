using Access2Justice.DataFixes.DataAccess;
using System.Threading.Tasks;

namespace Access2Justice.DataFixes.DataFixers
{
    public interface IDataFixer
    {
        Task ApplyFixAsync(CosmosDbSettings cosmosDbSettings, CosmosDbService cosmosDbService);
    }
}
