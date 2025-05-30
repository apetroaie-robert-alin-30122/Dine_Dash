using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientSpawner : MonoBehaviour
{
    [Header("Client Settings")]
    public GameObject[] clientPrefabs; // Client1, Client2, Client3 (prefabs)

    [Header("Spawn Points")]
    public Transform entryPoint;       // Entry point (unde apar toți)
    public Transform[] seatPoints;     // Cele 5 SeatPoints

    private List<Transform> availableSeats = new List<Transform>();

    void Start()
    {
        // Adaugă toate locurile disponibile într-o listă
        availableSeats.AddRange(seatPoints);

        // Spawnează fiecare client la un timp random între 5 și 60 sec
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
            yield break; // Nu mai sunt locuri

        // Alege un loc aleatoriu din cele libere
        int index = Random.Range(0, availableSeats.Count);
        Transform chosenSeat = availableSeats[index];
        availableSeats.RemoveAt(index); // elimină-l din listă

        // Creează clientul la EntryPoint
        GameObject newClient = Instantiate(clientPrefab, entryPoint.position, Quaternion.identity);

        // Trimite SeatPoint către ClientMover
        ClientMover mover = newClient.GetComponent<ClientMover>();
        if (mover != null)
        {
            mover.SeatPoint = chosenSeat;
        }
    }
}
