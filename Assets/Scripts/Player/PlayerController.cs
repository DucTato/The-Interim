using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    
    public float runSpeed;
    public Animator anim;
    [SerializeField] private Transform weaponHand;
    [SerializeField] private Rigidbody2D playerRB;
    
    [SerializeField] private float moveSpeed;
    [SerializeField] private GameObject deadBody;
    public float physResistMult, magResistMult, maxHealth, maxStamina, maxMana;
    private float currSpeed, secondCounter, angle;
    private Vector2 moveInput, mousePos, screenPoint;
    private CameraController camRef;
    private PlayerStatusSystem playerStats;

    // For custom interactions in which the player's input is ignored
    // EPC = Enable Player Control
    public bool EPC = true;
    public bool notShielding = true;
    public bool isRunning = false;
    public bool followMouse = true;
    
    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        camRef = CameraController.instance;
        playerStats = PlayerStatusSystem.instance;
        currSpeed = moveSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if(EPC)
        {
            moveInput.x = Input.GetAxisRaw("Horizontal");
            moveInput.y = Input.GetAxisRaw("Vertical");
            // Normalizes the Vector so that the player's speed stays consistent
            moveInput.Normalize();     
            playerRB.velocity = moveInput * currSpeed;
            // Checks the current mouse position related to the screen
            mousePos = Input.mousePosition;
            screenPoint = camRef.mainCamera.WorldToScreenPoint(transform.localPosition);
            // If x of mouse position is less than that of the screen, flip the player's sprite
            if (mousePos.x < screenPoint.x)
            {
                transform.localScale = new Vector2(-1f, 1f);    // flip about the x axis
                weaponHand.localScale = new Vector2(-1f, -1f);
            }
            else
            {
                transform.localScale = Vector2.one;
                weaponHand.localScale = Vector2.one;
            }
            Vector2 offset = new Vector2(mousePos.x - screenPoint.x, mousePos.y - screenPoint.y);
            angle = Mathf.Atan2(offset.y, offset.x) * 57.295f;
            
        }
        
        if (followMouse)
        {
            // Rotates the weapon hand so that it follows the mouse
            weaponHand.rotation = Quaternion.Euler(0, 0, angle);
        }
        else
        {
            weaponHand.localScale = Vector2.one;
            weaponHand.rotation = Quaternion.Slerp(weaponHand.rotation, Quaternion.Euler(Vector2.zero), 0.06f);
        }


        // Animating the player
        if (playerRB.velocity != Vector2.zero)
        {
            anim.SetBool("isMoving", true);
            // Hold SHIFT to run
            if (Input.GetKey(KeyCode.LeftShift) && playerStats.CheckStaminaThenPerform(10f) && notShielding)
            {
                isRunning = true;
                currSpeed = runSpeed;

                anim.SetFloat("animSpeed", runSpeed / moveSpeed);
                // Running consumes 10 point(s) Stamina per second
                if (secondCounter > 0)
                {
                    secondCounter -= Time.deltaTime;
                }
                else
                {
                    playerStats.ConsumeStamina(10f);
                    secondCounter = 1f;
                }
                /////////////////////////////////////////////
            }
            else
            {
                isRunning = false;
                currSpeed = moveSpeed;
                anim.SetFloat("animSpeed", 1f);
            }
        }
        else
        {
            anim.SetBool("isMoving", false);
            isRunning = false;
        }
        
    }
    private void onDeath()
    {
        gameObject.SetActive(false);
        Instantiate(deadBody, new Vector3 (transform.position.x, transform.position.y - 0.9f, 1f), transform.rotation);
    }
    private void startDying()
    {
        PlayerStatusSystem.instance.notInvinc = false;
        playerRB.velocity = Vector2.zero;
    }
}
