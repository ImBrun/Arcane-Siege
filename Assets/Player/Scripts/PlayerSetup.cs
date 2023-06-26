using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Netcode;

public class Movement : NetworkBehaviour
{

    public float speed;
    public LayerMask layersToHit;
    public Vector3 rotationMask;
    private Vector2 move;

    public void OnMove(InputAction.CallbackContext context){
        move = context.ReadValue<Vector2>();
    }

    void Start()
    {
        
    }

    void Update() {
        movePlayer();
    }

    public void movePlayer() {
        Vector3 movement = new Vector3(move.x, 0f, move.y);
        Vector3 screenPosition, worldPosition, direction;
        
        screenPosition = Input.mousePosition;

        Ray ray = Camera.main.ScreenPointToRay(screenPosition);

        if (Physics.Raycast(ray, out RaycastHit hitData, 100f, layersToHit)){
            worldPosition = hitData.point;
            direction = worldPosition - transform.position;
            Vector3 lookAtRot = Quaternion.LookRotation(direction).eulerAngles;
            transform.rotation = Quaternion.Euler(Vector3.Scale(lookAtRot, rotationMask));
        }

        transform.Translate(movement * speed * Time.deltaTime, Space.World);
    }

    
}
