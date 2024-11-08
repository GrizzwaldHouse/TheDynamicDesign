using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CoinSystem : MonoBehaviour
{
    public TMP_Text coinTxt;
    public int defaultCoin;
    public int coin;

    void Start()
    {
        coin = defaultCoin;
        UpdateCoin();
    }

    public void Gain(int val)
    {
        coin += val;
        UpdateCoin();
    }

    public bool Spend(int val)
    {
        if (HasEnough(val))
        {
            coin -= val;
            UpdateCoin() ;
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool HasEnough(int val)
    {
        if (val <= coin)
            return true;
        else 
            return false;
    }

    public void UpdateCoin()
    {
        coinTxt.text = coin.ToString();
    }
}
