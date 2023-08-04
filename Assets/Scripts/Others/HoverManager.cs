using UnityEngine;
using TMPro;
using System;

public class HoverManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI tipText;
    [SerializeField] private RectTransform tipWindow;

    public static Action<string, Vector2> OnMouseHover;
    public static Action OnMouseLoseFocus;
    private void OnEnable()
    {
        OnMouseHover += ShowTip;
        OnMouseLoseFocus += HideTip;
    }
    private void OnDisable()
    {
        OnMouseHover -= ShowTip;
        OnMouseLoseFocus -= HideTip;
    }
    // Start is called before the first frame update
    void Start()
    {
        HideTip();  
    }

    // Update is called once per frame
    //void Update()
    //{
        
    //}
    private void ShowTip(string tip, Vector2 mousePosition)
    {
        tipText.text = tip;
        tipWindow.gameObject.SetActive(true);
        tipWindow.sizeDelta = new Vector2(tipText.preferredWidth > 400 ? 400 : tipText.preferredWidth, tipText.preferredHeight);
        tipWindow.transform.position = new Vector2 (mousePosition.x + 25 + tipWindow.sizeDelta.x / 2f, mousePosition.y);
    }
    private void HideTip()
    {
        tipText.text = default;
        tipWindow.gameObject.SetActive(false);
    }
}
