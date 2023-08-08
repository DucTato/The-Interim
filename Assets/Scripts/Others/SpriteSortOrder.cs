using UnityEngine;

public class SpriteSortOrder : MonoBehaviour
{
    
    [SerializeField] private bool consistentWithRoot;
    private SpriteRenderer objectSR;
    private int startingOrder;
    private bool inFront;
    // Start is called before the first frame update
    void Start()
    {
        objectSR = GetComponent<SpriteRenderer>();
        startingOrder = objectSR.sortingOrder;
        if (consistentWithRoot)
        {
            
            if (startingOrder >= transform.root.GetComponent<SpriteRenderer>().sortingOrder)
            {
                inFront = true;
            }
            else
            {
                inFront = false;
            }
            startingOrder = Mathf.Abs(startingOrder);
        }
        
    }
    private void Update()
    {
       
        if (consistentWithRoot)
        {
            if (inFront)
            {
                objectSR.sortingOrder = (int)transform.root.position.y * -10 + startingOrder;
            }
            else
            {
                objectSR.sortingOrder = (int)transform.root.position.y * -10 - startingOrder; 
            }
           
        }
        else
        {
            objectSR.sortingOrder = (int)transform.position.y * -10;
        }
    }
    public void SortOrderOverride(bool isInFront)
    {
        inFront = isInFront;
    }
}
