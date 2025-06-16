using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IdleClicker
{
    [CreateAssetMenu(fileName = "ApiCont", menuName = "ContainerList/PlayerData", order = 1)]
    public class MainPlayerData : ScriptableObject
    {
        public int id;
        public string User_Name;
        public int Score;
        public int OnTapUpgradeLevel;
        public int OnIdleUpgradeLevel;
        public int TotalBalance;
    }
}
