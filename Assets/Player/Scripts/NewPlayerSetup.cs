using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Unity.Netcode;

public class NewPlayerSetup : NetworkBehaviour
{
    [SerializeField]
    Behaviour[] compsToDisable;

    Camera sceneCamera;

    // Start is called before the first frame update
    void Start()
    {
        if(!IsLocalPlayer) {
            for(int i = 0; i < compsToDisable.Length; i++) {
                compsToDisable[i].enabled = false;
            }
        }

        sceneCamera = Camera.main;
        if(sceneCamera != null) {
            sceneCamera.gameObject.SetActive(false);
        }
    }

    void OnDisable() {
        if (sceneCamera != null) {
            sceneCamera.gameObject.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
