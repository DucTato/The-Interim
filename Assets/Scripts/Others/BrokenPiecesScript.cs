using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenPiecesScript : MonoBehaviour
{
    [SerializeField] private float flySpeed, deceleration, fadeSpeed, lifeTime;
    private SpriteRenderer pieceSR;
    private Vector3 flyDirection;
    // Start is called before the first frame update
    private void OnEnable()
    {
        pieceSR= GetComponent<SpriteRenderer>();
        flyDirection.x = Random.Range(-flySpeed, flySpeed);
        flyDirection.y = Random.Range(-flySpeed, flySpeed);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += flyDirection * Time.deltaTime;
        flyDirection = Vector3.Lerp(flyDirection, Vector3.zero, deceleration * Time.deltaTime);
        lifeTime -= Time.deltaTime;
        if (lifeTime < 0)
        {
            pieceSR.color = Color.Lerp(pieceSR.color, Color.clear, fadeSpeed * Time.deltaTime);
        }
    }
}
