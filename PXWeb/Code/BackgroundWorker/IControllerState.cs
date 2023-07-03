using System;
using System.Collections.Generic;

namespace PxWeb.Code.BackgroundWorker
{
    public interface IControllerState
    {
        /// <summary>
        /// Indicate that the controller has begun working
        /// </summary>
        public void Begin();

        /// <summary>
        /// Indicate that the controller has finished working
        /// </summary>
        public void End();

        /// <summary>
        /// Add an event to the current controller state
        /// </summary>
        public void AddEvent(Event e);
    }
    public enum ControllerStateType
    {
        NotRun,
        Running,
        Finished
    }
}
