using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

namespace IdleClicker
{
    [CreateAssetMenu(fileName = "ShopCont", menuName = "ScriptableObjects/ShopCont", order = 4)]
    public class ShopCont : ScriptableObject
    {
        public List<int> autoCollectPrice;
        public List<int> tapPerSecondPrice;







    }
}
