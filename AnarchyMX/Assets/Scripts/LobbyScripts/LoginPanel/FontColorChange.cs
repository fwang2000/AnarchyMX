using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class FontColorChange : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private GameObject loginText;
    private TextMeshProUGUI loginTextGUI;
    private bool mouseOver;
    private Color mouseOverColor;

    // Start is called before the first frame update
    void Start()
    {
        loginTextGUI = loginText.GetComponent<TextMeshProUGUI>();
        mouseOver = false;
        mouseOverColor = new Color(1, 0.9098f, 0.6745f);
    }

    // Update is called once per frame
    void Update()
    {
        if (mouseOver)
        {
            loginTextGUI.color = mouseOverColor;
        }
        else
        {
            loginTextGUI.color = Color.white;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        mouseOver = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mouseOver = false;
    }
}
