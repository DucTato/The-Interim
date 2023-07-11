using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldScript : MonoBehaviour
{
    public Animator anim;
    private CircleCollider2D shieldCollision;
    [SerializeField] private GameObject shieldImpactFX;
    // Start is called before the first frame update
    void Start()
    {
        shieldCollision= GetComponent<CircleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerController.instance.EPC)
        {
            // Hold Space Bar to use the Shield
            if (Input.GetKey("space"))
            {
                anim.SetBool("useShield", true);
                PlayerController.instance.notShielding = false;
            }
            else
            {
                PlayerController.instance.notShielding = true;
                anim.SetBool("useShield", false);
            }
        }
    }
    private void shieldUp()
    {
        shieldCollision.enabled = true;
    }    
    private void shieldDown()
    {
        shieldCollision.enabled = false;
    }
}
