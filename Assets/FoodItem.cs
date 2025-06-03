using UnityEngine;

public class FoodItem : MonoBehaviour
{
    private Transform player;
    private bool isHeld = false;
    private Transform holdPoint;
	
	public FoodSpawner spawner;
    public Transform spawnPoint;

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

    public void PickUp()
    {
        isHeld = true;
        GetComponent<Collider>().enabled = false;
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb) rb.isKinematic = true;
    }
	
	private void OnDestroy()
    {
        if (spawner != null && spawnPoint != null)
        {
            spawner.FreeSpawnPoint(spawnPoint);
        }
    }
}
