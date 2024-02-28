using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonAlivenessTracker : MonoBehaviour
{
    private List<Civilian> civilians = new List<Civilian>();
    float currentPercentage = 0f;

    private void Start()
    {
        AddAllCivilians();
    }

    public int GetAliveCount()
    {
        int aliveCount = 0;
        foreach (var civilian in civilians)
        {
            if (civilian.IsAlive)
            {
                aliveCount++;
            }
        }
        return aliveCount;
    }

    public float GetAlivePercentage()
    {
        if (civilians.Count == 0) return 0f;
        int aliveCount = GetAliveCount();
        return (float)aliveCount / civilians.Count * 100; // Convert to percentage
    }
    private void Update()
    {
        float alivePercentage = GetAlivePercentage();
        if (alivePercentage != currentPercentage)
        {
            currentPercentage = alivePercentage;
            SaturationController saturationController = SaturationController.instance;
            saturationController.AdjustSaturation(alivePercentage / 100);
            AudioVolumeController audioVolumeController = AudioVolumeController.instance;
            audioVolumeController.AdjustVolume(alivePercentage / 100);
        }
    }
    

    // This method will be called to add all civilians in the scene to the list
    public void AddAllCivilians()
    {
        // Find all GameObjects tagged as "Person"
        GameObject[] personObjects = GameObject.FindGameObjectsWithTag("Person");

        // Iterate through each GameObject
        foreach (var personObject in personObjects)
        {
            // Attempt to get the Civilian component
            Civilian civilian = personObject.GetComponent<Civilian>();

            // If the GameObject has a Civilian component and it's not already in the list, add it
            if (civilian != null && !civilians.Contains(civilian))
            {
                civilians.Add(civilian);
            }
        }
    }
}


