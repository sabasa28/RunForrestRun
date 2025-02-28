using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WaterTaxes : MonoBehaviour
{
    [SerializeField] float timeBetweenTaxes;
    [SerializeField] float moneyPerSecondOfWater;
    float timeUsingWaterSinceLastTax;
    [SerializeField] float moneyOwedThisTax = 0.0f;
    float timer = 0.0f;
    int timeLeft = 0;
    PlayerThreeD player;
    bool bIsUsingWater = false;
    [SerializeField] TextMeshProUGUI CurrentTaxDebtText;
    private void Start()
    {
        player = FindObjectOfType<PlayerThreeD>();
        timeLeft = (int)timeBetweenTaxes;
        UpdateTaxDebtText();
    }
    private void Update()
    {
        bool bShouldUpdateText = false;
        timer += Time.deltaTime;
        if (bIsUsingWater)
        {
            timeUsingWaterSinceLastTax += Time.deltaTime;
            moneyOwedThisTax = timeUsingWaterSinceLastTax * moneyPerSecondOfWater;
            bShouldUpdateText = true;
        }
        if (timer >= timeBetweenTaxes)
        {
            player.AddMoney(-(int)moneyOwedThisTax);
            timer = 0.0f;
            timeUsingWaterSinceLastTax = 0.0f;
            moneyOwedThisTax = 0.0f;
            bShouldUpdateText = true;
        }
        if ((int)(timeBetweenTaxes - timer) != timeLeft)
        {
            timeLeft = (int)(timeBetweenTaxes - timer);
            bShouldUpdateText = true;
        }
        if (bShouldUpdateText)
        {
            UpdateTaxDebtText();
        }
    }
    public void SetUsingWater(bool newIsUsingWater)
    {
        bIsUsingWater = newIsUsingWater;
    }

    void UpdateTaxDebtText()
    {
        CurrentTaxDebtText.text = "$" + (int)moneyOwedThisTax + "(" +  timeLeft + ")";
    }
}
