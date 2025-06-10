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

    public interface IApiCommand
    {
        IEnumerator Execute(ApiHolder apiHolder, string url, string jsonPayload = null);
    }


    public interface IDebugger
    {
        void RunDebuggerCommand(DebuggerNameList debuggerNameList);
    }

    [Flags]
    public enum DebuggerNameList
    {
        NONE = 0,
        API_DEBUGGER =1 ,
        BOUNCE_DEBUGGER =2
       


    };



}
