using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerNetworkStats : NetworkBehaviour
{
    public bool playerIsReady = false;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SetReadyPlayer() {
        playerIsReady = !playerIsReady;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
