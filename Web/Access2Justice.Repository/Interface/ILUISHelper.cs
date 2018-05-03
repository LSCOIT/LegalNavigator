namespace Access2Justice.Repository
{
    using System.Threading.Tasks;


    public interface ILUISHelper
    {
        Task GetLUISIntent(LUISInput luisInput);
    }
}
