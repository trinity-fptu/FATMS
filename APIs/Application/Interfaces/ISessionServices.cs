namespace Application.Interfaces
{
    public interface ISessionServices
    {
        void SaveToken(int id, string token);
        string GetTokenByKey(int id);
        bool RemoveToken(int id);
    }
}
