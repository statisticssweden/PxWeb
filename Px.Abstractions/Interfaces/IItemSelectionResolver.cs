using PCAxis.Menu;

namespace Px.Abstractions.Interfaces
{
    public interface IItemSelectionResolver
    {
        ItemSelection Resolve(string language, string selection);
    }
}
