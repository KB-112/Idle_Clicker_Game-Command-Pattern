using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IdleClicker
{
    [CreateAssetMenu(fileName = "StatCont", menuName = "ScriptableObjects/StatCont", order = 2)]
    public class StatContainer : ScriptableObject
    {
        public string buttonName;
        public float currentCollectedCoin;
        public float coinPerSecond;
        public float totalNumberOfUpgrades;

    }
}
