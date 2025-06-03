using UnityEngine;

public class FoodPickup : MonoBehaviour
{
    public float pickupRange = 3f;
    public Transform holdPosition; // Assign in inspector or set to player's hold point
    private Transform player;
    private bool isCarried = false;

    void Start()
    {
		player = GameObject.FindWithTag("Player")?.transform;

    if (player != null)
    {
        // Look for child object named "HoldPoint"
        Transform hold = player.Find("HoldPoint");
        if (hold != null)
        {
            holdPosition = hold;
        }
        else
        {
            Debug.LogWarning("HoldPoint not found as child of Player.");
        }
    }
    }

    void Update()
    {
        if (isCarried)
        {
            // Keep the food in front of player, facing player
            transform.position = holdPosition.position;
            transform.rotation = Quaternion.LookRotation(player.position - transform.position);
        }
        else
        {
            if (player == null) return;

            float distance = Vector3.Distance(transform.position, player.position);
            if (distance <= pickupRange && Input.GetKeyDown(KeyCode.E))
            {
                PickUp();
            }
        }
		
    }

    void PickUp()
    {
        isCarried = true;

        // Optional: Disable physics while carrying
        if (TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            rb.isKinematic = true;
        }

        // Optional: Disable collider to avoid interference while carrying
        if (TryGetComponent<Collider>(out Collider col))
        {
            col.enabled = false;
        }
    }
}
