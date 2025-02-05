using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class PlayerThreeD : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float rotationSpeed;
    bool shootingWater;
    [SerializeField] bool canMoveWhileShooting;
    float horizontalValue;
    float verticalValue;
    bool moving;
    [SerializeField] GameObject water;
    [SerializeField] float timeToFillWater;
    float timeFillingWater = 0.0f;
    CharacterController characterController;
    [SerializeField] Slider HealthBar;
    [SerializeField] float MaxHealth;
    [SerializeField] float CurrentHealth;
    [SerializeField] float FireDamage; // xd
    [SerializeField] float CurrenthoseLength;
    [SerializeField] Transform HoseBaseTransform;
    [SerializeField] float HoseRubberStrenght;
    [SerializeField] float debugDistToHose;
    [SerializeField] bool storeAvailable;
    [SerializeField] bool burning;
    [SerializeField] TextMeshProUGUI SeedsAmountText;
    [SerializeField] int seedsAmount;
    [SerializeField] TextMeshProUGUI FruitsAmountText;
    [SerializeField] int fruitsAmount;
    bool AttemptingToPlant;
    [SerializeField] GameObject TreePlantingRangeVisualizer;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Start()
    {
        TreePlantingRangeVisualizer.transform.localScale = Vector3.one * 2 * TreeManager.Get().GetMinDistanceBetweenTrees(); //multiplicamos por dos porque la distancia es el radio y la escala el diametro 
        CurrentHealth = MaxHealth;
        UpdateSeedsAmountText();
        UpdateFruitsAmountText();
        UpdateHealthBar();
    }

    private void Update() //hice todo un quilombo con las direcciones pero no me voy a poner a arreglarlo, anda
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (seedsAmount > 0)
            {
                AttemptingToPlant = true;
                TreePlantingRangeVisualizer.SetActive(true);
            }
            else
            {
                //play failed to spawn tree sound
            }
        }
        if (Input.GetKeyUp(KeyCode.Z) && AttemptingToPlant)
        {
            AttemptingToPlant = false;
            if (TreeManager.Get().TrySpawnTree(TreePlantingRangeVisualizer.transform.position))
            {
                AddSeedAmount(-1);
            }
            else
            {
                //play failed to spawn tree sound
            }
            TreePlantingRangeVisualizer.SetActive(false);
        }

        if (Input.GetButtonDown("Jump"))
        {
            if (storeAvailable)
            {
                //open store
            }
            else
            {
                shootingWater = true;
                //if (verticalValue > 0.0f || horizontalValue > 0.0f) //esto se usa si queremos que la rotacion al disparar vaya mas bien con los inputs mas que con la direccion
                //{
                //    bool bOnXAxis = (Mathf.Abs(verticalValue) > Mathf.Abs(horizontalValue)); //me quedo el right como forward pero me dio fiaca cambiarlo
                //    float axisSign = Mathf.Sign(bOnXAxis ? verticalValue : horizontalValue); //para saber si es positiva o negativa la direccion a la que mira en el axis
                //    transform.rotation = Quaternion.LookRotation((bOnXAxis ? Vector3.forward : Vector3.right) * axisSign, Vector3.up);
                //}
                water.SetActive(true);
            }
        }
        if (shootingWater)
        {
            if (Input.GetButton("Jump"))
            {
                timeFillingWater += Time.deltaTime;
                float scaleToSet = Mathf.InverseLerp(0.0f, timeToFillWater, timeFillingWater);
                water.transform.localScale = new Vector3(scaleToSet, scaleToSet, scaleToSet);
            }
            else
            {
                water.SetActive(false);
                water.transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
                shootingWater = false;
                timeFillingWater = 0.0f;
            }
        }
        moving = Input.GetAxisRaw("Horizontal") != 0.0f || Input.GetAxisRaw("Vertical") != 0.0f;
        horizontalValue = Input.GetAxis("Horizontal");
        verticalValue = Input.GetAxis("Vertical");
    }
    void FixedUpdate()
    {
        if (burning)
        {
            CurrentHealth -= FireDamage * Time.fixedDeltaTime;
            CurrentHealth = Mathf.Clamp(CurrentHealth, 0.0f, MaxHealth);
            UpdateHealthBar();
            burning = false;
        }
        if (!shootingWater || canMoveWhileShooting)
        {
            if (moving && (Mathf.Abs(horizontalValue) > 0.01f || Mathf.Abs(verticalValue) > 0.01f))
            {
                transform.rotation = Quaternion.LookRotation(Vector3.Slerp(transform.forward, new Vector3(horizontalValue, 0.0f, verticalValue), rotationSpeed * Time.fixedDeltaTime), Vector3.up); //no se si este uso de deltatime esta del todo bien
            }
            Vector3 movement = speed * Time.fixedDeltaTime * new Vector3(verticalValue, 0.0f, -horizontalValue);
            characterController.SimpleMove(movement);
            float distTohose = Vector3.Distance(HoseBaseTransform.position, transform.position);
            debugDistToHose = distTohose;
            if (distTohose > CurrenthoseLength)
            {
                Vector3 normalVector = (HoseBaseTransform.position - transform.position).normalized;
                characterController.SimpleMove(normalVector * (distTohose - CurrenthoseLength) * HoseRubberStrenght);
                Debug.Log("Pushed Back " + (normalVector * (distTohose - CurrenthoseLength)));
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Fruit"))
        {
            AddFruitAmount(1);
            Destroy(other.gameObject);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Fire"))
        {
            burning = true;
        }
    }

    void UpdateHealthBar()
    {
        HealthBar.value = Mathf.Clamp(CurrentHealth / MaxHealth, 0.0f, 1.0f);
    }


    void AddSeedAmount(int AmountToAdd)
    {
        seedsAmount += AmountToAdd;
        if (seedsAmount < 0)
        {
            seedsAmount = 0;
        }
        UpdateSeedsAmountText();
    }
    void UpdateSeedsAmountText()
    {
        SeedsAmountText.text = seedsAmount.ToString();
    }
    void AddFruitAmount(int AmountToAdd)
    {
        fruitsAmount += AmountToAdd;
        if (fruitsAmount < 0)
        {
            fruitsAmount = 0;
        }
        UpdateFruitsAmountText();
    }
    void UpdateFruitsAmountText()
    {
        FruitsAmountText.text = fruitsAmount.ToString();
    }
}
