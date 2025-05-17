using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public float Boundary = 0.3f;
    public float maxSpeed = 5f;
    public int directionMagnitude;

    private int screenWidth;
    private GameObject player;
    Vector3 newPosition;
    private Vector3 velocity = Vector3.zero;

    public float smoothTime;

    // Start is called before the first frame update
    void Start()
    {
        
        player = GameObject.FindGameObjectWithTag("Player");
    }


    void LateUpdate()
    {
        Vector3 playerPosition = Camera.main.WorldToScreenPoint(player.transform.position);
        Vector3 targetPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);

        if (playerPosition.x > Screen.width - Boundary)
        {
            targetPosition += Vector3.right * directionMagnitude;
        }

        if (playerPosition.x < Boundary)
        {
            targetPosition += Vector3.left * directionMagnitude;
        }
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime, maxSpeed);

    }
}
