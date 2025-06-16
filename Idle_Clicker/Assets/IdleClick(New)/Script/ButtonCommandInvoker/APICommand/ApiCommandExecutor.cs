using System.Collections;
using UnityEngine;
using System;

namespace IdleClicker
{
    public class ApiCommandExecutor : MonoBehaviour
    {
        public ApiHolder apiHolder;
      

        public void ExecuteCommand(IApiCommand command, string url, string jsonPayload = null)
        {
            apiHolder.onSuccess.template.Clear();
          //  Array.Clear(apiHolder.onSuccess.template,0, apiHolder.onSuccess.template.Length);
          
            apiHolder.onFailure.error.Clear();
            StartCoroutine(command.Execute(apiHolder, url, jsonPayload));
        }
    }
}
