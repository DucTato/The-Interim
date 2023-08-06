using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class UIController : MonoBehaviour
{
    PlayerStatusSystem playerStat;
    public static UIController instance;
    public Slider hpSlider,manaSlider,staSlider;
    public GameObject sliderOutlines, pausePanel, ingamePanel, deathPanel;
    public Text coinText, waveText, gameMessage;
    public TextMeshProUGUI itemName, currentName;
    [SerializeField] private Sprite bareHand;
    public Image equipmentImage, whiteFadeScreen, avatarImage;
    private float secondCounter, staminaCounter, manaCounter, messageCounter;
    private bool fadeIn, fadeOut;

    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        fadeOut = true;
        fadeIn = false;
        currentName.text = CharacterTracker.instance.currentCharacterName;
        avatarImage.sprite = CharacterTracker.instance.avatarInGame;
        playerStat = PlayerStatusSystem.instance;
        secondCounter = 0;
        staminaCounter = 0;
        manaCounter = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(playerStat.currHealth < playerStat.maxHealth * 0.15f)
        {
            if (secondCounter > 0)
            {
                secondCounter -= Time.deltaTime;
            }
            else
            {
                BlinkBar(0);
                secondCounter = 2f; // Blinks the HP bar every 2 seconds if the current health is low (<15% max health)
            }
        }
        if (staminaCounter > 0)
        {
            staminaCounter -= Time.deltaTime;
        }
        if (manaCounter > 0)
        {
            manaCounter -= Time.deltaTime;
        }
        // Fade screen logics
        if (fadeOut)
        {
            // 0.006f = 0.02f * 0.3f, is the speed similar to Time.deltaTime * 0.3f. During the match, the fade needs to be able to function even with timescale = 0.
            whiteFadeScreen.color = new Color(whiteFadeScreen.color.r, whiteFadeScreen.color.g, whiteFadeScreen.color.b, Mathf.MoveTowards(whiteFadeScreen.color.a, 0f, 0.006f));
            if (whiteFadeScreen.color.a == 0f)
            {
                fadeOut = false;
                whiteFadeScreen.gameObject.SetActive(false);
            }
        }
        if (fadeIn)
        {
            whiteFadeScreen.gameObject.SetActive(true);
            whiteFadeScreen.color = new Color(whiteFadeScreen.color.r, whiteFadeScreen.color.g, whiteFadeScreen.color.b, Mathf.MoveTowards(whiteFadeScreen.color.a, 1f, 0.006f));
            if (whiteFadeScreen.color.a == 1f)
            {
                fadeIn = false;
            }
        }
    }
    public void UpdateNewMax(float maxHP, float maxMP, float maxSP)
    {
        hpSlider.maxValue = maxHP;
        manaSlider.maxValue = maxMP;
        staSlider.maxValue = maxSP;
    }
    public void UpdateLocalUI(float HP, float MP, float SP)
    {
        hpSlider.value = HP; 
        manaSlider.value = MP; 
        staSlider.value = SP;
    }
    public void BlinkBar(int type)
    {
        // Which bar to blink? 0 = HP, 1 = Mana, 2 = Stamina
        switch (type)
        {
            case 0:
                StartCoroutine(BlinkThenWait(hpSlider));
                break;
            case 1:
                if (manaCounter <= 0)
                {
                    StartCoroutine(BlinkThenWait(manaSlider));
                    manaCounter = 2f;
                }
                break;
            case 2:
                if (staminaCounter <= 0) 
                {
                    StartCoroutine(BlinkThenWait(staSlider));
                    staminaCounter = 2f;
                }
                break;
        }
    }
    
    private IEnumerator BlinkThenWait(Slider bar)
    {
        for (int i = 0; i < 5; i++)
        {
            bar.fillRect.localScale = Vector2.zero;
            yield return new WaitForSeconds(0.2f);
            bar.fillRect.localScale = Vector2.one;
            yield return new WaitForSeconds(0.2f);
        }
    }
    public void GoldenOutline(bool state)
    {
       sliderOutlines.SetActive(state);
    }
    public void SetCoinText(int coin)
    {
        if (coin >= 999999999)
        {
            coin = 999999999;
        }
        coinText.text = coin.ToString();
    }
    public void DisplayGameMessage(string message, float phaseDuration)
    {
        gameMessage.text = message;
        if (messageCounter > 0)
        {
            messageCounter -= Time.deltaTime;
        }
        else
        {
            StartCoroutine(FlashMessage());
            messageCounter = phaseDuration;
        }
    }
    public void DisplayWaveText(string message)
    {
        waveText.text = message;        
    }
    private IEnumerator FlashMessage()
    {
        for (int i = 0; i < 3; i++) 
        {
            gameMessage.color = new Color(1f, 1f, 1f, 1f);
            yield return new WaitForSeconds(0.5f);
            gameMessage.color = Color.clear;
            yield return new WaitForSeconds(0.5f);
        }
    }
    public void ReturnToMenuButton()
    {
        //Time.timeScale = 1f;
        //SceneManager.LoadScene("Main Menu");
        CharacterTracker.instance.DestroyCurrentCharacter();
        WaveController.instance.EndOfGameProcedure("Main Menu");
    }
    public void RetryButton()
    {
        WaveController.instance.EndOfGameProcedure(CharacterTracker.instance.currentMap);
    }
    public void UpdateEquipmentUI(Equippables item)
    {
        itemName.text = item.equipmentName;
        equipmentImage.sprite = item.equipmentUiSprite;
    }
    public void SetUIBareHand()
    {
        equipmentImage.sprite = bareHand;
        itemName.text = "Bare hand";
    }
    public void SetPausePanel(bool state)
    {
        pausePanel.SetActive(state);
    }
    public void SetIngameElements(bool state)
    {
        if(state)
        {
            // Updates the values before enabling them all
            hpSlider.maxValue = playerStat.maxHealth;
            hpSlider.value = playerStat.currHealth;
            manaSlider.maxValue = playerStat.maxMana;
            manaSlider.value = playerStat.currMana;
            staSlider.maxValue = playerStat.maxSta;
            staSlider.value = playerStat.currSta;
        }
        ingamePanel.SetActive(state);
    }
    public void StartFadeIn()
    {
        fadeIn = true;
    }
}
