using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossBehaviour : EnemyController
{
    private EnemyPathFindingBehaviour pathFinder;
    private EnemyShootingBehaviour shootBehaviour;
    [SerializeField] private GameObject onScreenStatus;
    [SerializeField] private Slider osHealthSlider;
    [SerializeField] private int currentAction, currentSequence;
    [SerializeField] private BossSequence[] sequences;
    private BossAction[] currentActions;
    private float actionCounter, meleeCounter, secondCounter;
    private Animator anim;
    
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        pathFinder = GetComponent<EnemyPathFindingBehaviour>();
        shootBehaviour = GetComponent<EnemyShootingBehaviour>();
        osHealthSlider.maxValue = Health;
        UpdateOnScreenHealth(Health);
        playerRef = PlayerController.instance;
        GetComponent<EnemyPathFindingBehaviour>().SetFollowTarget(playerRef.transform);
        if (PlayerStatusSystem.instance.gameType == GameMode.ArenaMode)
        {
            // In Arena mode, Monsters can always find the Player
            detectRange = float.PositiveInfinity;
        }
        currentSequence = 0;
        currentActions = sequences[currentSequence].actionsOfThisSequence;
        ChangeCurrentAction();
        secondCounter = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(transform.position, playerRef.transform.position) < detectRange)
        {
            //Facing the player when within detect range
            if (transform.position.x > playerRef.transform.position.x)
            {
                transform.localScale = new Vector2(-1f, 1f);
                onScreenStatus.transform.localScale = new Vector2(-1f, 1f);
            }
            else
            {
                transform.localScale = Vector2.one;
                onScreenStatus.transform.localScale = Vector2.one;
            }
        }
        if (meleeCounter > 0)
        {
            meleeCounter -= Time.deltaTime;
        }
        ////BOSS SEQUENCE CONTROLLER///////////////////////////////////////////////////////////////////////////////////
        if (actionCounter > 0)
        {
            actionCounter -= Time.deltaTime;
        }
        else
        {
            // Duration of the current action is over, change action
            ChangeCurrentAction();
        }
        if (Vector2.Distance(transform.position, playerRef.transform.position) < currentActions[currentAction].meleeRange)
        {
            DoMeleeAnimation();
        }
        if (currentActions[currentAction].canSpawnMinions)
        {
            if (secondCounter > 0)
            {
                secondCounter -= Time.deltaTime;
            }
            else
            {
                // Spawns minions every x seconds during this action
                StartCoroutine(SpawnThenWait(1f));
                secondCounter = currentActions[currentAction].spawnGap;
            }
        }
        
    }
    private IEnumerator SpawnThenWait(float time)
    {
        for (int i = 0; i < currentActions[currentAction].numberOfMinions; i++)
        {
            Instantiate(currentActions[currentAction].minionsToSpawn, transform.position, Quaternion.identity);
            yield return new WaitForSeconds(time);
        }
    }
    private void UpdateOnScreenHealth(float currentHP)
    {
        osHealthSlider.value = currentHP;
    }
    public void TakeDamage(float damage)
    {
        Health -= damage;
        UpdateOnScreenHealth(Health);
        
        if (Health <= 0)
        {
            // Death sequence
            // Spawning a Coin upon death
            GameObject drop = Instantiate(coin, transform.position, Quaternion.identity);
            drop.GetComponent<CoinScript>().coinAmount = coinReward;
            if (dropItems.Length > 0)
            {
                for (int i = 0; i <dropItems.Length; i++) 
                {
                    Instantiate(dropItems[i], transform.position, Quaternion.identity); 
                }
            }
            WaveController.instance.KillMonster(scoreReward);
            gameObject.SetActive(false);
        }
        else
        {
            if (Health <= sequences[currentSequence].healthThreshold)
            {
                //Reached the current HP threshold, switch to the next sequence (phase)
                currentSequence++;
                currentActions = sequences[currentSequence].actionsOfThisSequence;
                ChangeCurrentAction();
            }
        }
    }
    private void ChangeCurrentAction()
    {
        Debug.Log("Change action");
        
        anim.SetBool("isSpawning", false);
        
        currentAction = Random.Range(0, currentActions.Length);
        anim.SetBool("isMoving", currentActions[currentAction].canMove);
        if (currentActions[currentAction].canSpawnMinions)
        {
            anim.SetBool("isSpawning", true);
        }
        actionCounter = currentActions[currentAction].actionDuration;
        pathFinder.enabled = currentActions[currentAction].canMove;
        if (!currentActions[currentAction].canMove)
        {
            // if the boss can't move during current action, also can not wander around
            currentActions[currentAction].canWander = false;
        }
        else
        {
            pathFinder.ChangeMoveSpeed(currentActions[currentAction].moveSpeed);
        }
        shootBehaviour.enabled = currentActions[currentAction].canShoot;
        if (currentActions[currentAction].canShoot)
        {
            // Set up bullets, fire rate, etc...
            shootBehaviour.ChangeFireRate(currentActions[currentAction].burstSize, currentActions[currentAction].ROF, currentActions[currentAction].shotDelay);
            shootBehaviour.ChangeRotationSpeed(currentActions[currentAction].rotationSpeed);
            shootBehaviour.ChangeCurrentCastingSpell(currentActions[currentAction].ammoToShoot);
        }
    }
    private void DoMeleeAnimation()
    {
        if (meleeCounter <= 0)
        {
            if (currentActions[currentAction].canMeleeWhileMove)
            {
                // Plays the animation
                anim.SetTrigger("meleeLight");
            }
            else
            {
                pathFinder.enabled = false;
                // Plays the animation
                anim.SetTrigger("meleeLight");
            }
            meleeCounter = currentActions[currentAction].meleeCooldown;
        }
    }
    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.gameObject.CompareTag("Player") && !GetComponent<EnemyController>().isStunned)
    //    {
    //        // Do the melee animation
    //        DoMeleeAnimation();            
    //    }
    //}
    
    private void OnMeleeStart()
    {
        //meleeHitBox.enabled = true;
    }
    private void OnMeleeStop()
    {
        pathFinder.enabled = true;
        //meleeHitBox.enabled = false;
    }
}
[System.Serializable]
public class BossAction
{
    [Header("Action")]

    public float actionDuration;
    public float meleeRange, meleeCooldown, shotDelay, rotationSpeed, moveSpeed, spawnGap;
    [Tooltip("Rate of Fire, higher means more shots are fired per minute")]
    public float ROF;
    public int burstSize;
    [Tooltip("Number of minions each spawn")]
    public int numberOfMinions;
    public bool canShoot, canMove, canWander, canSpawnMinions, canMeleeWhileMove;
    public GameObject ammoToShoot, minionsToSpawn;
}
[System.Serializable]
public class BossSequence
{
    [Header("Sequence")]

    public BossAction[] actionsOfThisSequence;
    [Tooltip("At what Health percentage will the Boss start changing to the next action sequences?")]
    public float healthThreshold;
}
