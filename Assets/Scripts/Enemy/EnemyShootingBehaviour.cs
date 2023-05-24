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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(transform.position, PlayerController.instance.transform.position) < shootRange)
        {
            //Enemy that can shoot at the player will be able to "aim" at the player
            aimDirection = (PlayerController.instance.transform.position - rotationPoint.position).normalized;
            float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * 57.295f - 90f;
            rotationAngle = Quaternion.AngleAxis(angle, Vector3.forward);
            rotationPoint.rotation = Quaternion.Slerp(rotationPoint.rotation, rotationAngle, Time.deltaTime * rotationSpeed);
        }
        else
        {
            rotationPoint.rotation = Quaternion.Slerp(rotationPoint.rotation, Quaternion.Euler(Vector2.zero), Time.deltaTime * rotationSpeed);
        }
            
    }
}
