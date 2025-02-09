using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoolerTree : MonoBehaviour
{
    bool burning = false;
    float currentHealth = 100;
    [SerializeField] float fireDamage; // :)
    [SerializeField] float timeBeforeDespawningAfterDead;
    [SerializeField] GameObject HealthyModel;
    [SerializeField] GameObject DamagedModel;
    [SerializeField] GameObject DeadModel;
    [SerializeField] float timeBetweenFruitSpawns;
    [SerializeField] float fruitSpawningRadius;
    [SerializeField] GameObject fruitPrefab;
    [SerializeField] float spawnedFruitY;
    enum TreeState
    {
        Healthy,
        Damaged,
        Dead
    }
    TreeState treeState = TreeState.Healthy;
    private void Start()
    {
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
        Instantiate(fruitPrefab, spawnFruitPosition, Quaternion.identity);
    }
}
