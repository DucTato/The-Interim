using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatusSystem : MonoBehaviour
{
    public static PlayerStatusSystem instance;

    public float currHealth, currSta, currMana;
    public float maxHealth, maxSta, maxMana;
    private float physResistMult;
    private float magResistMult;
    public bool notInvinc;
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
        magResistMult = 1f;
        physResistMult = 1f;

        UIController.instance.hpSlider.maxValue = maxHealth;
        UIController.instance.manaSlider.maxValue =maxMana;
        UIController.instance.staSlider.maxValue = maxSta;

        UIController.instance.manaSlider.value = currMana;
        UIController.instance.hpSlider.value = currHealth;
        UIController.instance.staSlider.value = currSta;

        notInvinc = true;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void magicDamage(float Damage)
    {
        if (notInvinc)
        {
            //magResist = 
            currHealth = currHealth - (Damage * magResistMult);
            UIController.instance.hpSlider.value = currHealth;
            if (currHealth <= 0)
            {
                PlayerController.instance.EPC = false;
                PlayerController.instance.gameObject.GetComponent<Animator>().SetTrigger("Death");
            }
        }
    }
    public void physDamage(float Damage) 
    {
        if (notInvinc)
        {
            //physResist = 
            currHealth = currHealth - (Damage * physResistMult);
            UIController.instance.hpSlider.value = currHealth;
            if (currHealth <= 0)
            {
                PlayerController.instance.EPC = false;
                PlayerController.instance.gameObject.GetComponent<Animator>().SetTrigger("Death");
            }
        }
    }
}
