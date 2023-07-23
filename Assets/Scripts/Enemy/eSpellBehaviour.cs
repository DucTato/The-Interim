using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class eSpellBehaviour : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float angleChangingSpeed;
    [SerializeField] private Rigidbody2D RGBD;
    [SerializeField] private GameObject xplosionFX;
    [SerializeField] private float Damage;
    [SerializeField] private ParticleSystem spellEffect;
    private Vector3 moveDirection;
    private PlayerController playerRef;
    private int isHoming;

    // Start is called before the first frame update
    void Start()
    {
        //spellEffect.Play();
        playerRef = PlayerController.instance;

        isHoming = Random.Range(0, 3); // Randomly decide if this spell should be Homing or not. Odd: 1 out of 3
        if (isHoming != 1) 
        {
            moveDirection = playerRef.transform.position - transform.position;
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
            moveDirection = playerRef.transform.position - transform.position;
            moveDirection.Normalize();
            float rotateAmount = Vector3.Cross(moveDirection, transform.right).z;
            RGBD.angularVelocity = -angleChangingSpeed * rotateAmount;
            RGBD.velocity = transform.right * speed;
        }
        transform.position += moveDirection * speed * Time.deltaTime; 
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        Instantiate(xplosionFX, transform.position, Quaternion.identity);   
        //spellEffect.Play();
        Destroy(gameObject);
        if (other.CompareTag("Player"))
        {
            PlayerStatusSystem.instance.magicDamage(Damage);
        }
        if (other.CompareTag("Player mShield"))
        {
            other.GetComponent<mShieldScript>().ImpactShield(Damage);
        }
        if (other.CompareTag("Player Shield"))
        {
            other.GetComponent<ShieldScript>().ImpactShield(Damage);    
        }
    }
   
}
