
using UnityEngine;

public class DeadBodies : MonoBehaviour
{
    public Sprite[] deadbodies;
    private SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = deadbodies[Random.Range(0, deadbodies.Length)];
    }
}
    