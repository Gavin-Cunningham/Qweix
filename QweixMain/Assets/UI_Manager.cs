using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    public static UI_Manager instance { get; private set; }

    [SerializeField] private RawImage overlayImage;
    private void Awake()
    {
        if (null != instance && this != instance)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    private void Start()
    {
        //overlayImage.color = new Color(1, 1, 1, 1);
        LocalManager.instance.matchActive.OnValueChanged += OnMatchStateChange;
    }

    private void OnMatchStateChange(bool previous, bool current)
    {
        if (true == current)
        {
            //FadeTransition(overlayImage, false);
        }
    }

    private void FadeTransition(RawImage image, bool visible)
    {
        float alphaStart = image.color.a;
        float alphaEnd;
        if (visible) { alphaEnd = 1.0f; }
        else { alphaEnd = 0.0f; }


        float fadeAmount = 0.0f;
        Color setColor = new Color(1, 1, 1, 0);
        while (image.color.a != alphaEnd)
        {
            setColor.a = Mathf.Lerp(alphaStart, alphaEnd, fadeAmount);
            Debug.Log("Lerp = " + fadeAmount);
            image.color = setColor;
            fadeAmount += Time.deltaTime * 0.01f;
        }
    }
}
