using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using System;

public class Rising : MonoBehaviour
{

    public float speed;
    public float fadeTime;
    // Start is called before the first frame update
    void Start()
    {
        TextMeshProUGUI txt = GetComponent<TextMeshProUGUI>();
        txt.DOFade(0f, fadeTime);
        Action delete = () => Destroy(gameObject);
        delete.Delay(fadeTime);
    }

    // Update is called once per frame
    void Update()
    {
        (transform as RectTransform).anchoredPosition += 
            new Vector2(0f, speed * Time.deltaTime);
        
    }
}
