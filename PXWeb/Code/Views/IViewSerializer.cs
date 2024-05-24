﻿using PCAxis.Paxiom;
using PCAxis.Query;

namespace PXWeb.Views
{
    /// <summary>
    /// Interface to save and restores the state of the presentation view and layout
    /// </summary>
    public interface IViewSerializer
    {
        /// <summary>
        /// Creates a Output that defines the current state of the presentation view and layout
        /// </summary>
        /// <returns></returns>
        Output Save();

        /// <summary>
        /// Restores the presntation view and layout from a Output object
        /// </summary>
        /// <returns></returns>
        void Render(string format, PCAxis.Query.SavedQuery query, PXModel model, bool safe);

    }
}
