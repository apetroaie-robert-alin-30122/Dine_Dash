using System.Collections.Generic;
using UnityEngine;

public class FoodSpawner : MonoBehaviour
{
    public List<Transform> spawnPoints;
    public List<Transform> freeSpawnPoints;
    public List<GameObject> foodPrefabs;

    void Start()
    {
        freeSpawnPoints = new List<Transform>(spawnPoints);
    }

    public void SpawnFood(GameObject prefab)
    {
        if (freeSpawnPoints.Count == 0)
        {
            Debug.LogWarning("No free spawn points available.");
            return;
        }

        Transform point = freeSpawnPoints[0];
        freeSpawnPoints.RemoveAt(0);

        GameObject obj = Instantiate(prefab, point.position, point.rotation);
        FoodItem foodItem = obj.GetComponent<FoodItem>();
        if (foodItem != null)
        {
            foodItem.spawner = this;
            foodItem.spawnPoint = point;
        }
    }

    // ✅ Metodă nouă pentru a elibera spawn point-ul
    public void ReleaseSpawnPoint(Transform point)
    {
        if (!spawnPoints.Contains(point))
            return;

        if (!freeSpawnPoints.Contains(point))
        {
            freeSpawnPoints.Add(point);
        }
    }
}
