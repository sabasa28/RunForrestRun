using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UISalesman : MonoBehaviour
{
    [SerializeField] PlayerThreeD player;
    [SerializeField] int moneyPerFruit;
    [SerializeField] int moneyPerSeed;
    [SerializeField] int inflationPerSeed;
    [SerializeField] int moneyPerHoseUpgrade;
    [SerializeField] int inflationPerHoseUpgrade;
    [SerializeField] int hoseMetersPerUpgrade;
    [SerializeField] int moneyPerSneakersUpgrade;
    [SerializeField] int inflationPerSneakersUpgrade;
    [SerializeField] int speedPerSneakersUpgrade;
    [SerializeField] TextMeshProUGUI FruitDescription;
    [SerializeField] TextMeshProUGUI SeedDescription;
    [SerializeField] TextMeshProUGUI HoseDescription;
    [SerializeField] TextMeshProUGUI SneakersDescription;
    [SerializeField] AudioClip noMoneyClip;
    [SerializeField] AudioClip yesMoneyClip;
    [SerializeField] AudioClip getMoneyClip;
    private void Awake()
    {
        UpdatePricesText();
    }
    public void SellFruit()
    {
        int fruitSold = player.RemoveAllFruit();
        player.AddMoney(fruitSold * moneyPerFruit);
        if (fruitSold > 0)
        {
            PlayRichSound();
        }
        else
        {
            PlayPoorSound();
        }
    }
    public void BuySeed()
    {
        if (player.TrySpendMoney(moneyPerSeed))
        {
            player.AddSeedAmount(1);
            moneyPerSeed += inflationPerSeed;
            UpdatePricesText();
            PlayBuySound();
        }
        else
        {
            PlayPoorSound();
        }
    }
    public void BuySneakersUpgrade()
    {
        if (player.TrySpendMoney(moneyPerSneakersUpgrade))
        {
            player.AddToSpeed(speedPerSneakersUpgrade);
            moneyPerSneakersUpgrade += inflationPerSneakersUpgrade;
            UpdatePricesText();
            PlayBuySound();
        }
        else
        {
            PlayPoorSound();
        }
    }
    public void BuyHoseUpgrade()
    {
        if (player.TrySpendMoney(moneyPerHoseUpgrade))
        {
            player.AddToHoseLenght(hoseMetersPerUpgrade);
            moneyPerHoseUpgrade += inflationPerHoseUpgrade;
            UpdatePricesText();
            PlayBuySound();
        }
        else
        {
            PlayPoorSound();
        }
    }
    public void CloseSalesman()
    {
        Time.timeScale = 1.0f;
        gameObject.SetActive(false);
        AudioManager.Get().PlayUIBack();
    }

    void UpdatePricesText()
    {
        FruitDescription.text = "$" + moneyPerFruit + " cada una";
        SeedDescription.text = "$" + moneyPerSeed + " cada una";
        HoseDescription.text = "$" + moneyPerHoseUpgrade + " cada " + hoseMetersPerUpgrade + " metros";
        SneakersDescription.text = "$" + moneyPerSneakersUpgrade + " cada mejora";
    }

    void PlayBuySound()
    {
        AudioManager.Get().PlaySFX(yesMoneyClip, 0.2f);
    }

    void PlayPoorSound()
    {
        AudioManager.Get().PlaySFX(noMoneyClip, 0.2f);
    }
    void PlayRichSound()
    {
        AudioManager.Get().PlaySFX(getMoneyClip, 0.2f);
    }

}
