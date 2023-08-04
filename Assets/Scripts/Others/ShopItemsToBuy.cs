using UnityEngine;
using UnityEngine.UI;

public class ShopItemsToBuy : MonoBehaviour
{
    [SerializeField] private Equippables itemToBuy;
    //private BarterMenu parentBM;
    // Start is called before the first frame update
    void Start()
    {
        SetTextHoverTip("<color=#9F9F9F><i>" + itemToBuy.equipmentName + "</color></i>\n" + itemToBuy.description + "\n<color=#FFF547>Cost: " + itemToBuy.value + "</color>");
        Image[] images = GetComponentsInChildren<Image>();
        images[1].sprite = itemToBuy.equipmentUiSprite;
    }    
    public void BuyItem()
    {
        if (transform.root.GetComponentInChildren<BarterMenu>().CheckAndBuy(itemToBuy.value))
        {
            // The Player has enough money to buy the item
            if (PlayerController.instance.availEquipment.Count >= 3)
            {
                // The Player don't have any more slots for new equipments/items
            }
            else
            {
                PlayerController.instance.AddEquipment(itemToBuy);
                PlayerStatusSystem.instance.AddCoins(itemToBuy.value);
                transform.root.GetComponentInChildren<BarterMenu>().SetCurrentCoinText(PlayerStatusSystem.instance.currentCoins);
                transform.root.GetComponentInChildren<BarterMenu>().PlayCoinTextAnimation(-itemToBuy.value);
                transform.root.GetComponentInChildren<BarterMenu>().RedrawCurrentEquipment();
            }
        }
        else
        {
            // The Player doesn't have enough money to buy
            transform.root.GetComponentInChildren<BarterMenu>().DialogueDuringMenu("You don't have enough money");
        }
    }
    private void SetTextHoverTip(string message)
    {
        GetComponent<HoverTip>().UpdateMessage(message);
    }
}
