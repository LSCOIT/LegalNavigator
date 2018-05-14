namespace Access2Justice.CognitiveServices
{
    using System.Threading.Tasks;

    public interface ILuisHelper
    {
        Task<IntentWithScore> GetLuisIntent(LuisInput luisInput);
    }
}
