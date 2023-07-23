using UnityEngine;
using UnityEngine.UI;

public class PausedPanel : MonoBehaviour
{
    [SerializeField] private GameObject ingameElements;
    [SerializeField] private Text pausedText;
    private float lifeTime;
    private Color tempColor;
    private void OnEnable()
    {
        lifeTime = 2f;
        tempColor = new Color (1f, 1f, 1f, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        lifeTime -= 0.02f;
        if (lifeTime < -1f)
        {
            pausedText.color = Color.Lerp(pausedText.color, tempColor, 0.04f);
        }
        if (lifeTime < -4f)
        {
            ingameElements.SetActive(true);
        }
    }
    private void OnDisable()
    {
        ingameElements.SetActive(false);
        pausedText.color = Color.clear;
    }
    
}
