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
        public TextMeshProUGUI playerName;
        private string imgName = "IdleClicker";
        private string savedImageKey = "ProfileImagePath";
        public Button profileButton;

        void Start()
        {
            if (profileImage == null)
            {
                Debug.LogError("Profile Image is not assigned!");
            }
           

        
            Init();
            profileButton.onClick.AddListener(() => PickImage(profileSize));
        }


       
     

      

      
        void Init()
        {
           

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
