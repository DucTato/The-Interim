using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodenCrateScript : MonoBehaviour
{
    [SerializeField] private SpriteRenderer crateSR;
    [SerializeField] private GameObject[] brokenPieces;
    [SerializeField] private GameObject objectToDrop;
    //// Start is called before the first frame update
    //void Start()
    //{
        
    //}

    //// Update is called once per frame
    //void Update()
    //{
        
    //}
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player Bullet") || collision.CompareTag("Enemy Bullet") || collision.CompareTag("Enemy mBullet"))
        {
            BreakCrate();
            GameObject drop = Instantiate(objectToDrop, transform.position, transform.rotation);
            drop.GetComponent<BoonItems>().type = (BoonItemType) Random.Range(0, 8);
            ThrowPieces(Random.Range(1, transform.childCount));
        }
    }
    private void BreakCrate()
    {
        crateSR.enabled = false;
        foreach (BoxCollider2D col in GetComponents<BoxCollider2D>())
        {
            col.enabled = false;
        }
        Destroy(gameObject, 5.5f);
    }
    private void ThrowPieces(int maxPieces)
    {

        for (int i = 0; i < maxPieces; i++)
        {
            brokenPieces[i].SetActive(true);
        }
    }
}
