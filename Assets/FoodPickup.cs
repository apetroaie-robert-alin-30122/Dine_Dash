using UnityEngine;

public class FoodPickup : MonoBehaviour
{
    private bool isHeld = false;
    private Transform player;
    private Transform holdPoint;
    private Rigidbody rb;

    void Start()
    {
        player = GameObject.FindWithTag("Player")?.transform;
        holdPoint = GameObject.Find("HoldPoint")?.transform;
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (isHeld && player != null && holdPoint != null)
        {
            transform.position = holdPoint.position;
            transform.rotation = Quaternion.LookRotation(player.forward);
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (!isHeld && other.CompareTag("Player") && Input.GetKeyDown(KeyCode.E))
        {
            Pickup();
        }
    }

    void Pickup()
{
    GameObject player = GameObject.FindGameObjectWithTag("Player");
    Transform holdPoint = player.transform.Find("HoldPoint");

    if (holdPoint != null)
    {
        transform.SetParent(holdPoint);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true; // Disable physics
            rb.detectCollisions = false;
        }

        isHeld = true;
    }
}
}
