using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private BoxCollider2D collision;
    // Start is called before the first frame update
    void Start()
    {
        collision= GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(PlayerController.instance.EPC)
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
            if (Input.GetMouseButtonUp(0))
            {
                anim.SetBool("isCasting", false);
            }
        }

    }
    private void castSpell()
    {
        if (shotCounter <= 0)
        {
            StartCoroutine(brstFire(burstSize));
            shotCounter = delay;
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
        // Muzzle FX   
            yield return new WaitForSeconds(60f / fireRate);
        }
    }
}
