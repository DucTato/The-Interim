using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainMenuScript : MonoBehaviour
{
    public static MainMenuScript instance;
    [SerializeField] private Image whiteFade;
    [SerializeField] private float fadeSpeed, waitTime, titleFadeSpeed;
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private GameObject interactables, arenaPanel;
    public bool fadeIn, fadeOut;
    private bool firstStart;
    private void Awake()
    {
        instance = this;
    }
   
    // Start is called before the first frame update
    void Start()
    {
        fadeIn = false;
        fadeOut = true;
        firstStart = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (fadeOut)
        {
            whiteFade.color = new Color(whiteFade.color.r, whiteFade.color.g, whiteFade.color.b, Mathf.MoveTowards(whiteFade.color.a, 0f, fadeSpeed * Time.deltaTime));
            if(whiteFade.color.a == 0f)
            {
                fadeOut = false;
                whiteFade.gameObject.SetActive(false);
            }
        } 
        if (!whiteFade.gameObject.activeInHierarchy)
        {
            if (waitTime> 0f)
            {
                waitTime -= Time.deltaTime;
            }
            else
            {
                title.color = new Color(title.color.r, title.color.g, title.color.b, Mathf.MoveTowards(title.color.a, 1f, titleFadeSpeed * Time.deltaTime));
                if (title.color.a == 1f && firstStart)
                {
                    firstStart = false;
                    interactables.SetActive(true);
                }
            }
        }
        if (fadeIn)
        {
            whiteFade.gameObject.SetActive(true);
            whiteFade.color = new Color(whiteFade.color.r, whiteFade.color.g, whiteFade.color.b, Mathf.MoveTowards(whiteFade.color.a, 1f, fadeSpeed * Time.deltaTime));
            if(whiteFade.color.a == 1f)
            {
                fadeIn = false;
            }
        }
    }
    public void ToggleInteractables()
    {
        if (interactables.activeInHierarchy)
        {
            interactables.SetActive(false);
        }
        else
            interactables.SetActive(true);
    }
    public void ArenaModeButton()
    {
        ToggleInteractables();
        arenaPanel.SetActive(true);
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}
