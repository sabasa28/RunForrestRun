using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoolerTree : MonoBehaviour
{
    enum TreeType
    { 
        NotSet = 0,
        Ciruelo,
        Durazno,
        Manzana,
        Num
    }
    enum TreeState
    {
        Healthy,
        Damaged,
        Dead
    }

    bool burning = false;
    float currentHealth = 100;
    [SerializeField] float fireDamage; // :)
    [SerializeField] float timeBeforeDespawningAfterDead;
    [SerializeField] GameObject HealthyModel1;
    [SerializeField] GameObject DamagedModel1;
    [SerializeField] GameObject HealthyModel2;
    [SerializeField] GameObject DamagedModel2;
    [SerializeField] GameObject HealthyModel3;
    [SerializeField] GameObject DamagedModel3;
    [SerializeField] GameObject DeadModel;
    GameObject HealthyModel;
    GameObject DamagedModel;
    [SerializeField] float timeBetweenFruitSpawns;
    [SerializeField] float fruitSpawningRadius;
    [SerializeField] Fruit FruitPrefab;
    [SerializeField] float spawnedFruitY;
    TreeType TypeOfTree = TreeType.NotSet;
    TreeState treeState = TreeState.Healthy;
    private void Start()
    {
        if (TypeOfTree == TreeType.NotSet)
        {
            TypeOfTree = (TreeType)Random.Range((int)TreeType.NotSet + 1 , (int)TreeType.Num);
        }
        switch (TypeOfTree)
        {
            case TreeType.Ciruelo:
                HealthyModel = HealthyModel1;
                DamagedModel = DamagedModel1;
                break;
            case TreeType.Durazno:
                HealthyModel = HealthyModel2;
                DamagedModel = DamagedModel2;
                break;
            default:
            case TreeType.Manzana:
                HealthyModel = HealthyModel3;
                DamagedModel = DamagedModel3;
                break;
        }
        UpdateState(TreeState.Healthy);
        StartCoroutine(SpawnFruitContinuously());
    }
    private void FixedUpdate()
    {
        if (burning)
        {
            if (currentHealth > 0)
            {
                currentHealth -= fireDamage * Time.fixedDeltaTime;
                switch (treeState)
                {
                    case TreeState.Healthy:
                        if (currentHealth < 50.0f)
                        {
                            UpdateState(TreeState.Damaged);
                        }
                        break;
                    case TreeState.Damaged:
                        if (currentHealth < 0.0f)
                        {
                            UpdateState(TreeState.Dead);
                        }
                        break;
                    case TreeState.Dead:
                        break;
                    default:
                        break;
                }
            }
            burning = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (treeState != TreeState.Dead && other.CompareTag("Fire"))
        {
            burning = true;
        }
    }

    void UpdateState(TreeState newTreeState)
    {
        treeState = newTreeState;
        HealthyModel1.SetActive(false);
        HealthyModel2.SetActive(false);
        HealthyModel3.SetActive(false);
        switch (treeState)
        {
            case TreeState.Healthy:
                HealthyModel.SetActive(true);
                DamagedModel.SetActive(false);
                DeadModel.SetActive(false);
                break;
            case TreeState.Damaged:
                HealthyModel.SetActive(false);
                DamagedModel.SetActive(true);
                DeadModel.SetActive(false);
                break;
            case TreeState.Dead:
                HealthyModel.SetActive(false);
                DamagedModel.SetActive(false);
                DeadModel.SetActive(true);
                StartCoroutine(DespawnTree());
                break;
            default:
                break;
        }
    }

    IEnumerator DespawnTree()
    {
        yield return new WaitForSeconds(timeBeforeDespawningAfterDead);
        TreeManager.Get().DespawnTree(this);
    }

    IEnumerator SpawnFruitContinuously()
    {
        while (treeState != TreeState.Dead)
        {
            yield return new WaitForSeconds(timeBetweenFruitSpawns);
            SpawnFruit();
        }
    }

    void SpawnFruit()
    {
        Vector3 randomDir = new(Random.Range(-1.0f, 1.0f), 0.0f, Random.Range(-1.0f, 1.0f));
        randomDir = randomDir.normalized * Random.Range(1.0f, fruitSpawningRadius);
        Vector3 spawnFruitPosition = transform.position + randomDir;
        spawnFruitPosition.y = spawnedFruitY;
        Fruit spawnedFruit = Instantiate<Fruit>(FruitPrefab, spawnFruitPosition, Quaternion.identity);
        spawnedFruit.SetModel((int)TypeOfTree);
    }
}
