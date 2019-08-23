using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PCAxis.Paxiom;

namespace PCAxis.Query.Serializers
{
    /// <summary>
    /// Interface for serializing save query operation
    /// </summary>
    public interface IOperationSerializer
    {
        /// <summary>
        /// Serializes a operationa seialization object
        /// </summary>
        /// <param name="obj">object to serialize</param>
        /// <returns>A WorkStep object</returns>
        WorkStep Serialize(object obj);

        /// <summary>
        /// Creates a operation description object out of an WorkStep
        /// </summary>
        /// <param name="step">the workStep</param>
        /// <returns>A operation description object</returns>
        object Deserialize(WorkStep step);

        /// <summary>
        /// Creates the corrsponding operations object.
        /// </summary>
        /// <returns>corrsponding operations object</returns>
        IPXOperation CreateOperation();
        

    }
}
