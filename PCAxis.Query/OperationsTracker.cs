using PCAxis.Query.Serializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PCAxis.Paxiom.Operations;

namespace PCAxis.Query
{
    /// <summary>
    /// Tracks made operations 
    /// </summary>
    public class OperationsTracker
    {
        private List<WorkStep> _steps;
  

        public OperationsTracker()
        {
            _steps = new List<WorkStep>();
        }

        public OperationsTracker(WorkStep[] workStep)
        {
            _steps = new List<WorkStep>();
            _steps.AddRange(workStep);
        }

        public bool RemoveLastStep()
        {
            _steps = _steps.Take(_steps.Count() - 1).ToList();

            return true;
        }
        
        /// <summary>
        /// Resets the list of operations
        /// </summary>
        public void Reset()
        {
           _steps.Clear();
        }

        public bool AddStep(string type, object opDescription)
        { 
            IOperationSerializer ser = null;
            
            ser = CreateSerializer(type);

            _steps.Add(ser.Serialize(opDescription));

            return true;
        }

        /// <summary>
        /// Indicates that operations where text can be changes has been executed.
        /// </summary>
        public bool IsUnsafe { get; set; }

        /// <summary>
        /// Indicates that an operation is dependent on time
        /// </summary>
        public bool IsTimeDependent { get; set; }

        public static IOperationSerializer CreateSerializer(string type)
        {
            IOperationSerializer ser = null;

            if (type == OperationConstants.PIVOT)
            {
                ser = new PivotSerializer();
            }
            else if (type == OperationConstants.PIVOT_CW)
            {
                ser = new PivotCWSerializer();
            }
            else if (type == OperationConstants.PIVOT_CCW)
            {
                ser = new PivotCCWSerializer();
            }
            else if (type == OperationConstants.PER_PART)
            {
                ser = new CalculatePerPartSerializer();
            }
            else if (type == OperationConstants.CHANGE_VALUE_ORDER)
            {
                ser = new ChangeValueOrderSerializer();
            }
            else if (type == OperationConstants.DELETE_VALUE)
            {
                ser = new DeleteValueSerializer();
            }
            else if (type == OperationConstants.DELETE_VARIABLE)
            {
                ser = new DeleteVariableSerializer();
            }
            else if (type == OperationConstants.SORT_TIME)
            {
                ser = new SortTimeVariableSerializer();
            }
            else if (type == OperationConstants.SPLIT_TIME)
            {
                ser = new SplitTimevariableSerializer();
            }
            else if (type == OperationConstants.SUM)
            {
                ser = new SumSerializer();
            }
            else if (type == OperationConstants.CHANGE_DECIMALS)
            {
                ser = new ChangeDecimalsSerializer();
            }
            else if (type == OperationConstants.CHANGE_TEXT)
            {
                ser = new ChangeTextSerializer();
            }
            else if (type == OperationConstants.PIVOT_TIME_TO_HEADING)
            {
                ser = new PivotTimeToHeadingSerializer();
            }
            else if (type == OperationConstants.CHANGE_TEXT_CODE_PRESENTATION)
            {
                ser = new ChangeTextCodePresentationSerializer();
            }
            
            return ser;
        }

        public WorkStep[] GetSteps()
        {
            return _steps.ToArray();
        }
    }
}
