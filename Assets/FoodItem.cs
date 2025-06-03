using UnityEngine;

public class FoodItem : MonoBehaviour
{
    private Transform player;
    private Transform holdPoint;
    private bool isHeld = false;

    public FoodSpawner spawner;
    public Transform spawnPoint;
    public int foodValue = -1;

    void Start()
    {
        player = GameObject.FindWithTag("Player")?.transform;
        holdPoint = GameObject.Find("FoodHoldPoint")?.transform;
    }

    void Update()
    {
        if (isHeld && holdPoint != null)
        {
            transform.position = holdPoint.position;
            transform.rotation = holdPoint.rotation;
        }
        else if (player != null)
        {
            Vector3 lookPos = new Vector3(player.position.x, transform.position.y, player.position.z);
            transform.LookAt(lookPos);
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.E))
        {
            PlayerController playerController = other.GetComponent<PlayerController>();
            if (playerController != null && playerController.carriedFoodValue == -1)
            {
                playerController.carriedFoodValue = foodValue;
                playerController.heldDishObject = gameObject;

                Debug.Log("Picked up food with value: " + foodValue);

                // 🔄 ELIBEREAZĂ spawn point-ul
                if (spawner != null && spawnPoint != null)
                {
                    spawner.ReleaseSpawnPoint(spawnPoint);
                }

                if (holdPoint != null)
                {
                    transform.SetParent(holdPoint);
                    transform.localPosition = Vector3.zero;
                    transform.localRotation = Quaternion.identity;
                }

                Rigidbody rb = GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.isKinematic = true;
                    rb.detectCollisions = false;
                }

                isHeld = true;
            }
            else
            {
                Debug.LogWarning("Player already holds food: " + playerController?.carriedFoodValue);
            }
        }
    }
}
