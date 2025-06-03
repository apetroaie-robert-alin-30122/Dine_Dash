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
    public GameObject foodPrefab;
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
        playerController = Object.FindAnyObjectByType<PlayerController>();
        if (playerController == null)
            Debug.LogWarning("PlayerController not found in the scene!");

        foodSpawner = Object.FindAnyObjectByType<FoodSpawner>();
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

        TooltipManager.Instance?.HideTooltip();
    }

    void OnResponseSelected(int selectedButtonIndex)
    {
        if (selectedButtonIndex == currentPhrase.correctButtonIndex)
        {
            Debug.Log("Correct answer selected!");
            AddScore(10);

            if (foodSpawner != null && currentPhrase.foodPrefab != null)
            {
                foodSpawner.SpawnFood(currentPhrase.foodPrefab);
            }

            if (currentClient != null)
            {
                ClientMover mover = currentClient.GetComponent<ClientMover>();
                if (mover != null)
                {
                    // FIX: NU mai dezactivăm interacțiunea
                    // mover.canInteract = false;
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
                    mover.ReturnToSpawn(true);
            }
        }

        HideDialogue();
    }

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

    public void OnFoodDeliveredWrong()
    {
        Debug.Log("Wrong food delivered!");

        if (currentClient != null)
        {
            ClientMover mover = currentClient.GetComponent<ClientMover>();
            if (mover != null)
            {
                mover.ReturnToSpawn(true);
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
            scoreText.text = "Points: " + score.ToString();
    }
}
