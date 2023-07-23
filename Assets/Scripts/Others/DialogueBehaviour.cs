using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueBehaviour : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;
    [SerializeField] private Vector3 currentSize;
    [SerializeField] private float displayTime, displayInterval;
    [SerializeField] private string[] messages;
    [SerializeField] private RectTransform dialogueBubble;
    private float timeCounter;
    private bool isDisplaying;

    //// Start is called before the first frame update
    //void Start()
    //{
       
    //}
    private void OnEnable()
    {

        currentSize = Vector3.zero;
    }
    // Update is called once per frame
    void Update()
    {
        transform.localScale = currentSize;
        if(isDisplaying)
        {
            currentSize = Vector3.Lerp(currentSize, Vector3.one, 4f * Time.deltaTime);
        }
        else
        {
            currentSize = Vector3.Lerp(currentSize, Vector3.zero, 6f * Time.deltaTime);
        }
        if (transform.root.localScale.x < 0)
        {
            dialogueText.transform.localScale = new Vector3(-0.9f, 0.9f, 0.9f);
        }
        else
        {
            dialogueText.transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
        }
        if (timeCounter > 0)
        {
            timeCounter -= Time.deltaTime;

        }
        else
        {
            timeCounter = displayInterval;
            
            if(Random.Range(0,2) == 1)
            {
                dialogueText.text = messages[Random.Range(0, messages.Length)];
                dialogueBubble.sizeDelta = new Vector2(dialogueText.renderedWidth <= 100 ? 100 : dialogueText.renderedWidth, dialogueText.preferredHeight > 32 ? dialogueText.preferredHeight : 32);
                // Each dialogue interval, there's a chance that it won't display the dialogue and will just redo the waiting again
                StartCoroutine(DisplayDialogueThenWait(displayTime));
            }

        }
    }
    private IEnumerator DisplayDialogueThenWait(float time)
    {

        isDisplaying = true;
        yield return new WaitForSeconds(time);
        isDisplaying = false;
    }
}
