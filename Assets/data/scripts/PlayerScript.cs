using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using StarterAssets;
using UnityEngine;
using UnityEngine.InputSystem;
using PixelCrushers.DialogueSystem;
using Random = UnityEngine.Random;

public class PlayerScript : MonoBehaviour {
	public Transform circle;
	public Transform spawnPoint;
	public Transform armature;
	public Transform oldGuyNpc;
	public Transform fetchItemsHolder;
	public Transform[] spawnPoints;
	public Vector3 spawnPointOffset;
	public Vector3 armatureZeroPos;
	public Vector3 circleStartingPos;
	public Vector3 circleStartingEuler;
	public bool isAnimating;
	public bool isInDialogue;
	public StarterAssetsInputs inputs;

	public ThirdPersonController tpController;
	public PlayerInput playerInput;
	public CharacterController characterController;
	public Transform cameraTarget;
	public Transform DialogueTarget;
	public CinemachineBrain cBrain;
	public CinemachineVirtualCamera entryAndExitCamera;
	public CinemachineVirtualCamera playerFollowCamera;
	public CinemachineVirtualCamera dialogueCamera;
	public CinemachineTargetGroup cTargets;

	public DialogueEntry dialogue;

	//NPC holder
	public NPCScript nearbyNPC;

	//UI
	public GameObject UIPressEToCollect;
	public GameObject UIPressEToTalk;


	//Create a quest structure item
	//Should allow me to have a bunch of preset quests
	public struct Quest {
		public string questType;
		public string dialogue;
		public string questName;
		public Transform questItem;
	}

	//Quest stuff
	public string questTypeLast = "none";
	public bool canCollectQuestItem;
	public Quest quest;

	//public string questName;
	//public Transform questItem;


	//Prefabs
	public Transform[] fetchQuestItemPrefabs;


	private Vector3 cameraTargetStartPos;
	private bool isAnimatingLast = true;
	private bool isInDialogueLast = true;
	private Animator circleAnim;


	// Start is called before the first frame update
	void Start() {
		armatureZeroPos = armature.localPosition;
		circleAnim = GetComponent<Animator>();
		circleStartingPos = circle.localPosition;
		circleStartingEuler = circle.localEulerAngles;
		cameraTargetStartPos = cameraTarget.localPosition;
		cTargets.m_Targets[0].target = armature;


		quest = new Quest();

		ResetUI();
	}

	// Update is called once per frame
	void Update() {
		if (!isAnimating) {
			if (!circleAnim.GetCurrentAnimatorStateInfo(0).IsName("idle") && Input.GetKeyDown(KeyCode.O)) {
				isAnimating = true;

				spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

				armature.localPosition = armatureZeroPos;

				transform.position = spawnPoint.position + spawnPointOffset;

				circle.localPosition = circleStartingPos;
				Cursor.lockState = CursorLockMode.Locked;

				circleAnim.SetTrigger("entering");
			}

			if (!circleAnim.GetCurrentAnimatorStateInfo(0).IsName("idle-down") && Input.GetKeyDown(KeyCode.P)) {
				/*isAnimating = true;
				circleAnim.SetTrigger("leaving");*/
			}
		}

		if (isAnimating != isAnimatingLast || isInDialogue != isInDialogueLast) {
			//Set newest values
			isAnimatingLast = isAnimating;
			isInDialogueLast = isInDialogue;

			//Turn the player controls off if in dialogue, or animating
			if (isAnimating) {
				playerInput.SwitchCurrentActionMap("Dialogue");
				playerInput.DeactivateInput();
				Cursor.lockState = CursorLockMode.Locked;
				switchToCamera(entryAndExitCamera);
			}

			else if (isInDialogue && DialogueTarget) {
				playerInput.SwitchCurrentActionMap("Dialogue");
				playerInput.DeactivateInput();
				inputs.cursorInputForLook = false;
				Cursor.lockState = CursorLockMode.Confined;

				transform.LookAt(
					new Vector3(DialogueTarget.position.x, transform.position.y, DialogueTarget.position.z));

				switchToCamera(dialogueCamera);
			}

			//Otherwise turn them back on
			else {
				playerInput.ActivateInput();
				playerInput.SwitchCurrentActionMap("Player");
				Cursor.lockState = CursorLockMode.Locked;

				inputs.cursorInputForLook = true;

				switchToCamera(playerFollowCamera);
			}
		}

		circle.localEulerAngles = new Vector3(circleStartingEuler.x, armature.localEulerAngles.y, circleStartingEuler.z + circleStartingEuler.z);

		/*
			if (!isInDialogue && Input.GetKeyDown(KeyCode.I)) {
				Debug.Log(1111111);
				if (QuestLog.CurrentQuestState(questName) != QuestLog.ActiveStateString) {
					Debug.Log(222222);

					DialogueManager.StartConversation("Old Guy's Medicine", armature, oldGuyNpc, 0);
				}
			}
		*/

		if (!isAnimating && !isInDialogue) {
			if (nearbyNPC) {
				UIPressEToTalk.SetActive(true);
				if (Input.GetKeyDown(KeyCode.E)) {
					if (nearbyNPC.questType == "fetch") {
						var item = nearbyNPC.questItem.GetComponent<CollectableItemScript>();
						DialogueLua.SetVariable("CurrentItem", item.itemName);
					}

					DialogueManager.StartConversation(nearbyNPC.dialogue, armature, nearbyNPC.transform, 0);
				}
			}
			else {
				UIPressEToTalk.SetActive(false);
			}
		}
		else {
			ResetUI();
		}


		//Quest on change events
		if (quest.questType != questTypeLast) {
			switch (quest.questType) {
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
		switch (quest.questType) {
			case "none":

				break;

			case "fetch":
				if (quest.questItem != null && canCollectQuestItem && Input.GetKeyDown(KeyCode.E)) {
					canCollectQuestItem = false;

					Destroy(quest.questItem.gameObject);
					ResetUI();
					QuestLog.SetQuestState(quest.questName, QuestState.ReturnToNPC);
				}

				break;

			case "paint":

				break;

			case "clean":

				break;

			case "move":

				break;
		}
	}

	void switchToCamera(CinemachineVirtualCamera cam) {
		entryAndExitCamera.enabled = false;
		playerFollowCamera.enabled = false;
		dialogueCamera.enabled = false;

		cam.enabled = true;
	}

	public void StartDialogue() {
		isInDialogue = true;
		Cursor.lockState = CursorLockMode.Confined;
	}

	public void EndDialogue() {
		isInDialogue = false;
		Cursor.lockState = CursorLockMode.Locked;
		DialogueManager.StopAllConversations();
	}

	public void SpawnFetchQuestObject(Transform obj) {
		quest.questItem = Instantiate(obj, new Vector3(0, 0, -999), Quaternion.identity, fetchItemsHolder);
	}

	public void SetQuestType(NPCScript npc) {
		quest = new Quest {
			questType = npc.questType,
			dialogue = npc.dialogue,
			questName = npc.questName,
			questItem = npc.questItem
		};

		if (quest.questItem != null) {
			SpawnFetchQuestObject(quest.questItem);
		}
	}

	void ResetUI() {
		UIPressEToCollect.SetActive(false);
		UIPressEToTalk.SetActive(false);
	}

	public void QuestFailed() {
		var state = QuestLog.CurrentQuestState("QuestFailed");
		if (state == QuestLog.ActiveStateString) {
			QuestLog.SetQuestState("QuestFailed", QuestState.Unassigned);
			isAnimating = true;
			circleAnim.SetTrigger("leaving");
		}
	}

	public void QuestSuccess() {
		var state = QuestLog.CurrentQuestState("QuestSuccess");
		if (state == QuestLog.ActiveStateString) {
			QuestLog.SetQuestState("QuestSuccess", QuestState.Unassigned);
			isAnimating = true;
			circleAnim.SetTrigger("leaving");
		}
	}

	public void QuestFetch() {
		var state = QuestLog.CurrentQuestState("QuestFetch");
		if (state == QuestLog.ActiveStateString) {
			isAnimating = false;
			SetQuestType(nearbyNPC);
		}
	}

	public void StartFailedConversation() {
		var state = QuestLog.CurrentQuestState("StartFailedConversation");
		if (state == QuestLog.ActiveStateString) {
			DialogueManager.StartConversation("FailedSpeech", armature, armature, 0);
		}
	}

	public void TriggerEnter(Collider other) {
		if (quest.questItem != null && other.transform == quest.questItem.transform) {
			UIPressEToCollect.SetActive(true);
			Debug.Log(22222222222);

			canCollectQuestItem = true;
		}
		else if (other.CompareTag("NPC")) {
			NPCScript npc = other.GetComponent<NPCScript>();
			if (npc.dialogue != null && npc.dialogue.Length > 0) {
				nearbyNPC = npc;
			}
		}
	}

	public void TriggerExit(Collider other) {
		if (quest.questItem != null && other.transform == quest.questItem.transform) {
			UIPressEToCollect.SetActive(false);
			Debug.Log(33333333333);

			canCollectQuestItem = false;
		}
		else if (other.CompareTag("NPC")) {
			NPCScript npc = other.GetComponent<NPCScript>();
			if (nearbyNPC == npc) {
				nearbyNPC = null;
			}
		}
	}
}