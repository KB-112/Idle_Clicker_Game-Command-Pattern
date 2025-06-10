using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace IdleClicker
{
    public class LoginAnimationHandling : MonoBehaviour
    {
        [Header("UI References")]
        public Image backgroundFadeImage;
        public RectTransform profileImage;
        public Button confirmButton;
        public GraphicRaycaster raycaster;

        [Header("Animation Settings")]
        public float fadeDuration = 1f;
        public Vector2 profileTargetScale;
        public Vector2 profileTargetPosition;
        public float profileAnimationDuration = 0.5f;

        [Header("Move Down Elements")]
        public List<RectTransform> elementsToMoveDown;
        public Vector2 targetDownPosition;

        [Header("Other UI Elements")]
        public List<GameObject> componentsToHideAfterFade;
        public List<GameObject> menuGroupsToHideOnLoad;
        public ParticleSystem profileParticle;

        private Vector4 fadeColor;
        private float fadeElapsedTime = 0f;
        private float fadeLerpValue = 1f;

        private void Start()
        {
            if (!string.IsNullOrEmpty(PlayerPrefs.GetString("User_Input")))
            {
                Debug.Log("Saved Username: " + PlayerPrefs.GetString("User_Input"));

                fadeColor.w = 0f;
                ApplyPostFadeState();

                foreach (GameObject group in menuGroupsToHideOnLoad)
                    group.SetActive(false);

                profileImage.localScale = profileTargetScale;
                profileImage.localPosition = profileTargetPosition;
                profileParticle.Stop();
            }
            else
            {
                fadeColor = backgroundFadeImage.color;
            }
        }

        private void OnEnable()
        {
            LoginInputHandling.OnNameEntered += InitializeConfirmButton;
        }

        private void OnDisable()
        {
            LoginInputHandling.OnNameEntered -= InitializeConfirmButton;
        }

        private void InitializeConfirmButton()
        {
            confirmButton.onClick.AddListener(HandleConfirmButtonClick);
        }

        private void HandleConfirmButtonClick()
        {
            AnimateElementsDownward();
            AnimateProfileImage();
            StartCoroutine(FadeOutBackground());
        }

        private void AnimateProfileImage()
        {
            Sequence animation = DOTween.Sequence();
            animation.Append(profileImage.DOScale(profileTargetScale, profileAnimationDuration / 2).SetEase(Ease.InQuad));
            profileImage.DOAnchorPos(profileTargetPosition, profileAnimationDuration);
        }

        private void AnimateElementsDownward()
        {
            foreach (var element in elementsToMoveDown)
                element.DOAnchorPos(targetDownPosition, profileAnimationDuration);
        }

        private IEnumerator FadeOutBackground()
        {
            while (fadeLerpValue > 0f)
            {
                if (fadeElapsedTime < fadeDuration)
                {
                    fadeLerpValue = Mathf.Lerp(1f, 0f, fadeElapsedTime / fadeDuration);
                    fadeElapsedTime += Time.deltaTime;
                }
                else
                {
                    fadeLerpValue = 0f;
                    profileParticle.Stop();
                    ApplyPostFadeState();
                }

                fadeColor.w = fadeLerpValue;
                backgroundFadeImage.color = fadeColor;

                yield return null;
            }
        }

        private void ApplyPostFadeState()
        {
            foreach (GameObject obj in componentsToHideAfterFade)
                obj.SetActive(false);

            raycaster.blockingMask = LayerMask.GetMask("Nothing");
        }
    }
}
