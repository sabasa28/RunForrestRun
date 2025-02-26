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
    PlayerThreeD player;
    bool bIsUsingWater = false;
    [SerializeField] TextMeshProUGUI CurrentTaxDebtText;
    private void Start()
    {
        player = FindObjectOfType<PlayerThreeD>();
    }
    private void Update()
    {
        timer += Time.deltaTime;
        if (bIsUsingWater)
        {
            timeUsingWaterSinceLastTax += Time.deltaTime;
            moneyOwedThisTax = timeUsingWaterSinceLastTax * moneyPerSecondOfWater;
            CurrentTaxDebtText.text = "$" + (int)moneyOwedThisTax;
        }
        if (timer >= timeBetweenTaxes)
        {
            player.AddMoney(-(int)moneyOwedThisTax);
            timer = 0.0f;
            timeUsingWaterSinceLastTax = 0.0f;
            moneyOwedThisTax = 0.0f;
            CurrentTaxDebtText.text = "$" + (int)moneyOwedThisTax;
        }
    }
    public void SetUsingWater(bool newIsUsingWater)
    {
        bIsUsingWater = newIsUsingWater;
    }
}
