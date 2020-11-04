using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMovement : MonoBehaviour
{

    public float moveSpeed = 5.0f;
    private float minX;
    private float maxX;
    private float minY;
    private float maxY;
    private float randomX;
    private float randomY;
    private float tChange = 0f;

    void Start()
    {
        minX = GameController.shared.leftBoundary.transform.position.x;
        maxX = GameController.shared.rightBoundary.transform.position.x;
        minY = GameController.shared.bottomBoundary.transform.position.y;
        maxY = GameController.shared.topBoundary.transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time >= tChange)
        {
            //randomX = Random.Range(minX, maxX);
            //randomY = Random.Range(minY, maxY); 
            randomX = Random.Range(-2.0f, 2.0f);
            randomY = Random.Range(-2.0f, 2.0f);
            tChange = Time.time + Random.Range(0.5f, 1.5f);
        }
        Vector3 newPosition = new Vector3(randomX, randomY, 0);
        transform.Translate(newPosition * moveSpeed * Time.deltaTime);

        if (transform.position.x >= maxX || transform.position.x <= minX)
        {
            randomX = -randomX;
        }
        if (transform.position.y >= maxY || transform.position.y <= minY)
        {
            randomY = -randomY;
        }
        Vector3 transformPosition = transform.position;
        transformPosition.x = Mathf.Clamp(transform.position.x, minX, maxX);
        transformPosition.y = Mathf.Clamp(transform.position.y, minY, maxY);

        transform.position = transformPosition;
    }
}
