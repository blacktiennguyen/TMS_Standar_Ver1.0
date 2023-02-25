namespace TMS.Core
{
    public interface ISessionWrapper
    {
        T GetFromSession<T>(string key);

        void SetInSession(string key, object value);
    }
}