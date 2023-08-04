using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverTip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string tipToShow;
    private float waitTime = 0.5f;
    private bool isShowing = false;
    public void OnPointerEnter(PointerEventData eventData)
    {
        StopAllCoroutines();
        StartCoroutine(StartTimer());
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        StopAllCoroutines();
        HoverManager.OnMouseLoseFocus();
        isShowing = false;
    }
    private void OnDisable()
    {
        StopAllCoroutines();
        HoverManager.OnMouseLoseFocus();
        isShowing = false;
    }
    private void ShowMessage(string message)
    {
        HoverManager.OnMouseHover(message, Input.mousePosition);
        isShowing = true;
        //Debug.Log(Input.mousePosition);
    }
    private IEnumerator StartTimer()
    {
        yield return new WaitForSeconds(waitTime);
        ShowMessage(tipToShow);
    }
    public void UpdateMessage(string message)
    {
        tipToShow = message;
        if (isShowing)
        {
            ShowMessage(message);
        }
    }
}
