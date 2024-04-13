using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour {
	public Animator circle;
	public Transform spawnPoint;
	public Transform armature;
	public Transform[] spawnPoints;
	public Vector3 spawnPointOffset;
	public Vector3 armatureZeroPos;
	public bool isAnimating;

	// Start is called before the first frame update
	void Start() {
		armatureZeroPos = armature.localPosition;
	}

	// Update is called once per frame
	void Update() {
		
		//Debug
		if (!isAnimating) {
			if (!circle.GetCurrentAnimatorStateInfo(0).IsName("idle") && Input.GetKeyDown(KeyCode.O)) {
				isAnimating = true;

				spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

				armature.localPosition = armatureZeroPos;

				transform.position = spawnPoint.position + spawnPointOffset;

				circle.SetTrigger("entering");
			}

			if (!circle.GetCurrentAnimatorStateInfo(0).IsName("idle-down") &&Input.GetKeyDown(KeyCode.P)) {
				isAnimating = true;
				circle.SetTrigger("leaving");
			}
		}
	}
}