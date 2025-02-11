using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireManager : MonoBehaviour
{
    Dictionary<Vector2Int, Fire> fires = new();
    List<FireSource> fireSources = new();
    [SerializeField] Fire fireTemplate;
    [SerializeField] PlayerThreeD player;
    [SerializeField] Vector2Int minLimit;
    [SerializeField] Vector2Int maxLimit;
    [SerializeField] float maxInitialDistanceToPlayer;

    private void Start()
    {
        SpawnInitialFires();
    }
    void SpawnInitialFires()
    {
        for (int i = minLimit.x; i <= maxLimit.x; i++)
        {
            for (int j = minLimit.y; j <= maxLimit.y; j++)
            {
                Vector2Int spawnPos = new(i, j);
                if (Vector2.Distance(spawnPos, new Vector2(player.transform.position.x, player.transform.position.z)) > maxInitialDistanceToPlayer && CheckIfAvailable(spawnPos))
                {
                    SpawnFire(spawnPos);
                }
            }
        }
    }
    public bool CheckIfAvailable(Vector2Int gridSlot)
    {
        //check if within limits
        if (gridSlot.x < minLimit.x || gridSlot.x > maxLimit.x || gridSlot.y < minLimit.y || gridSlot.y > maxLimit.y)
        {
            return false;
        }
        return (!fires.ContainsKey(gridSlot));
    }

    public void SpawnFire(Vector2Int gridSlot)
    {
        Fire spawnedFire = Instantiate(fireTemplate, new Vector3(gridSlot.x, 0.5f, gridSlot.y), Quaternion.identity);
        spawnedFire.fireManager = this;
        spawnedFire.debugGridPos = gridSlot; 
        fires.Add(gridSlot, spawnedFire);
    }

    public void DespawnFire(Vector2Int gridSlot)
    {
        Fire fireToDespawn;
        if (fires.TryGetValue(gridSlot, out fireToDespawn))
        {
            Destroy(fireToDespawn.gameObject);
            fires.Remove(gridSlot);
        }
        else
        {
            fires.Remove(gridSlot);
        }
    }

    public void OccupyGridSlot(Vector2Int gridSlot, Fire fire) //used for fires that are already in the level insted of being instanced from this manager 
    {
        fires.Add(gridSlot, fire);
    }

    public void AddFireSource(FireSource newFireSource)
    {
        fireSources.Add(newFireSource);
    }

    public void RemoveFireSource(FireSource deadFireSource)
    {
        fireSources.Remove(deadFireSource);
        if (fireSources.Count <= 0)
        {
            GameplayController.Get().OnWin();
        }
    }

}
