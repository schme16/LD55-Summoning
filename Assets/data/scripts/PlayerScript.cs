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
	public Transform[] fetchItemsSpawnLocations;
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


	//Quest stuff
	public string questType = "none";
	public string questTypeLast = "none";
	public string questName;
	public Transform questItem;
	public bool canCollectQuestItem;

	
	//Create a quest structure item
	//Should allow me to have a bunch of preset quests
	public struct Quest {
		private string questType;
		private string questName;
		private Transform questItem;
	}

	//Prefabs
	public Transform HeartPillsPrefab;


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

				inputs.cursorInputForLook = true;

				switchToCamera(playerFollowCamera);
			}
		}

		circle.localEulerAngles = new Vector3(circleStartingEuler.x, armature.localEulerAngles.y,
			circleStartingEuler.z + circleStartingEuler.z);

		/*
		if (!isInDialogue && Input.GetKeyDown(KeyCode.I)) {
			Debug.Log(1111111);
			if (QuestLog.CurrentQuestState(questName) != QuestLog.ActiveStateString) {
				Debug.Log(222222);

				DialogueManager.StartConversation("Old Guy's Medicine", armature, oldGuyNpc, 0);
			}
		}*/

		if (!isAnimating && !isInDialogue) {
			if (nearbyNPC) {
				UIPressEToTalk.SetActive(true);
				if (Input.GetKeyDown(KeyCode.E)) {
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
		if (questType != questTypeLast) {
			switch (questType) {
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
		switch (questType) {
			case "none":

				break;

			case "fetch":
				if (canCollectQuestItem && Input.GetKeyDown(KeyCode.E)) {
					canCollectQuestItem = false;
					Destroy(questItem.gameObject);
					ResetUI();
					QuestLog.SetQuestState(questName, QuestState.ReturnToNPC);
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

	public void SpawnFetchQuestObject(Transform obj, Vector3 pos) {
		questItem = Instantiate(obj, pos, Quaternion.identity, fetchItemsHolder);
	}

	public void SetQuestType(string type, string quest, Transform obj = null) {
		questType = type;
		questName = quest;
		if (obj != null) {
			SpawnFetchQuestObject(obj,
				fetchItemsSpawnLocations[Random.Range(0, fetchItemsSpawnLocations.Length)].position);
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

	public void QuestMyHeartPills() {
		var state = QuestLog.CurrentQuestState("QuestMyHeartPills");
		if (state == QuestLog.ActiveStateString) {
			SetQuestType("fetch", "QuestMyHeartPills", HeartPillsPrefab);
		}
	}

	public void StartFailedConversation() {
		var state = QuestLog.CurrentQuestState("StartFailedConversation");
		if (state == QuestLog.ActiveStateString) {
			DialogueManager.StartConversation("FailedSpeech", armature, armature, 0);
		}
	}

	public void TriggerEnter(Collider other) {
		if (other.transform == questItem) {
			UIPressEToCollect.SetActive(true);
			canCollectQuestItem = true;
		}
		else if (other.CompareTag("NPC")) {
			NPCScript npc = other.GetComponent<NPCScript>();
			if (npc.dialogue.Length > 0) {
				nearbyNPC = npc;
			}
		}
	}

	public void TriggerExit(Collider other) {
		if (other.transform == questItem) {
			UIPressEToCollect.SetActive(false);
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