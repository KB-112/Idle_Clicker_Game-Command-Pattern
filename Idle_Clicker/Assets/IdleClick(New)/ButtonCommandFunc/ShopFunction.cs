using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace IdleClicker
{
    public class ShopFunction : MonoBehaviour
    {
        public void FetchShopDetails(ShopConfig shopConfig, TapCounterFunction tapCounterFunction, string buttonName)
        {
            Debug.Log($"Button Pressed: {buttonName}");
            Debug.Log($"Upgrade1 Button Name: {shopConfig.buttonNameUpgrade1}");
            Debug.Log($"Upgrade2 Button Name: {shopConfig.buttonNameUpgrade2}");

            if (shopConfig.buttonNameUpgrade1 == buttonName)
            {
                BuyTapUpgrade(shopConfig, tapCounterFunction);
            }
            else if (shopConfig.buttonNameUpgrade2 == buttonName)
            {
                BuyIdleUpgrade(shopConfig, tapCounterFunction);
            }
        }

        public void Initializer(ShopConfig shopConfig, TapCounterFunction tapCounterFunction)
        {
            UpdateInitLevelStatus(shopConfig);
            UpdateTapUpgradeUI(shopConfig, tapCounterFunction, shopConfig.tapUpgradeLevel);
            UpdateIdleUpgradeUI(shopConfig, tapCounterFunction, shopConfig.idleUpgradeLevel);
        }

        private void BuyTapUpgrade(ShopConfig shopConfig, TapCounterFunction tapCounterFunction)
        {
            int level = shopConfig.tapUpgradeLevel;

            if (level < shopConfig.tapUpgrades.Count)
            {
                var upgrade = shopConfig.tapUpgrades[level];
                Debug.Log($"[TAP] Current Balance: {shopConfig.mainPlayerData.TotalBalance}, Upgrade Cost: {upgrade.cost}");

                if (upgrade.cost <= shopConfig.mainPlayerData.TotalBalance)
                {
                    shopConfig.mainPlayerData.TotalBalance -= upgrade.cost;
                    shopConfig.tapUpgradeLevel++;
                    Debug.Log($"[TAP] Upgrade Purchased. New Level: {shopConfig.tapUpgradeLevel}, Remaining Balance: {shopConfig.mainPlayerData.TotalBalance}");
                    UpdateTapUpgradeUI(shopConfig, tapCounterFunction, shopConfig.tapUpgradeLevel);
                }
            }

            UpdateBuyStatus(shopConfig);
        }

        private void BuyIdleUpgrade(ShopConfig shopConfig, TapCounterFunction tapCounterFunction)
        {
            int level = shopConfig.idleUpgradeLevel;

            if (level < shopConfig.idleUpgrades.Count)
            {
                var upgrade = shopConfig.idleUpgrades[level];
                Debug.Log($"[IDLE] Current Balance: {shopConfig.mainPlayerData.TotalBalance}, Upgrade Cost: {upgrade.cost}");

                if (upgrade.cost <= shopConfig.mainPlayerData.TotalBalance)
                {
                    shopConfig.mainPlayerData.TotalBalance -= upgrade.cost;
                    shopConfig.idleUpgradeLevel++;
                    Debug.Log($"[IDLE] Upgrade Purchased. New Level: {shopConfig.idleUpgradeLevel}, Remaining Balance: {shopConfig.mainPlayerData.TotalBalance}");
                    UpdateIdleUpgradeUI(shopConfig, tapCounterFunction, shopConfig.idleUpgradeLevel);
                }
            }

            UpdateBuyStatus(shopConfig);
        }

        void UpdateInitLevelStatus(ShopConfig shopConfig)
        {
            shopConfig.apiExecutor.ExecuteCommand(
                new GetCommand(),
                "https://6824498265ba05803399a0a2.mockapi.io/api/v1/User_Name/" + shopConfig.mainPlayerData.id
            );

            shopConfig.tapUpgradeLevel = shopConfig.mainPlayerData.OnTapUpgradeLevel;
            shopConfig.idleUpgradeLevel = shopConfig.mainPlayerData.OnIdleUpgradeLevel;

            Debug.Log($"[GET] Tap Level: {shopConfig.tapUpgradeLevel}, Idle Level: {shopConfig.idleUpgradeLevel} , Total Balance : {shopConfig.mainPlayerData.TotalBalance} ");
        }

        void UpdateBuyStatus(ShopConfig shopConfig)
        {
            shopConfig.mainPlayerData.OnTapUpgradeLevel = shopConfig.tapUpgradeLevel;
            shopConfig.mainPlayerData.OnIdleUpgradeLevel = shopConfig.idleUpgradeLevel;

            string jsonData = JsonUtility.ToJson(shopConfig.mainPlayerData);
            shopConfig.apiExecutor.ExecuteCommand(
                new PutCommand(),
                "https://6824498265ba05803399a0a2.mockapi.io/api/v1/User_Name/" + shopConfig.mainPlayerData.id,
                jsonData
            );

            Debug.Log($"[PUT] Tap Level: {shopConfig.mainPlayerData.OnTapUpgradeLevel}, Idle Level: {shopConfig.mainPlayerData.OnIdleUpgradeLevel}");
        }

        private void UpdateTapUpgradeUI(ShopConfig shopConfig, TapCounterFunction tapCounterFunction, int level)
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

        private void UpdateIdleUpgradeUI(ShopConfig shopConfig, TapCounterFunction tapCounterFunction, int level)
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

        public ApiCommandExecutor apiExecutor;
        public MainPlayerData mainPlayerData;
    }
}
