using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CoinUI : MonoBehaviour
{
    public int coinStartQuantity;
    public static int currentQuantity; // has to be static for other script to pick up on and change

    public TMP_Text coinRenderer;

    
    // Start is called before the first frame update
    void Start()
    {
        currentQuantity = coinStartQuantity;
    }

    // Update is called once per frame
    void Update()
    {
        coinRenderer.text = currentQuantity.ToString();
    }
}
