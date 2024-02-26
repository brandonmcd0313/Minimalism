using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class SaturationController : MonoBehaviour
{
    public PostProcessVolume volume; // Assign your Post Processing Volume in the inspector
    private ColorGrading colorGrading;
    public static SaturationController instance;
    bool isSaturationChanging = false;
    float currentSaturation = 0;
    float targetSaturation = 0;
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
        // Check if Color Grading is enabled in the Post Processing Volume and get it
        if (volume.profile.TryGetSettings(out colorGrading))
        {
            // Set the saturation to 0 by default
            colorGrading.saturation.value = 0;
        }
        else
        {
            Debug.LogError("Color Grading is not enabled in the Post Processing Volume");
        }

    }

    public void AdjustSaturation(float targetNormalizedSaturation)
    {
        // Ensure the input is clamped between 0 and 1
        targetNormalizedSaturation = Mathf.Clamp01(targetNormalizedSaturation);
        targetSaturation = targetNormalizedSaturation;

        // If there's already a coroutine running, wait.
        if (isSaturationChanging)
        {
            return;
        }

        // Start a new coroutine to adjust the saturation smoothly
        StartCoroutine(AdjustSaturationOverTime(targetNormalizedSaturation));
    }

    void Update()
    {
        if (isSaturationChanging)
        {
            return;
        }

        if (targetSaturation != currentSaturation)
        {
            AdjustSaturation(targetSaturation);
        }
    }

    private IEnumerator AdjustSaturationOverTime(float targetNormalizedSaturation)
    {
        isSaturationChanging = true;
        float time = 0;
        float duration = 0.5f; // Duration in seconds over which the change will occur
        float startSaturation = colorGrading.saturation.value;
        float endSaturation = Mathf.Lerp(-100, 0, targetNormalizedSaturation); // Map 0-1 range to -100 to 0

        while (time < duration)
        {
            // Interpolate the current saturation value over time
            colorGrading.saturation.value = Mathf.Lerp(startSaturation, endSaturation, time / duration);
            time += Time.deltaTime;

            yield return null; // Wait until the next frame
        }

        // Ensure the final value is set
        colorGrading.saturation.value = endSaturation;
        currentSaturation = targetNormalizedSaturation;
        isSaturationChanging = false;
    }
}
