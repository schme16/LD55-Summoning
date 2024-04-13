using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXTriggerScript : MonoBehaviour {
	private AudioSource sfx;

	public bool play;

	// Start is called before the first frame update
	void Start() {
		sfx = GetComponent<AudioSource>();
	}

	// Update is called once per frame
	void Update() {
		if (play) {
			play = false;
			sfx.PlayOneShot(sfx.clip);
		}
	}
}