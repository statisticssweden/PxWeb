using System.Collections.Generic;

namespace PXWeb
{
    /// <summary>
    /// Interface for the General.Global.ShowInformationTypes settings
    /// </summary>
    public interface IShowInformationTypesSettings
    {
        /// <summary>
        /// Show Official statistics or not?
        /// </summary>
        bool OfficialStatistics { get; }

        /// <summary>
        /// Show Unit or not?
        /// </summary>
        bool Unit { get; }

        /// <summary>
        /// Show Contact or not?
        /// </summary>
        bool Contact { get; }

        /// <summary>
        /// Show LastUpdated or not?
        /// </summary>
        bool LastUpdated { get; }

        /// <summary>
        /// Show RefPeriod or not?
        /// </summary>
        bool RefPeriod { get; }

        /// <summary>
        /// Show StockFa or not?
        /// </summary>
        bool StockFa { get; }

        /// <summary>
        /// Show CFPrices or not?
        /// </summary>
        bool CFPrices { get; }

        /// <summary>
        /// Show DayAdj or not?
        /// </summary>
        bool DayAdj { get; }

        /// <summary>
        /// Show SeasAdj or not?
        /// </summary>
        bool SeasAdj { get; }

        /// <summary>
        /// Show BasePeriod or not?
        /// </summary>
        bool BasePeriod { get; }

        /// <summary>
        /// Show UpdateFrequency or not?
        /// </summary>
        bool UpdateFrequency { get; }

        /// <summary>
        /// Show NextUpdate or not?
        /// </summary>
        bool NextUpdate { get; }

        /// <summary>
        /// Show Survey or not?
        /// </summary>
        bool Survey { get; }

        /// <summary>
        /// Show Link or not?
        /// </summary>
        bool Link { get; }

        /// <summary>
        /// Show CreationDate or not?
        /// </summary>
        bool CreationDate { get; }

        /// <summary>
        /// Show Copyright or not?
        /// </summary>
        bool Copyright { get; }

        /// <summary>
        /// Show Source or not?
        /// </summary>
        bool Source { get; }

        /// <summary>
        /// Show Matrix or not?
        /// </summary>
        bool Matrix { get; }

        /// <summary>
        /// Show Database or not?
        /// </summary>
        bool Database { get; }

        /// <summary>
        /// List with enabled Information types
        /// </summary>
        /// <returns></returns>
        List<PCAxis.Enums.InformationType> GetSelectedInformationTypes();
    }
}
