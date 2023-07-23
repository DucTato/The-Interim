using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WaveController : MonoBehaviour
{
    [SerializeField] private GameObject oletteVendor;
    [SerializeField] private BoxCollider2D[] spawners;
    public static WaveController instance;
    public WaveBehaviour[] Wave;
    [HideInInspector]
    public int currentNumberOfMonsters, currentWave, maxMonster, maxWave;
    private UIController uiRef;
    //private PlayerStatusSystem playerStats;
    private float timeCounter, typeLimit;
    //private string waveMessage;
    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        maxWave = Wave.Length;
        uiRef = UIController.instance;
        currentWave = 0;
        currentNumberOfMonsters= 0;
        maxMonster = TotalMonster(Wave[currentWave].numberOfMonsters);
        typeLimit = 1;
    }

    // Update is called once per frame
    void Update()
    {
       
        if (Wave[currentWave].isBuyPhase)
        {
            // If true then this is a buy phase. Do what a buy phase would do
            
            // Buy phase has a duration of 1.5 minutes
            Wave[currentWave].phaseDuration -= Time.deltaTime;
            // Calls UI to update the timer
            uiRef.DisplayWaveText(Wave[currentWave].waveName + "\n" + Mathf.RoundToInt(Wave[currentWave].phaseDuration));
            // End of phase condition
            if (Wave[currentWave].phaseDuration <= 0)
            {
                oletteVendor.SetActive(false);
                ContinueWave();
            }
        }
        else if (Wave[currentWave].waveEnd || Wave[currentWave].waveStart)
        {
            uiRef.DisplayWaveText(Wave[currentWave].waveName);
            // Start of a wave and End of a wave have different durations
            Wave[currentWave].phaseDuration -= Time.deltaTime;
                
            if (Wave[currentWave].phaseDuration <= 0)
            {
                ContinueWave();
            }
            // Calls UI to display Start/End of wave message
            if (Wave[currentWave].waveStart)
            {
                uiRef.DisplayGameMessage("Get Ready!", Wave[currentWave].phaseDuration);
            }
            else
            {
                if (currentWave == maxWave - 1)
                {
                    uiRef.DisplayGameMessage("Wave Completed\nYou Won!", Wave[currentWave].phaseDuration);
                }
                else
                    uiRef.DisplayGameMessage("Wave Completed!", Wave[currentWave].phaseDuration);
            }
        }
        else
        {
            // If not all of the above phases, it's action phase in which monsters should be spawned in
            uiRef.DisplayWaveText(Wave[currentWave].waveName + "\n" + currentNumberOfMonsters + "/" + maxMonster);
            if (timeCounter > 0)
            {
                timeCounter -= Time.deltaTime;
            }
            else
            {
                //Spawns monsters
                typeLimit += 0.02f;
                if (currentNumberOfMonsters < maxMonster)
                {
                    SpawnMonster(Wave[currentWave].monstersToSpawn, Wave[currentWave].numberOfMonsters);
                }
                timeCounter = Wave[currentWave].spawnTimeGap;
            }
            if (currentNumberOfMonsters >= maxMonster)
            {
                ContinueWave();
            }
        }
        
        
    }
    private int TotalMonster(int[] numberOfMonster)
    {
        int total = 0;
        for (int i = 0; i < numberOfMonster.Length; i++)
        {
            total += numberOfMonster[i];
        }
        return total;
    }
    private void ContinueWave()
    {
        // Reset the current number of slain monsters
        currentNumberOfMonsters = 0;
        typeLimit = 1;
        currentWave++;
        if (currentWave < maxWave)
        {
            uiRef.waveText.text = Wave[currentWave].waveName;
            // Reset the max number of monster to kill this wave
            maxMonster = TotalMonster(Wave[currentWave].numberOfMonsters);
            if (Wave[currentWave].isBuyPhase)
            {
                // Get the spawners' locations randomly
                int chosenSpawner = Random.Range(0, spawners.Length);
                // Inside the chosen spawner, spawn the pick ups randomly
                float randomX = Random.Range(spawners[chosenSpawner].bounds.min.x, spawners[chosenSpawner].bounds.max.x);
                float randomY = Random.Range(spawners[chosenSpawner].bounds.min.y, spawners[chosenSpawner].bounds.max.y);
                Vector2 randomPos = new Vector2(randomX, randomY);
                oletteVendor.transform.position = randomPos;
                oletteVendor.SetActive(true);
            }
        }
        else
        {
            // End of game reached
            //Debug.LogWarning("End of game reached");
            SceneManager.LoadScene("Main Menu");
        }
    }
    public void KillMonster()
    {
        currentNumberOfMonsters++;
    }
    private void SpawnMonster(GameObject[] spawningMonster, int[] monstersNumber)
    {
        // Pick the numbers of each type of Monster to spawn
        for (int i= 0; i < spawningMonster.Length; i++)
        {
            for (int j = 0; j < Mathf.RoundToInt(typeLimit); j++)
            {
                // Check if the current Monster Type still have monster number to spawn
                if (monstersNumber[i] > 0)
                {
                    PlaceInWorld(spawningMonster[i]);
                    monstersNumber[i]--;
                }
                //Move on to the next monster type
            }
        }
    }
    private void PlaceInWorld(GameObject monster)
    {
        // Get the spawners' locations randomly
        int chosenSpawner = Random.Range(0, spawners.Length);
        // Inside the chosen spawner, spawn the pick ups randomly
        float randomX = Random.Range(spawners[chosenSpawner].bounds.min.x, spawners[chosenSpawner].bounds.max.x);
        float randomY = Random.Range(spawners[chosenSpawner].bounds.min.y, spawners[chosenSpawner].bounds.max.y);
        Vector2 randomPos = new Vector2(randomX, randomY);
        // Instantiate the monster
        Instantiate(monster, randomPos, Quaternion.identity);
    }   
  
}
[System.Serializable]
public class WaveBehaviour
{
    
    [Header("This wave is:")]
    [Tooltip("Leave all of them unticked if it's Monster wave")]
    public bool isBuyPhase, waveEnd, waveStart;
    public string waveName;
    public GameObject[] monstersToSpawn;
    [Tooltip("Match this with the monsterToSpawn array")]
    public int[] numberOfMonsters;
    [Tooltip("How long between each time monsters get spawned in")]
    public float spawnTimeGap, phaseDuration;
    
}
