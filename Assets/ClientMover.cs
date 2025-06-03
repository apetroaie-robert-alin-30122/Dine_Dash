using UnityEngine;
using System.Collections;

public class ClientMover : MonoBehaviour
{
    public Transform SeatPoint;
    public float speed = 2f;
    public float stopDistance = 0.1f;

    public GameObject exclamationMark;
    public GameObject angryFace;

    private Transform player;
    private bool isMoving = true;
    [HideInInspector] public bool canInteract = false;
    private DialogueManager dialogueManager;
	
	[HideInInspector]
    public int requiredFoodValue = -1;
    public bool hasReceivedFood = false;

    public Transform entryPoint;

    void Start()
    {
        if (exclamationMark != null)
            exclamationMark.SetActive(false);

        if (angryFace != null)
            angryFace.SetActive(false);

        player = GameObject.Find("Player")?.transform;
        dialogueManager = FindObjectOfType<DialogueManager>();

        if (entryPoint == null)
        {
            GameObject spawnerObj = GameObject.Find("Spawner");
            if (spawnerObj != null)
                entryPoint = spawnerObj.GetComponent<ClientSpawner>().entryPoint;
        }
    }

    void Update()
    {
        if (!isMoving && canInteract && Input.GetKeyDown(KeyCode.E))
{
    if (!dialogueManager.IsDialogueActive && player != null)
    {
        float dist = Vector3.Distance(player.position, transform.position);
        if (dist < 3f)
        {
            PlayerController controller = player.GetComponent<PlayerController>();
            if (hasReceivedFood)
                return;

            if (requiredFoodValue == -1)
            {
                // Initial dialogue
                dialogueManager.currentClient = gameObject;
                HideExclamation();
                dialogueManager.ShowRandomDialogue();
            }
            else if (controller != null && controller.carriedFoodValue != -1)
            {
                if (controller.carriedFoodValue == requiredFoodValue)
                {
                    // Correct food delivered
                    dialogueManager.AddScore(10);
                    hasReceivedFood = true;
                    controller.carriedFoodValue = -1;
                    Destroy(this.exclamationMark); // Optionally remove "!" when done
                }
                else
                {
                    ReturnToSpawn(true);
                    controller.carriedFoodValue = -1;
                    hasReceivedFood = true;
                }
            }
        }
    }
}

        if (!isMoving || SeatPoint == null)
            return;

        transform.position = Vector3.MoveTowards(transform.position, SeatPoint.position, speed * Time.deltaTime);

        if (player != null)
        {
            Vector3 lookPos = new Vector3(player.position.x, transform.position.y, player.position.z);
            transform.LookAt(lookPos);
        }

        if (Vector3.Distance(transform.position, SeatPoint.position) < stopDistance)
        {
            isMoving = false;
            canInteract = true;

            if (exclamationMark != null)
                exclamationMark.SetActive(true);
        }
    }

    public void HideExclamation()
    {
        if (exclamationMark != null)
            exclamationMark.SetActive(false);
    }

    public void ReturnToSpawn(bool showAngryFace)
    {
        if (showAngryFace && angryFace != null)
            angryFace.SetActive(true);

        StartCoroutine(MoveBackToEntry());
    }

    IEnumerator MoveBackToEntry()
    {
        if (entryPoint == null)
        {
            Debug.LogError("EntryPoint is null in ClientMover.");
            yield break;
        }

        Vector3 target = entryPoint.position;
        while (Vector3.Distance(transform.position, target) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
            yield return null;
        }

        if (angryFace != null)
            angryFace.SetActive(false);

        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        if (SeatPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(SeatPoint.position, stopDistance);
        }
    }
	
	public void LeaveAfterDelay(float delaySeconds = 5f)
{
    StartCoroutine(LeaveAfterDelayCoroutine(delaySeconds));
}

private IEnumerator LeaveAfterDelayCoroutine(float delay)
{
    yield return new WaitForSeconds(delay);  // waits in seconds
    ReturnToSpawn(false);
}

public void SetCanInteract(bool value)
{
    canInteract = value;
    if (!value)
    {
        HideExclamation();
    }
}
}
