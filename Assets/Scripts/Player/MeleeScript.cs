using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeScript : MonoBehaviour
{
    // MELEE TYPE: 0 - Claymore, 1 - Sword, 2 - Rapier
    [SerializeField] private Animator anim;
    [SerializeField] private int meleeType;
    private BoxCollider2D collision;
    [SerializeField] private float attackDelay;
    private float timeBetweenAttacks;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerController.instance.EPC && PlayerController.instance.notShielding) // Check if the player can move or not
        {
            if (timeBetweenAttacks < 0)
            {
                if (Input.GetMouseButtonDown(0))// Left Click - Light Attacks
                {
                    anim.SetInteger("meleeType", meleeType);
                    anim.SetTrigger("meleeLight");
                    timeBetweenAttacks = attackDelay;
                }
                if (Input.GetMouseButtonDown(1))// Right Click - Heavy Attacks
                {
                    anim.SetInteger("meleeType", meleeType);
                    anim.SetTrigger("meleeHeavy");
                    timeBetweenAttacks = attackDelay;
                }
            }
            timeBetweenAttacks -= Time.deltaTime;
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
}
