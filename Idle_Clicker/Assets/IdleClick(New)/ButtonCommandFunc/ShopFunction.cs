using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace IdleClicker
{
    public class ShopFunction : MonoBehaviour
    {
        public void FetchShopDetails(ShopConfig shopConfig, string buttonName)
        {
            if (shopConfig.buttonNameUpgrade1 == buttonName)
            {
                
                BuyTapUpgrade(shopConfig);
            }

            if (shopConfig.buttonNameUpgrade2 == buttonName)
            {

                BuyIdleUpgrade(shopConfig);
            }
        }

        public void Initializer(ShopConfig shopConfig)
        {
            UpdateTapUpgradeUI(shopConfig, shopConfig.tapUpgradeLevel);
            UpdateIdleUpgradeUI(shopConfig, shopConfig.idleUpgradeLevel);
        }

        private void BuyTapUpgrade(ShopConfig shopConfig)
        {
            int level = shopConfig.tapUpgradeLevel;

            if (level < shopConfig.tapUpgrades.Count)
            {
                var upgrade = shopConfig.tapUpgrades[level];
                Debug.Log($"[TAP] Current Balance: {shopConfig.totalBalance}, Upgrade Cost: {upgrade.cost}");

                if (upgrade.cost <= shopConfig.totalBalance)
                {
                    shopConfig.totalBalance -= upgrade.cost;
                    shopConfig.tapUpgradeLevel++;
                    Debug.Log($"[TAP] Upgrade Purchased. New Level: {shopConfig.tapUpgradeLevel}, Remaining Balance: {shopConfig.totalBalance}");
                    UpdateTapUpgradeUI(shopConfig, shopConfig.tapUpgradeLevel);
                }
            }
        }

        private void BuyIdleUpgrade(ShopConfig shopConfig)
        {
            int level = shopConfig.idleUpgradeLevel;

            if (level < shopConfig.idleUpgrades.Count)
            {
                var upgrade = shopConfig.idleUpgrades[level];
                Debug.Log($"[IDLE] Current Balance: {shopConfig.totalBalance}, Upgrade Cost: {upgrade.cost}");

                if (upgrade.cost <= shopConfig.totalBalance)
                {
                    shopConfig.totalBalance -= upgrade.cost;
                    shopConfig.idleUpgradeLevel++;
                    Debug.Log($"[IDLE] Upgrade Purchased. New Level: {shopConfig.idleUpgradeLevel}, Remaining Balance: {shopConfig.totalBalance}");
                    UpdateIdleUpgradeUI(shopConfig, shopConfig.idleUpgradeLevel);
                }
            }
        }

        private void UpdateTapUpgradeUI(ShopConfig shopConfig, int level)
        {
            if (level < shopConfig.tapUpgrades.Count)
            {
                var upgrade = shopConfig.tapUpgrades[level];
                shopConfig.tapUpgradeText.text = $"{upgrade.tapPerIncrement}/1 tap";
                shopConfig.tapUpgradeCostText.text = upgrade.cost.ToString();
            }
            else
            {
                shopConfig.tapUpgradeText.text = "MAX";
                shopConfig.tapUpgradeCostText.text = "-";
            }
        }

        private void UpdateIdleUpgradeUI(ShopConfig shopConfig, int level)
        {
            if (level < shopConfig.idleUpgrades.Count)
            {
                var upgrade = shopConfig.idleUpgrades[level];
                shopConfig.idleUpgradeText.text = $"{upgrade.idlePerIncrement}/1 sec";
                shopConfig.idleUpgradeCostText.text = upgrade.cost.ToString();
            }
            else
            {
                shopConfig.idleUpgradeText.text = "MAX";
                shopConfig.idleUpgradeCostText.text = "-";
            }
        }
    }

    [System.Serializable]
    public class ShopTapUpgrade
    {
        public int tapPerIncrement;
        public int cost;
    }

    [System.Serializable]
    public class ShopIdleUpgrade
    {
        public int idlePerIncrement;
        public int cost;
    }

    [System.Serializable]
    public class ShopConfig
    {
        [Header("Tap Upgrade Settings")]
        public string buttonNameUpgrade1;
        public List<ShopTapUpgrade> tapUpgrades;
        public int tapUpgradeLevel = 0;
        public TextMeshProUGUI tapUpgradeText;
        public TextMeshProUGUI tapUpgradeCostText;
      

        [Header("Idle Upgrade Settings")]
        public string buttonNameUpgrade2;
        public List<ShopIdleUpgrade> idleUpgrades;
        public int idleUpgradeLevel = 0;
        public TextMeshProUGUI idleUpgradeText;
        public TextMeshProUGUI idleUpgradeCostText;
        

        [Header("Balance")]
        public int totalBalance;
    }
}
