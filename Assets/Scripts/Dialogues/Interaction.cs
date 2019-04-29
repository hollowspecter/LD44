using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class Interaction : MonoBehaviour
{
    public GameObject dialogueRunner;
    private bool interacting;
    public string activeCharacter;

    // Start is called before the first frame update
    void Start()
    {
        interacting = true;
    }

    // Update is called once per frame
    void OnMouseDown()
    {
        if(interacting)
        {
            name = activeCharacter;
            dialogueRunner.GetComponent<DialogueRunner>().StartDialogue(activeCharacter);
            interacting = false;
        }
    }
}
