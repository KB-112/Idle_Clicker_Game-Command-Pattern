using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace IdleClicker
{
  
    public class PanelSwitcherFunction : MonoBehaviour
    {
       
        public void UpdateDescription(PanelSwitcherConfig panelSwitcherConfig,string buttonAvailableToPlayer)
        {
            var panel = panelSwitcherConfig;
            if (panel.panelStatusText != null)
                panel.panelStatusText.text = buttonAvailableToPlayer.Split('_')[0];

            string statusStr = buttonAvailableToPlayer.Split('_')[0];

            foreach (GameObject go in panel.panelOption)
            {
                if (go == null) continue;

              
                bool match = go.name.Split('_')[0] == statusStr;
                go.SetActive(match);
            }
            if(statusStr == "Stat")
            {
                panel.scrollRect.vertical = false;
                panel.verticalLayoutGroup.padding.top = 883;
            }
            
            else
            {
                if (statusStr == "Rank")
                {
                    panel.verticalLayoutGroup.padding.right = -100;
                    panel.verticalLayoutGroup.padding.top = 200;
                    panel.verticalLayoutGroup.padding.bottom = panel.apiHolder.onSuccess.template.Count * 148;
                    panel.scrollRect.verticalNormalizedPosition = 1f;
                }

                panel.scrollRect.movementType = ScrollRect.MovementType.Clamped;
            }


        }
    }




    [System.Serializable]
    public class PanelSwitcherConfig
    {
        public TextMeshProUGUI panelStatusText;
        public List<GameObject> panelOption;
        public ScrollRect scrollRect;
        
        public VerticalLayoutGroup verticalLayoutGroup;
        public ApiHolder apiHolder;


    }

}
