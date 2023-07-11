using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    [SerializeField] Transform weaponHand;
    [SerializeField] private Rigidbody2D playerRB;
    [SerializeField] private Animator anim;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private GameObject deadBody; 
    private float currSpeed;
    private Vector2 moveInput;
    

    // For custom interactions in which the player's input is ignored
    // EPC = Enable Player Control
    public bool EPC = true;
    public bool notShielding = true;
    public bool isRunning = false;
    
    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
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
            
            // Check the current mouse position related to the screen
            Vector2 mousePos= Input.mousePosition;
            Vector2 screenPoint = CameraController.instance.mainCamera.WorldToScreenPoint(transform.localPosition);
            // If x of mouse position is less than that of the screen, flip the player's sprite
            if (mousePos.x < screenPoint.x)
            {
                transform.localScale = new Vector2(-1f, 1f); // flip about the x axis
                weaponHand.localScale = new Vector2(-1f, -1f);
            }
            else
            {
                transform.localScale = Vector2.one;
                weaponHand.localScale = Vector2.one;
            }
            // Rotate the weapon hand so that it follows the mouse
            Vector2 offset = new Vector2(mousePos.x - screenPoint.x, mousePos.y - screenPoint.y);
            float angle = Mathf.Atan2(offset.y, offset.x) * 57.295f;
            weaponHand.rotation = Quaternion.Euler(0,0,angle);
            playerRB.velocity = moveInput * currSpeed;

        }
       
        
        // Animating the player
        if (playerRB.velocity != Vector2.zero)
        {
            anim.SetBool("isMoving", true);
            // Hold SHIFT to run
            if (Input.GetKey("left shift"))
            {
                isRunning = true;
                currSpeed = runSpeed;
                anim.SetFloat("animSpeed", runSpeed / moveSpeed);
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
