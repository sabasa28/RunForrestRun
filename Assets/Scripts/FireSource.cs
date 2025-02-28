using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSource : Fire
{
    bool watered;
    float currentHealth;
    float maxHealth = 100.0f;
    Vector3 feedbackFireInitialScale;
    [SerializeField] Transform feedbackFire;
    [SerializeField] float waterDamage;
    enum DirectionsToExpandTo
    {
        UpLeft,
        UpUpLeft,
        UpUpRight,
        UpRight,
        LeftLeftUp,
        LeftLeftDown,
        RightRightUp,
        RightRightDown,
        DownLeft,
        DownDownLeft,
        DownDownRight,
        DownRight,
        Num
    }

    void Awake()
    {
        transform.position = new((int)transform.position.x + Mathf.Sign(transform.position.x) * 0.5f, (int)transform.position.y + Mathf.Sign(transform.position.y) * 0.5f, (int)transform.position.z + Mathf.Sign(transform.position.z) * 0.5f); //busco el .5 mas cercano
        fireManager.OccupyGridSlot(new((int)(transform.position.x - 0.5f), (int)(transform.position.z - 0.5f)), this);
        fireManager.OccupyGridSlot(new((int)(transform.position.x - 0.5f), (int)(transform.position.z + 0.5f)), this);
        fireManager.OccupyGridSlot(new((int)(transform.position.x + 0.5f), (int)(transform.position.z - 0.5f)), this);
        fireManager.OccupyGridSlot(new((int)(transform.position.x + 0.5f), (int)(transform.position.z + 0.5f)), this);
        fireManager.AddFireSource(this);
        posibleDirections = (int)DirectionsToExpandTo.Num;
        feedbackFireInitialScale = feedbackFire.localScale;
    }

    protected override void Start()
    {
        currentHealth = maxHealth;
        base.Start();
    }

    public override Vector2 GetNewPositionFromDirection(int directionsEnumIndex)
    {
        Vector2 spawnOffset = Vector2.zero;
        switch ((DirectionsToExpandTo)directionsEnumIndex)
        {
            case DirectionsToExpandTo.UpLeft:
                spawnOffset = Vector2.up + Vector2.left;
                break;
            case DirectionsToExpandTo.UpUpLeft:
                spawnOffset = Vector2.left + Vector2.up / 3.0f;
                break;
            case DirectionsToExpandTo.UpUpRight:
                spawnOffset = Vector2.right + Vector2.up / 3.0f;
                break;
            case DirectionsToExpandTo.UpRight:
                spawnOffset = Vector2.up + Vector2.right;
                break;
            case DirectionsToExpandTo.LeftLeftUp:
                spawnOffset = Vector2.up + Vector2.left / 3.0f;
                break;
            case DirectionsToExpandTo.LeftLeftDown:
                spawnOffset = Vector2.down + Vector2.left / 3.0f;
                break;
            case DirectionsToExpandTo.RightRightUp:
                spawnOffset = Vector2.up + Vector2.right / 3.0f;
                break;
            case DirectionsToExpandTo.RightRightDown:
                spawnOffset = Vector2.down + Vector2.right / 3.0f;
                break;
            case DirectionsToExpandTo.DownLeft:
                spawnOffset = Vector2.down + Vector2.left;
                break;
            case DirectionsToExpandTo.DownDownLeft:
                spawnOffset = Vector2.left + Vector2.down / 3.0f;
                break;
            case DirectionsToExpandTo.DownDownRight:
                spawnOffset = Vector2.right + Vector2.down / 3.0f;
                break;
            case DirectionsToExpandTo.DownRight:
                spawnOffset = Vector2.down + Vector2.right;
                break;
            case DirectionsToExpandTo.Num:
                break;
            default:
                break;
        }
        spawnOffset *= 1.5f; // como el fire source tiene mayor escala multiplico el offset
        return new Vector2(transform.position.x, transform.position.z) + spawnOffset;
    }
    private void FixedUpdate()
    {
        if (watered)
        {
            if (currentHealth > 0.0f)
            {
                currentHealth -= waterDamage * Time.fixedDeltaTime;
                feedbackFire.localScale = (currentHealth / maxHealth) * feedbackFireInitialScale;
                if (currentHealth <= 0.0f)
                {
                    fireManager.RemoveFireSource(this);
                    Destroy(gameObject);
                }
            }
            watered = false;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            watered = true;
        }
    }
}
