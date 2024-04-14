using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using StarterAssets;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour {
	public Transform circle;
	public Transform spawnPoint;
	public Transform armature;
	public Transform[] spawnPoints;
	public Vector3 spawnPointOffset;
	public Vector3 armatureZeroPos;
	public Vector3 circleStartingPos;
	public Vector3 circleStartingEuler;
	public bool isAnimating;
	public bool isInDialogue;

	public ThirdPersonController tpController;
	public PlayerInput playerInput;
	public CharacterController characterController;
	public Transform cameraTarget;
	public CinemachineBrain cBrain;
	public CinemachineVirtualCamera entryAndExitCamera;
	public CinemachineVirtualCamera playerFollowCamera;


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
				isAnimating = true;
				circleAnim.SetTrigger("leaving");
			}
		}

		if (isAnimating != isAnimatingLast || isInDialogue != isInDialogueLast) {
			//Set newest values
			isAnimatingLast = isAnimating;
			isInDialogueLast = isInDialogue;

			//Turn the player controls off if in dialogue, or animating
			if (isAnimating || isInDialogue) {
				playerInput.SwitchCurrentActionMap("Dialogue");
				switchToCamera(entryAndExitCamera);
			}

			//Otherwise turn them back on
			else {
				playerInput.SwitchCurrentActionMap("Player");
				switchToCamera(playerFollowCamera);
			}
		}

		circle.localEulerAngles = new Vector3(circleStartingEuler.x, armature.localEulerAngles.y,
			circleStartingEuler.z + circleStartingEuler.z);
		Debug.Log(circle.localEulerAngles);
	}

	void switchToCamera(CinemachineVirtualCamera cam) {
		entryAndExitCamera.enabled = false;
		playerFollowCamera.enabled = false;

		cam.enabled = true;
	}
}