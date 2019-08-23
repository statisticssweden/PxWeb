using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PCAxis.Metadata
{
    /// <summary>
    /// Class representing a metadata system
    /// </summary>
    public class MetadataSystem
    {
        /// <summary>
        /// Identity of the metadata system
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// Name of the metadata system
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="id">System identity</param>
        /// <param name="name">System name</param>
        public MetadataSystem(string id, string name)
        {
            this.ID = id;
            this.Name = name;
        }
    }
}
