using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Median : MonoBehaviour
{
    public GameObject player1;
    public GameObject player2;

    private float medianX;
    private float medianY;

    private float cameraSpeed = 1.0f;

    private Vector2 destination;

    private void Start()
    {
        
    }

    private void Update()
    {

    }

    private void FixedUpdate()
    {
        
            medianX = (player1.transform.position.x + player2.transform.position.x) /2;


        if (player1.transform.position.y > player2.transform.position.y)
        {
            medianY = player1.transform.position.y - player2.transform.position.y;
        }
        else if (player2.transform.position.y > player1.transform.position.y)
        {
            medianY = (player2.transform.position.y - player1.transform.position.y)/2;
        }

        gameObject.transform.position = new Vector2(medianX, medianY);
    }
}
