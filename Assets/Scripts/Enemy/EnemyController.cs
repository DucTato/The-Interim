using System.Collections;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    protected PlayerController playerRef;
    [SerializeField] protected float detectRange;
    [SerializeField] protected float Health;
    [SerializeField] protected GameObject rabidFX, coin;
    [SerializeField] protected GameObject[] dropItems;
    [SerializeField] protected int coinReward, scoreReward;
   
    public bool isStunned, canRabid;
    

    // Start is called before the first frame update
    void Start()
    {
        playerRef = PlayerController.instance;
        GetComponent<EnemyPathFindingBehaviour>().SetFollowTarget(playerRef.transform);
        // Randomly pick a reward range (in coins)
        coinReward = Random.Range(coinReward, coinReward + 10);
        if (canRabid)
        {
            // Randomly decides if the monster is rabid or not upon spawning. Chance: 1 out of 5
            if (1 == Random.Range(0, 6))
            {
                GetComponent<EnemyPathFindingBehaviour>().isRabid = true;
                rabidFX.SetActive(true);
                Health *= 0.5f;
                detectRange += 3f;
                coinReward += Mathf.RoundToInt(coinReward * .2f);
                scoreReward += Mathf.RoundToInt(scoreReward * .2f);
            }
        }
        if(PlayerStatusSystem.instance.gameType == GameMode.ArenaMode)
        {
            // In Arena mode, Monsters can always find the Player
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
            // Spawning a Coin upon death
            GameObject drop = Instantiate(coin, transform.position, Quaternion.identity);
            drop.GetComponent<CoinScript>().coinAmount = coinReward;
            // Spawning an Item if it can
            if (dropItems.Length > 0)
            {
                for (int i = 0; i < dropItems.Length; i++)
                {
                    GameObject item = Instantiate(dropItems[i], transform.position, Quaternion.identity);
                }
            }
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
