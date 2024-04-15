using System.Collections;
using System.Collections.Generic;
using PixelCrushers.DialogueSystem;
using Unity.Collections;
using UnityEngine;

public class QuestGiverScript : MonoBehaviour {
    public Transform[] options;
    public DialogueActor actor;
    // Start is called before the first frame update
    void Start()
    {
        foreach (var option in options) {
            option.gameObject.SetActive(false);
        }

        var npc = options[Random.Range(0, options.Length)];
            npc.gameObject.SetActive(true);
            actor.portrait = npc.GetComponent<LookAtTarget>().portrait;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
