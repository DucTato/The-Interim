using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OletteScript : MonoBehaviour
{
    public static OletteScript instance;
    private PlayerController playerRef;
    [SerializeField] private GameObject notification, dialogue, clairvoyance;
    [SerializeField] private float lookRange;
    private bool nearPlayer, isBuying;
    //private string oletteMessage;
    private void Awake()
    {
        instance = this; 
    }
   
    private void OnEnable()
    {
        playerRef = PlayerController.instance;
        clairvoyance.transform.position = playerRef.transform.position;
        clairvoyance.SetActive(true);
        nearPlayer = false;
        isBuying = false;
    }
    private void OnDisable()
    {
        clairvoyance.SetActive(false);
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
                dialogue.SetActive(true);
            }
            else
            {
                // Player is opening the Buy menu
                dialogue.SetActive(false);
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
                isBuying = true;
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
            //StartCoroutine(ClairvoyanceEffect());
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
    //private IEnumerator ClairvoyanceEffect()
    //{
    //    clairvoyance.GetComponent<TrailRenderer>().enabled = false;
    //    clairvoyance.transform.position = playerRef.transform.position;
    //    yield return new WaitForSeconds(0.5f);
    //    clairvoyance.GetComponent<TrailRenderer>().enabled = true;
    //}
}
