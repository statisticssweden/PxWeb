using PCAxis.Paxiom;
using PxWeb.Api2.Server.Models;

namespace PxWeb.Code.Api2.DataSelection
{
    public interface ISelectionHandler
    {
        public bool Verify(PXModel model, VariablesSelection wantedSelection);
        public Selection[] GetSelection(PXModel model, VariablesSelection wantedSelection);
    }
}
