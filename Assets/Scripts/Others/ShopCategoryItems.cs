using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopCategoryItems : MonoBehaviour
{
    [SerializeField] private Button[] itemsOfThisCategory;
    [SerializeField] private int index, currentPage;
    [SerializeField] private string[] romanNumbers;
    private Text pageText;
    // Start is called before the first frame update
    void Start()
    {
        pageText = GetComponentInChildren<Text>(); 
        index = 0;
        currentPage = 1;
        DrawItems();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void DrawItems()
    {
        if (itemsOfThisCategory.Length > 0)
        {
            // Redraw all of the buttons
            for (int i = 0; i < itemsOfThisCategory.Length; i++)
            {
                itemsOfThisCategory[i].gameObject.SetActive(false);
            }
            // Only draws 3 items at a time
            for (int i = 0; i < 3; i++)
            {
                if (index >= itemsOfThisCategory.Length)
                {
                    break;
                }
                itemsOfThisCategory[index].gameObject.SetActive(true);
                index++;
            }
        }
        else
        {
            currentPage = 0;
        }
        pageText.text = romanNumbers[currentPage];
    }
    public void LeftPageButton()
    {
        if (currentPage > 1)
        {
            if (currentPage != Mathf.CeilToInt(itemsOfThisCategory.Length / 3f))
            {
                index -= 6;
                if (index < 0)
                {
                    index = 0;
                }
            }
            else
            {
                index = itemsOfThisCategory.Length - 6 + ((Mathf.CeilToInt(itemsOfThisCategory.Length / 3f) * 3) - itemsOfThisCategory.Length);
            }
            currentPage--;
        }
        else
        {
            // currentPage <= 0 cases: Jumps to the last page of the current category
            currentPage = Mathf.CeilToInt(itemsOfThisCategory.Length / 3f);
            index = itemsOfThisCategory.Length - 3 + ((Mathf.CeilToInt(itemsOfThisCategory.Length / 3f) * 3) - itemsOfThisCategory.Length);  // Find the number of missing buttons to draw
            //index = itemsOfThisCategory.Length - 3 + itemsOfThisCategory.Length % 3;
        }
        DrawItems();
    }
    public void RightPageButton()
    {
        if (currentPage >= Mathf.CeilToInt(itemsOfThisCategory.Length / 3f))
        {
            currentPage = 1;
            index= 0;   
        }
        else
        {
            currentPage++;
        }
        DrawItems();
    }
}
