using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
	[System.Serializable]
    public class DialoguePhrase
    {
    [TextArea(2, 5)]
    public string phrase;
    public int correctButtonIndex;  // index of the correct button in responseButtons array
    }
public class DialogueManager : MonoBehaviour
{
    public GameObject dialogueBox;
    public TMP_Text clientText;
    public Button[] responseButtons;


    public List<DialoguePhrase> dialoguePhrases;
	public DialoguePhrase currentPhrase;

    public bool IsDialogueActive => dialogueBox.activeSelf;
	
	public PlayerController playerController;

    void Awake()
    {
        playerController = FindObjectOfType<PlayerController>();
        if (playerController == null)
        {
            Debug.LogWarning("PlayerController not found in the scene!");
        }
    }
    public void ShowRandomDialogue()
    {if (dialoguePhrases.Count == 0) return;

    currentPhrase = dialoguePhrases[Random.Range(0, dialoguePhrases.Count)];

    dialogueBox.SetActive(true);
    clientText.text = currentPhrase.phrase;

    for (int i = 0; i < responseButtons.Length; i++)
    {
        Button btn = responseButtons[i];
        btn.gameObject.SetActive(true);
        btn.onClick.RemoveAllListeners();

        int buttonIndex = i;  // capture the current index for closure
        btn.onClick.AddListener(() => OnResponseSelected(buttonIndex));
    }

    Cursor.visible = true;
    Cursor.lockState = CursorLockMode.None;

    if (playerController != null)
        playerController.canLookAround = false;  // Lock camera rotation}

     void HideDialogue()
    {
        dialogueBox.SetActive(false);

        // Hide and lock cursor back after dialogue closes
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
		
		 if (playerController != null)
        playerController.canLookAround = true;
    }

   void OnResponseSelected(int selectedButtonIndex)
{
    if (selectedButtonIndex == currentPhrase.correctButtonIndex)
    {
        Debug.Log("Correct answer selected!");
        // Optionally add positive feedback here
        HideDialogue();
    }
    else
    {
        Debug.Log("Wrong answer selected!");
        // Optionally add negative feedback here (shake UI, sound, etc.)
        // You can also keep dialogue open for retry if you want
    }
}
}
}
