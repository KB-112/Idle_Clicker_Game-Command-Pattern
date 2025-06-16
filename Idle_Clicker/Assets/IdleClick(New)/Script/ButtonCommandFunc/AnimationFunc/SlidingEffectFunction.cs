using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using UnityEngine.UI;

namespace IdleClicker
{
    public class SlidingEffectFunction : MonoBehaviour
    {
        private Vector2? originalPosition = null;  // Save only once

        public void SlideEffect(string buttonName, List<Button> buttonList, RectTransform rectTransform, float distanceCover, float slidingSpeed, bool snap, bool isActive, GameObject panel, Action onComplete)
        {
           // Debug.Log("Command calling for Sliding function....");

            if (buttonList.Any(n => n.name == buttonName))
            {
                if(isActive)
                {
                    panel.SetActive(isActive);

                }
               

                if (rectTransform == null)
                {
                    onComplete?.Invoke();
                    return;
                }

                // Save original position only once
                if (originalPosition == null)
                {
                    originalPosition = rectTransform.anchoredPosition;
                }

             //   Debug.Log($"Button Registered for Panel, Panel status: {isActive}");

                float targetY = originalPosition.Value.y + distanceCover;
                if (!isActive)
                {
                    Sequence slideSequence = DOTween.Sequence();
                    slideSequence.Insert(slidingSpeed - 0.5f,
                        DOVirtual.DelayedCall(0f, () => panel.SetActive(false)));
                }



                rectTransform.DOAnchorPos3DY(targetY, slidingSpeed, snap)
                             .SetLoops(1, LoopType.Yoyo)
                             .OnComplete(() =>
                             {
                                 onComplete?.Invoke();
                                
                             });
               
            }
        }
    }
}
