using UnityEngine;

public class DishSpawner : MonoBehaviour
{
    public GameObject[] dishPrefabs; // Assign în Inspector
    public Transform spawnPoint;

    public void SpawnDishByName(string dishName)
    {
        foreach (GameObject prefab in dishPrefabs)
        {
            if (prefab.name == dishName)
            {
                Instantiate(prefab, spawnPoint.position, Quaternion.identity);
                return;
            }
        }

        Debug.LogWarning("No dish prefab found with name: " + dishName);
    }
}
