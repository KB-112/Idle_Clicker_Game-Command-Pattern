using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;


namespace IdleClicker
{
    public class BounceEffectFunction : MonoBehaviour
    {
        public DebuggerName debuggerName;

        public void BouncyEffect(string buttonName,List<Button> buttonList,RectTransform targetTransform, bool isAnimating, Vector3 orgScale,Vector3 newScale,float scaleDuration, Action onComplete)
        { 
            
            if(buttonList.Any(n => n.name ==  buttonName))
            {
                
                //debuggerName.bounceDebuggerCommand = () => Debug.Log("Debug,Test");
                //debuggerName.StoreDebuggerCommand(DebuggerNameList.BOUNCE_DEBUGGER);
                if (isAnimating || targetTransform == null)
                {
                    onComplete?.Invoke();
                    return;
                }

                isAnimating = true;

                Sequence seq = DOTween.Sequence();
                seq.Append(targetTransform.DOScale(newScale, scaleDuration / 2).SetEase(Ease.OutQuad));
                seq.Append(targetTransform.DOScale(orgScale, scaleDuration / 2).SetEase(Ease.InQuad));
                seq.OnComplete(() =>
                {
                    isAnimating = false;
                    onComplete?.Invoke();
                });
            }
        }
    }
}
