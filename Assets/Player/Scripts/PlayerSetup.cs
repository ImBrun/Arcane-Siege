using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Netcode;

public class Movement : NetworkBehaviour
{

    public float speed = 5.0f;
    private float horizontalInput;
    private float forwardInput;
    public GameObject sn;

    private void Start()
    {
        sn = GameObject.Find("SpawnManagement");
        transform.position = sn.GetComponent<Spawner>().SpawnPlayer().position;
    }

    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        forwardInput = Input.GetAxis("Vertical");

        transform.Translate(Vector3.forward * Time.deltaTime * speed * forwardInput);
        transform.Translate(Vector3.right * Time.deltaTime * speed * horizontalInput);
    }

    
}
