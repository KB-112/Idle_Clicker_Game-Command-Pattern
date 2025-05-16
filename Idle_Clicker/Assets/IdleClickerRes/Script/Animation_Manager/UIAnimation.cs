using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace IdleClicker.AM
{
    [CreateAssetMenu(fileName = "UIAnimData", menuName = "ScriptableObjects/UIAnimations", order = 1)]
    public class UIAnimation : ScriptableObject
    {
        [System.Serializable]
        public class UIEffectEntry
        {
            public string targetName;

            public bool shouldBounce = false;
            public bool shouldGlide = false;

            [Header("Bounce Effects")]
            public float scaleDuration = 0.5f;
            public Vector3 orgScale = Vector3.one;
            public Vector3 newScale = Vector3.one;
            [HideInInspector] public bool isAnimating = false;

            [Header("Glide Effects")]
            public float glideSpeed = 0.5f;
            public float distance;
            public bool snap = false;
        }

        [Header("Targets with Effects")]
        public List<UIEffectEntry> effects;

        public void ApplyEffect<T>(string name, T target, Action onComplete = null) where T : Component
        {
            if (target == null)
            {
                Debug.LogWarning($"Target is null for '{name}'");
                onComplete?.Invoke();
                return;
            }

            UIEffectEntry entry = effects.Find(e => e.targetName == name);

            if (entry == null)
            {
                Debug.LogWarning($"No animation entry found for '{name}'");
                onComplete?.Invoke();
                return;
            }

            RectTransform rectTransform = target.GetComponent<RectTransform>();

            if (entry.shouldBounce)
            {
                BouncyEffect(rectTransform, entry, onComplete);
            }
            else if (entry.shouldGlide)
            {
                GlideEffect(rectTransform, entry, onComplete);
            }
            else
            {
                onComplete?.Invoke(); // No animation set
            }
        }

        private void BouncyEffect(RectTransform targetTransform, UIEffectEntry entry, Action onComplete)
        {
            if (entry.isAnimating || targetTransform == null)
            {
                onComplete?.Invoke();
                return;
            }

            entry.isAnimating = true;

            Sequence seq = DOTween.Sequence();
            seq.Append(targetTransform.DOScale(entry.newScale, entry.scaleDuration / 2).SetEase(Ease.OutQuad));
            seq.Append(targetTransform.DOScale(entry.orgScale, entry.scaleDuration / 2).SetEase(Ease.InQuad));
            seq.OnComplete(() =>
            {
                entry.isAnimating = false;
                onComplete?.Invoke();
            });
        }

        private void GlideEffect(RectTransform rectTransform, UIEffectEntry entry, Action onComplete)
        {
            if (rectTransform == null)
            {
                onComplete?.Invoke();
                return;
            }

            rectTransform.DOAnchorPos3DY(rectTransform.anchoredPosition.y + entry.distance, entry.glideSpeed, entry.snap)
                         .SetLoops(1, LoopType.Yoyo)
                         .OnComplete(() =>
                         {
                             onComplete?.Invoke();
                         });
        }
    }
}
