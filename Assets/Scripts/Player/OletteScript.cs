using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OletteScript : MonoBehaviour
{
    public static OletteScript instance;
    private PlayerController playerRef;
    private DialogueBehaviour dialogueScript;
    [SerializeField] private GameObject notification, dialogue, clairvoyance, buyMenu;
    [SerializeField] private float lookRange;
    private bool nearPlayer, isBuying;
    //private string oletteMessage;
    private void Awake()
    {
        instance = this; 
    }
   
    private void OnEnable()
    {
        
        clairvoyance.transform.position = PlayerController.instance.transform.position;
        clairvoyance.SetActive(true);
        nearPlayer = false;
        isBuying = false;
    }
    private void OnDisable()
    {
        clairvoyance.SetActive(false);
        buyMenu.SetActive(false);
    }
    private void Start()
    {
        playerRef = PlayerController.instance;
        dialogueScript = dialogue.GetComponent<DialogueBehaviour>();
    }
    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(transform.position, playerRef.transform.position) < lookRange)
        {
            //Facing the player
            if (transform.position.x > playerRef.transform.position.x)
            {
                transform.localScale = new Vector2(-1f, 1f);
                notification.transform.localScale = new Vector2(-1f, 1f);
            }
            else
            {
                transform.localScale = Vector2.one;
                notification.transform.localScale = Vector2.one;
            }
            if (!isBuying)
            {
                // Displays dialogue when they player is within look range and is not buying (not opening the Buy menu)
                dialogueScript.canRandomChatter = true;
            }
            else
            {
                // Player is opening the Buy menu
                dialogueScript.canRandomChatter = false;
            }
            clairvoyance.SetActive(false);
        }
        else
        {
            clairvoyance.SetActive(true);
        }
        if (nearPlayer)
        {
            
            // Allows Player Interaction
            if (Input.GetKeyDown(KeyCode.E)) 
            {
                BuyUnbuy();
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            nearPlayer = true;
            notification.SetActive(true);
        }
        if (collision.CompareTag("Clairvoyance"))
        {
            clairvoyance.transform.position = playerRef.transform.position;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            nearPlayer = false;
            notification.SetActive(false);
        }
    }
    private void BuyUnbuy()
    {
        if (isBuying)
        {
            isBuying = false;
            PlayerController.instance.EPC = !isBuying;
            buyMenu.SetActive(isBuying);
            UIController.instance.SetIngameElements(true);
            CameraController.instance.CameraZoom(isBuying);
        }
        else
        {
            isBuying = true;
            PlayerController.instance.EPC = !isBuying;
            UIController.instance.SetIngameElements(false);
            buyMenu.SetActive(isBuying);
            CameraController.instance.CameraZoom(buyMenu.transform, isBuying, 3.9f);
        }
    }
}
