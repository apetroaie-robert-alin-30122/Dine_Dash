using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientSpawner : MonoBehaviour
{
    [Header("Client Settings")]
    public GameObject[] clientPrefabs;

    [Header("Spawn Points")]
    public Transform entryPoint;
    public Transform[] seatPoints;

    private List<Transform> availableSeats = new List<Transform>();
    private List<GameObject> unusedPrefabs = new List<GameObject>();

    void Start()
    {
        availableSeats.AddRange(seatPoints);
        unusedPrefabs.AddRange(clientPrefabs);

        StartCoroutine(SpawnLoop());
    }

    IEnumerator SpawnLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(5f, 30f));

            if (availableSeats.Count == 0)
                continue;

            if (unusedPrefabs.Count == 0)
                unusedPrefabs.AddRange(clientPrefabs);

            SpawnClient();
        }
    }

    void SpawnClient()
    {
        // Choose random seat and remove from available
        int seatIndex = Random.Range(0, availableSeats.Count);
        Transform chosenSeat = availableSeats[seatIndex];
        availableSeats.RemoveAt(seatIndex);

        // Choose random unused prefab and remove from list
        int prefabIndex = Random.Range(0, unusedPrefabs.Count);
        GameObject selectedPrefab = unusedPrefabs[prefabIndex];
        unusedPrefabs.RemoveAt(prefabIndex);

        // Instantiate client at entry point
        GameObject newClient = Instantiate(selectedPrefab, entryPoint.position, Quaternion.identity);

        // Assign references in ClientMover
        ClientMover mover = newClient.GetComponent<ClientMover>();
        if (mover != null)
        {
            mover.SeatPoint = chosenSeat;
            mover.entryPoint = entryPoint;

            Transform exclamation = newClient.transform.Find("ExclamationMark");
            if (exclamation != null)
                mover.exclamationMark = exclamation.gameObject;

            Transform angry = newClient.transform.Find("AngryFace");
            if (angry != null)
                mover.angryFace = angry.gameObject;

            // Hook into client removal to free seat when it leaves
            StartCoroutine(TrackClientLifecycle(mover, chosenSeat));
        }
    }

    IEnumerator TrackClientLifecycle(ClientMover mover, Transform seat)
    {
        // Wait until client is destroyed
        while (mover != null)
            yield return null;

        // Add the seat back once the client is gone
        if (!availableSeats.Contains(seat))
            availableSeats.Add(seat);
    }
}
