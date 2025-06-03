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
    public GameObject foodPrefab; // Food prefab to spawn for this phrase
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

    private FoodSpawner foodSpawner;

    void Awake()
    {
        playerController = FindObjectOfType<PlayerController>();
        if (playerController == null)
            Debug.LogWarning("PlayerController not found in the scene!");

        foodSpawner = FindObjectOfType<FoodSpawner>();
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
            playerController.isLocked = true;

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
            playerController.isLocked = false;

        TooltipManager.Instance.HideTooltip();
    }

    void OnResponseSelected(int selectedButtonIndex)
    {
        if (selectedButtonIndex == currentPhrase.correctButtonIndex)
        {
            Debug.Log("Correct answer selected!");
			AddScore(10);

            // Spawn the food prefab associated with this dialogue phrase
            if (foodSpawner != null && currentPhrase.foodPrefab != null)
            {
                foodSpawner.SpawnFood(currentPhrase.foodPrefab);
            }

            // Disable client interaction until food is delivered
            if (currentClient != null)
            {
                ClientMover mover = currentClient.GetComponent<ClientMover>();
                if (mover != null)
                {
                    mover.canInteract = false;
                }
            }
        }
        else
        {
            Debug.Log("Wrong answer selected!");

            if (currentClient != null)
            {
                var mover = currentClient.GetComponent<ClientMover>();
                if (mover != null)
                    mover.ReturnToSpawn(true); // angry face + leave
            }
        }

        HideDialogue();
    }

    // Call this from player interaction when they deliver the correct food
    public void OnFoodDeliveredCorrect()
    {
        Debug.Log("Correct food delivered!");

        AddScore(10);

        if (currentClient != null)
        {
            ClientMover mover = currentClient.GetComponent<ClientMover>();
            if (mover != null)
            {
                mover.canInteract = false;
                mover.LeaveAfterDelay(5f);
            }
        }
    }

    // Call this from player interaction when they deliver the wrong food
    public void OnFoodDeliveredWrong()
    {
        Debug.Log("Wrong food delivered!");

        if (currentClient != null)
        {
            ClientMover mover = currentClient.GetComponent<ClientMover>();
            if (mover != null)
            {
                mover.ReturnToSpawn(true); // angry face + leave
            }
        }
    }

    public void AddScore(int amount)
    {
        score += amount;
        UpdateScoreUI();
    }

    void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = "$ " + score.ToString();
    }
}
