using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    [SerializeField]
    private Vector3 rotationDirection;

    void Update()
    {
        transform.Rotate(rotationDirection);
    }
}
