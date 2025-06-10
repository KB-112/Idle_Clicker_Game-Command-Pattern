using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace IdleClicker
{
    [CreateAssetMenu(fileName = "ApiCont", menuName = "ContainerList/APICont", order = 1)]
    public class ApiHolder : ScriptableObject
    {
        public OnSuccess onSuccess;
        public OnFailure onFailure;

        public void CheckStatusReq(UnityWebRequest req)
        {
            if (req.result == UnityWebRequest.Result.Success)
            {
                try
                {
                    // Deserialize into the wrapper class
                    TemplateListWrapper wrapper = JsonUtility.FromJson<TemplateListWrapper>(req.downloadHandler.text);

                    if (wrapper != null && wrapper.template != null)
                    {
                        onSuccess.template = wrapper.template;
                        onSuccess.template.AddRange(wrapper.template);
                        foreach (var t in onSuccess.template)
                        {
                            Debug.Log($"Player id: {t.id}, Name: {t.Name}, Status: {t.status}");
                        }
                    }
                    else
                    {
                        Debug.LogWarning("Parsed wrapper or template list is null.");
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError($"Deserialization failed: {e.Message}");
                }
            }
            else
            {
                onFailure.error.Add($"Request failed: {req.error}");
                
               
            }
        }
    }

    [Serializable]
    public class Template
    {
        public int id;
        public string Name;
        public string status;
    }

    [Serializable]
    public class TemplateListWrapper
    {
        public List<Template> template;
    }

    [Serializable]
    public class OnSuccess
    {
        public List<Template> template;
    }

    [Serializable]
    public class OnFailure
    {
        public List<string> error;
    }
}
