using System.Collections.Generic;
using UnityEngine;

public class QuestManagerScript : MonoBehaviour {
	//Create a quest structure item
	//Should allow me to have a bunch of preset quests
	public struct Quest {
		public string questType;
		public string dialogue;
		public string questName;
		public Transform questItem;
		public Transform questItemPrefab;
	}

	public List<Transform> trophies;
	public Transform trophyHolder;

	public Quest quest;
	public PlayerScript player;

	
	//Prefabs
	public Transform[] fetchQuestItemPrefabs;
	public float count = 0;
	public int randCount = 500;


	void Awake() {
		var qms = GameObject.Find("QuestManager");
		if (qms != null && qms != this.gameObject) {
			Destroy(this.gameObject);
		}
		DontDestroyOnLoad(this);
	}

	public void StartRandomQuest() {
		var item = fetchQuestItemPrefabs[Random.Range(0, fetchQuestItemPrefabs.Length)];
		quest = new Quest {
			questType = "fetch",
			dialogue = item.GetComponent<CollectableItemScript>().dialogue,
			questName = "QuestFetch",
			questItemPrefab = item
		};
	}

	// Start is called before the first frame update
	void Start() {
		player = GameObject.Find("player").GetComponent<PlayerScript>();
	}

	// Update is called once per frame
	void Update() {
		if (player == null) {
			player = GameObject.Find("player").GetComponent<PlayerScript>();
		}
		else if (player.isHomeBase) {
			
			if (count > randCount && quest.questType == null) {
				count = (randCount + 2);
				StartRandomQuest();
			}
			else if (count < randCount) {
				count += Time.deltaTime;
			}
		}
		else {
			count = 0;
		}
	}
}