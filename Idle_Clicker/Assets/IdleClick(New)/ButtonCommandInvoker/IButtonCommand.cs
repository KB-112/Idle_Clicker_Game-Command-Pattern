using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace IdleClicker
{
    public interface IButtonCommander
    {
         void StoreButtonListenerCommand();

    }


  

    public interface IDebugger
    {
        void StoreDebuggerCommand(DebuggerNameList debuggerNameList);
    }

    [Flags]
    public enum DebuggerNameList
    {
        API_DEBUGGER =1 ,
        BOUNCE_DEBUGGER =2,
        NONE=0


    };



}
