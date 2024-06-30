using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public int currentLevel = 1; // Başlangıç seviyesi
    public float clicksayisi = 0; // Başlangıç tıklama sayısı
    public float[] levelThresholds; // Her seviye için tıklama eşikleri
    public Slider levelBar; // Level bar için UI Slider referansı
    public Text Level;
    public Text levelclickmiktarı;

    void Start()
    {
        // Level bar'ı başlat
        InitializeLevelBar();

        // Kayıtlı verileri yükle (Eğer daha önce kaydedilmişse)
        LoadPlayerProgress();
    }

    void Update()
    {
        UpdateLevel();
    }

    void InitializeLevelBar()
    {
        levelBar.minValue = 0;
        levelBar.maxValue = 1; // Slider değeri 0'dan 1'e kadar olacak
    }

    public void OnButtonClick()
    {
        // Tıklama sayısını artır
        clicksayisi++;

        // Level ilerlemesini kontrol et
        UpdateLevel();
    }

    void UpdateLevel()
    {
        // Mevcut seviyeye ait tıklama eşiğini al
        float currentThreshold = levelThresholds[currentLevel - 1];

        // Eğer mevcut tıklama sayısı, seviyeye ait tıklama eşiğini geçtiyse
        if (clicksayisi > currentThreshold)
        {
           
            clicksayisi = 1;
            // Yeni seviyeye geç
            currentLevel++;

            // Level bar'ı sıfırdan başlat
            levelBar.value = 0;

            // Yeni seviyeye geçişte yapılacak diğer işlemler
            // Örneğin, yeni seviyeye ait threshold'ları ayarlama vs.

        }

        // Level bar'ı güncelle
        UpdateLevelBar(clicksayisi);
        SavePlayerProgress();
    }

    void UpdateLevelBar(float clickCount)
    {
        // Mevcut seviyenin maksimum tıklama sayısını al
        float maxClickCountForLevel = levelThresholds[currentLevel - 1];

        // Level bar değerini tıklama sayısına göre ayarla
        levelBar.value = clickCount / maxClickCountForLevel;
        levelclickmiktarı.text = clickCount.ToString() + "/" + maxClickCountForLevel.ToString();
        Level.text = "LEVEL" + currentLevel.ToString();
    }

    void SavePlayerProgress()
    {
        // PlayerPrefs üzerine kayıt yap
        PlayerPrefs.SetInt("CurrentLevel", currentLevel);
        PlayerPrefs.SetFloat("ClickCount", clicksayisi);
        PlayerPrefs.Save();
    }

    void LoadPlayerProgress()
    {
        // PlayerPrefs'ten kaydedilmiş verileri yükle
        if (PlayerPrefs.HasKey("CurrentLevel"))
        {
            currentLevel = PlayerPrefs.GetInt("CurrentLevel");
            clicksayisi = PlayerPrefs.GetFloat("ClickCount");
        }
    }

}
