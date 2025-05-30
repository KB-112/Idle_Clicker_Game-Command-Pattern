using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace IdleClicker
{
    [CreateAssetMenu(fileName = "ApiCont", menuName = "ContainerList/APICont", order = 1)]
    public class ApiHolder : ScriptableObject
    {
        public const string apiURL = "https://6824498265ba05803399a0a2.mockapi.io/api/v1/";
        public const int timeOut = 10;
        public delegate void  StatementList(string statement);
        StatementList statementList;
     
        public DebuggerName apiDebuggerName;
        public enum ApiType
        {
           PUT,
           GET,
           POST
            
        };

        public enum ReqType
        { 
            User_Name 
        
        };


        public ApiType apiType;
        public ReqType urlReqType;

        public void CreateReq( string apiMethod, string jsonData, string reqType)
        {
           

         

            if(!string.IsNullOrEmpty(apiMethod) || !string.IsNullOrEmpty(jsonData) || !string.IsNullOrEmpty(reqType))
            {

                DebugRecorder($"  Null request Paramter Passed ");
            }
            else
            {
                urlReqType = ReqType.User_Name;
                reqType = urlReqType.ToString();
                string url = apiURL + reqType;
                UnityWebRequest unityWebRequest = new UnityWebRequest(url, apiMethod);
                unityWebRequest.SetRequestHeader("Content-Type", "application/json");

                DebugRecorder($" ");
            }
        }

        void SelectReqType(ReqType urlReqType , ApiType apiTypes)
        {




            
        }




        void DebugRecorder(string statement)
        {
            Debug.Log( statement);
        }



       

        public void StoreDebuggerCommand(List<string> debuggerList)
        {

            if(debuggerList.Contains(apiDebuggerName.ToString()))
            {
                statementList = DebugRecorder;
            }

           
        }
    }
}
