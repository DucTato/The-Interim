
using UnityEngine;

public class CategoryToggleButton : MonoBehaviour
{
    [SerializeField] private GameObject categoryPanel;
    public void SelectCategory()
    {
        transform.root.GetComponentInChildren<BarterMenu>().ReDrawAllCategoryPanel();
        categoryPanel.SetActive(true);
    }
}
