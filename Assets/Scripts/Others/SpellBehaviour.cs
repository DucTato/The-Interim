using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpellBehaviour : MonoBehaviour
{
    private Transform target;
    private Vector2 direction;
    [SerializeField]
    private float speed = 6;
    [SerializeField]
    private float angleChangingSpeed = 400;
    [SerializeField]
    private Rigidbody2D spellRGBD;
    [SerializeField]
    private GameObject xplosionFX;

    // Start is called before the first frame update
    void Start()
    {
        target = findClosestTarget().transform;
    }

    // Update is called once per frame
    void Update()
    {
        // If no target is present at the time of casting, behave like a non-homing spell
        if (target != null)
        {
            direction = target.position - transform.position;
            direction.Normalize();
            float rotateAmount = Vector3.Cross(direction, transform.right).z;
            spellRGBD.angularVelocity = -angleChangingSpeed * rotateAmount;
            spellRGBD.velocity = transform.right * speed;
        }
        else
        {
            spellRGBD.velocity = transform.right * speed;
        }
    }
    private GameObject findClosestTarget()
    {
        GameObject[] GO1s = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject[] GO2s = GameObject.FindGameObjectsWithTag("Enemy mBullet");
        GameObject[] GOs = GO1s.Concat(GO2s).ToArray();
        GameObject closest = null;
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
            }
        }
        return closest;
    }
}
