using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PCAxis.Metadata
{
    /// <summary>
    /// Interface for generating links via META-ID to metadata systems
    /// </summary>
    public interface IMetaIdProvider
    {
        /// <summary>
        /// Method to initialize the Meta id provider
        /// </summary>
        /// <param name="configurationFile">Configuration file used for initialization</param>
        /// <returns>True if the provider was successfully initialized, else false</returns>
        bool Initialize(string configurationFile);
        
        /// <summary>
        /// Available metadata systems
        /// </summary>
        MetadataSystem[] MetadataSystems { get; }

        /// <summary>
        /// Get links on table level to metadata systems for the specified META-ID and language
        /// </summary>
        /// <param name="metaId">META-ID for a table</param>
        /// <param name="language">Language</param>
        /// <returns>Array of MetaLink objects</returns>
        MetaLink[] GetTableLinks(string metaId, string language);

        /// <summary>
        /// Get links on variable level to metadata systems for the specified META-ID and language
        /// </summary>
        /// <param name="metaId">META-ID for a variable</param>
        /// <param name="language">Language</param>
        /// <returns>Array of MetaLink objects</returns>
        MetaLink[] GetVariableLinks(string metaId, string language);

        /// <summary>
        /// Get links on value level to metadata systems for the specified META-ID and language
        /// </summary>
        /// <param name="metaId">META-ID for a value</param>
        /// <param name="language">Language</param>
        /// <returns>Array of MetaLink objects</returns>
        MetaLink[] GetValueLinks(string metaId, string language);
    }
}
