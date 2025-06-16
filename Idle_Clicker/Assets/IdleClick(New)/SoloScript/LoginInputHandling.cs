using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace IdleClicker
{
    public class LoginInputHandling : MonoBehaviour
    {
        [Header("UI Components")]
        public TMP_InputField nameInputField;
        public TextMeshProUGUI previewNameText;

        [Header("Login Settings")]
        public int requiredNameLength = 4;
        public ApiCommandExecutor apiExecutor;
        public MainPlayerData mainPlayerData;
        public static Action OnNameEntered;

        private bool hasNameSubmitted = false;

        public Button okButton;
        private void Start()
        {
            ApplyInputConstraints();
            AssignNameOnInitialize();
           
            okButton.onClick.AddListener( () => CreateNewUser(nameInputField.text.ToString()));
        }


        void AssignNameOnInitialize()
        {
            string savedName = PlayerPrefs.GetString("User_Input");
            if (!string.IsNullOrEmpty(savedName))
            {
                nameInputField.text = savedName;
            }
        }

        private void Update()
        {
            HandleLiveInputValidation();
        }

        private void ApplyInputConstraints()
        {
            nameInputField.characterLimit = requiredNameLength;
            nameInputField.contentType = TMP_InputField.ContentType.Name;
        }

        private void HandleLiveInputValidation()
        {
            previewNameText.text = nameInputField.text;

            bool isNameValid = nameInputField.text.Length == requiredNameLength;

            if (!hasNameSubmitted && isNameValid)
            {
                hasNameSubmitted = true;
                SubmitName();
            }
            else if (hasNameSubmitted && !isNameValid)
            {
                hasNameSubmitted = false;
            }
        }

        private void SubmitName()
        {
            OnNameEntered?.Invoke();             
        }

        public void CreateNewUser(string userName)
        {
            PlayerPrefs.SetString("User_Input", userName);
            PlayerPrefs.Save();
            string savedName = PlayerPrefs.GetString("User_Input");

            if (nameInputField.text.Length == requiredNameLength)
            {


                mainPlayerData.User_Name = savedName;
                mainPlayerData.Score = 0;
                mainPlayerData.OnTapUpgradeLevel = 0;
                mainPlayerData.OnIdleUpgradeLevel = 0;
                mainPlayerData.TotalBalance = 100;
              
            }
            if(!string.IsNullOrEmpty(mainPlayerData.User_Name))
            {
                Debug.Log($"User_Name : {mainPlayerData.User_Name}");
                string jsonData = JsonUtility.ToJson(mainPlayerData);
                apiExecutor.ExecuteCommand(new PostCommand(), "https://6824498265ba05803399a0a2.mockapi.io/api/v1/User_Name", jsonData);
            }
              

                
            
        }
    }
}