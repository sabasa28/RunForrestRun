using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UISalesman : MonoBehaviour
{
    [SerializeField] PlayerThreeD player;
    [SerializeField] int moneyPerFruit;
    [SerializeField] int moneyPerSeed;
    [SerializeField] int moneyPerHoseUpgrade;
    [SerializeField] int hoseMetersPerUpgrade;
    [SerializeField] int moneyPerSneakersUpgrade;
    [SerializeField] int speedPerSneakersUpgrade;
    [SerializeField] TextMeshProUGUI FruitDescription;
    [SerializeField] TextMeshProUGUI SeedDescription;
    [SerializeField] TextMeshProUGUI HoseDescription;
    [SerializeField] TextMeshProUGUI SneakersDescription;
    private void Awake()
    {
        FruitDescription.text = moneyPerFruit + "$ cada una";
        SeedDescription.text = moneyPerSeed + "$ cada una";
        HoseDescription.text = moneyPerHoseUpgrade + "$ cada " + hoseMetersPerUpgrade + " metros";
        SneakersDescription.text = moneyPerSneakersUpgrade + "$ cada mejora";
    }
    public void SellFruit()
    {
        player.AddMoney(player.RemoveAllFruit() * moneyPerFruit);
    }
    public void BuySeed()
    {
        if (player.TrySpendMoney(moneyPerSeed))
        {
            player.AddSeedAmount(1);
        }
    }
    public void BuySneakersUpgrade()
    {
        if (player.TrySpendMoney(moneyPerSneakersUpgrade))
        {
            player.AddToSpeed(speedPerSneakersUpgrade);
        }
    }
    public void BuyHoseUpgrade()
    {
        if (player.TrySpendMoney(moneyPerHoseUpgrade))
        {
            player.AddToHoseLenght(hoseMetersPerUpgrade);
        }
    }
    public void CloseSalesman()
    {
        Time.timeScale = 1.0f;
        gameObject.SetActive(false);
    }
}
