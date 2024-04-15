using UnityEngine;

public class NPCScript : MonoBehaviour {
	public string questType;
	public string dialogue;
	public string questName;
	public Transform questItem;

	public PlayerScript player;

	// Start is called before the first frame update
	void Start() {
		//Get the player object
		player = GameObject.Find("player").GetComponent<PlayerScript>();
		Debug.Log(player);

		//Set a random fetch item
		questItem = player.fetchQuestItemPrefabs[Random.Range(0, player.fetchQuestItemPrefabs.Length)];

		Debug.Log(questItem);
	}

	// Update is called once per frame
	void Update() {
	}
}