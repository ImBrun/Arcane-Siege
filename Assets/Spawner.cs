using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField]
    private List<Transform> spawnPositions;
    private int i=-1;


    public Transform SpawnPlayer() {
        i++;
        Debug.Log("got here");
        return spawnPositions[i];
    }

}
