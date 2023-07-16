using UnityEngine;
using UnityEngine.UI;

public class CoinTextElement : MonoBehaviour
{
    [SerializeField] private Text plusText;
    private float lifeTime;
    public int amount;
    private void OnEnable()
    {
        lifeTime = 0.25f;
        plusText.text = "+" + amount;
        switch (Random.Range(0,3))
        {
            case 0:
                plusText.alignment = TextAnchor.UpperLeft;
                break;
            case 1:
                plusText.alignment = TextAnchor.UpperCenter;
                break;
            case 2:
                plusText.alignment = TextAnchor.UpperRight;
                break;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.up * 1.5f * Time.deltaTime;
        lifeTime -= Time.deltaTime;
        if (lifeTime < 0)
        {
            plusText.color = Color.Lerp(plusText.color, Color.clear, 1f * Time.deltaTime);
        }
    }
}
