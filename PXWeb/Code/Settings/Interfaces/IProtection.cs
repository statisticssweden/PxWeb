using PX.Security;

namespace PXWeb
{
    public interface IProtection
    {
        bool IsProtected { get; }
        IAuthorization AuthorizationMethod { get; }
    }
}
