using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StaffCastingScript : Equippables
{
    [SerializeField] private Transform[] shootPoints;
    [SerializeField] private GameObject[] spellsToCast;
    [SerializeField] private float delay;
    [SerializeField] private int burstSize;
    [SerializeField] private float fireRate;
    
    [SerializeField] private BoxCollider2D collision;
    [SerializeField] private float knockBackRecovery, spellCost;
    public float bashDamage;
    public float bashForce;
    private int currentShot;
    private float shotCounter, attackCounter;
    private PlayerController playerRef;
    private PlayerStatusSystem playerStats;
    
    // Start is called before the first frame update
    void Start()
    {
        currentShot = 1;
        playerStats = PlayerStatusSystem.instance;
        playerRef = PlayerController.instance;
    }

    // Update is called once per frame
    void Update()
    {
        if(playerRef.EPC && playerRef.notShielding)
        {
            if (shotCounter > 0)
            {
                shotCounter -= Time.deltaTime;
            }
            if (attackCounter > 0)
            {
                attackCounter -= Time.deltaTime;
            }
            else
            {
                if (Input.GetMouseButtonDown(1) && playerStats.CheckStaminaThenPerform(20f))// Right Click - Staff Bash Attacks 
                {
                    anim.SetTrigger("staffBash");
                    playerStats.ConsumeStamina(20f);
                    attackCounter = 1.2f;
                }
            }
            if (Input.GetMouseButtonDown(0) && playerStats.CheckManaThenPerfrom(spellCost * burstSize))// Hold down Left Click - Begin spell casting 
            {
                anim.SetBool("isCasting", true);
            }
            if (currentShot == burstSize && !Input.GetMouseButton(0))
            {
                anim.SetBool("isCasting", false);
                currentShot = 1;
            }
        }

    }
    private void castSpell()
    {
        if (shotCounter <= 0)
        {
            StartCoroutine(brstFire(burstSize));
            shotCounter = delay;
            currentShot = 1;
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
    private IEnumerator brstFire (int burstSize)
    {
        for (int i = 0; i < burstSize; i++)
        {
            int n = Random.Range(0, shootPoints.Length);
            Instantiate(spellsToCast[0], shootPoints[n].position, shootPoints[n].rotation);
            playerStats.ConsumeMana(spellCost);
            currentShot++;
        // Muzzle FX   
            yield return new WaitForSeconds(60f / fireRate);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        EnemyController ec = other.GetComponentInParent<EnemyController>();
        if (ec != null)
        {
            ec.BashKnockBack(bashForce, knockBackRecovery);
            ec.damageEnemy(bashDamage);
        }
    }
   
}
