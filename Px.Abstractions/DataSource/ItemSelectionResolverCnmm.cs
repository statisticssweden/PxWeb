using PCAxis.Menu;
using Px.Abstractions.Interfaces;

namespace Px.Abstractions.DataSource
{
    public class ItemSelectionResolverCnmm : IItemSelectionResolver
    {
        public ItemSelection Resolve(string selection)
        {
            //todo: Try to get from cache
            //todo : Get ItemSelection from pcaxis.sql from Norway

            var itemSelection = string.IsNullOrEmpty(selection) ? new ItemSelection() : new ItemSelection("START", selection);
            
            //todo: store in cache
            
            return itemSelection;
        }
    }
}
