using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mShieldScript : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private GameObject[] mShieldImpactFX;
    [SerializeField] private float manaCost;
    private CircleCollider2D magicShieldCol;
    private PlayerController playerRef;
    private PlayerStatusSystem playerStats;
    private float secondCounter;
    // Start is called before the first frame update
    void Start()
    {
        magicShieldCol= GetComponent<CircleCollider2D>();
        playerRef = PlayerController.instance;
        playerStats = PlayerStatusSystem.instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerRef.EPC)
        {


            // Hold Space Bar to use the Shield
            if (Input.GetKey(KeyCode.Space) && playerStats.CheckManaThenPerfrom(manaCost))
            {
                anim.SetBool("useShield", true);
                playerRef.notShielding = false;
                if (secondCounter > 0)
                {
                    secondCounter -= Time.deltaTime;
                }
                else
                {
                    playerStats.ConsumeMana(manaCost);
                    secondCounter = 1f;
                }
            }
           else
            {
                playerRef.notShielding = true;
                anim.SetBool("useShield", false);
                secondCounter = 0f;
            }
        }
    }
    private void shieldUp()
    {
        magicShieldCol.enabled = true;
    }
    private void shieldDown()
    {
        magicShieldCol.enabled = false;
    }
    public void ImpactShield(float damage)
    {
        if(!playerStats.CheckManaThenPerfrom(damage))
        {
            playerStats.ConsumeMana(damage);
            playerStats.currHealth -= damage * 0.8f;
            UIController.instance.hpSlider.value = playerStats.currHealth;
        }
        else
        {
            playerStats.ConsumeMana(damage * 0.8f);
        }
        
    }
}
