using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using GogoGaga.OptimizedRopesAndCables;
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
    bool storeAvailable = false;
    [SerializeField] bool burning;
    [SerializeField] TextMeshProUGUI SeedsAmountText;
    [SerializeField] int seedsAmount;
    [SerializeField] TextMeshProUGUI FruitsAmountText;
    [SerializeField] int fruitsAmount;
    bool AttemptingToPlant;
    [SerializeField] GameObject TreePlantingRangeVisualizer;
    [SerializeField] Rope Hose;
    [SerializeField] Transform HoseHolder;
    [SerializeField] float HoseHangingAmount;
    [SerializeField] float CurrenthoseLength;
    [SerializeField] float HoseRubberStrenght;
    [SerializeField] float debugDistToHose;
    [SerializeField] LayerMask groundLayer;
    Camera mainCamera;
    [SerializeField] TextMeshProUGUI MoneyAmountText;
    [SerializeField] int currentMoney;
    [SerializeField] GameObject StoreMenu;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        mainCamera = Camera.main;
    }

    private void Start()
    {
        TreePlantingRangeVisualizer.transform.localScale = Vector3.one * 2 * TreeManager.Get().GetMinDistanceBetweenTrees(); //multiplicamos por dos porque la distancia es el radio y la escala el diametro 
        CurrentHealth = MaxHealth;
        UpdateSeedsAmountText();
        UpdateFruitsAmountText();
        UpdateMoneyAmountText();
        UpdateHealthBar();
        StartCoroutine(UpdateHoseLenght());
    }

    private void Update() //hice todo un quilombo con las direcciones pero no me voy a poner a arreglarlo, anda
    {
        if (Input.GetKeyDown(KeyCode.Space))
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
        if (Input.GetKeyUp(KeyCode.Space) && AttemptingToPlant)
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


        if (Input.GetKeyDown(KeyCode.E))
        {
            if (storeAvailable)
            {
                StoreMenu.SetActive(true);
                Time.timeScale = 0.0f;
            }
        }
        if (Input.GetButtonDown("Fire1") && Time.timeScale != 0.0f)
        {
            if (GetWorldMousePosition(out Vector3 MousePos))
            {
                shootingWater = true;
                Vector3 ShootingDir = (MousePos - transform.position);
                ShootingDir.y = 0.0f;
                float aux = ShootingDir.z;
                ShootingDir.z = ShootingDir.x;
                ShootingDir.x = -aux; //escuchame no tengo mucho tiempo, la razon por la que hice esto es
                transform.forward = ShootingDir;

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
            if (Input.GetButton("Fire1"))
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
            if (CurrentHealth <= 0.0f)
            {
                GameplayController.Get().OnLose();
            }
        }
        if (!shootingWater || canMoveWhileShooting)
        {
            if (moving && (Mathf.Abs(horizontalValue) > 0.01f || Mathf.Abs(verticalValue) > 0.01f))
            {
                transform.rotation = Quaternion.LookRotation(Vector3.Slerp(transform.forward, new Vector3(horizontalValue, 0.0f, verticalValue), rotationSpeed * Time.fixedDeltaTime), Vector3.up); //no se si este uso de deltatime esta del todo bien
            }
            Vector3 movement = speed * Time.fixedDeltaTime * new Vector3(verticalValue, 0.0f, -horizontalValue);
            characterController.SimpleMove(movement);
            float distTohose = Vector3.Distance(HoseHolder.position, transform.position);
            debugDistToHose = distTohose;
            if (distTohose > CurrenthoseLength)
            {
                Vector3 normalVector = (HoseHolder.position - transform.position).normalized;
                characterController.SimpleMove(normalVector * (distTohose - CurrenthoseLength) * HoseRubberStrenght);
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
        if (other.CompareTag("Store"))
        {
            storeAvailable = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Store"))
        {
            storeAvailable = false;
            if (StoreMenu.activeInHierarchy)
            {
                StoreMenu.SetActive(false);
            }
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


    public void AddSeedAmount(int AmountToAdd)
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

    public void AddMoney(int AmountToAdd)
    {
        currentMoney += AmountToAdd;
        if (currentMoney < 0)
        {
            currentMoney = 0;
        }
        UpdateMoneyAmountText();
    }
    void UpdateMoneyAmountText()
    {
        MoneyAmountText.text = "$" + currentMoney.ToString();
    }

    public void AddToSpeed(float speedToAdd)
    {
        speed += speedToAdd;
        if (speed < 0.0f)
        {
            speed = 0.0f;
        }
    }

    public void AddToHoseLenght(float lenghtToAdd)
    {
        CurrenthoseLength += lenghtToAdd;
        if (CurrenthoseLength < 0.0f)
        {
            CurrenthoseLength = 0.0f;
        }
    }

    IEnumerator UpdateHoseLenght()
    {
        while (true)
        {
            Hose.ropeLength = Mathf.Min(Vector3.Distance(transform.position, HoseHolder.position), CurrenthoseLength) * (1.0f + HoseHangingAmount);
            yield return new WaitForSeconds(0.1f);
        }
    }

    private bool GetWorldMousePosition(out Vector3 position)
    {
        var ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        
        if (Physics.Raycast(ray, out RaycastHit hitInfo, 100.0f, groundLayer))
        {
            position = hitInfo.point;
            return true;
        }
        else
        {
            position = Vector3.zero;
            return false;
        }
    }

    public bool TrySpendMoney(int cost)
    {
        if (currentMoney >= cost)
        {
            currentMoney -= cost;
            UpdateMoneyAmountText();
            return true;
        }
        else
        {
            return false;
        }
    }

    public int RemoveAllFruit()
    {
        int aux = fruitsAmount;
        fruitsAmount = 0;
        UpdateFruitsAmountText();
        return aux;
    }
}
