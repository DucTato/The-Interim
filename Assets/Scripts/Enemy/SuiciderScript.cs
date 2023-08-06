using UnityEngine;

public class SuiciderScript : MonoBehaviour
{    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<EnemyController>().damageEnemy(10000);
        }   
    }
}
