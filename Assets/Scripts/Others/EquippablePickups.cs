using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class EquippablePickups : MonoBehaviour
{
    [SerializeField] private Equippables itemToEquip;
    [SerializeField] private SpriteRenderer itemSR;
    [SerializeField] private GameObject item;
    private Vector3 dropDirection;
    private Animator animator;
    private float waitTime;
    private bool pickedUp;
    // Start is called before the first frame update
    void Start()
    {
        dropDirection.x = Random.Range(-3f, 3f);
        dropDirection.y = Random.Range(-3f, 3f);
        animator = GetComponentInChildren<Animator>();
        pickedUp = false;
        waitTime = 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += dropDirection * Time.deltaTime;
        dropDirection = Vector3.Lerp(dropDirection, Vector3.zero, 5f * Time.deltaTime);
        if (waitTime > 0) 
        {
            waitTime -= Time.deltaTime;
        }
        if (pickedUp)
        {
            itemSR.color = Color.Lerp(itemSR.color, Color.clear, Time.deltaTime);
            item.transform.position += transform.up * 1.5f * Time.deltaTime;
            if (waitTime < 0)
            {
                Destroy(gameObject);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController pc = collision.GetComponent<PlayerController>();
        if (pc != null && waitTime < 0 && pc.availEquipment.Count < 3)
        {
                animator.StopPlayback();
                GetComponent<CircleCollider2D>().enabled = false;
                pc.AddEquipment(itemToEquip);
                PickUpProcedure();
        }
    }
    private void PickUpProcedure()
    {
        float randomAngle = Random.Range(-90, 90);
        item.transform.rotation = Quaternion.Euler(0f,0f,randomAngle);
        waitTime = 0.5f;
        pickedUp = true;
    }
}
