namespace Maddalena.Core.Settings
{
    public interface ISettingsService
    {
        void Save<T>(T obj);
        T Get<T>();
    }
}