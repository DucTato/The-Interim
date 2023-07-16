using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatusSystem : MonoBehaviour
{
    public static PlayerStatusSystem instance;
    PlayerController playerRef;
    UIController uiRef;

    public float currHealth, currSta, currMana, staminaRecovery, manaRecovery;
    public float maxHealth, maxSta, maxMana, recoveryMult;
    public bool notInvinc, regenStamina, regenMana, hasVigor, isPaused;
    public int currentCoins;
    private float staSecondCounter, manaSecondCounter, staminaCooldown, manaCooldown;

    //public float currMana;
    //public float currSta;

    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        currHealth = maxHealth;
        currSta = maxSta;
        currMana = maxMana;
        currentCoins = 0;
        uiRef = UIController.instance;
        playerRef = PlayerController.instance;
        // Update new maxes
        uiRef.UpdateNewMax(maxHealth, maxMana, maxSta);
        // Update current values
        uiRef.UpdateLocalUI(currHealth, currMana, currSta);
        uiRef.SetCoinText(currentCoins);
        notInvinc = true;
        regenStamina = false; 
        regenMana = false;
        hasVigor = false;
        staSecondCounter = 0;
        manaSecondCounter = 0;
        recoveryMult = 1f;
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckAndRegenStamina();
        CheckAndRegenMana();
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            PauseUnpause();
        }
    }
    public void magicDamage(float Damage)
    {
        if (notInvinc)
        {
            // Magical Resistance is taken into account when dealing magical damage 
            currHealth -= (Damage * playerRef.magResistMult);
            // Update UI 
            uiRef.hpSlider.value = currHealth;
            if (currHealth <= 0)
            {
                playerRef.EPC = false;
                playerRef.gameObject.GetComponent<Animator>().SetTrigger("Death");
            }
        }
    }
    public void physDamage(float Damage) 
    {
        if (notInvinc)
        {
            // Physical Resistance is taken into account when dealing physical damage 
            currHealth -= (Damage * playerRef.physResistMult);
            // Update UI
            uiRef.hpSlider.value = currHealth;
            if (currHealth <= 0)
            {
                playerRef.EPC = false;
                playerRef.gameObject.GetComponent<Animator>().SetTrigger("Death");
            }
        }
    }
    public bool CheckStaminaThenPerform(float cost)
    {
        if (currSta - cost >= 0 && !regenStamina) 
        {
            return true;
        }
        else
        {
            // Calls UI function to reflect this and let the Player know
            uiRef.BlinkBar(2);
            return false;
        }  
    }
    public bool CheckManaThenPerfrom(float cost)
    {
        if (currMana - cost >= 0 && !regenMana)
        {
            return true;
        }
        else
        {
            // Calls UI function to feflect this and let the Player know
            uiRef.BlinkBar(1);
            return false;
        }
    }    
    public void ConsumeMana(float amount)
    {
        currMana -= amount;
        uiRef.manaSlider.value = currMana;
        manaCooldown = 2f;
        if (currMana <= 0)
        {
            regenMana = true;
        }
    }
    public void ConsumeStamina(float amount)
    {
        currSta -= amount;
        uiRef.staSlider.value = currSta;
        staminaCooldown = 2f;
        if (currSta <= 0)
        {
            regenStamina = true;
        }
    }
    private void CheckAndRegenMana()
    {
        if (currMana < maxMana)
        {
            if (manaCooldown > 0)
            {
                manaCooldown -= Time.deltaTime;
            }
            else
            {
                // Starts regenerating Mana after 2 seconds since the last action
                if (manaSecondCounter > 0)
                {
                    manaSecondCounter -= Time.deltaTime;
                }
                else
                {
                    currMana += manaRecovery * recoveryMult;
                    if (currMana >= maxMana * .2f)
                    {
                        regenMana = false;
                    }
                    if (currMana >= maxMana)
                    {
                        currMana = maxMana;
                    }
                    uiRef.manaSlider.value = currMana;
                    manaSecondCounter = 1f;
                }
            }
        }
    }
    private void CheckAndRegenStamina()
    {
        if (currSta < maxSta)
        {
            if (staminaCooldown > 0)
            {
                staminaCooldown -= Time.deltaTime;
            }
            else
            {
                //Starts regenerating Stamina after 2 seconds since the last action
                if (staSecondCounter > 0)
                {
                    staSecondCounter -= Time.deltaTime;
                }
                else
                {
                    currSta += staminaRecovery * recoveryMult;
                    if (currSta >= maxSta * .2f)
                    {
                        regenStamina = false; // At 20% of the Maximum value, the play may be able to perform actions again
                    }
                    if (currSta >= maxSta)
                    {
                        currSta = maxSta; // Caps off the value so that it won't overflow :v
                        
                    }
                    uiRef.staSlider.value = currSta;
                    staSecondCounter = 1f;
                }
            }
        }
    }
    public void HealPlayer(float amount)
    { 
        currHealth += amount * recoveryMult;
        if (currHealth >= maxHealth)
        {
            currHealth = maxHealth;
        }
        uiRef.hpSlider.value = currHealth;
    }
    public void RecoverMana(float amount)
    {
        currMana += amount * recoveryMult;
        if (currMana >= maxMana)
        {
            currMana = maxMana;
        }
        uiRef.manaSlider.value = currMana;
    }
    public void RecoverStamina(float amount)
    {
        currSta += amount * recoveryMult;
        if (currSta >= maxSta)
        {
            currSta = maxSta;   
        }
        uiRef.staSlider.value = currSta;
    }  
    public void VigorEffect(float duration)
    {
        StartCoroutine(VigorDuration(duration));
    }
    private IEnumerator VigorDuration(float time)
    {
        hasVigor = true;
        recoveryMult *= 2;
        uiRef.GoldenOutline(hasVigor);
        yield return new WaitForSeconds(time);
        hasVigor = false;
        recoveryMult /= 2;
        uiRef.GoldenOutline(hasVigor);
    }
    public void AddCoins(int amount)
    {
        currentCoins += amount;
        uiRef.SetCoinText(currentCoins);
    }
    public void RemoveCoins(int amount)
    {
        currentCoins -= amount;
        uiRef.SetCoinText(currentCoins);
    }
    private void PauseUnpause ()
    {
        if (!isPaused)
        {
            uiRef.pausePanel.SetActive(true);

            isPaused = true;
            playerRef.EPC = false;
            playerRef.followMouse = false;
            Time.timeScale = 0f;
        }
        else
        {
            uiRef.pausePanel.SetActive(false);

            isPaused = false;
            playerRef.EPC = true;
            playerRef.followMouse = true;   
            Time.timeScale = 1f;
        }
    }
}
