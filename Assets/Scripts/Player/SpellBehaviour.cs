using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpellBehaviour : MonoBehaviour
{
    private Transform target;
    private Vector3 direction;
    [SerializeField]
    private float speed = 6;
    [SerializeField]
    private float angleChangingSpeed = 400;
    [SerializeField]
    private Rigidbody2D spellRGBD;
    [SerializeField]
    private GameObject xplosionFX;
    public int dmgToGive;
    private static bool hasTarget = false;

    // Start is called before the first frame update
    void Start()
    {
        direction = transform.right;
        if (findClosestTarget() != null)
        {
            target = findClosestTarget().transform;
        }
        Destroy(gameObject, 6f); // Auto destroy after a while
    }

    // Update is called once per frame
    void Update()
    {
        // If no target is present at the time of casting, behave like a non-homing spell
        if (hasTarget && target != null)
        {
            direction = target.position - transform.position;
            direction.Normalize();
            float rotateAmount = Vector3.Cross(direction, transform.right).z;
            spellRGBD.angularVelocity = -angleChangingSpeed * rotateAmount;
            spellRGBD.velocity = transform.right * speed;
        }
        else if (!hasTarget) 
        {
            transform.position += direction * speed * Time.deltaTime;
        }
    }
    private GameObject findClosestTarget()
    {
        GameObject[] GO1s = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject[] GO2s = GameObject.FindGameObjectsWithTag("Enemy mBullet");
        GameObject[] GOs = GO1s.Concat(GO2s).ToArray();
        GameObject closest = null;
        hasTarget = false;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject go in GOs) 
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = go;
                distance = curDistance;
                hasTarget = true;
            }
        }
        return closest;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        
        Destroy(gameObject);
        EnemyController ec = other.GetComponent<EnemyController>();
        if (ec != null)
        {
            ec.damageEnemy(dmgToGive);
        }
    }
    private void OnDestroy()
    {
        Instantiate(xplosionFX, transform.position, transform.rotation);
    }
}
