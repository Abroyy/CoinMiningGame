using UnityEngine;
using UnityEngine.UI;
using System;

public class EnergyBar : MonoBehaviour
{
    public RectTransform yellowBar; // Sar� bar�n RectTransform'u
    public float fillDuration = 60.0f; // Dolum s�resi (saniye cinsinden)
    public float maxBarWidth = 850f; // Sar� bar�n y�zde 100 dolu oldu�undaki geni�li�i
    private float lastEnergyUpdateTime;

    private string startTimeKey = "EnergyStartTime";
    private string lastEnergyUpdateTimeKey = "LastEnergyUpdateTime";

    void Start()
    {
        if (PlayerPrefs.HasKey(startTimeKey))
        {
            // Enerjinin ba�lang�� zaman�n� y�kle
            DateTime startTime = DateTime.Parse(PlayerPrefs.GetString(startTimeKey));
            float timeElapsed = (float)(DateTime.Now - startTime).TotalSeconds;

            // Enerjiyi zamana g�re hesapla
            UpdateEnergyBar(timeElapsed);

            // Son enerji g�ncelleme zaman�n� kaydet
            lastEnergyUpdateTime = timeElapsed % fillDuration;
        }
        else
        {
            // Ba�lang�� zaman�n� kaydet
            PlayerPrefs.SetString(startTimeKey, DateTime.Now.ToString());
            PlayerPrefs.Save();
            SetBarWidth(0f); // Sar� bar� ba�lang��ta bo� yap
        }
    }

    void Update()
    {
        if (lastEnergyUpdateTime < fillDuration)
        {
            // Ge�en s�reyi hesapla ve bar� g�ncelle
            float timeElapsed = (float)(DateTime.Now - DateTime.Parse(PlayerPrefs.GetString(startTimeKey))).TotalSeconds;
            UpdateEnergyBar(timeElapsed);
        }
    }

    void UpdateEnergyBar(float timeElapsed)
    {
        // Enerji dolum s�resine g�re enerji y�zdesini hesapla
        float fillPercentage = Mathf.Clamp01(timeElapsed / fillDuration);
        SetBarWidth(fillPercentage * maxBarWidth);
    }

    void SetBarWidth(float width)
    {
        yellowBar.sizeDelta = new Vector2(width, yellowBar.sizeDelta.y);
    }

    private void OnDestroy()
    {
        // Son enerji g�ncelleme zaman�n� kaydet
        PlayerPrefs.SetString(lastEnergyUpdateTimeKey, DateTime.Now.ToString());
        PlayerPrefs.Save();
    }
}
