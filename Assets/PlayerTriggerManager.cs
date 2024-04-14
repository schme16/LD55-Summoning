using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTriggerManager : MonoBehaviour {
	public PlayerScript player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
	private void OnTriggerEnter(Collider other) {
		player.TriggerEnter(other);
	}

	private void OnTriggerExit(Collider other) {
		player.TriggerExit(other);
	}
}
