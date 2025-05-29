using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace IdleClicker
{
    public class LogoBounceEffect : MonoBehaviour
    {
        public Vector3 orgScale, newScale;
        public float scaleDuration;
        public Transform targetTransform;
    
        void Start()
        {
            Sequence seq = DOTween.Sequence();
            seq.Append(targetTransform.DOScale(newScale, scaleDuration / 2).SetEase(Ease.OutQuad));
            seq.Append(targetTransform.DOScale(orgScale, scaleDuration / 2).SetEase(Ease.InQuad));
            seq.SetLoops(-1);

        }

       
    }
}
