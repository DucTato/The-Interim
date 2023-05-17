using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    public float moveSpeed;
    private Vector2 moveInput;
    public Rigidbody2D playerRB;
    // For custom interactions in which the player's input is ignored
    // EPC = Enable Player Control
    public bool EPC = true;
    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        
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
            playerRB.velocity = moveInput*moveSpeed;
            // Check the current mouse position related to the screen
            Vector2 mousePos= Input.mousePosition;
            Vector2 screenPoint = CameraController.instance.mainCamera.WorldToScreenPoint(transform.localPosition);
            // If x of mouse position is less than that of the screen, flip the player's sprite
            if (mousePos.x < screenPoint.x)
            {
                transform.localScale = new Vector2(-1f, 1f); // flip about the x axis
            }
            else
            {
                transform.localScale = Vector2.one;
            }
        }
    }
}
