using UnityEngine;
using UnityEngine.UI;

public class CoinTextElement : MonoBehaviour
{
    [SerializeField] private Text plusText;
    private float lifeTime;
    //private int amount;
    private void OnEnable()
    {
        lifeTime = 0.25f;
        
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
    private void OnDisable()
    {
        plusText.transform.localPosition = Vector3.zero;
        lifeTime = 0.25f;
        plusText.color = new Color(1f, 1f, 1f, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        plusText.transform.localPosition += transform.up * 1.5f * Time.deltaTime;
        lifeTime -= Time.deltaTime;
        if (lifeTime < 0)
        {
            plusText.color = Color.Lerp(plusText.color, Color.clear, 1f * Time.deltaTime);
        }
        if (lifeTime < -1.5f)
        {
            gameObject.SetActive(false);
        }
    }
    public void SetCoinPopUpText(int coinAmount, bool needColor)
    {
        if(needColor)
        {
            if (coinAmount >= 0)
            {
                plusText.text = "<color=green>+" + coinAmount +"</color>";
            }
            else
            {
                plusText.text = "<color=red>" + coinAmount + "</color>";
            }
        }
        else
        {
            plusText.text = "+" + coinAmount;
        }
    }
}
