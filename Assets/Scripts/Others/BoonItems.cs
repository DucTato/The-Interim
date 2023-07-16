using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class BoonItems : MonoBehaviour
{
    public BoonItemType type;
    [SerializeField] private Sprite[] spriteImage;
    private Light2D glowTint;
    private SpriteRenderer itemSR;
    private float pickupTime;
    // Start is called before the first frame update
    void Start()
    { 
        glowTint = GetComponent<Light2D>();
        itemSR = GetComponent<SpriteRenderer>();
        if (type == BoonItemType.HealthVial || type == BoonItemType.HealthPotion)
        {
            glowTint.color = Color.red;
        }
        else if (type == BoonItemType.ManaVial || type == BoonItemType.ManaPotion)
        {
            glowTint.color = Color.blue;
        }
        else if (type == BoonItemType.StaminaVial || type == BoonItemType.StaminaPotion)
        {
            glowTint.color = Color.green;
        }
        else
        {
            glowTint.color = Color.yellow;
        }
        itemSR.sprite = spriteImage[(int)type];
        pickupTime = 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        pickupTime -= Time.deltaTime;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && pickupTime < 0)
        {
            Boon(type);
        }
    }
    private void Boon(BoonItemType boon)
    {
        switch (boon)
        {
            case BoonItemType.HealthVial:
                PlayerStatusSystem.instance.HealPlayer(20);
                break;
            case BoonItemType.HealthPotion:
                PlayerStatusSystem.instance.HealPlayer(50);
                break;
            case BoonItemType.ManaVial:
                PlayerStatusSystem.instance.RecoverMana(20);
                break;
            case BoonItemType.ManaPotion:
                PlayerStatusSystem.instance.RecoverMana(50);
                break;
            case BoonItemType.StaminaVial: 
                PlayerStatusSystem.instance.RecoverStamina(20);
                break;
            case BoonItemType.StaminaPotion:
                PlayerStatusSystem.instance.RecoverStamina(50);
                break;
            case BoonItemType.VigorVial:
                if (PlayerStatusSystem.instance.hasVigor)
                {
                    return;
                }
                else
                {
                    PlayerStatusSystem.instance.VigorEffect(20);
                    break;
                }
            case BoonItemType.VigorPotion:
                if (PlayerStatusSystem.instance.hasVigor)
                {
                    return;
                }
                else
                {
                    PlayerStatusSystem.instance.VigorEffect(40);
                    break;
                } 
        }
        Destroy(gameObject);
    }
}
public enum BoonItemType
{
    HealthVial = 0,
    HealthPotion = 1,
    ManaVial = 2,
    ManaPotion = 3,
    StaminaVial = 4,
    StaminaPotion = 5,
    VigorVial = 6,
    VigorPotion = 7
}
