using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    float speed;
    bool shootingWater;
    float horizontalValue;
    float verticalValue;
    [SerializeField]
    GameObject water;
    [SerializeField]
    float timeToFillWater;
    float timeFillingWater = 0.0f;

    private void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            shootingWater = true;
            water.SetActive(true);
        }
        if (shootingWater)
        {
            if (Input.GetButton("Jump"))
            {
                horizontalValue = 0.0f;
                verticalValue = 0.0f;
                timeFillingWater += Time.deltaTime;
                float scaleToSet = Mathf.InverseLerp(0.0f, timeToFillWater, timeFillingWater);
                water.transform.localScale = new Vector3(scaleToSet, scaleToSet, 1.0f);
            }
            else
            {
                water.SetActive(false);
                water.transform.localScale = new Vector3(0.0f, 0.0f, 1.0f);
                shootingWater = false;
                timeFillingWater = 0.0f;
            }

        }
        horizontalValue = Input.GetAxis("Horizontal");
        verticalValue = Input.GetAxis("Vertical");
    }
    void FixedUpdate()
    {
        if (!shootingWater)
        {
            if (Mathf.Abs(horizontalValue) > 0.01f || Mathf.Abs(verticalValue) > 0.01f)
            {
                transform.rotation = Quaternion.LookRotation(Vector3.forward, new Vector3(horizontalValue, verticalValue, 0.0f));
            }
            transform.position += speed * Time.fixedDeltaTime * new Vector3(horizontalValue, verticalValue, 0.0f);
        }
    }
}
