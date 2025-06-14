using System;
using TMPro;
using UnityEngine;

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

        public static Action OnNameEntered;

        private bool hasNameSubmitted = false;

        private void Start()
        {
            ApplyInputConstraints();

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
                SubmitName(nameInputField.text);
            }
            else if (hasNameSubmitted && !isNameValid)
            {
                hasNameSubmitted = false;
            }
        }

        private void SubmitName(string userName)
        {
            OnNameEntered?.Invoke();
            string savedName = PlayerPrefs.GetString("User_Input");
            if (string.IsNullOrEmpty(savedName))
            {
                string jsonPayload = $"{{\"User_Name\":\"{userName}\", \"Score\":0}}";

                apiExecutor.ExecuteCommand(new PostCommand(), "https://6824498265ba05803399a0a2.mockapi.io/api/v1/User_Name", jsonPayload);

                PlayerPrefs.SetString("User_Input", userName);
                PlayerPrefs.Save();
            }
        }
    }
}
