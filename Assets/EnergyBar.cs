using UnityEngine;
using UnityEngine.UI;
using System;

public class EnergyBar : MonoBehaviour
{
    public RectTransform yellowBar; // Sarý barýn RectTransform'u
    public float fillDuration = 60.0f; // Dolum süresi (saniye cinsinden)
    public float maxBarWidth = 850f; // Sarý barýn yüzde 100 dolu olduðundaki geniþliði
    private float lastEnergyUpdateTime;

    private string startTimeKey = "EnergyStartTime";
    private string lastEnergyUpdateTimeKey = "LastEnergyUpdateTime";

    void Start()
    {
        if (PlayerPrefs.HasKey(startTimeKey))
        {
            // Enerjinin baþlangýç zamanýný yükle
            DateTime startTime = DateTime.Parse(PlayerPrefs.GetString(startTimeKey));
            float timeElapsed = (float)(DateTime.Now - startTime).TotalSeconds;

            // Enerjiyi zamana göre hesapla
            UpdateEnergyBar(timeElapsed);

            // Son enerji güncelleme zamanýný kaydet
            lastEnergyUpdateTime = timeElapsed % fillDuration;
        }
        else
        {
            // Baþlangýç zamanýný kaydet
            PlayerPrefs.SetString(startTimeKey, DateTime.Now.ToString());
            PlayerPrefs.Save();
            SetBarWidth(0f); // Sarý barý baþlangýçta boþ yap
        }
    }

    void Update()
    {
        if (lastEnergyUpdateTime < fillDuration)
        {
            // Geçen süreyi hesapla ve barý güncelle
            float timeElapsed = (float)(DateTime.Now - DateTime.Parse(PlayerPrefs.GetString(startTimeKey))).TotalSeconds;
            UpdateEnergyBar(timeElapsed);
        }
    }

    void UpdateEnergyBar(float timeElapsed)
    {
        // Enerji dolum süresine göre enerji yüzdesini hesapla
        float fillPercentage = Mathf.Clamp01(timeElapsed / fillDuration);
        SetBarWidth(fillPercentage * maxBarWidth);
    }

    void SetBarWidth(float width)
    {
        yellowBar.sizeDelta = new Vector2(width, yellowBar.sizeDelta.y);
    }

    private void OnDestroy()
    {
        // Son enerji güncelleme zamanýný kaydet
        PlayerPrefs.SetString(lastEnergyUpdateTimeKey, DateTime.Now.ToString());
        PlayerPrefs.Save();
    }
}
