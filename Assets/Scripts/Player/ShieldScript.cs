
using UnityEngine;

public class ShieldScript : MonoBehaviour
{
    public Animator anim;
    private CircleCollider2D shieldCollision;
    [SerializeField] private GameObject shieldImpactFX;
    private PlayerController playerRef;
    private PlayerStatusSystem playerStats;
    [SerializeField] private float parryWindow;
    //private SpriteRenderer shieldSR;
    // Start is called before the first frame update
    void Start()
    {
        shieldCollision= GetComponent<CircleCollider2D>();
        //shieldSR= GetComponent<SpriteRenderer>();
        playerRef = PlayerController.instance;
        playerStats = PlayerStatusSystem.instance;
        parryWindow = 0.3f;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (playerRef.EPC)
        {
            // Hold Space Bar to use the Shield
            if (Input.GetKey(KeyCode.Space))
            {
                anim.SetBool("useShield", true);
                parryWindow -= Time.deltaTime;
                playerRef.notShielding = false;
                playerStats.ConsumeStamina(0f);
            }
            else
            {
                playerRef.notShielding = true;
                anim.SetBool("useShield", false);
                parryWindow = 0.3f;
            }
        }
    }
    private void shieldUp()
    {
        //shieldSR.sortingOrder = transform.root.GetComponent<SpriteRenderer>().sortingOrder + 10;
        GetComponent<SpriteSortOrder>().SortOrderOverride(true);
        shieldCollision.enabled = true;
    }    
    private void shieldDown()
    {
        shieldCollision.enabled = false;
        GetComponent<SpriteSortOrder>().SortOrderOverride(false);
        //shieldSR.sortingOrder = transform.root.GetComponent<SpriteRenderer>().sortingOrder - 1;
    }
    public void ImpactShield(float damage)
    {
        if(parryWindow >= 0)
        {
            shieldImpactFX.SetActive(true);
            return;
        }
        if (!playerStats.CheckStaminaThenPerform(damage))
        {
            playerStats.ConsumeStamina(damage);
            playerStats.currHealth -= damage * 0.9f;
            UIController.instance.hpSlider.value = playerStats.currHealth;
        }
        else
        {
            playerStats.ConsumeStamina(damage * 0.9f);
        }
    }
}
