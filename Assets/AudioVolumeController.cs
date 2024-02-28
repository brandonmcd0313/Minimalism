using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioVolumeController : MonoBehaviour
{
    private AudioSource audioSource; // Reference to the AudioSource component
    public static AudioVolumeController instance;
    bool isVolumeChanging = false;
    float currentVolume = 1f; // Default volume
    float targetVolume = 1f;
    bool isEnabled = true;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("AudioSource component missing from the object.");
        }
        else
        {
            // Set the initial volume
            audioSource.volume = currentVolume;
        }
    }

    public void AdjustVolume(float targetNormalizedVolume)
    {
        if (!isEnabled || audioSource == null)
        {
            return;
        }
        // Ensure the input is clamped between 0 and 1
        targetNormalizedVolume = Mathf.Clamp01(targetNormalizedVolume);
        targetVolume = targetNormalizedVolume;

        if (isVolumeChanging)
        {
            return;
        }

        StartCoroutine(AdjustVolumeOverTime(targetNormalizedVolume));
    }

    void Update()
    {
        if (!isEnabled || audioSource == null)
        {
            return;
        }

        if (isVolumeChanging)
        {
            return;
        }

        if (targetVolume != currentVolume)
        {
            AdjustVolume(targetVolume);
        }
    }

    private IEnumerator AdjustVolumeOverTime(float targetNormalizedVolume)
    {
        isVolumeChanging = true;
        float time = 0;
        float duration = 0.5f; // Duration in seconds over which the change will occur
        float startVolume = audioSource.volume;
        float endVolume = targetNormalizedVolume; // Direct mapping as volume is already in 0-1 range

        while (time < duration)
        {
            audioSource.volume = Mathf.Lerp(startVolume, endVolume, time / duration);
            time += Time.deltaTime;

            yield return null;
        }

        audioSource.volume = endVolume;
        currentVolume = targetNormalizedVolume;
        isVolumeChanging = false;
    }

    public void EnableVolumeControl(bool enable)
    {
        isEnabled = enable;
        if (!isEnabled && audioSource != null)
        {
            // Reset volume to full when disabled
            audioSource.volume = 1f;
        }
    }

    public void DisableMusic()
    {
        isEnabled = false;
        audioSource.volume = 0;
    }
}
