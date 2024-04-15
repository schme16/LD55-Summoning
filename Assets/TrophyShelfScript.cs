using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrophyShelfScript : MonoBehaviour {
	public Transform[] itemSlots;

	public QuestManagerScript questManager;


	void Start() {
		//Get the Quest Manager
		questManager = GameObject.Find("QuestManager").GetComponent<QuestManagerScript>();

		var count = 0;
		foreach (var trophy in questManager.trophies) {
			if (itemSlots[count] != null) {
				var _trophy = Instantiate(trophy, itemSlots[count]).GetComponent<CollectableItemScript>();
				_trophy.transform.localPosition = _trophy.trophyPos;
				_trophy.transform.localEulerAngles = _trophy.trophyRot;
				_trophy.transform.localScale = _trophy.trophyScale;
			}

			count++;
		}
	}

	// Update is called once per frame
	void Update() {
	}
}