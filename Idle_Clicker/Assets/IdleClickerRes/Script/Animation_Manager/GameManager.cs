using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using IdleClicker.ScoreRef;

namespace IdleClicker.AM
{
    public class GameManager : UIStateObserver<Button>
    {
        [Header("Assign Buttons Here")]
        [SerializeField] private List<Button> buttons = new List<Button>();

        private readonly List<string> buttonNames = new List<string>();

        // Reference to ScriptableObject and UI components
        [SerializeField] private LeaderShipManagerSO leaderShipManagerSO;
        [SerializeField] private LeaderShipUIFields leaderShipUIFields;
        [SerializeField] private UIAnimationController uiAnimationController;
        [SerializeField] private UIPanelManager uiPanelManager;
        [SerializeField] private ScoreUpdation scoreUpdation;

        private void Awake()
        {
            // Initialize UIAnimationController
            if (uiAnimationController == null)
            {
                uiAnimationController = FindObjectOfType<UIAnimationController>();
                if (uiAnimationController == null)
                {
                    Debug.LogWarning("UIAnimationController not found in the scene! Please assign it in the Inspector or ensure it exists.");
                }
            }

            if (uiPanelManager == null)
            {
                uiPanelManager = FindObjectOfType<UIPanelManager>();
                if (uiPanelManager == null)
                {
                    Debug.LogWarning("UIPanelManager not found in the scene! Please assign it in the Inspector or ensure it exists.");
                }
            }

            if (scoreUpdation == null)
            {
                scoreUpdation = FindObjectOfType<ScoreUpdation>();
                if (scoreUpdation == null)
                {
                    Debug.LogWarning("ScoreUpdation not found in the scene! Please assign it in the Inspector or ensure it exists.");
                }
            }

            // Cache button names and register listeners
            CacheButtonNames();
            RegisterButtonListeners();

            // Initialize LeaderShipManagerSO with UI fields
            if (leaderShipManagerSO != null && leaderShipUIFields != null)
            {
                leaderShipManagerSO.Initialize(
                    leaderShipUIFields.iconName,
                    leaderShipUIFields.nameInputField,
                    leaderShipUIFields.leaderboardDisplay,
                    this
                );
            }
            else
            {
                Debug.LogWarning("LeaderShipManagerSO or LeaderShipUIFields reference not assigned in the Inspector!");
            }
        }

        private void CacheButtonNames()
        {
            buttonNames.Clear();

            foreach (Button btn in buttons)
            {
                if (btn != null)
                {
                    // Clean name: replace '_' with ' ', 'Button' with ' ', and trim spaces
                    string cleanName = btn.name.Replace("_", " ").Replace("Button", " ").Trim();
                    buttonNames.Add(cleanName);
                    Debug.Log($"Cached button: {cleanName} (Original: {btn.name})");
                }
                else
                {
                    Debug.LogWarning("Null button found in buttons list! Check Inspector assignments.");
                }
            }

            Debug.Log("Cached Button Names: " + string.Join(", ", buttonNames));
        }

        private void RegisterButtonListeners()
        {
            foreach (Button btn in buttons)
            {
                if (btn != null)
                {
                    Button capturedBtn = btn;
                    btn.onClick.RemoveAllListeners();
                    btn.onClick.AddListener(() => OnButtonClicked(capturedBtn));
                }
            }
        }

        private void OnButtonClicked(Button clickedButton)
        {
            if (clickedButton == null)
            {
                Debug.LogWarning("Clicked button is null!");
                return;
            }

            CurrentState = clickedButton;
            Debug.Log($"Button clicked: {clickedButton.name}");

            // Dynamically set iconName with clean button name
            if (leaderShipUIFields != null && leaderShipUIFields.iconName != null)
            {
                string cleanButtonName = clickedButton.name.Replace("_", " ").Replace("Button", " ").Trim();
                leaderShipUIFields.iconName.text = cleanButtonName;
                Debug.Log($"Set iconName.text to: {cleanButtonName}");
            }
            else
            {
                Debug.LogWarning("LeaderShipUIFields or iconName is null! Check Inspector assignments.");
            }

            // Log UI fields for debugging
            DebugUIFields();

            // Call other managers with original button name
            CallLeaderShipManager(clickedButton.name);
            CallUIAnimationController(clickedButton.name);
            CallUIPanelController(clickedButton.name);
            UpdateScores(clickedButton.name);
        }

        private void DebugUIFields()
        {
            if (leaderShipUIFields != null)
            {
                string iconNameText = leaderShipUIFields.iconName != null ? leaderShipUIFields.iconName.text : "null";
                string nameInputText = leaderShipUIFields.nameInputField != null ? leaderShipUIFields.nameInputField.text : "null";
                string leaderboardText = leaderShipUIFields.leaderboardDisplay != null ? leaderShipUIFields.leaderboardDisplay.text : "null";

                Debug.Log($"UI Fields at Click:\n" +
                          $"Icon Name Text: {iconNameText}\n" +
                          $"Name Input Field Text: {nameInputText}\n" +
                          $"Leaderboard Display Text: {leaderboardText}");
            }
            else
            {
                Debug.LogWarning("LeaderShipUIFields reference is null! Check Inspector assignments.");
            }
        }

        private void CallLeaderShipManager(string buttonName)
        {
            if (leaderShipManagerSO != null)
            {
                Debug.Log($"Calling LeaderShipManagerSO.Execute with buttonName: {buttonName}");
                leaderShipManagerSO.Execute(buttonName);
            }
            else
            {
                Debug.LogWarning("LeaderShipManagerSO reference not assigned! Check Inspector.");
            }
        }

        private void CallUIAnimationController(string buttonName)
        {
            if (uiAnimationController != null)
            {
                Debug.Log($"Calling UIAnimationController.Execute with buttonName: {buttonName}");
                uiAnimationController.Execute(buttonName);
            }
            else
            {
                Debug.LogWarning("UIAnimationController is null! Check Inspector or scene setup.");
            }
        }

        private void CallUIPanelController(string buttonName)
        {
            if (uiPanelManager != null)
            {
                Debug.Log($"Calling UIPanelManager.Execute with buttonName: {buttonName}");
                uiPanelManager.Execute(buttonName);
            }
            else
            {
                Debug.LogWarning("UIPanelManager is null! Check Inspector or scene setup.");
            }
        }

        private void UpdateScores(string buttonName)
        {
            if (scoreUpdation != null)
            {
                Debug.Log($"Calling ScoreUpdation.Execute with buttonName: {buttonName}");
                scoreUpdation.Execute(buttonName);
            }
            else
            {
                Debug.LogWarning("ScoreUpdation is null! Check Inspector or scene setup.");
            }
        }

        public IReadOnlyList<string> GetButtonNames()
        {
            return buttonNames.AsReadOnly();
        }
    }

    [System.Serializable]
    public class LeaderShipUIFields
    {
        public TextMeshProUGUI iconName;
        public TMP_InputField nameInputField;
        public TextMeshProUGUI leaderboardDisplay;
    }
}