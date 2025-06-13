using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;


namespace IdleClicker
{
  
    public class PanelSwitcherFunction : MonoBehaviour
    {
       
        public void UpdateDescription(PanelSwitcherConfig panelSwitcherConfig,string buttonAvailableToPlayer)
        {
            var panel = panelSwitcherConfig;
         


            if (panel.buttonName.Any(n => n == buttonAvailableToPlayer))
            {

                if (panel.panelStatusText != null)
                    panel.panelStatusText.text = buttonAvailableToPlayer.Split('_')[0];

                string statusStr = buttonAvailableToPlayer.Split('_')[0];

                foreach (GameObject go in panel.panelOption)
                {
                    if (go == null) continue;


                    bool match = go.name.Split('_')[0] == statusStr;
                    go.SetActive(match);
                }

                if (statusStr == "Rank")
                {
                    panel.scrollRect.content = panelSwitcherConfig.verticalLayoutGroup[0].GetComponent<RectTransform>();

                    panel.verticalLayoutGroup[0].padding.top = 120;

                    panel.scrollRect.verticalNormalizedPosition = 1f;


                }
                if (statusStr == "Shop")
                {
                    panel.scrollRect.content = panelSwitcherConfig.verticalLayoutGroup[1].GetComponent<RectTransform>();
                    panel.verticalLayoutGroup[1].padding.top = 200;
                    panel.scrollRect.verticalNormalizedPosition = 1f;


                }
            }

        }
    }




    [System.Serializable]
    public class PanelSwitcherConfig
    {
        public string[] buttonName;
        public TextMeshProUGUI panelStatusText;
        public List<GameObject> panelOption;
        public ScrollRect scrollRect;
        
        public List<VerticalLayoutGroup> verticalLayoutGroup;
        public ApiHolder apiHolder;


    }

}
