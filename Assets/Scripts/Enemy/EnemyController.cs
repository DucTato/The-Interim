using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] protected float detectRange;
    [SerializeField] private float Health;
    [SerializeField] private Rigidbody2D enemyRB;
    [SerializeField] private float moveSpeed;
    [SerializeField] private Animator anim;
    private Vector2 moveDirection;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector2.Distance(transform.position, PlayerController.instance.transform.position) < detectRange)
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
            //Walking towards the player
            moveDirection = PlayerController.instance.transform.position - transform.position;
        }
        else
        {
            moveDirection = Vector2.zero;
        }
        moveDirection.Normalize();
        enemyRB.velocity = moveDirection * moveSpeed;

        //Animating the enemy
        if (enemyRB.velocity != Vector2.zero)
        {
            anim.SetBool("isMoving", true);

        }
        else
        {
            anim.SetBool("isMoving", false);
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
