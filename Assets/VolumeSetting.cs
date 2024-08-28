using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField] private AudioMixer myMixer;
    [SerializeField] private Slider masterSlider;

    private const string VolumeKey = "MasterVolume";

    void Start()
    {
        // Load the saved volume level
        if (PlayerPrefs.HasKey(VolumeKey))
        {
            float savedVolume = PlayerPrefs.GetFloat(VolumeKey);
            masterSlider.value = savedVolume;
            SetMasterVolume(); // Apply the saved volume level
        }
        else
        {
            // If no saved volume, set a default value (e.g., 0.75)
            masterSlider.value = 0.75f;
            SetMasterVolume();
        }

        // Add a listener to the slider to handle volume changes
        masterSlider.onValueChanged.AddListener(delegate { SetMasterVolume(); });
    }

    public void SetMasterVolume()
    {
        float volume = masterSlider.value;
        myMixer.SetFloat("master", Mathf.Log10(volume) * 20);

        // Save the volume level
        PlayerPrefs.SetFloat(VolumeKey, volume);
    }
}
