using IdleClicker.UITween;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace IdleClicker
{
    public class UIPanelManager : MonoBehaviour, IButtonCommand
    {
        [Header("Close Button")]
        [SerializeField] private GameObject panel;
        [SerializeField] private UIAnimation uiAnimationClose;
        [SerializeField] private List<Button> buttonList;
        [Header("Buttons to Animate")]
        public RectTransform rectAnimate;

        public void Execute(string name)
        {
            OnStateChanged(name);
        }


        public void OnStateChanged(string buttonName)
        {

            if (buttonList.Any(b => b.name == buttonName))
            {
                Debug.Log($"Button pressed Panel: {buttonName}");
                PanelInit();
            }


        }

        private void PanelInit()
        {
            panel.SetActive(true);
            uiAnimationClose.ApplyEffect("Panel", rectAnimate);
            Debug.Log("Any button pressed");
        }

        public void CloseButton(string name)
        {
            uiAnimationClose.ApplyEffect(name, rectAnimate, () => { panel.SetActive(false); });
        }
    }
}
