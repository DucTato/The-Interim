using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class StaffCastingScript : MonoBehaviour
{
    [SerializeField]
    private Transform[] shootPoints;
    [SerializeField]
    private GameObject[] spellsToCast;
    [SerializeField]
    private float delay;
    private float shotCounter, attackCounter;
    [SerializeField]
    private int burstSize;
    [SerializeField]
    private float fireRate;
    [SerializeField]
    private Animator anim;
    [SerializeField] private BoxCollider2D collision;
    [SerializeField]
    private float knockBackRecovery;
    public float bashDamage;
    public float bashForce;
    private int currentShot;
    private PlayerController playerRef;
    // Start is called before the first frame update
    void Start()
    {
        currentShot = 1;
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
                if (Input.GetMouseButtonDown(1))// Right Click - Staff Bash Attacks
                {
                    anim.SetTrigger("staffBash");
                    attackCounter = 1.2f;
                }
            }
            if (Input.GetMouseButtonDown(0))// Hold down Left Click - Begin spell casting 
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
            currentShot++;
        // Muzzle FX   
            yield return new WaitForSeconds(60f / fireRate);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        EnemyController ec = other.GetComponent<EnemyController>();
        if (ec != null)
        {
            ec.isStunned = true;
            BashKnockBack(ec.gameObject);
            ec.damageEnemy(bashDamage);

        }
    }
    private void BashKnockBack(GameObject target)
    {
        StartCoroutine(KnockBackThenRecover(target, knockBackRecovery));
        Rigidbody2D targetRB = target.GetComponent<Rigidbody2D>();
        targetRB.velocity = Vector2.zero;
        // Find the vector that represents the current position of the target and the player
        Vector2 knockDirection = target.transform.position - playerRef.transform.position;
        // Apply the force
        targetRB.AddForce(knockDirection * bashForce, ForceMode2D.Impulse);
        
    }
    private IEnumerator KnockBackThenRecover(GameObject target, float recoverTime)
    {
        target.GetComponent<EnemyPathFindingBehaviour>().enabled = false;
        target.GetComponent<Animator>().SetBool("isMoving", false);
        yield return new WaitForSeconds(recoverTime);
        target.GetComponent<EnemyPathFindingBehaviour>().enabled = true;
        target.GetComponent<EnemyController>().isStunned = false;
    }
}
