using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class Interaction : MonoBehaviour
{
    public GameObject dialogueRunner;
    private bool interacting;

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
            dialogueRunner.GetComponent<DialogueRunner>().StartDialogue();
            interacting = false;
        }
    }
}
