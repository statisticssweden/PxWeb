namespace PX.Security
{
    public interface IAuthorization
    {
        bool IsAuthorized(string dbid, string menu, string selection);
        bool IsAuthorized(string dbid);
    }
}
