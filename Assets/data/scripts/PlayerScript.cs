// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable ArrangeTypeMemberModifiers

using Cinemachine;
using StarterAssets;
using UnityEngine;
using UnityEngine.InputSystem;
using PixelCrushers.DialogueSystem;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour {
	public Transform circle;
	public GameObject exitPortalTrigger;
	public Vector3 spawnPoint;

	public Transform armature;
	public CharacterController characterController;
	public Vector3 circleOffset = new Vector3(0, 0.035f, 0);

	public Transform fetchItemsHolder;
	public Vector3 spawnPointOffset;
	public Vector3 armatureZeroPos;
	public Vector3 circleStartingPos;
	public Vector3 circleStartingEuler;
	public StarterAssetsInputs inputs;
	public bool isHomeBase;

	public PlayerInput playerInput;
	public NPCScript dialogueTarget;


	public CinemachineBrain primaryCamera;
	public CinemachineVirtualCamera entryAndExitCamera;
	public CinemachineVirtualCamera playerFollowCamera;
	public CinemachineVirtualCamera dialogueCamera;
	private Vector3 cameraMainInitialPos;
	private Quaternion cameraMainInitialRot;
	private Vector3 playerFollowCameraInitialPos;
	private Quaternion playerFollowCameraInitialRot;
	private Vector3 entryAndExitCameraInitialPos;
	private Quaternion entryAndExitCameraInitialRot;
	private Vector3 dialogueCameraInitialPos;
	private Quaternion dialogueCameraInitialRot;

	private QuestManagerScript questManager;

	public CinemachineTargetGroup cTargets;

	//NPC holder
	public NPCScript nearbyNpc;
	public NPCScript summoningNpc;

	//UI
	public GameObject UIPressEToCollect;
	public GameObject UIPressEToTalk;


	//Quest stuff
	public string questTypeLast = "none";
	public bool canCollectQuestItem;

	public string state;
	public string stateLast;


	private Animator circleAnim;
	private static readonly int Entering = Animator.StringToHash("entering");
	private static readonly int Leaving = Animator.StringToHash("leaving");


	// Start is called before the first frame update
	void Start() {
		//Get the Quest Manager
		questManager = GameObject.Find("QuestManager").GetComponent<QuestManagerScript>();

		armatureZeroPos = armature.localPosition;
		circleAnim = GetComponent<Animator>();
		circleStartingPos = circle.localPosition;
		circleStartingEuler = circle.localEulerAngles;

		cameraMainInitialPos = Camera.main.transform.position;
		cameraMainInitialRot = Camera.main.transform.rotation;
		playerFollowCameraInitialPos = entryAndExitCamera.transform.position;
		playerFollowCameraInitialRot = entryAndExitCamera.transform.rotation;
		entryAndExitCameraInitialPos = playerFollowCamera.transform.position;
		entryAndExitCameraInitialRot = playerFollowCamera.transform.rotation;
		dialogueCameraInitialPos = dialogueCamera.transform.position;
		dialogueCameraInitialRot = dialogueCamera.transform.rotation;

		circleAnim.SetBool("isHomeBase", isHomeBase);

		cTargets.m_Targets[0].target = armature;

		state = "entering level";


		SetSpawnPoint();

		transform.position = spawnPoint;

		ResetUI();
	}

	// Update is called once per frame
	void Update() {
		//Quest on change events
		if (questManager.quest.questType != questTypeLast) {
			switch (questManager.quest.questType) {
				case "none":

					break;

				case "fetch":

					break;

				case "paint":

					break;

				case "clean":

					break;

				case "move":

					break;
			}
		}


		//Quest each frame events
		switch (questManager.quest.questType) {
			case "none":

				break;

			case "fetch":
				if (questManager.quest.questItem != null && canCollectQuestItem && Input.GetKeyDown(KeyCode.E)) {
					canCollectQuestItem = false;

					Destroy(questManager.quest.questItem.gameObject);

					UIPressEToCollect.SetActive(false);

					QuestLog.SetQuestState(questManager.quest.questName, QuestState.ReturnToNPC);
				}

				break;

			case "paint":

				break;

			case "clean":

				break;

			case "move":

				break;
		}


		//state on change events
		if (state != stateLast) {
			switch (state) {
				case "playing":

					playerInput.ActivateInput();
					playerInput.SwitchCurrentActionMap("Player");

					inputs.cursorInputForLook = true;

					SwitchToCamera(playerFollowCamera);


					break;


				//This is called when the player is "summoned" into a level
				case "entering level":

					//Clear ui
					ResetUI();

					//More the armature back to "zero"
					armature.localPosition = armatureZeroPos;

					//Move the player object back to the spawn
					transform.position = spawnPoint;

					//Move the summoning circle back to "zero"
					circle.localPosition = circleStartingPos;

					//Rotate the summoning circle back to "zero"
					circle.localEulerAngles = circleStartingEuler;


					//Lock the mouse
					Cursor.lockState = CursorLockMode.Locked;

					//Trigger the player summon animation
					circleAnim.SetTrigger(Entering);

					var lookAtOnSpawn = summoningNpc != null ? summoningNpc.transform.position : armature.forward;

					//Make the player face the spawn point
					armature.LookAt(new Vector3(lookAtOnSpawn.x, armature.position.y, lookAtOnSpawn.z));


					//Switch to the entry exit camera
					SwitchToCamera(entryAndExitCamera);

					playerInput.SwitchCurrentActionMap("Dialogue");
					playerInput.DeactivateInput();
					break;

				case "leaving level":

					//Clear ui
					ResetUI();

					//More the armature back to "zero"
					armature.localPosition = armatureZeroPos;

					//Move the player object back to the spawn
					transform.position = spawnPoint;

					//Move the summoning circle back to "zero"
					circle.localPosition = circleStartingPos;

					//Rotate the summoning circle back to "zero"
					circle.localEulerAngles = circleStartingEuler;


					//Lock the mouse
					Cursor.lockState = CursorLockMode.Locked;

					//Trigger the player summon animation
					circleAnim.SetTrigger(Leaving);

					//Switch to the entry exit camera
					SwitchToCamera(entryAndExitCamera);

					playerInput.SwitchCurrentActionMap("Dialogue");
					playerInput.DeactivateInput();

					break;

				case "dialogue":

					//See if there was a lookat target in the npc
					Transform lookAtTarget = dialogueTarget.transform.GetComponentInChildren<LookAtTarget>().transform;

					//If there was a lookat target, add it to the camera group, otherwise just target the npc's transform
					cTargets.m_Targets[1].target = lookAtTarget != null ? lookAtTarget : dialogueTarget.transform;

					//Lock the mouse
					Cursor.lockState = CursorLockMode.Confined;

					inputs.cursorInputForLook = false;

					playerInput.SwitchCurrentActionMap("Dialogue");
					playerInput.DeactivateInput();

					armature.LookAt(new Vector3(dialogueTarget.transform.position.x, armature.position.y, dialogueTarget.transform.position.z));

					SwitchToCamera(dialogueCamera);

					break;


				case "unsummoned":
					SceneManager.LoadScene("data/scenes/home");
					break;
			}

			stateLast = state;
		}

		//Each frame state-stuff
		switch (state) {
			case "playing":


				Cursor.lockState = CursorLockMode.Locked;

				if (nearbyNpc) {
					UIPressEToTalk.SetActive(true);

					if (Input.GetKeyDown(KeyCode.E)) {
						if (nearbyNpc.questType == "fetch") {
							var item = nearbyNpc.questItem.GetComponent<CollectableItemScript>();
							DialogueLua.SetVariable("CurrentItem", item.itemName);
						}

						//Set who you're talking to
						dialogueTarget = nearbyNpc;
						DialogueManager.StartConversation(nearbyNpc.dialogue, armature, nearbyNpc.transform, 0);
						SetState("dialogue");
					}
				}
				else {
					UIPressEToTalk.SetActive(false);
				}

				if (isHomeBase && questManager.quest.questType != null) {
					circleAnim.SetTrigger("portalReady");
					
				}

				if (isHomeBase) {
					circle.localPosition = circleStartingPos;
				}
				else if (characterController.enabled) {
					circle.position = transform.position + circleOffset;
				}


				break;


			//This is called when the player is "summoned" into a level
			case "entering level":
				Cursor.lockState = CursorLockMode.Locked;
				break;

			case "leaving level":
				Cursor.lockState = CursorLockMode.Locked;
				break;

			case "dialogue":
				Cursor.lockState = CursorLockMode.Confined;
				break;

			case "unsummoned":
				//DEBUG
				//If the playing is unsummoned, and presses O, re-summon
				if (Input.GetKeyDown(KeyCode.O)) {
					SetState("entering level");
				}

				break;
		}
	}

	void SetSpawnPoint() {
		var fallback = GameObject.Find("/PlayerSpawnPoint");
		var npc = GameObject.FindAnyObjectByType<QuestGiverScript>();

		if (npc != null) {
			summoningNpc = npc.GetComponent<NPCScript>();

			spawnPoint = npc.transform.position + (npc.transform.forward * 2.1f) + spawnPointOffset;
		}
		else if (fallback != null) {
			spawnPoint = fallback.transform.position + spawnPointOffset;
		}
		else {
			spawnPoint = transform.position;
		}
	}

	void SwitchToCamera(CinemachineVirtualCamera cam) {
		entryAndExitCamera.enabled = false;
		playerFollowCamera.enabled = false;
		dialogueCamera.enabled = false;

		cam.enabled = true;
	}

	public void StartDialogue() {
		SetState("dialogue");
	}

	public void EndDialogue() {
		Cursor.lockState = CursorLockMode.Locked;
		DialogueManager.StopAllConversations();
	}

	public void SpawnFetchQuestObject(Transform obj) {
		questManager.quest.questItem = Instantiate(obj, new Vector3(0, 0, -999), Quaternion.identity, fetchItemsHolder);
	}

	public void SetQuestType(NPCScript npc) {
		questManager.StartRandomQuest();


		if (questManager.quest.questItem != null) {
			SpawnFetchQuestObject(questManager.quest.questItem);
		}
	}

	void ResetUI() {
		UIPressEToCollect.SetActive(false);
		UIPressEToTalk.SetActive(false);
	}

	void ResetCameras() {
		bool last = primaryCamera.enabled;

		primaryCamera.enabled = false;
		primaryCamera.transform.SetPositionAndRotation(cameraMainInitialPos, cameraMainInitialRot);
		primaryCamera.enabled = last;


		last = entryAndExitCamera.enabled;
		entryAndExitCamera.enabled = false;
		entryAndExitCamera.transform.SetPositionAndRotation(playerFollowCameraInitialPos, playerFollowCameraInitialRot);
		entryAndExitCamera.enabled = last;

		last = playerFollowCamera.enabled;
		playerFollowCamera.enabled = false;
		playerFollowCamera.transform.SetPositionAndRotation(playerFollowCameraInitialPos, playerFollowCameraInitialRot);
		playerFollowCamera.enabled = last;


		last = dialogueCamera.enabled;
		dialogueCamera.enabled = false;
		dialogueCamera.transform.SetPositionAndRotation(dialogueCameraInitialPos, dialogueCameraInitialRot);
		playerFollowCamera.enabled = last;
	}

	public void SetState(string newState) {
		state = newState;
		ResetUI();
	}

	public void QuestFailed() {
		var questState = QuestLog.CurrentQuestState("QuestFailed");
		if (questState == QuestLog.ActiveStateString) {
			QuestLog.SetQuestState("QuestFailed", QuestState.Unassigned);
			SetState("leaving level");
		}
	}

	public void QuestSuccess() {
		var questState = QuestLog.CurrentQuestState("QuestSuccess");
		if (questState == QuestLog.ActiveStateString) {
			QuestLog.SetQuestState("QuestSuccess", QuestState.Unassigned);
			SetState("leaving level");
		}
	}

	void QuestFetch() {
		var questState = QuestLog.CurrentQuestState("QuestFetch");
		if (questState == QuestLog.ActiveStateString) {
			SetQuestType(nearbyNpc);
			SetState("playing");
		}
	}

	public void StartFailedConversation() {
		var questState = QuestLog.CurrentQuestState("StartFailedConversation");
		if (questState == QuestLog.ActiveStateString) {
			DialogueManager.StartConversation("FailedSpeech", armature, armature, 0);
		}
	}

	public void TriggerEnter(Collider other) {
		if (questManager.quest.questItem != null && other.transform == questManager.quest.questItem.transform) {
			UIPressEToCollect.SetActive(true);

			canCollectQuestItem = true;
		}
		else if (other.CompareTag("NPC")) {
			NPCScript npc = other.GetComponent<NPCScript>();
			if (npc.dialogue != null && npc.dialogue.Length > 0) {
				nearbyNpc = npc;
			}
		}
	}

	public void TriggerExit(Collider other) {
		if (questManager.quest.questItem != null && other.transform == questManager.quest.questItem.transform) {
			UIPressEToCollect.SetActive(false);

			canCollectQuestItem = false;
		}
		else if (other.CompareTag("NPC")) {
			NPCScript npc = other.GetComponent<NPCScript>();
			if (nearbyNpc == npc) {
				nearbyNpc = null;
			}
		}

		//TODO: make the trigger zone only appear when you step out of the portals
		else if (isHomeBase && other.CompareTag("ExitPortalTrigger")) {
			//Trigger the player summon animation
			//circleAnim.SetTrigger(Leaving);
			exitPortalTrigger.SetActive(false);
		}

		Debug.Log(other.CompareTag("ExitPortalTrigger"));
	}
}