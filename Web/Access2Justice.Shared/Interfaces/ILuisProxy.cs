namespace Access2Justice.Shared
{
    using System.Threading.Tasks;

    public interface ILuisProxy
    {
        Task<IntentWithScore> GetLuisIntent(LuisInput luisInput);
    }
}
