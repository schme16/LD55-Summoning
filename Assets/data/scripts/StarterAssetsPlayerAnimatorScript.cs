using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarterAssetsPlayerAnimatorScript : MonoBehaviour {

    private Animator anim;
    private CharacterController characterController;
    public Transform circle;
    public bool animEnabeled;
    public Vector3 circleOffset = new Vector3(0,0,0);
    // Start is called before the first frame update
    void Start() {
        anim = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update() {
        anim.enabled = animEnabeled;

        if (characterController.enabled) {
            circle.position = transform.position + circleOffset;
        }
    }
}
