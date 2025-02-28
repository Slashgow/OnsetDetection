using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UIButtonsSFX : MonoBehaviour
{
    [SerializeField]
    private AudioSource audioSource;

    private List<Button> buttons = new List<Button>();

    private void Awake()
    {
        buttons = GetComponentsInChildren<Button>(true).ToList();
        audioSource.ignoreListenerPause = true;
    }

    private void Start() => buttons.ForEach(button => button.onClick.AddListener(PlaySound));

    private void OnDestroy() => buttons.ForEach(button => button.onClick.RemoveListener(PlaySound));

    private void PlaySound()
    {
       audioSource.Play();
    }
}
