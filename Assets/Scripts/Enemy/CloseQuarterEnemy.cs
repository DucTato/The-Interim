using Unity.VisualScripting;
using UnityEngine;

public class CloseQuarterEnemy : MonoBehaviour
{
    [SerializeField] private CQCDamageType damageType;
    [SerializeField] private float damageToDeal;
    private enum CQCDamageType
    {
        Magical = 0,
        Physical = 1
    }
    private void Start()
    {
        EnemyPathFindingBehaviour pb = GetComponent<EnemyPathFindingBehaviour>();
        if (pb != null)
        {
            if (pb.isRabid)
            damageToDeal *= 1.2f;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player mShield"))
        {
            collision.gameObject.GetComponent<mShieldScript>().ImpactShield(damageToDeal);
            return;
        }
        if (collision.gameObject.CompareTag("Player Shield"))
        {
            collision.gameObject.GetComponent<ShieldScript>().ImpactShield(damageToDeal);
            return;
        }
        if (collision.gameObject.CompareTag("Player"))
        {
            EnemyController ec = GetComponentInParent<EnemyController>();
            if (ec != null)
            {
                if (ec.isStunned)
                    return;
            }
            switch (damageType)
            {
                case CQCDamageType.Physical:
                    PlayerStatusSystem.instance.physDamage(damageToDeal);
                    break;
                case CQCDamageType.Magical:
                    PlayerStatusSystem.instance.magicDamage(damageToDeal);
                    break;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player mShield"))
        {
            collision.GetComponent<mShieldScript>().ImpactShield(damageToDeal);
            return;
        }
        if (collision.CompareTag("Player Shield"))
        {
            collision.GetComponent<ShieldScript>().ImpactShield(damageToDeal);
            return;
        }
        if (collision.CompareTag("Player"))
        {
            EnemyController ec = GetComponentInParent<EnemyController>();
            if (ec != null)
            {
                if (ec.isStunned)
                    return;
            }
            switch (damageType)
            {
                case CQCDamageType.Physical:
                    PlayerStatusSystem.instance.physDamage(damageToDeal);
                    break;
                case CQCDamageType.Magical:
                    PlayerStatusSystem.instance.magicDamage(damageToDeal);
                    break;
            }
        }
    }
}
