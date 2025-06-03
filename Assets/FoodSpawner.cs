using System.Collections.Generic;
using UnityEngine;

public class FoodSpawner : MonoBehaviour
{
    public Transform[] spawnPoints;
    public GameObject[] foodPrefabs;  // Assign all your food prefabs here in Inspector

    // Track occupied spawn points
    private List<Transform> occupiedSpawnPoints = new List<Transform>();

    // Existing method stays as is
    public void SpawnFood(GameObject foodPrefab)
    {
        if (spawnPoints.Length == 0 || foodPrefab == null)
        {
            Debug.LogWarning("No spawn points or prefab provided.");
            return;
        }

        // Get free spawn points by filtering out occupied ones
        List<Transform> freeSpawnPoints = new List<Transform>();

        foreach (var point in spawnPoints)
        {
            if (!occupiedSpawnPoints.Contains(point))
                freeSpawnPoints.Add(point);
        }

        if (freeSpawnPoints.Count == 0)
        {
            Debug.LogWarning("No free spawn points available.");
            return;
        }

        // Pick a random free spawn point
        Transform spawnPoint = freeSpawnPoints[Random.Range(0, freeSpawnPoints.Count)];

        // Instantiate food
        GameObject food = Instantiate(foodPrefab, spawnPoint.position, Quaternion.identity);

        // Mark this spawn point as occupied
        occupiedSpawnPoints.Add(spawnPoint);

        // Make food face the player (optional)
        food.transform.LookAt(Camera.main.transform.position);

        // Add FoodItem component for management
        FoodItem foodItem = food.AddComponent<FoodItem>();
        foodItem.spawner = this;
        foodItem.spawnPoint = spawnPoint;
    }

    // New overload: spawn by index
    public void SpawnFood(int foodIndex)
    {
        if (foodPrefabs == null || foodPrefabs.Length == 0)
        {
            Debug.LogWarning("Food prefabs array is empty.");
            return;
        }

        if (foodIndex < 0 || foodIndex >= foodPrefabs.Length)
        {
            Debug.LogWarning("Invalid food index.");
            return;
        }

        SpawnFood(foodPrefabs[foodIndex]);
    }

    public void FreeSpawnPoint(Transform spawnPoint)
    {
        if (occupiedSpawnPoints.Contains(spawnPoint))
        {
            occupiedSpawnPoints.Remove(spawnPoint);
        }
    }
}
