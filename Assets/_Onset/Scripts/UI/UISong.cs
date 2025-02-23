using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UISong : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI songTitle;

    [SerializeField]
    private Toggle toggle;
    public Toggle Toggle => toggle;

    public void UpdateSongTitle(string title)
    {
        songTitle.text = title;
    }
}
