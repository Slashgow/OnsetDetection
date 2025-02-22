using UnityEngine;
using UnityEngine.UI;
using UnityTimer;

public class UISongProgression : MonoBehaviour
{
    [SerializeField]
    private Slider slider;

    [SerializeField, Range(0f,5f)]
    private float refreshRate = 2f;

    [SerializeField]
    private AudioSource audioSource;

    private void Start()
    {
        UpdateSliderValue(0f);
        this.AttachTimer(refreshRate, () => {
            if (audioSource.isPlaying)
            {
                UpdateSliderValue(audioSource.time / audioSource.clip.length);
            }
        }, isLooped: true);
    }
    private void UpdateSliderValue(float value)
    {
        slider.value = value;   
    }
}
