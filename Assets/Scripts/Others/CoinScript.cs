using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinScript : MonoBehaviour
{
    
    [SerializeField] private GameObject coin, textElement;
    public int coinAmount;
    private float timeToPickup;
    private Vector3 dropDirection;
    // Start is called before the first frame update
    void Start()
    {
        timeToPickup = 0.5f;
        textElement.GetComponent<CoinTextElement>().amount = coinAmount;
        dropDirection.y = Random.Range(-3f, 3f);
        dropDirection.x = Random.Range(-3f, 3f);
    }

    // Update is called once per frame
    void Update()
    {
        timeToPickup -= Time.deltaTime;
        transform.position += dropDirection * Time.deltaTime ;
        dropDirection = Vector3.Lerp(dropDirection, Vector3.zero, 5f * Time.deltaTime);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && timeToPickup < 0)
        {
            Destroy(gameObject, 1.6f);
            // Picks up the coin
            coin.SetActive(false);
            GetComponent<CircleCollider2D>().enabled = false;
            textElement.SetActive(true);
            
            PlayerStatusSystem.instance.AddCoins(coinAmount);
        }
    }
}
