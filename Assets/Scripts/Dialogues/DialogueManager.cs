using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Text;
using System.Collections.Generic;
using Yarn.Unity;
using Yarn;
using System;
using TMPro;
using static Yarn.Dialogue;

public class DialogueManager : Yarn.Unity.DialogueUIBehaviour
{
    /// The object that contains the dialogue and the options.
    /** This object will be enabled when conversation starts, and 
     * disabled when it ends.
     */
    public GameObject dialogueContainer;

    //public GameObject interact;

    /// The UI element that displays lines
    public TMP_Text lineText;

    //UI Box
    public GameObject boxText;

    /// A delegate (ie a function-stored-in-a-variable) that
    /// we call to tell the dialogue system about what option
    /// the user selected
    private Yarn.OptionChooser SetSelectedOption;

    /// How quickly to show the text, in seconds per character
    [Tooltip("How quickly to show the text, in seconds per character")]
    public float textSpeed = 0.025f;

    /// The buttons that let the user choose an option
    public List<Button> optionButtons;

    /// Make it possible to temporarily disable the controls when
    /// dialogue is active and to restore them when dialogue ends
    public RectTransform gameControlsContainer;
    
    private bool dialogueTimer;
    private bool count;
    //otherwise will skip
    public bool needsInput = true;
    //public bool nothing;

    public float autoSkipAfterSeconds = 3f;
    public RandomPitchSound voice;
    [Header("Every n-th letter is voiced")]
    public int voiceSpeed = 4;

    void Awake()
    {
        // Start by hiding the container, line and option buttons
        if (dialogueContainer != null)
            dialogueContainer.SetActive(false);

        lineText.gameObject.SetActive(false);
        boxText.SetActive(false);

        foreach (var button in optionButtons)
        {
            button.gameObject.SetActive(false);
        }

    }

    private void OnDisable()
    {
        Debug.Log ( "Dialogue Manager OnDisable: set all buttons inactive" );
        foreach ( var button in optionButtons ) button.gameObject.SetActive ( false );
    }

    /// Show a line of dialogue, gradually
    public override IEnumerator RunLine(Yarn.Line line)
    {
        // Show the text
        lineText.gameObject.SetActive(true);
       boxText.SetActive(true);

        if (textSpeed > 0.0f)
        {
            // Display the line one character at a time
            var stringBuilder = new StringBuilder();
            int count = 0;
            foreach (char c in line.text)
            {
                count++;
                if ( count % voiceSpeed == 0 ) voice.PlaySound ();
                stringBuilder.Append(c);
                lineText.text = stringBuilder.ToString();
                yield return new WaitForSeconds(textSpeed);
            }
        }
        else
        {
            // Display the line immediately if textSpeed == 0
            lineText.text = line.text;
        }


        // Wait for any user input
        if(needsInput)
        {
            float timer = 0f;
            while (Input.GetKeyDown(KeyCode.Space) == false
                && timer <= autoSkipAfterSeconds)
            {
                timer+=Time.deltaTime;
                yield return null;
            }
        }
        else //otherwise skip ahead
        {
            yield return new WaitForSeconds(2.0f);
        }


        // Hide the text and prompt
        lineText.gameObject.SetActive(false);
        boxText.SetActive(false);

    }

    /// Show a list of options, and wait for the player to make a selection.
    public override IEnumerator RunOptions(Yarn.Options optionsCollection,
                                            Yarn.OptionChooser optionChooser)
    {
        // Do a little bit of safety checking
        if (optionsCollection.options.Count > optionButtons.Count)
        {
            Debug.LogWarning("There are more options to present than there are" +
                             "buttons to present them in. This will cause problems.");
        }

        // Display each option in a button, and make it visible
        int i = 0;
        foreach (var optionString in optionsCollection.options)
        {
            optionButtons[i].gameObject.SetActive(true);
            //TO DO animate the button
            optionButtons[i].GetComponentInChildren<TMP_Text>().text = optionString;
            i++;
        }

        //start the timer of how long the player takes to answer
        //count = true;
        //if (count == true)
        //{
        //    optionButtons[0].GetComponent<TimerDialogue>().StartCount();
        //    count = false;
        //}

        // Record that we're using it
        SetSelectedOption = optionChooser;

        // Wait until the chooser has been used and then removed (see SetOption below)
        while (SetSelectedOption != null)
        {
            yield return null;
        }

        // Hide all the buttons
        foreach (var button in optionButtons)
        {
            button.gameObject.SetActive(false);
            //TO DO animate the button going away
        }
    }

    /// Called by buttons to make a selection.
    public void SetOption(int selectedOption)
    {

        //TO DO selectedOption is one of the randomnized options
        //if (nothing)
        //{
        //    var randomOption = UnityEngine.Random.Range(0, 3);
        //    selectedOption = randomOption;
        //    nothing = false;
        //}

        // Call the delegate to tell the dialogue system that we've
        // selected an option.
        SetSelectedOption(selectedOption);

        // Now remove the delegate so that the loop in RunOptions will exit
        SetSelectedOption = null;
    }

    /// Run an internal command.
    public override IEnumerator RunCommand(Yarn.Command command)
    {
        // "Perform" the command
        Debug.Log("Command: " + command.text);

        yield break;
    }

    /// Called when the dialogue system has started running.
    public override IEnumerator DialogueStarted()
    {
        Debug.Log("Dialogue starting!");

        // Enable the dialogue controls.
        if (dialogueContainer != null)
            dialogueContainer.SetActive(true);
            //TO DO animate the button

        // Hide the game controls.
        if (gameControlsContainer != null)
        {
            gameControlsContainer.gameObject.SetActive(false);
        }

        yield break;
    }

    /// Called when the dialogue system has finished running.
    public override IEnumerator DialogueComplete()
    {
        Debug.Log("Complete!");

        // Hide the dialogue interface.
        if (dialogueContainer != null)
            dialogueContainer.SetActive(false);
            //TO DO animate the button

        // Show the game controls.
        if (gameControlsContainer != null)
        {
            gameControlsContainer.gameObject.SetActive(true);
        }

        yield break;
    }
}
