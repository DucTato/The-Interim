using UnityEngine;

public class PoisionPuddle : MonoBehaviour
{
    [SerializeField] private Sprite[] puddleSprites;
    private void Start()
    {
        GetComponent<SpriteRenderer>().sprite = puddleSprites[Random.Range(0, puddleSprites.Length)];
        Destroy(gameObject, 25f);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerStatusSystem.instance.physDamage(10);
        }
    }
}
