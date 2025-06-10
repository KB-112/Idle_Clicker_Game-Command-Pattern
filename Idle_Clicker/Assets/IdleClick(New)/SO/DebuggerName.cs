using System;
using UnityEngine;

namespace IdleClicker
{
    [CreateAssetMenu(fileName = "DebuggerProfile", menuName = "ContainerList/Debugger Profile", order = 1)]
    public class DebuggerName : ScriptableObject, IDebugger
    {
        [Header("Assigned Debugger Flags")]
        public DebuggerNameList manualDebuggerAssigned = DebuggerNameList.NONE;
        public delegate void DebugCommand();

        [Header("Assigned Debugger Commands")]
        public DebugCommand apiDebuggerCommand;
        public DebugCommand bounceDebuggerCommand;

        public void RunDebuggerCommand(DebuggerNameList inputType)
        {
            // Skip if there is no debugger assigned
            if (inputType == DebuggerNameList.NONE)
            {
                Debug.Log("No debugger input received.");
                return;
            }

            // Only respond to matching flags
            if ((manualDebuggerAssigned & inputType) != 0)
            {
                if ((inputType & DebuggerNameList.API_DEBUGGER) != 0)
                {
                    apiDebuggerCommand?.Invoke();
                }

                if ((inputType & DebuggerNameList.BOUNCE_DEBUGGER) != 0)
                {
                    bounceDebuggerCommand?.Invoke();
                }
            }
            else
            {
                Debug.LogWarning($"Debugger [{name}] does not match requested flags: {inputType}");
            }
        }
    }

}
