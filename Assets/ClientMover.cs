using UnityEngine;

public class ClientMover : MonoBehaviour
{
    public Transform SeatPoint;              // Setat automat din ClientSpawner
    public float speed = 2f;
    public float stopDistance = 0.1f;
    public GameObject exclamationMark;       // PNG-ul "!" deasupra capului

    private Transform player;
    private bool isMoving = true;
	private bool canInteract = false;
    private DialogueManager dialogueManager;


    void Start()
    {
        // Ascunde "!" la început
        if (exclamationMark != null)
            exclamationMark.SetActive(false);

        // Găsește jucătorul
        GameObject playerObj = GameObject.Find("Player");
        if (playerObj != null)
            player = playerObj.transform;
		
		dialogueManager = FindObjectOfType<DialogueManager>();
    }

    void Update()
    {
		 if (!isMoving && canInteract && Input.GetKeyDown(KeyCode.E))
        {
            if (!dialogueManager.IsDialogueActive)
            {
                dialogueManager.ShowRandomDialogue();
            }
        }
		
        if (!isMoving || SeatPoint == null)
            return;

        // Deplasare spre scaun
        transform.position = Vector3.MoveTowards(transform.position, SeatPoint.position, speed * Time.deltaTime);

        // Se uită spre jucător doar pe Y
        if (player != null)
        {
            Vector3 lookPos = new Vector3(player.position.x, transform.position.y, player.position.z);
            transform.LookAt(lookPos);
        }

        // Când ajunge la scaun
        if (Vector3.Distance(transform.position, SeatPoint.position) < stopDistance)
        {
            isMoving = false;
			canInteract = true;

            // Afișează semnul "!"
            if (exclamationMark != null)
                exclamationMark.SetActive(true);
        }
		
    }
	void OnDrawGizmosSelected()
    {
        if (SeatPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(SeatPoint.position, stopDistance);
        }
    }
}
