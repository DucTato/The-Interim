using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShootingBehaviour : MonoBehaviour
{
    private Vector2 aimDirection;
    private Quaternion rotationAngle;
    [SerializeField]
    private Transform rotationPoint;
    [SerializeField]
    private float rotationSpeed;
    [SerializeField] 
    protected float shootRange;
    [SerializeField] private float Delay;
    private float shotCounter;
    [SerializeField] 
    private int BurstSize;
    [SerializeField] 
    private float fireRate;
    [SerializeField] 
    private GameObject spellToCast;
    [SerializeField] 
    private Transform shootPoint;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(transform.position, PlayerController.instance.transform.position) < shootRange)
        {
            // Enemies that can shoot at the player will be able to "aim" at the player
            aimDirection = (PlayerController.instance.transform.position - rotationPoint.position).normalized;
            float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * 57.295f - 90f;
            rotationAngle = Quaternion.AngleAxis(angle, Vector3.forward);
            rotationPoint.rotation = Quaternion.Slerp(rotationPoint.rotation, rotationAngle, Time.deltaTime * rotationSpeed);
            // Enemies will shoot when the Player is within shoot range
            if (shotCounter > 0)
            {
                shotCounter -= Time.deltaTime;
            }
            else
            {
                StartCoroutine(brstSpell(BurstSize));
                shotCounter = Delay;
            }
        }
        else
        {
            rotationPoint.rotation = Quaternion.Slerp(rotationPoint.rotation, Quaternion.Euler(Vector2.zero), Time.deltaTime * rotationSpeed);
        }
            
    }
    private IEnumerator brstSpell(int BurstSize)
    {
        for (int i = 0; i < BurstSize; i++)
        {
            Instantiate(spellToCast, shootPoint.position, shootPoint.rotation);
            yield return new WaitForSeconds(60f / fireRate);
        }
    }
}
