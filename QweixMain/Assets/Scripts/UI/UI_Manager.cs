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
        overlayImage.color = new Color(1, 1, 1, 1);
        LocalManager.instance.matchActive.OnValueChanged += OnMatchStateChange;
    }

    private void OnMatchStateChange(bool previous, bool current)
    {
        if (true == current)
        {
            StartCoroutine(FadeImageToTransparent(overlayImage));
        }
    }

    IEnumerator FadeImageToTransparent(RawImage image)
    {
        Color alphaColor = image.color;

        for (float fadeAmount = 1f; fadeAmount >= 0; fadeAmount -= 0.01f)
        {
            alphaColor.a = fadeAmount;
            image.color = alphaColor;
            yield return null;
        }

    }
}
