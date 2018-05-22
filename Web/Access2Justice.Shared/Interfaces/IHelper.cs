namespace Access2Justice.Shared.Interfaces
{
    using System.Threading.Tasks;

    public interface IHelper
    {
        Task<dynamic> GetTopicAsync(string keywords);
    }
}
