using UnityEngine;

public class TrafficManager : MonoBehaviour
{
    [Header("Congestion Settings")]
    public int maxCongestionLevel = 10;
    public int currentCongestionLevel = 10;

    [Header("Congestion Objects In Scene")]
    public GameObject[] congestionObjects;

    void Start()
    {
        UpdateCongestionVisuals();
    }

    public void ReduceCongestion(int amount)
    {
        currentCongestionLevel -= amount;
        currentCongestionLevel = Mathf.Clamp(currentCongestionLevel, 0, maxCongestionLevel);
        UpdateCongestionVisuals();
    }

    public void IncreaseCongestion(int amount)
    {
        currentCongestionLevel += amount;
        currentCongestionLevel = Mathf.Clamp(currentCongestionLevel, 0, maxCongestionLevel);
        UpdateCongestionVisuals();
    }

    void UpdateCongestionVisuals()
    {
        if (congestionObjects == null || congestionObjects.Length == 0)
            return;

        float normalized = (float)currentCongestionLevel / maxCongestionLevel;
        int objectsToShow = Mathf.CeilToInt(normalized * congestionObjects.Length);

        for (int i = 0; i < congestionObjects.Length; i++)
        {
            if (congestionObjects[i] != null)
            {
                congestionObjects[i].SetActive(i < objectsToShow);
            }
        }
    }
}

