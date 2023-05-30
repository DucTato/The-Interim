using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeScript : MonoBehaviour
{
    // MELEE TYPE: 0 - Claymore, 1 - Sword, 2 - Rapier
    [SerializeField] private Animator anim;
    [SerializeField] private int meleeType;
    [SerializeField] private BoxCollider2D collision;
    [SerializeField] private float attackDelay;
    //[SerializeField] private Transform effectPoint;
    //[SerializeField] private float effectRadius;
    private float timeBetweenLAttacks, timeBetweenHAttacks;
    public float Damage;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerController.instance.EPC && PlayerController.instance.notShielding) // Check if the player can move or not
        {
            if (timeBetweenLAttacks < 0)
            {
                if (Input.GetMouseButtonDown(0))// Left Click - Light Attacks
                {
                    anim.SetInteger("meleeType", meleeType);
                    anim.SetTrigger("meleeLight");
                    timeBetweenLAttacks = attackDelay;
                }   
            }
            if (timeBetweenHAttacks < 0)
            {
                if (Input.GetMouseButtonDown(1))// Right Click - Heavy Attacks
                {
                    anim.SetInteger("meleeType", meleeType);
                    anim.SetTrigger("meleeHeavy");
                    timeBetweenHAttacks = attackDelay + 1;
                }
            }
            timeBetweenLAttacks -= Time.deltaTime;
            timeBetweenHAttacks -= Time.deltaTime;
        }
    }
    private void hitFrameON()
    {
        collision.enabled = true;
    }
    private void hitFrameOFF()
    {
        collision.enabled = false;
    }
    private void OnTriggerEnter2D (Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            other.GetComponent<EnemyController>().damageEnemy(Damage);
        }
    }
    //private void OnDrawGizmosSelected()
    //{
    //    Gizmos.color = Color.white;
    //    Vector3 FXposition = effectPoint == null ? Vector3.zero : effectPoint.position;
    //    Gizmos.DrawWireSphere(FXposition, effectRadius);
    //}
    //public void detectColliders()
    //{
    //    foreach (Collider2D collider in Physics2D.OverlapCircleAll(effectPoint.position, effectRadius)) 
    //    {
    //        Debug.Log(collider.name);
    //    }
    //}
}
