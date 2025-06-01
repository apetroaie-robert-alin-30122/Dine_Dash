using UnityEngine;
using TMPro;

public class TooltipManager : MonoBehaviour
{
    public static TooltipManager Instance;

    public GameObject tooltipPanel;
    public TextMeshProUGUI tooltipText;

    void Awake()
    {
        Instance = this;
        HideTooltip();
    }

    public void ShowTooltip(string content)
    {
        tooltipPanel.SetActive(true);
        tooltipText.text = content;
    }

    public void HideTooltip()
    {
        tooltipPanel.SetActive(false);
    }
}
