using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using GogoGaga.OptimizedRopesAndCables;
public class PlayerThreeD : MonoBehaviour
{
    [SerializeField] float speed;
    float initialSpeed;
    [SerializeField] float animSpeedMultiplier;
    [SerializeField] float rotationSpeed;
    bool shootingWater;
    [SerializeField] bool canMoveWhileShooting;
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
    bool burningLastFrame = false;
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
    [SerializeField] Animator AnimControl;
    [SerializeField] bool bIsHoldingHose;
    bool bIsInRangeOfHoseBase = false;
    [SerializeField] Transform HoseHoldingPoint;
    [SerializeField] GameObject HoseEndModel;
    Vector3 hoseHoldingPointOrigin;
    Vector3 movement;
    [SerializeField] WaterTaxes waterTaxes;
    public bool initialAnimationOver = false;

    [SerializeField] AudioClip grabFruitSound;
    [SerializeField, Range(0.0f, 1.0f)] float fruitGrabVolume;
    [SerializeField] AudioClip failedPlant;
    [SerializeField, Range(0.0f, 1.0f)] float failVolume;
    [SerializeField] AudioSource waterAudioSource;
    [SerializeField] AudioClip shootWaterSound;
    [SerializeField, Range(0.0f, 1.0f)] float waterVolume;
    [SerializeField] AudioSource hurtAudioSource;
    [SerializeField] AudioClip getHurtSound;
    [SerializeField, Range(0.0f, 1.0f)] float oofVolume;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        mainCamera = Camera.main;
    }

    private void Start()
    {
        TreePlantingRangeVisualizer.transform.localScale = Vector3.one * 2 * TreeManager.Get().GetMinDistanceBetweenTrees(); //multiplicamos por dos porque la distancia es el radio y la escala el diametro 
        CurrentHealth = MaxHealth;
        initialSpeed = speed;
        hoseHoldingPointOrigin = HoseHoldingPoint.localPosition;
        waterAudioSource.clip = shootWaterSound;
        hurtAudioSource.clip = getHurtSound;
        hurtAudioSource.loop = true;
        waterAudioSource.volume = waterVolume;
        hurtAudioSource.volume = oofVolume;
        ChangeHoseHoldingState(bIsHoldingHose);
        UpdateSeedsAmountText();
        UpdateFruitsAmountText();
        UpdateMoneyAmountText();
        UpdateHealthBar();
        StartCoroutine(UpdateHoseLenght());
    }

    private void Update() //hice todo un quilombo con las direcciones pero no me voy a poner a arreglarlo, anda
    {
        if (!initialAnimationOver)
        {
            return;
        }
        float horizontalValue;
        float verticalValue;
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
                AudioManager.Get().PlaySFX(failedPlant, failVolume);
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
            if (bIsInRangeOfHoseBase)
            {
                ChangeHoseHoldingState(!bIsHoldingHose);
            }
        }
        if (Input.GetButtonDown("Fire1") && Time.timeScale != 0.0f && currentMoney >= 0 && bIsHoldingHose)
        {
            if (GetWorldMousePosition(out Vector3 MousePos))
            {
                shootingWater = true;
                waterAudioSource.Play();
                waterTaxes.SetUsingWater(shootingWater);
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
            if (Input.GetButton("Fire1") && currentMoney >= 0 && bIsHoldingHose)
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
                waterAudioSource.Stop();
                waterTaxes.SetUsingWater(shootingWater);
                timeFillingWater = 0.0f;
            }
        }
        moving = Input.GetAxisRaw("Horizontal") != 0.0f || Input.GetAxisRaw("Vertical") != 0.0f;
        horizontalValue = Input.GetAxis("Horizontal");
        verticalValue = Input.GetAxis("Vertical");

        float magnitudeMovement = Mathf.Max(Mathf.Abs(horizontalValue), Mathf.Abs(verticalValue));
        movement = new Vector3(horizontalValue, 0.0f, verticalValue).normalized * magnitudeMovement;
    }
    void FixedUpdate()
    {
        if (burning && !burningLastFrame)
        {
            hurtAudioSource.Play();
        }
        else if (burningLastFrame && !burning)
        {
            hurtAudioSource.Stop();
        }
        burningLastFrame = burning;
        if (burning)
        {
            burning = false;
            CurrentHealth -= FireDamage * Time.fixedDeltaTime;
            CurrentHealth = Mathf.Clamp(CurrentHealth, 0.0f, MaxHealth);
            UpdateHealthBar();
            if (CurrentHealth <= 0.0f)
            {
                hurtAudioSource.Stop();
                GameplayController.Get().OnLose();
            }
        }

        if (!shootingWater || canMoveWhileShooting)
        {
            if (moving && (Mathf.Abs(movement.x) > 0.01f || Mathf.Abs(movement.z) > 0.01f))
            {
                transform.rotation = Quaternion.LookRotation(Vector3.Slerp(transform.forward, new Vector3(-movement.z, 0.0f, movement.x).normalized, rotationSpeed * Time.fixedDeltaTime), Vector3.up); //no se si este uso de deltatime esta del todo bien
            }
            Vector3 finalMovement = speed * Time.fixedDeltaTime * new Vector3(movement.x, 0.0f, movement.z);
            characterController.SimpleMove(finalMovement);
            float distTohose = Vector3.Distance(HoseHolder.position, transform.position);
            debugDistToHose = distTohose;
            if (distTohose > CurrenthoseLength && bIsHoldingHose)
            {
                Vector3 normalVector = (HoseHolder.position - transform.position).normalized;
                characterController.SimpleMove((distTohose - CurrenthoseLength) * HoseRubberStrenght * normalVector);
            }
        }
        AnimControl.SetBool("Caminar", (moving));
        AnimControl.SetBool("AgarroManguera", bIsHoldingHose);
        AnimControl.SetFloat("VelocidadCaminar", shootingWater ? 0 : (movement.magnitude * speed) / initialSpeed * animSpeedMultiplier);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Fruit"))
        {
            AddFruitAmount(1);
            AudioManager.Get().PlaySFX(grabFruitSound, fruitGrabVolume);
            Destroy(other.gameObject);
        }
        if (other.CompareTag("Store"))
        {
            storeAvailable = true;
        }
        if (other.CompareTag("Hose"))
        {
            bIsInRangeOfHoseBase = true;
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
        if (other.CompareTag("Hose") && bIsInRangeOfHoseBase)
        {
            bIsInRangeOfHoseBase = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Fire") && burning == false)
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
        UpdateMoneyAmountText();
    }
    void UpdateMoneyAmountText()
    {
        MoneyAmountText.text = "$" + currentMoney.ToString();
        if (currentMoney >= 0)
        {
            MoneyAmountText.color = Color.white;
        }
        else 
        {
            MoneyAmountText.color = Color.red;
        }
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

    void ChangeHoseHoldingState(bool newHoldingState)
    {
        bIsHoldingHose = newHoldingState;
        if (bIsHoldingHose)
        {
            HoseHoldingPoint.parent = transform;
            HoseHoldingPoint.SetLocalPositionAndRotation(hoseHoldingPointOrigin, Quaternion.identity);
            HoseEndModel.SetActive(true);
        }
        else
        {
            HoseHoldingPoint.parent = HoseHolder;
            HoseHoldingPoint.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
            HoseEndModel.SetActive(false);
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
