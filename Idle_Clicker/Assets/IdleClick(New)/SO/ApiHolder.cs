using System;
using System.Collections.Generic;
using System.Linq;
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
        public MainPlayerData playerData;
       

        public void FetchPlayerInfo()
        {
           
            string currentUser = PlayerPrefs.GetString("User_Input");
           // Debug.Log($"[GlobalUserData] CurrentUser from PlayerPrefs: '{currentUser}'");

            foreach (var entry in onSuccess.template)
            {
               // Debug.Log($"[GlobalUserData] Template Entry: '{entry.User_Name}', Score: {entry.score},TotalBalance : {entry.TotalBalance}, ID: {entry.id}");
            }

            var matchedEntry = onSuccess.template
                .FirstOrDefault(t => t.User_Name.Trim().ToLower() == currentUser.Trim().ToLower());

            if (matchedEntry != null)
            {
                SetPlayerData(matchedEntry);
            }
            else
            {
               // Debug.LogWarning("Matched entry is null — no matching user found.");
               
            }
        }

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
                    List<Template> templateList = new List<Template>();

                    if (json.TrimStart().StartsWith("["))
                    {
                        
                        templateList = JsonConvert.DeserializeObject<List<Template>>(json);
                    }
                    else
                    {
                        
                        Template singleTemplate = JsonConvert.DeserializeObject<Template>(json);
                        templateList.Add(singleTemplate);
                    }

                    if (templateList.Count > 0)
                    {
                        onSuccess.template.AddRange(templateList);
                     //   Debug.Log($"onSuccess.template now has {onSuccess.template.Count} total item(s)");
                        FetchPlayerInfo();
                    }
                    else
                    {
                        Debug.LogWarning("No items found in template list.");
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



        public void SetPlayerData(Template template)
        {
            if (template == null)
            {
                Debug.LogWarning("SetPlayerData: Template is null");
                return;
            }

            playerData.User_Name = template.User_Name;
            playerData.Score = template.score;
            playerData.id = template.id;
            playerData.OnIdleUpgradeLevel = template.OnIdleUpgradeLevel;
            playerData.OnTapUpgradeLevel = template.OnTapUpgradeLevel;
            playerData.TotalBalance = template.TotalBalance;

          //  Debug.Log($"[SetPlayerData] Name={playerData.User_Name}, Score={playerData.Score}, ID={playerData.id}, Idle={playerData.OnIdleUpgradeLevel}, Tap={playerData.OnTapUpgradeLevel} , TotalBalnce = {playerData.TotalBalance}");
        }

    
       
    }

    [System.Serializable]
    public class Template
    {
        public string User_Name;
        public int score;
        public int id;
        public int OnTapUpgradeLevel;
        public int OnIdleUpgradeLevel;
        public int TotalBalance;
    }

   
    [Serializable]
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
