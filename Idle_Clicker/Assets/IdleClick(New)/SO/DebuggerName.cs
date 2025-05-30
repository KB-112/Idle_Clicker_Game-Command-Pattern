using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IdleClicker
{
    [CreateAssetMenu(fileName = "DebugCont", menuName = "ContainerList/DebuggerNameCont", order = 1)]
    public class DebuggerName : ScriptableObject,IDebugger
    {
        public DebuggerNameList manualDebuggerAssigned;
        public delegate void DebugCommand();
        public DebugCommand apiDebuggerCommand,bounceDebuggerCommand;
      

        public void StoreDebuggerCommand(DebuggerNameList inputType )
        {
            if (manualDebuggerAssigned == inputType)
            {
                if((inputType & DebuggerNameList.API_DEBUGGER)!=0)
                {
                    apiDebuggerCommand?.Invoke();

                }
                if ((inputType & DebuggerNameList.BOUNCE_DEBUGGER) != 0)
                {
                    bounceDebuggerCommand?.Invoke();

                }
                if ((inputType & DebuggerNameList.NONE) == 0)
                {
                    Debug.Log("No debugger assigned.");
                }
               
            }
        }
    }
}
