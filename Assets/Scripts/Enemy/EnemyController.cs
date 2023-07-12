using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] protected float detectRange;
    [SerializeField] private float Health;
    [SerializeField] private GameObject rabidFX;
    
   
    public bool isStunned;
    

    // Start is called before the first frame update
    void Start()
    {
        // Randomly decides if the monster is rabid or not upon spawning
        if(1 == Random.Range(0,2))
        {
            GetComponent<EnemyPathFindingBehaviour>().isRabid = true;
            rabidFX.SetActive(true);
            Health *= 0.5f;
            detectRange += 3f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(!isStunned)
        {
            if (Vector2.Distance(transform.position, PlayerController.instance.transform.position) < detectRange)
            {
                //Facing the player
                if (transform.position.x > PlayerController.instance.transform.position.x)
                {
                    transform.localScale = new Vector2(-1f, 1f);
                }
                else
                {
                    transform.localScale = Vector2.one;
                }
            }
        }
    }
    public void damageEnemy(float damage)
    {
        Health -= damage;
        // Hit FX
        if (Health <= 0)
        {
            Destroy(gameObject);
            // Spawning an item upon death
        }
    }
}
