using UnityEngine;
using UnityEngine.UI;


public class BarterMenu : MonoBehaviour
{
    private enum UpgradeType
    {
        Health = 0,
        Mana = 1,
        Stamina = 2
    }
    [SerializeField] private Text currentStatText, currentCoinText, noEquipmentWarning;
    [SerializeField] private float initialHealth, initialMana, initialStamina;
    [SerializeField] private GameObject coinTextAnimation;
    [SerializeField] private Button upgradeHP, upgradeMP, upgradeSP;
    [SerializeField] private Button[] currentEquipments;
    [SerializeField] private GameObject[] categoryPanels;
    private void OnEnable()
    {
        SetCurrentStatText();
        SetCurrentCoinText(PlayerStatusSystem.instance.currentCoins);
        RedrawCurrentEquipment();
        coinTextAnimation.SetActive(false);
    }
    // Start is called before the first frame update
    void Start()
    {
        initialHealth = CharacterTracker.instance.Health;
        initialMana = CharacterTracker.instance.Mana;
        initialStamina = CharacterTracker.instance.Stamina;
        SetTextHoverTip(upgradeHP, "Upgrade 100 HP?\n<color=#FFF547>Cost: " + UpgradeCostCalculator(PlayerStatusSystem.instance.currHealth, UpgradeType.Health) + "</color>");
        SetTextHoverTip(upgradeSP, "Upgrade 100 SP?\n<color=#FFF547>Cost: " + UpgradeCostCalculator(PlayerStatusSystem.instance.currSta, UpgradeType.Stamina) + "</color>");
        SetTextHoverTip(upgradeMP, "Upgrade 100 MP?\n<color=#FFF547>Cost: " + UpgradeCostCalculator(PlayerStatusSystem.instance.currMana, UpgradeType.Mana) + "</color>");
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.root.localScale.x < 0)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        else
        {
            transform.localScale = Vector3.one;
        }
    }
    public void UpgradeHPButton()
    {
        if (CheckAndBuy(UpgradeCostCalculator(PlayerStatusSystem.instance.currHealth, UpgradeType.Health)))
        {
            // Upgrade the player's Health
            PlayerStatusSystem.instance.UpgradeHealth();
            SetCurrentStatText();
            PlayCoinTextAnimation(-100);
            SetCurrentCoinText(PlayerStatusSystem.instance.currentCoins);
            // Update the hover tip for new pricing information
            SetTextHoverTip(upgradeHP, "Upgrade 100 HP?\n<color=#FFF547>Cost: " + UpgradeCostCalculator(PlayerStatusSystem.instance.currHealth, UpgradeType.Health) + "</color>");
        }
        
    }
    public void UpgradeMPButton()
    {
        if (CheckAndBuy(UpgradeCostCalculator(PlayerStatusSystem.instance.currMana, UpgradeType.Mana)))
        {
            // Upgrade the player's Mana
            PlayerStatusSystem.instance.UpgradeMana();
            SetCurrentStatText();
            PlayCoinTextAnimation(-100);
            SetCurrentCoinText(PlayerStatusSystem.instance.currentCoins);
            // Update the hover tip for new pricing information
            SetTextHoverTip(upgradeMP, "Upgrade 100 MP?\n<color=#FFF547>Cost: " + UpgradeCostCalculator(PlayerStatusSystem.instance.currMana, UpgradeType.Mana) + "</color>");
        }

    }
    public void UpgradeSPButton()
    {
        if (CheckAndBuy(UpgradeCostCalculator(PlayerStatusSystem.instance.currSta, UpgradeType.Stamina)))
        {
            // Upgrade the player's Stamina
            PlayerStatusSystem.instance.UpgradeStamina();
            SetCurrentStatText();
            PlayCoinTextAnimation(-100);
            SetCurrentCoinText(PlayerStatusSystem.instance.currentCoins);
            // Update the hover tip for new pricing information
            SetTextHoverTip(upgradeSP, "Upgrade 100 SP?\n<color=#FFF547>Cost: " + UpgradeCostCalculator(PlayerStatusSystem.instance.currSta, UpgradeType.Stamina) + "</color>");
        }
    }
    public void SellEquipmentSlot0()
    {
        SellEquipment(0);
    }
    public void SellEquipmentSlot1()
    {
        SellEquipment(1);
    }
    public void SellEquipmentSlot2()
    {
        SellEquipment(2);
    }
    private void SellEquipment(int slot)
    {
        PlayerStatusSystem.instance.AddCoins(PlayerController.instance.availEquipment[slot].value);
        PlayCoinTextAnimation(PlayerController.instance.availEquipment[slot].value);
        PlayerController.instance.RemoveEquipment(slot);
        RedrawCurrentEquipment();
        SetCurrentCoinText(PlayerStatusSystem.instance.currentCoins);
    }
    private void SetCurrentStatText()
    {
        currentStatText.text = "<color=#ff6663>HP: </color>" + PlayerStatusSystem.instance.maxHealth +
                                "\t<color=#9ec1cf>MP: </color>" + PlayerStatusSystem.instance.maxMana +
                                "\t<color=#9ee09e>SP: </color>" + PlayerStatusSystem.instance.maxSta;
    }
    public void SetCurrentCoinText(int coin)
    {
        if (coin >= 999999999)
        {
            coin = 999999999;
        }
        currentCoinText.text = "Coin: " + coin.ToString();
    }
    public void DialogueDuringMenu(string message)
    {
        transform.root.gameObject.GetComponentInChildren<DialogueBehaviour>().DisplayDialogueWithMessage(message);
    }
    //public Transform MiddlePointOfMenuAndActor(Transform Actor)
    //{
    //    GameObject midPoint = new GameObject();
    //    Vector3 position = new Vector3((transform.position.x - Actor.position.x) / 3f, (transform.position.y - Actor.position.y) / 3f);
    //    //Vector3 position = new Vector3((transform.localPosition.x - Actor.localPosition.x) / 2f, (transform.localPosition.y - Actor.localPosition.y) / 2f);
    //    midPoint.transform.position = position;
    //    Debug.Log(midPoint.transform);
    //    return midPoint.transform;
    //}
    public void PlayCoinTextAnimation(int coinChange)
    {
        coinTextAnimation.SetActive(false);
        coinTextAnimation.GetComponent<CoinTextElement>().SetCoinPopUpText(coinChange, true);
        coinTextAnimation.SetActive(true);
    }
    protected void SetTextHoverTip(Button button, string message)
    {
        button.gameObject.GetComponent<HoverTip>().UpdateMessage(message);
    }
    public bool CheckAndBuy(int cost)
    {
        if (PlayerStatusSystem.instance.currentCoins - cost >= 0)
        {
            // The Player has enough money, can buy
            PlayerStatusSystem.instance.RemoveCoins(cost);
            return true;
        }
        else
        {
            // They Player doesn't have enough money, cannot buy
            DialogueDuringMenu("You can't afford that");
            return false;
        }
    }
    public void ReDrawAllCategoryPanel()
    {
        for (int i = 0; i < categoryPanels.Length; i++)
        {
            categoryPanels[i].SetActive(false);
        }
    }
    private int UpgradeCostCalculator(float currentStatus, UpgradeType type)
    {
        currentStatus += 100f;
        int result = 0;
        switch (type)
        {
            case UpgradeType.Health:
                result = (int)((currentStatus - initialHealth) * 0.5f);
                break;
            case UpgradeType.Mana:
                result = (int)((currentStatus - initialMana) * 0.5f);
                break;
            case UpgradeType.Stamina:
                result = (int)((currentStatus - initialStamina) * 0.5f);
                break;
        }
        return result;
    }
    public void RedrawCurrentEquipment()
    {
        // Wipe all currently active buttons
        for (int i = 0; i < currentEquipments.Length; i++)
        {
            currentEquipments[i].gameObject.SetActive(false);
        }
        if (PlayerController.instance.availEquipment.Count == 0)
        {
            // The Player currently has no equipment in his inventory to sell
            noEquipmentWarning.gameObject.SetActive(true);
        }
        else
        {
            noEquipmentWarning.gameObject.SetActive(false);
            for (int i = 0; i < PlayerController.instance.availEquipment.Count; i++)
            {
                currentEquipments[i].gameObject.SetActive(true);
                SetUpEquipmentButton(currentEquipments[i], PlayerController.instance.availEquipment[i]);
            }
        }
    }   
    private void SetUpEquipmentButton(Button equipmentButton, Equippables equipment)
    {
        Image[] images = equipmentButton.GetComponentsInChildren<Image>();
        images[1].sprite = equipment.equipmentUiSprite;
        //equipmentButton.GetComponentInChildren<TextMeshProUGUI>().text = equipment.equipmentName;
        SetTextHoverTip(equipmentButton, "<color=#9F9F9F><i>" + equipment.equipmentName + "</color></i>\n" + equipment.description + "\n<color=#FFF547>Value: " + equipment.value + "</color>");
    }

}

