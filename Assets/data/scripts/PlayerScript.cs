using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using StarterAssets;
using UnityEngine;
using UnityEngine.InputSystem;
using PixelCrushers.DialogueSystem;

public class PlayerScript : MonoBehaviour {
	public Transform circle;
	public Transform spawnPoint;
	public Transform armature;
	public Transform oldGuyNpc;
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
		cTargets.m_Targets[0].target = transform;
		Cursor.lockState = CursorLockMode.Locked;
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
				switchToCamera(entryAndExitCamera);
			}

			else if (isInDialogue && DialogueTarget) {
				playerInput.SwitchCurrentActionMap("Dialogue");
				playerInput.DeactivateInput();

				transform.LookAt(
					new Vector3(DialogueTarget.position.x, transform.position.y, DialogueTarget.position.z));

				switchToCamera(dialogueCamera);
			}

			//Otherwise turn them back on
			else {
				playerInput.ActivateInput();
				playerInput.SwitchCurrentActionMap("Player");
				switchToCamera(playerFollowCamera);
			}
		}

		circle.localEulerAngles = new Vector3(circleStartingEuler.x, armature.localEulerAngles.y,
			circleStartingEuler.z + circleStartingEuler.z);

		if (!isInDialogue && Input.GetKeyDown(KeyCode.I)) {
			DialogueManager.StartConversation("Not interested", armature, oldGuyNpc, 0);
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

	public void QuestFailed() {
		var state = QuestLog.CurrentQuestState("QuestFailed");
		if (state == "active") {
			QuestLog.SetQuestState("QuestFailed", QuestState.Unassigned);
			isAnimating = true;
			circleAnim.SetTrigger("leaving");
		}
	}

	public void QuestMyHeart() {
		var state = QuestLog.CurrentQuestState("QuestMyHeart");
		if (state == "active") {
			QuestLog.SetQuestState("QuestFailed", QuestState.Unassigned);
			isAnimating = true;
			circleAnim.SetTrigger("leaving");
		}
	}

	public void StartFailedConversation() {
		var state = QuestLog.CurrentQuestState("QuestFailed");
		if (state == "active") {
			DialogueManager.StartConversation("FailedSpeech", armature, armature, 0);
		}
	}
}