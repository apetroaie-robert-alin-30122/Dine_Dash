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

    [HideInInspector] public int requiredFoodValue = -1;
    public bool hasReceivedFood = false;

    public Transform entryPoint;

    public Transform dishPoint;
    public GameObject currentDishPrefab;

    void Start()
    {
        if (exclamationMark != null)
            exclamationMark.SetActive(false);

        if (angryFace != null)
            angryFace.SetActive(false);

        player = GameObject.Find("Player")?.transform;
        dialogueManager = Object.FindAnyObjectByType<DialogueManager>();

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
            Debug.Log("Trying to interact with client");

            if (!dialogueManager.IsDialogueActive && player != null)
            {
                float dist = Vector3.Distance(player.position, transform.position);
                if (dist < 3f)
                {
                    PlayerController controller = player.GetComponent<PlayerController>();
                    if (hasReceivedFood)
                    {
                        Debug.Log("Client already served.");
                        return;
                    }

                    if (requiredFoodValue == -1)
                    {
                        Debug.Log("Opening dialogue...");
                        dialogueManager.currentClient = gameObject;
                        HideExclamation();
                        dialogueManager.ShowRandomDialogue();

                        if (dialogueManager.currentPhrase != null && dialogueManager.currentPhrase.foodPrefab != null)
                        {
                            FoodItem foodItem = dialogueManager.currentPhrase.foodPrefab.GetComponent<FoodItem>();
                            if (foodItem != null)
                            {
                                requiredFoodValue = foodItem.foodValue;
                                Debug.Log("Client wants food value: " + requiredFoodValue);
                            }
                        }
                    }
                    else if (controller != null && controller.carriedFoodValue != -1)
                    {
                        Debug.Log("Trying to deliver food value: " + controller.carriedFoodValue);

                        if (controller.carriedFoodValue == requiredFoodValue)
                        {
                            Debug.Log("Correct food delivered!");

                            dialogueManager.AddScore(10);
                            hasReceivedFood = true;

                            if (dishPoint != null && dialogueManager.currentPhrase != null)
                            {
                                currentDishPrefab = Instantiate(dialogueManager.currentPhrase.foodPrefab, dishPoint.position, dishPoint.rotation);

                                Rigidbody rb = currentDishPrefab.GetComponent<Rigidbody>();
                                if (rb != null)
                                {
                                    rb.isKinematic = true;
                                    rb.useGravity = false;
                                }
                            }

                            controller.ClearHeldFood();
                            Destroy(this.exclamationMark);
                            StartCoroutine(RemoveDishAfterDelay());
                            LeaveAfterDelay(5f);
                        }
                        else
                        {
                            Debug.Log("Wrong food delivered!");
                            ReturnToSpawn(true);
                            controller.ClearHeldFood();
                            hasReceivedFood = true;
                        }
                    }
                    else
                    {
                        Debug.Log("Player has no food to deliver.");
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

    IEnumerator RemoveDishAfterDelay()
    {
        yield return new WaitForSeconds(5f);
        if (currentDishPrefab != null)
        {
            Destroy(currentDishPrefab);
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
        yield return new WaitForSeconds(delay);
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
