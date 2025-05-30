using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.IO;

namespace IdleClicker
{
    public class ProfileLoader : MonoBehaviour
    {
        public int profileSize;
        public RawImage profileImage;
        public Button profileButton;
        public TextMeshProUGUI playerName;

        public Button goButton;
        public List<GameObject> menuHandling;
        public Image fadeBg;
        public float timeElapsed;
        public float duration = 1f;
        public float lerpValue =1f;
        Vector4 fadeAlpha ;



        private string imgName = "IdleClicker";
        private string savedImageKey = "ProfileImagePath";

        void Start()
        {
            if (profileImage == null)
            {
                Debug.LogError("Profile Image is not assigned!");
            }
            fadeAlpha = fadeBg.color;

            goButton.onClick.AddListener(AfterClickOnGoButton);
            Init();
        }


       
        IEnumerator FadeInit()
        {
            while (lerpValue != 0)
            {
                if (timeElapsed < duration)
                {
                    float t = timeElapsed / duration;
                    lerpValue = Mathf.Lerp(1, 0, t);
                    timeElapsed += Time.deltaTime;
                }
                else
                {
                    lerpValue = 0f;
                }

                fadeAlpha.w = lerpValue;
                fadeBg.color = fadeAlpha;

                yield return null;
                Debug.Log("Next Process");
            }
        }

        void AfterClickOnGoButton()
        {
         
            StartCoroutine(FadeInit());
        }

      
        void Init()
        {
            profileButton.onClick.AddListener(() => PickImage(profileSize));

            // Load saved image if it exists
            string savedPath = PlayerPrefs.GetString(savedImageKey, string.Empty);
            if (!string.IsNullOrEmpty(savedPath) && File.Exists(savedPath))
            {
                byte[] imageBytes = File.ReadAllBytes(savedPath);
                Texture2D texture = new Texture2D(2, 2);
                texture.LoadImage(imageBytes);
                profileImage.texture = texture;

                Debug.Log("Loaded saved profile image from: " + savedPath);
            }
        }

        private void PickImage(int maxSize)
        {
            NativeGallery.GetImageFromGallery((path) =>
            {
                if (string.IsNullOrEmpty(path))
                {
                    Debug.LogError("No image selected or invalid path.");
                    return;
                }

                Debug.Log("Image path: " + path);
                Texture2D texture = NativeGallery.LoadImageAtPath(path, maxSize,false);

                if (texture == null)
                {
                    Debug.LogError("Couldn't load texture from " + path);
                }
                else
                {
                    profileImage.texture = texture;
                    Debug.Log("Profile image updated successfully.");

                    SaveImageLocally(texture);

                    // (Optional) Also save to gallery for external access
                    string galleryFolderName = PlayerPrefs.GetString("Img_Name", "IdleClicker");
                    NativeGallery.SaveImageToGallery(texture, galleryFolderName, $"{playerName.text}.png");
                }
            });
        }

        void SaveImageLocally(Texture2D texture)
        {
            byte[] imageBytes = texture.EncodeToPNG();
            string filePath = Path.Combine(Application.persistentDataPath, $"{imgName}.png");
            // To  Texture Readable 
            File.WriteAllBytes(filePath, imageBytes);

            PlayerPrefs.SetString(savedImageKey, filePath);
            PlayerPrefs.Save();

            Debug.Log("Image saved locally at: " + filePath);
        }
    }
}
