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

    void Start()
    {
        availableSeats.AddRange(seatPoints);

        foreach (GameObject clientPrefab in clientPrefabs)
        {
            float delay = Random.Range(5f, 60f);
            StartCoroutine(SpawnClientWithDelay(clientPrefab, delay));
        }
    }

    IEnumerator SpawnClientWithDelay(GameObject clientPrefab, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (availableSeats.Count == 0)
            yield break;

        int index = Random.Range(0, availableSeats.Count);
        Transform chosenSeat = availableSeats[index];
        availableSeats.RemoveAt(index);

        GameObject newClient = Instantiate(clientPrefab, entryPoint.position, Quaternion.identity);

        ClientMover mover = newClient.GetComponent<ClientMover>();
        if (mover != null)
        {
            mover.SeatPoint = chosenSeat;
            mover.entryPoint = entryPoint;

            Transform angry = newClient.transform.Find("AngryFace");
            if (angry != null)
                mover.angryFace = angry.gameObject;

            Transform exclamation = newClient.transform.Find("ExclamationMark");
            if (exclamation != null)
                mover.exclamationMark = exclamation.gameObject;
        }
    }
}
