using UnityEngine.Networking;
using System.Collections;

namespace IdleClicker
{
    public class GetCommand : IApiCommand
    {
        public IEnumerator Execute(ApiHolder apiHolder, string url, string jsonPayload = null)
        {
            using UnityWebRequest request = UnityWebRequest.Get(url);
            yield return request.SendWebRequest();
            apiHolder.CheckStatusReq(request);
        }
    }
}
