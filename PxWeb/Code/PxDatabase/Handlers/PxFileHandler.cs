using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using PCAxis.Paxiom;

namespace PXWeb.Database
{
    /// <summary>
    /// Item handler for PC-Axis PX files
    /// </summary>
    public class PxFileHandler : IItemHandler
    {

        #region IItemHandler Members

        /// <summary>
        /// Return the priority which is set to 1
        /// </summary>
        public int Priority
        {
            get { return 1; }
        }

        /// <summary>
        /// Checks if it is a PC-Axis PX file
        /// </summary>
        /// <param name="path"></param>
        /// <returns>If the path ends with .px it will return true otherwise false</returns>
        public bool CanHandle(string path)
        {
            return path.EndsWith(".px", StringComparison.InvariantCultureIgnoreCase);
        }

        /// <summary>
        /// Reads the metadata part of a px file.
        /// </summary>
        /// <param name="path">The path to the file</param>
        /// <returns>
        /// Return a PCAxis.Paxiom.PXMeta object containing the metadata 
        /// read from path
        /// </returns>
        public object Handle(string path, DatabaseLogger logger)
        {
            try
            {
                PCAxis.Paxiom.IPXModelBuilder builder = new PXFileBuilder();
                builder.SetPath(path);
                builder.BuildForSelection();
                if (builder.Errors.Count > 0)
                { 
                    for (int i = 0; i < builder.Errors.Count; i++)
			        {
                        logger(new DatabaseMessage() { MessageType = DatabaseMessage.BuilderMessageType.Error, Message = "PX file is corrupted " + path + " " + builder.Errors[i].Code });
                    }
                    return null;
                }
                if (builder.Warnings.Count > 0)
                {
                    for (int i = 0; i < builder.Warnings.Count; i++)
                    {
                        logger(new DatabaseMessage() { MessageType = DatabaseMessage.BuilderMessageType.Warning, Message = "PX file " + path + " " + builder.Warnings[i].Code });
                    }
                    return null;
                }
                return builder.Model.Meta;
            }
            catch (PCAxis.Paxiom.PXException ex)
            {
                logger(new DatabaseMessage() { MessageType = DatabaseMessage.BuilderMessageType.Error, Message = ex.ToString() });
            }

            return null;
        }

        #endregion
    }
}
