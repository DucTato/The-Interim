using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteSortOrder : MonoBehaviour
{
    private SpriteRenderer objectSR;
    // Start is called before the first frame update
    void Start()
    {
        objectSR = GetComponent<SpriteRenderer>();
        objectSR.sortingOrder = (int)transform.position.y * -10;
    }
}
