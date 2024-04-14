using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableItemScript : MonoBehaviour {
    public string itemName;
    public string description;
    public Transform[] spawnPoints;
    
    void Start() {
        if (spawnPoints.Length > 0) {
            //Auto move to one of it's spawn points
            transform.position = spawnPoints[Random.Range(0, spawnPoints.Length)].position;
        }
        else {
            Debug.Log(itemName + " is missing it's spawn points");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
