using UnityEngine;

public class FoodItem : MonoBehaviour
{
    private Transform player;
    private bool isHeld = false;
    private Transform holdPoint;
	
	public FoodSpawner spawner;
    public Transform spawnPoint;
	
	public int foodValue = -1; // 0–5 depending on food type

    void Start()
    {
        player = GameObject.Find("Player")?.transform;
        holdPoint = GameObject.Find("FoodHoldPoint")?.transform;
    }

    void Update()
    {
        if (player != null)
        {
            Vector3 lookPos = new Vector3(player.position.x, transform.position.y, player.position.z);
            transform.LookAt(lookPos);
        }		
        if (isHeld && holdPoint != null)
        {
            transform.position = holdPoint.position;
            transform.LookAt(player); // Always face player
        }
    }
	
	void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                PlayerController player = other.GetComponent<PlayerController>();
                if (player != null && player.carriedFoodValue == -1)
                {
                    player.carriedFoodValue = foodValue;
                    Destroy(gameObject); // Remove food from scene
                }
            }
        }
    }
}
