using System;
namespace PCAxis.Sql.QueryLib_21 {
    public interface IMetaVersionComparator {
        /// <summary>
        /// To determin if a feature( table/ column) should exists.
        /// </summary>
        /// <param name="featureFromVersion"></param>
        /// <returns>True if metaVersion is Greater than or equal to featureFromVersion</returns>
        bool metaVersionGE(string featureFromVersion);

        /// <summary>
        /// To determin if a feature( table/ column) should exists.
        /// </summary>
        /// <param name="featureToVersion"></param>
        /// <returns>True if metaVersion is Less than or equal to featureToVersion</returns>
        bool metaVersionLE(string featureToVersion);
    }
}
