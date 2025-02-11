using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class House : MonoBehaviour
{
    bool burning = false;
    [SerializeField] Slider HealthBar;
    [SerializeField] float fireDamageToHouse;
    float maxHealth = 100.0f;
    float currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
        HealthBar.value = Mathf.Clamp(currentHealth / maxHealth, 0.0f, 1.0f);
    }

    private void FixedUpdate()
    {
        if (burning)
        {
            if (currentHealth > 0.0f)
            {
                currentHealth -= fireDamageToHouse * Time.fixedDeltaTime;
                HealthBar.value = Mathf.Clamp(currentHealth / maxHealth, 0.0f, 1.0f);
                if (currentHealth <= 0.0f)
                {
                    GameplayController.Get().OnLose();
                }
            }
            burning = false;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Fire"))
        {
            burning = true;
        }
    }
}
