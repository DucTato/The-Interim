using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    public List<Equippables> availEquipment = new List<Equippables>();
    public float runSpeed;
    public Animator anim;
    [SerializeField] private Transform weaponHand, weaponPoint;
    [SerializeField] private Rigidbody2D playerRB;
    [SerializeField] private float moveSpeed;
    [SerializeField] private GameObject deadBody;
    public Transform internalFlame;
    private float currSpeed, secondCounter, angle;
    private Vector2 moveInput, mousePos, screenPoint;
    private CameraController camRef;
    private PlayerStatusSystem playerStats;
    private int currentEquipment;
    // For custom interactions in which the player's input is ignored
    // EPC = Enable Player Control
    public bool EPC = true;
    public bool notShielding = true;
    public bool isRunning = false;
    [HideInInspector]
    public bool followMouse;

    // Start is called before the first frame update
    //private void Awake()
    //{
    //    instance = this;
    //}
    private void OnEnable()
    {
        instance = this;
    }
    void Start()
    {
        camRef = CameraController.instance;
        playerStats = PlayerStatusSystem.instance;
        currSpeed = moveSpeed;
        //camRef.pausedTarget = internalFlame;
        currentEquipment = 0;
        SwitchEquipment();
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
            // Equipment switch mechanic. Player can press Q to cycle through each weapon or press 1, 2 or 3 respectively
            if (Input.GetKeyDown(KeyCode.Q))
            {
                if (availEquipment.Count > 0)
                {
                    //Rebind animator
                    availEquipment[currentEquipment].anim.Rebind();
                    availEquipment[currentEquipment].anim.Update(0f);
                    currentEquipment++;
                    if (currentEquipment >= availEquipment.Count)
                    {
                        currentEquipment = 0;
                    }
                    SwitchEquipment();
                }
            }
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                if (availEquipment.Count > 0 && currentEquipment != 0)
                {
                    //Rebind animator
                    availEquipment[currentEquipment].anim.Rebind();
                    availEquipment[currentEquipment].anim.Update(0f);
                    currentEquipment = 0;
                    SwitchEquipment();
                }
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                if (availEquipment.Count >= 2 && currentEquipment != 1)
                {
                    //Rebind animator
                    availEquipment[currentEquipment].anim.Rebind();
                    availEquipment[currentEquipment].anim.Update(0f);
                    currentEquipment = 1;
                    SwitchEquipment();
                }
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                if (availEquipment.Count == 3 && currentEquipment != 2)
                {
                    //Rebind animator
                    availEquipment[currentEquipment].anim.Rebind();
                    availEquipment[currentEquipment].anim.Update(0f);
                    currentEquipment = 2;
                    SwitchEquipment();
                }
            }
            ///////////////////////////////////////////////////
            // Drop the holding equipment
            if (Input.GetKeyDown(KeyCode.G))
            {
                if (currentEquipment >= 0)
                {
                    Instantiate(availEquipment[currentEquipment].droppedObject, weaponPoint.transform.position, Quaternion.identity);
                    RemoveEquipment(currentEquipment);
                }
            }
        }
        else
        {
            playerRB.velocity = Vector2.zero;
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
        playerStats.notInvinc = false;
        playerRB.velocity = Vector2.zero;
        UIController.instance.ingamePanel.SetActive(false);
        UIController.instance.deathPanel.SetActive(true);
    }
    public void SwitchEquipment()
    {
        foreach (Equippables equips in availEquipment)
        {
            equips.gameObject.SetActive(false);
        }
        UIController.instance.UpdateEquipmentUI(availEquipment[currentEquipment]);
        availEquipment[currentEquipment].gameObject.SetActive(true);
        followMouse = availEquipment[currentEquipment].needMouseAim;
    }
    // Checks if the Player already has that item. However I think it's alright to have duplicates
    //public bool CheckEquipment(string name)
    //{
    //    foreach (Equippables item in availEquipment)
    //    {
    //        if (item.equipmentName == name)
    //            return true;
    //        else
    //            continue;
    //    }
    //    return false;
    //}
    public void AddEquipment(Equippables equip)
    {
        Equippables clone = Instantiate(equip);
        clone.transform.position = weaponPoint.position;
        clone.transform.parent = weaponPoint;
        clone.transform.localRotation = Quaternion.Euler(Vector3.zero);
        clone.transform.localScale = Vector3.one;
        availEquipment.Add(clone);
        currentEquipment = availEquipment.Count - 1;
        SwitchEquipment();
    }
    public void RemoveEquipment(int index)
    {
        Destroy(availEquipment[index].gameObject);
        availEquipment.RemoveAt(index);
        
        if (availEquipment.Count == 0)
        {
            UIController.instance.SetUIBareHand();
            followMouse = false;
            currentEquipment = -1;
        }
        else
        {
            if(currentEquipment >= availEquipment.Count)
            {
                currentEquipment = availEquipment.Count - 1;
            }
            SwitchEquipment();
        }
            
    }
}
