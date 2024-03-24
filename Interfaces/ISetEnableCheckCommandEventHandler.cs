using System;

namespace MyMasterMind.Interfaces
{
    /// <summary>
    /// is used to enable the check command, if all four colors are entered
    /// </summary>
    public interface ISetEnableCheckCommandEventHandler
    {
        void SetEnableCheckCommandEventHandler(EventHandler enableCheckCommandEventHandler);
    }
}
