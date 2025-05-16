using System.Collections.Generic;
using UnityEngine;


namespace IdleClicker.ScoreRef
{
    [CreateAssetMenu(fileName = "ShopCont", menuName = "ScriptableObjects/ShopCont", order = 4)]
    public class ShopCont : ScriptableObject
    {
        public List<int> autoCollectPrice;
        public List<int> tapPerSecondPrice;







    }
}
