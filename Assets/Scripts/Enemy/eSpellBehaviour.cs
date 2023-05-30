using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class eSpellBehaviour : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float angleChangingSpeed;
    [SerializeField] private Rigidbody2D RGBD;
    private Vector3 moveDirection;
    private int isHoming;

    // Start is called before the first frame update
    void Start()
    {
        isHoming = Random.Range(0, 3); // Randomly decide if this spell should be Homing or not. Odd: 1 out of 3
        if (isHoming != 1) 
        {
            moveDirection = PlayerController.instance.transform.position - transform.position;
            moveDirection.Normalize();
        }
        else
        {
            transform.gameObject.tag = "Enemy mBullet"; // Change the tag of the spell object if it is Homing type
            Destroy(gameObject, 6f); // Auto destroy after a while
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isHoming == 1)
        {
            moveDirection = PlayerController.instance.transform.position - transform.position;
            moveDirection.Normalize();
            float rotateAmount = Vector3.Cross(moveDirection, transform.right).z;
            RGBD.angularVelocity = -angleChangingSpeed * rotateAmount;
            RGBD.velocity = transform.right * speed;
        }
        transform.position += moveDirection * speed * Time.deltaTime; 
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        Destroy(gameObject);
        //if (other.tag == "Player")
        //{
        //    other.GetComponent<EnemyController>().damageEnemy(dmgToGive);
        //}
    }
    private void OnBecameInvisible()
    {
        // Impact FX
    }
}
