using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;

namespace IdleClicker
{
    [CreateAssetMenu(fileName = "ApiCont", menuName = "ContainerList/APICont", order = 1)]
    public class ApiHolder : ScriptableObject
    {
        public OnSuccess onSuccess;
        public OnFailure onFailure;

        public void CheckStatusReq(UnityWebRequest req)
        {
            if (onSuccess.template == null)
                onSuccess.template = new List<Template>();
            if (onFailure.error == null)
                onFailure.error = new List<string>();

            if (req.result == UnityWebRequest.Result.Success)
            {
                string json = req.downloadHandler.text;
                Debug.Log(json);
                try
                {
               
                    // Deserialize into the wrapper class
                    List<Template> template = JsonConvert.DeserializeObject<List<Template>>(json);



                    if (template.Count > 0)
                    {
                        onSuccess.template.AddRange(template);
                        Debug.Log($"onSuccess.template now has {onSuccess.template.Count} total item(s)");
                    }
                    else
                    {
                        Debug.LogWarning("No items found in 'template' field.");
                    }
                   
                }
                catch (Exception e)
                {
                    Debug.LogError($"Deserialization failed: {e.Message}");
                }
            }
            else
            {
                if (onFailure.error == null)
                {
                    onFailure.error = new List<string>();
                }

                onFailure.error.Add($"Request failed: {req.error}");
            }
        }
    }

    [System.Serializable]
    public class Template
    {
        public string User_Name;
        public int score;
        public int id;
       
     
    }

   

    [System.Serializable]
    public class OnSuccess
    {
        public List<Template> template;
    }

    [System.Serializable]
    public class OnFailure
    {
        public List<string> error;
    }
}
