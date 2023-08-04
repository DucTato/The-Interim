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
    public bool canRandomChatter = true;
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
        // Check the transform of parent to flip the Text accordingly
        if (transform.root.localScale.x >= 0)
        {
            dialogueText.transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        else
        {
            dialogueText.transform.localScale = Vector3.one;
        }
        if (canRandomChatter)
        {
            // Interval between each random message roll
            if (timeCounter > 0)
            {
                timeCounter -= Time.deltaTime;

            }
            else
            {
                timeCounter = displayInterval;
                if (Random.Range(0, 2) == 1)
                {
                    // In each dialogue interval, there's a chance that it won't display the dialogue and will just redo the waiting again
                    DisplayDialogueWithMessage(messages[Random.Range(0, messages.Length)]);
                }

            }
        }
    }
    private IEnumerator DisplayDialogueThenWait(float time)
    {
        isDisplaying = true;
        yield return new WaitForSeconds(time);
        isDisplaying = false;
    }
    public void DisplayDialogueWithMessage(string message)
    {
        dialogueText.text = message;
        dialogueBubble.sizeDelta = new Vector2(dialogueText.renderedWidth <= 100 ? 100 : dialogueText.renderedWidth, dialogueText.preferredHeight > 32 ? dialogueText.preferredHeight : 32);
        StartCoroutine(DisplayDialogueThenWait(displayTime));
    }
}
