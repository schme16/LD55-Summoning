using System.Collections.Generic;
using UnityEngine;

public class NPCScript : MonoBehaviour {
	public string dialogue;
	private List<Transform> spawnPoints;
	public Transform spawnPoint;

	public QuestManagerScript questManager;

	// Start is called before the first frame update
	void Start() {
		spawnPoints = new List<Transform>();
		foreach (Transform child in GameObject.Find("/Summoner Spawn Points").transform) {
			spawnPoints.Add(child);
		}

		spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
		transform.position = spawnPoint.position;
		transform.rotation = spawnPoint.rotation;

		transform.position += new Vector3(0,0.55318747f,0);

		transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, transform.eulerAngles.z);
	}

	
	// Update is called once per frame
	void Update() {
	}
}