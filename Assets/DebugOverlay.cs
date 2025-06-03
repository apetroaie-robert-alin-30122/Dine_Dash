using UnityEngine;
using UnityEngine.UI;

public class DebugOverlay : MonoBehaviour
{
    public PlayerController player;
    public ClientMover nearestClient;

    void OnGUI()
    {
        if (player == null)
            player = Object.FindAnyObjectByType<PlayerController>();

        if (nearestClient == null)
            nearestClient = Object.FindAnyObjectByType<ClientMover>();

        if (player == null || nearestClient == null) return;

        float dist = Vector3.Distance(player.transform.position, nearestClient.transform.position);

        GUI.Label(new Rect(10, 10, 400, 20), $"[Player] carriedFoodValue = {player.carriedFoodValue}");
        GUI.Label(new Rect(10, 30, 400, 20), $"[Client] requiredFoodValue = {nearestClient.requiredFoodValue}");
        GUI.Label(new Rect(10, 50, 400, 20), $"[Client] canInteract = {nearestClient.canInteract}");
        GUI.Label(new Rect(10, 70, 400, 20), $"[Client] hasReceivedFood = {nearestClient.hasReceivedFood}");
        GUI.Label(new Rect(10, 90, 400, 20), $"[Distance to Client] = {dist:0.00}");
    }
}
