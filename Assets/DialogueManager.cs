using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class DialoguePhrase
{
    [TextArea(2, 5)]
    public string phrase;
    public int correctButtonIndex;
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

    public GameObject currentClient;
    public TMP_Text scoreText;
    private int score = 0;

    void Awake()
    {
        playerController = FindObjectOfType<PlayerController>();
        if (playerController == null)
        {
            Debug.LogWarning("PlayerController not found in the scene!");
        }
    }

    public void ShowRandomDialogue()
    {
        if (dialoguePhrases.Count == 0) return;

        currentPhrase = dialoguePhrases[Random.Range(0, dialoguePhrases.Count)];

        dialogueBox.SetActive(true);
        clientText.text = currentPhrase.phrase;

        for (int i = 0; i < responseButtons.Length; i++)
        {
            Button btn = responseButtons[i];
            btn.gameObject.SetActive(true);
            btn.onClick.RemoveAllListeners();

            int buttonIndex = i;
            btn.onClick.AddListener(() => OnResponseSelected(buttonIndex));
        }

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        if (playerController != null)
            playerController.canLookAround = false;

        if (currentClient != null)
        {
            var mover = currentClient.GetComponent<ClientMover>();
            if (mover != null)
                mover.HideExclamation();
        }
    }

    void HideDialogue()
    {
        dialogueBox.SetActive(false);

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
            score += 10;
            UpdateScoreUI();

            // Răspuns corect → clientul rămâne la masă
        }
        else
        {
            Debug.Log("Wrong answer selected!");

            if (currentClient != null)
            {
                var mover = currentClient.GetComponent<ClientMover>();
                if (mover != null)
                    mover.ReturnToSpawn(true); // Față nervoasă + plecare
            }
        }

        HideDialogue();
    }

    void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = "$ " + score.ToString();
    }
}
