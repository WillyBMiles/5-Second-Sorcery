using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PopupMaster : MonoBehaviour
{
    public static PopupMaster instance;
    public TextMeshProUGUI textPopup;
    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public enum Color
    {
        Gold,
        White
    }

    public void PopupText(string text, Color color)
    {

        var popup = Instantiate(textPopup, transform);
        popup.transform.position = Input.mousePosition;
        popup.text = text;

        popup.color = color switch
        {
            Color.Gold => new UnityEngine.Color(1f, 212f/255f,0f,1f) - new UnityEngine.Color(.1f,.1f,.1f,0f),
            Color.White => UnityEngine.Color.white,
            _ => UnityEngine.Color.white
        };
    }
}
