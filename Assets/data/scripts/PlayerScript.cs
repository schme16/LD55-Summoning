using System.Collections;
using System.Collections.Generic;
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
	public bool isAnimating;
	public bool isInDialogue;
	private bool isAnimatingLast;
	private bool isInDialogueLast;

	private Animator circleAnim;
	public PlayerInput playerInput;
	public CharacterController characterController;


	// Start is called before the first frame update
	void Start() {
		armatureZeroPos = armature.localPosition;
		circleAnim = GetComponent<Animator>();
		circleStartingPos = circle.localPosition;
	}

	// Update is called once per frame
	void Update() {
		//Debug
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
			}
			
			//Otherwise turn them back on
			else {
				playerInput.SwitchCurrentActionMap("Player");
			}
		}
	}
}