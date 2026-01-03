using UnityEngine;
using System.Collections.Generic;
using System.Collections;   

public class CameraMovement : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;

    void Start()
    {
        
    }

    void Update()
    {
        transform.position = target.position + offset;
    }
}
