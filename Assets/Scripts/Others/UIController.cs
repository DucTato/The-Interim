using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UI;


public class UIController : MonoBehaviour
{
    PlayerStatusSystem playerStat;
    public static UIController instance;
    public Slider hpSlider,manaSlider,staSlider;
    public GameObject sliderOutlines, pausePanel;
    public Text coinText;
    private float secondCounter, staminaCounter, manaCounter;

    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
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
                StartCoroutine(BlinkThenWait(manaSlider));
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
}
