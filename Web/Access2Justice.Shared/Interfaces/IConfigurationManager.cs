namespace Access2Justice.Shared.Interfaces
{
    public interface IConfigurationManager
    {
        T Bind<T>(string appsettingsFileDirectory, string sectionName) where T : new();
    }
}
