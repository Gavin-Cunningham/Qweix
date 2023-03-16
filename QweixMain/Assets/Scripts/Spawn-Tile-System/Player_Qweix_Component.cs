using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player_Qweix_Component : MonoBehaviour
{


    public int qweixCount = 0;


    public TextMeshProUGUI textQweixtCount;

    private void Update()
    {
        textQweixtCount.text = qweixCount.ToString();
    }

    public void AddQweix()
    {

        qweixCount++;

    }

    public void removeQweix()
    {

        qweixCount--;

    }


}
