using UnityEngine;

public class QuestManagerScript : MonoBehaviour {
	
	//Create a quest structure item
	//Should allow me to have a bunch of preset quests
	public struct Quest {
		public string questType;
		public string dialogue;
		public string questName;
		public Transform questItem;
	}
	
	
	public Quest quest;
	public PlayerScript player;

	//Prefabs
	public Transform[] fetchQuestItemPrefabs;
	public float count = 0;

	
	void Awake() {
		DontDestroyOnLoad(this);
	}

	public void StartRandomQuest() {
		//player.exitPortalTrigger.SetActive(true);
		quest = new Quest {
			questType = "fetch",
			dialogue = "Old Guy's Medicine",
			questName = "QuestFetch",
			questItem = fetchQuestItemPrefabs[Random.Range(0, fetchQuestItemPrefabs.Length)]
		};
	}

	// Start is called before the first frame update
	void Start() {
		player = GameObject.Find("player").GetComponent<PlayerScript>();
	}

	// Update is called once per frame
	void Update() {
		count += Time.deltaTime;
		
		if (count > 10) {
			StartRandomQuest();
		}
	}
}