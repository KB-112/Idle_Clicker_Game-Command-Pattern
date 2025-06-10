using UnityEngine.Networking;
using System.Collections;
using System.Text;

namespace IdleClicker
{
    public class PutCommand : IApiCommand
    {
        public IEnumerator Execute(ApiHolder apiHolder, string url, string jsonPayload = null)
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonPayload);
            using UnityWebRequest request = new UnityWebRequest(url, "PUT");
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();
            apiHolder.CheckStatusReq(request);
        }
    }
}
