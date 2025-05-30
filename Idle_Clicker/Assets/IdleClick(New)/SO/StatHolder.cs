using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IdleClicker
{
    [CreateAssetMenu(fileName = "SOCont", menuName = "ContainerList/StatCont", order = 2)]
    public class StatHolder : ScriptableObject
    {
        public string buttonName;
        public float totalBalance;
        
        public int currentNonTapBalance;//Current Balance

        public int currentOnTapCoinGenRate;
        public int currentOffTapCoinGenRate;

        public int totalNumberOfUpgrades;
    }
}
