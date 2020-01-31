using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEntity : MonoBehaviour
{

    private List<PlayerPart> holdingParts;
    private Rigidbody rb;


    [SerializeField]
    private int playerIndex = 0;


    [SerializeField]
    private int startParts = 5;


    [SerializeField]
    private float shootBoostSpeed = 5f;
    [SerializeField]
    private float normalSpeed = 100f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        for (int i = 0; i < startParts; i++)
        {
            GainPart();
        }
    }


    public void GainPart()
    {

    }

    public void ThrowPart()
    {

    }

    void Update()
    {
        Vector3 direction = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Horizontal"));
        rb.AddForce(direction * normalSpeed);
    }
}
