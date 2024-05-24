namespace PXWeb
{
    /// <summary>
    /// Interface for the General.Global settings
    /// </summary>
    public interface IGlobalSettings
    {
        /// <summary>
        /// If the content variable shall be removed if it only has one value
        /// </summary>
        bool RemoveSingleContent { get; }

        /// <summary>
        /// If strict check of aggregations shall be performed or not
        /// </summary>
        bool StrictAggregationCheck { get; }

        /// <summary>
        /// Secrecy option
        /// </summary>
        PCAxis.Paxiom.SecrecyOptionType SecrecyOption { get; }

        /// <summary>
        /// Rounding rule of decimal values
        /// </summary>
        PCAxis.Paxiom.RoundingType RoundingRule { get; }

        /// <summary>
        /// Text to display for measures having Data Symbol1
        /// </summary>
        string Symbol1 { get; }
        string Symbol2 { get; }
        string Symbol3 { get; }
        string Symbol4 { get; }
        string Symbol5 { get; }
        string Symbol6 { get; }

        /// <summary>
        /// Text to display for measures having Data Symbol Nil
        /// </summary>
        string DataSymbolNil { get; }

        /// <summary>
        /// Text to display for measures having Data Symbol Sum
        /// </summary>
        string DataSymbolSum { get; }

        ///// <summary>
        ///// String used as decimal separator
        ///// </summary>
        //string DecimalSeparator { get; }

        ///// <summary>
        ///// String used as thousand separator
        ///// </summary>
        //string ThousandSeparator { get; }

        /// <summary>
        /// If source description shall be shown or not
        /// </summary>
        bool ShowSourceDescription { get; }

        /// <summary>
        /// Level of information for table
        /// </summary>
        PCAxis.Paxiom.InformationLevelType TableInformationLevel { get; }

        /// <summary>
        /// If first character in values shall be uppercase or not
        /// </summary>
        bool Uppercase { get; }

        /// <summary>
        /// Where data notes shall be placed
        /// </summary>
        PCAxis.Paxiom.DataNotePlacementType DataNotePlacement { get; }

        /// <summary>
        /// ShowInformationTypes settings
        /// </summary>
        IShowInformationTypesSettings ShowInformationTypes { get; }

        /// <summary>
        /// if the link to the infofile shall appear under the information tab
        /// </summary>
        bool ShowInfoFile { get; }

    }
}
