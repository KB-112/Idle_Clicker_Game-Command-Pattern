using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace IdleClicker
{
    public class LoginInputHandling : MonoBehaviour
    {
        public  TMP_InputField inputName;
        public TextMeshProUGUI realTimeName;
        public int maxLength=4;


        private void Start()
        {

            UserNameConstraint();
        }

        void UserNameConstraint()
        {
            inputName.characterLimit = maxLength;
            inputName.contentType = TMP_InputField.ContentType.Name;
        }
        private void Update()
        {
            LoginRules();

        }
        void LoginRules()
        {

            realTimeName.text = inputName.text.ToString();
            

        }
    }
}
