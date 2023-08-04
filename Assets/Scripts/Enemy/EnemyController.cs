using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private PlayerController playerRef;
    [SerializeField] protected float detectRange;
    [SerializeField] private float Health;
    [SerializeField] private GameObject rabidFX, coin;
    [SerializeField] private int coinReward, scoreReward;
   
    public bool isStunned;
    

    // Start is called before the first frame update
    void Start()
    {
        playerRef = PlayerController.instance;
        // Randomly pick a reward range (in coins)
        coinReward = Random.Range(coinReward, coinReward + 10);
        // Randomly decides if the monster is rabid or not upon spawning. Chance: 1 out of 5
        if(1 == Random.Range(0,6))
        {
            GetComponent<EnemyPathFindingBehaviour>().isRabid = true;
            rabidFX.SetActive(true);
            Health *= 0.5f;
            detectRange += 3f;
            coinReward += Mathf.RoundToInt((float) coinReward * .2f);
            scoreReward += Mathf.RoundToInt((float)scoreReward * .2f);
        }
        if(PlayerStatusSystem.instance.gameType == GameMode.ArenaMode)
        {
            // In Arena mode, Monster can always find you
            detectRange = float.PositiveInfinity;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(!isStunned)
        {
            if (Vector2.Distance(transform.position, playerRef.transform.position) < detectRange)
            {
                //Facing the player
                if (transform.position.x > playerRef.transform.position.x)
                {
                    transform.localScale = new Vector2(-1f, 1f);
                }
                else
                {
                    transform.localScale = Vector2.one;
                }
            }
        }
    }
    public void damageEnemy(float damage)
    {
        Health -= damage;
        // Hit FX
        if (Health <= 0)
        {
            // Spawning an item upon death
            GameObject drop = Instantiate(coin, transform.position, Quaternion.identity);
            drop.GetComponent<CoinScript>().coinAmount = coinReward;
            Destroy(gameObject);
            WaveController.instance.KillMonster(scoreReward);
        }
    }
    public void BashKnockBack(float bashForce,float knockBackRecovery)
    {
        StopCoroutine(KnockBackThenRecover(knockBackRecovery));
        StartCoroutine(KnockBackThenRecover(knockBackRecovery));
        if (bashForce > 0)
        {
            Rigidbody2D targetRB = GetComponent<Rigidbody2D>();
            targetRB.velocity = Vector2.zero;
            // Find the vector that represents the current position of the target and the player
            Vector2 knockDirection = transform.position - PlayerController.instance.transform.position;
            // Apply the force
            targetRB.AddForce(knockDirection * bashForce, ForceMode2D.Impulse);

        }
    }
    private IEnumerator KnockBackThenRecover(float recoverTime)
    {
        EnemyPathFindingBehaviour epb = GetComponent<EnemyPathFindingBehaviour>();
        EnemyShootingBehaviour esb = GetComponent<EnemyShootingBehaviour>();
        isStunned = true;
        epb.enabled = false;
        if (esb != null)
        {
            esb.enabled = false;
        }
        
        GetComponent<Animator>().SetBool("isMoving", false);
        yield return new WaitForSeconds(recoverTime);
        if (gameObject == null)
            yield break;
        epb.enabled = true;
        if (esb != null)
        {
            esb.enabled = true;
        }
        isStunned = false;
    }
}
