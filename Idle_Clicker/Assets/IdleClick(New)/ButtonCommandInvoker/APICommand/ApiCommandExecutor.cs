using System.Collections;
using UnityEngine;

namespace IdleClicker
{
    public class ApiCommandExecutor : MonoBehaviour
    {
        public ApiHolder apiHolder;

        public void ExecuteCommand(IApiCommand command, string url, string jsonPayload = null)
        { apiHolder.onSuccess.template.Clear();
            apiHolder.onFailure.error.Clear();
            StartCoroutine(command.Execute(apiHolder, url, jsonPayload));
        }
    }
}
