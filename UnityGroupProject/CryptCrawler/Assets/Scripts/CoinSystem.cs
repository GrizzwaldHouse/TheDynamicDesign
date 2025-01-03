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
    public int TransferCoin(CoinSystem target, int amount)
    {

        if (Spend(amount))
        {
            target.Gain(amount);
            return amount;
        }
        else
        {
            return 0;
        }
    }
    public void AddCurrency(ItemData currencyItem)
    {
        if (currencyItem.category == ItemCategory.Currency)
        {
            Gain(currencyItem.currencyValue);
        }
    }
    public void RemoveCurrency(ItemData currencyItem)
    {
        if (currencyItem.category == ItemCategory.Currency)
        {
            Spend(currencyItem.currencyValue);
        }
    }
}
