using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableItemScript : MonoBehaviour {
	public string itemName;
	public string dialogue;
	public string spawnPointsDirectory;
	private List<Transform> spawnPoints;
	private Transform spawnPoint;

	void Start() {
		spawnPoints = new List<Transform>();
		foreach (Transform child in GameObject.Find("/fetchItemsSpawnPoints/" + spawnPointsDirectory).transform) {
			spawnPoints.Add(child);
		}

		if (spawnPoints.Count > 0) {
			//Auto pick a spawn point
			spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
			
			//Move to the spawn point
			transform.position = spawnPoint.position;
			
			//Rotate to match the spawn point
			transform.rotation = spawnPoint.rotation;
		}
		else {
		}
	}

	// Update is called once per frame
	void Update() {
	}
}