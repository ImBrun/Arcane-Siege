using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotation : MonoBehaviour
{
    public LayerMask layersToHit;
    public Vector3 rotationMask;
    [SerializeField]
    private Camera playerCamera;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 screenPosition, worldPosition, direction;
        screenPosition = Input.mousePosition;

        Ray ray = playerCamera.ScreenPointToRay(screenPosition);

        if (Physics.Raycast(ray, out RaycastHit hitData, 100f, layersToHit)){
            worldPosition = hitData.point;
            direction = worldPosition - transform.position;
            Vector3 lookAtRot = Quaternion.LookRotation(direction).eulerAngles;
            transform.rotation = Quaternion.Euler(Vector3.Scale(lookAtRot, rotationMask));
        }
    }
}
