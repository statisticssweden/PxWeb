using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PXWeb.Database
{
    /// <summary>
    /// Handler interface the DatabaseSpider. The DatabaseSpider uses 
    /// implementation of handlers to handle diffrent file types
    /// </summary>
    public interface IItemHandler
    {
        /// <summary>
        /// The Priority of the handle a smaller number gives a higher 
        /// priority over other handlers that handles the same type
        /// </summary>
        int Priority { get; }

        /// <summary>
        /// Checks to see if it can handle the file by inspecting the file name
        /// and/or file content and/or other refrence
        /// </summary>
        /// <param name="path">the path to the file that should be handeled</param>
        /// <returns>true if it is able to handle the file otherwise false</returns>
        bool CanHandle(string path);

        /// <summary>
        /// Handles tha file
        /// </summary>
        /// <param name="path">Path to the file to handle</param>
        /// <param name="logger">Callback method to log handler interactions</param>
        /// <returns>A information object that holds information read from the file</returns>
        object Handle(string path, DatabaseLogger logger);
    }

}
