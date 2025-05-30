using UnityEngine;

public class ExclamationFloat : MonoBehaviour
{
    public float floatSpeed = 2f;
    public float floatHeight = 0.1f;
    public float scaleSpeed = 2f;
    public float scaleAmount = 0.05f;

    private Vector3 startPos;
    private Vector3 originalScale;

    void Start()
    {
        startPos = transform.localPosition;
        originalScale = transform.localScale;
    }

    void Update()
    {
        // Plutire verticală
        float newY = Mathf.Sin(Time.time * floatSpeed) * floatHeight;
        transform.localPosition = startPos + new Vector3(0, newY, 0);

        // Pulsează ușor
        float scale = 1 + Mathf.Sin(Time.time * scaleSpeed) * scaleAmount;
        transform.localScale = originalScale * scale;
    }
}
