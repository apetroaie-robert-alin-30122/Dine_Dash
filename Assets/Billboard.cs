using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Transform player;

    void Start()
    {
        // Găsește playerul din scenă după nume
        player = GameObject.Find("Player").transform;
    }

    void Update()
    {
        if (player != null)
        {
            transform.LookAt(player);
            transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
        }
    }
}
