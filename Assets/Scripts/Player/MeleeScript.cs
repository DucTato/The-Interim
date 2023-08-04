using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeScript : Equippables
{
    
    
    [SerializeField] private MeleeType type;
    [SerializeField] private BoxCollider2D collision;
    [SerializeField] private float attackDelay, staminaCost;

    //[SerializeField] private Transform effectPoint;
    //[SerializeField] private float effectRadius;
    private int meleeType ;
    private PlayerController playerRef;
    private PlayerStatusSystem playerStats;
    private float timeBetweenLAttacks, timeBetweenHAttacks;
    public float Damage;

    // Start is called before the first frame update
    void Start()
    {
        meleeType = (int)type;
        playerStats = PlayerStatusSystem.instance;
        playerRef = PlayerController.instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerRef.EPC && playerRef.notShielding) // Check if the player can move or not
        {
            if (timeBetweenLAttacks < 0)
            {
                if (Input.GetMouseButtonDown(0) && playerStats.CheckStaminaThenPerform(staminaCost))// Left Click - Light Attacks
                {
                    anim.SetInteger("meleeType", meleeType);
                    anim.SetTrigger("meleeLight");
                    playerStats.ConsumeStamina(staminaCost);
                    timeBetweenLAttacks = attackDelay;
                }   
            }
            if (timeBetweenHAttacks < 0)
            {
                if (Input.GetMouseButtonDown(1) && playerStats.CheckStaminaThenPerform(staminaCost * 1.2f))// Right Click - Heavy Attacks
                {
                    anim.SetInteger("meleeType", meleeType);
                    anim.SetTrigger("meleeHeavy");
                    playerStats.ConsumeStamina(staminaCost * 1.2f);
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
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<EnemyController>().damageEnemy(Damage);
        }
    }
   
}
public enum MeleeType
{
    Claymore = 0,
    Sword = 1,
    Rapier = 2
}
