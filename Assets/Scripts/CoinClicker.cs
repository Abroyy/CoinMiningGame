using UnityEngine;
using UnityEngine.UI;
using System;

public class CoinClicker : MonoBehaviour
{
    public Text scoreText; // Skoru gösteren Text bileþeni
    public Text energyText; // Enerji seviyesini gösteren Text bileþeni
    public Text levelText;
    public Slider levelBar; // Levelbar için UI Slider referansý// Oyuncunun seviyesini gösteren Text bileþeni
    public Text itemCostText; // Enerji kapasitesini artýran ürünün maliyetini gösteren Text bileþeni
    public Text touchItemCostText; // "Touch" ürünü maliyetini gösteren Text bileþeni
    public Text levelclickText;

    private float clicksayisi;
    private float score; // Skoru tutan deðiþken
    public Animator coinAnimator; // Coin animatörü
    public string animationTriggerName = "Clicked"; // Animasyon tetikleyicisi
    private string scorePlayerPrefsKey = "PlayerScore"; // Skor için PlayerPrefs anahtarý
    private string clicksayisiPlayerPrefsKey = "Playerclicksayisi";

    public RectTransform yellowBar; // Sarý barýn RectTransform'u
    public float maxBarWidth = 850f; // Sarý barýn yüzde 100 dolu olduðundaki geniþliði
    public float energyDrainPerClick = 1f; // Her týklamada harcanacak enerji oraný
    private float currentEnergy; // Mevcut enerji miktarý
    private float fillDuration = 3600.0f; // Dolum süresi (saniye cinsinden)
    private string energyStartTimeKey = "EnergyStartTime"; // Enerji baþlangýç zamaný için PlayerPrefs anahtarý
    private string currentEnergyKey = "CurrentEnergy"; // Mevcut enerji için PlayerPrefs anahtarý
    private DateTime lastUpdateTime; // Son güncelleme zamaný

    private string currentLevelKey = "CurrentLevel"; // Oyuncunun seviyesi için PlayerPrefs anahtarý
    private int currentLevel = 1; // Oyuncunun baþlangýç seviyesi
    public float[] levelThresholds; // Level sýnýrlarý

    private string currentMaxEnergyKey = "CurrentMaxEnergy"; // Maksimum enerji kapasitesi için PlayerPrefs anahtarý
    private float currentMaxEnergy = 1000f; // Baþlangýçta enerji kapasitesi 1000
    private float energyIncreaseStep = 500f; // Enerji artýþ adýmý

    private string itemCostKey = "ItemCost"; // Enerji kapasitesi artýran ürünün maliyeti için PlayerPrefs anahtarý
    private float initialItemCost = 0.5f; // Enerji kapasitesi artýran ürünün baþlangýç maliyeti
    private float currentItemCost; // Mevcut ürün maliyeti

    private float touchClickValue = 1f;
    private float touchCoinValue = 0.01f; // Týklama baþýna baþlangýç coini
    private string touchCoinValueKey = "TouchCoinValue"; // Týklama baþýna coin için PlayerPrefs anahtarý
    private string touchClickValueKey = "TouchClickValue";

    private string touchItemCostKey = "TouchItemCost"; // "Touch" ürünü maliyeti için PlayerPrefs anahtarý
    private float initialTouchItemCost = 0.5f; // "Touch" ürünü baþlangýç maliyeti
    private float currentTouchItemCost; // Mevcut "Touch" ürünü maliyeti

    public Text energyFillSpeedupCostText; // Enerji doldurma süresini hýzlandýrma maliyetini gösteren Text bileþeni
    private string energyFillSpeedupCostKey = "EnergyFillSpeedupCost"; // Enerji doldurma süresini hýzlandýrma maliyeti için PlayerPrefs anahtarý
    private float energyFillSpeedupCost = 0.5f;

    public Text unlimitedEnergyCostText; // Sýnýrsýz enerji ürününün maliyetini gösteren Text bileþeni
    private float unlimitedEnergyCost = 1f; // Sýnýrsýz enerji ürününün maliyeti
    private TimeSpan unlimitedEnergyDuration = TimeSpan.FromMinutes(10); // Sýnýrsýz enerji süresi (10 dakika)
    private DateTime unlimitedEnergyEndTime; // Sýnýrsýz enerjinin bitiþ zamaný
    private bool isUnlimitedEnergyActive = false; // Sýnýrsýz enerjinin aktif olup olmadýðýný belirten bayrak
    private string unlimitedEnergyEndTimeKey = "UnlimitedEnergyEndTime"; // Sýnýrsýz enerji bitiþ zamaný için PlayerPrefs anahtarý

    public GameObject yetersizcoinpaneli;
    public Animator yetersizcoinanim;


    void Start()
    {
        if (PlayerPrefs.HasKey(clicksayisiPlayerPrefsKey))
        {
            clicksayisi = PlayerPrefs.GetFloat(clicksayisiPlayerPrefsKey);
        }
        else
        {
            clicksayisi = 0f; // Eðer kaydedilmiþ skor yoksa varsayýlan olarak 0 kabul edilir
        }
        // Oyun baþladýðýnda kaydedilmiþ skoru yükle
        if (PlayerPrefs.HasKey(scorePlayerPrefsKey))
        {
            score = PlayerPrefs.GetFloat(scorePlayerPrefsKey);
        }
        else
        {
            score = 0f; // Eðer kaydedilmiþ skor yoksa varsayýlan olarak 0 kabul edilir
        }

        // Enerjinin baþlangýç zamanýný kaydet veya yükle
        if (PlayerPrefs.HasKey(energyStartTimeKey))
        {
            lastUpdateTime = DateTime.Parse(PlayerPrefs.GetString(energyStartTimeKey));
        }
        else
        {
            lastUpdateTime = DateTime.Now;
            PlayerPrefs.SetString(energyStartTimeKey, lastUpdateTime.ToString());
            PlayerPrefs.Save();
        }

        // Kaydedilmiþ mevcut enerji seviyesini yükle
        if (PlayerPrefs.HasKey(currentEnergyKey))
        {
            currentEnergy = PlayerPrefs.GetFloat(currentEnergyKey);
        }
        else
        {
            currentEnergy = 1000f; // Baþlangýçta enerji dolu
        }

        // Kaydedilmiþ mevcut seviyeyi yükle
        if (PlayerPrefs.HasKey(currentLevelKey))
        {
            currentLevel = PlayerPrefs.GetInt(currentLevelKey);
        }
        else
        {
            currentLevel = 1; // Baþlangýçta seviye 1
        }

        if (PlayerPrefs.HasKey(currentMaxEnergyKey))
        {
            currentMaxEnergy = PlayerPrefs.GetFloat(currentMaxEnergyKey);
        }
        else
        {
            currentMaxEnergy = 1000f; // Baþlangýçta enerji kapasitesi yüzde 100
        }

        // Mevcut ürün maliyetini yükle
        if (PlayerPrefs.HasKey(itemCostKey))
        {
            currentItemCost = PlayerPrefs.GetFloat(itemCostKey);
        }
        else
        {
            currentItemCost = initialItemCost; // Baþlangýç maliyeti
        }

        if (PlayerPrefs.HasKey(touchCoinValueKey))
        {
            touchCoinValue = PlayerPrefs.GetFloat(touchCoinValueKey);
        }
        else
        {
            touchCoinValue = 0.01f; // Baþlangýç deðeri
        }

        if (PlayerPrefs.HasKey(touchClickValueKey))
        {
            touchClickValue = PlayerPrefs.GetFloat(touchClickValueKey);
        }
        else
        {
            touchClickValue = 1f; // Baþlangýç deðeri
        }

        // Mevcut "Touch" ürünü maliyetini yükle
        if (PlayerPrefs.HasKey(touchItemCostKey))
        {
            currentTouchItemCost = PlayerPrefs.GetFloat(touchItemCostKey);
        }
        else
        {
            currentTouchItemCost = initialTouchItemCost; // Baþlangýç maliyeti
        }

        if (PlayerPrefs.HasKey(energyFillSpeedupCostKey))
        {
            energyFillSpeedupCost = PlayerPrefs.GetFloat(energyFillSpeedupCostKey);
        }
        else
        {
            energyFillSpeedupCost = 0.5f; // Baþlangýç maliyeti
        }

        if (PlayerPrefs.HasKey(unlimitedEnergyEndTimeKey))
        {
            unlimitedEnergyEndTime = DateTime.Parse(PlayerPrefs.GetString(unlimitedEnergyEndTimeKey));
            isUnlimitedEnergyActive = unlimitedEnergyEndTime > DateTime.Now;
        }
        else
        {
            unlimitedEnergyEndTime = DateTime.MinValue;
            isUnlimitedEnergyActive = false;
        }

        UpdateUnlimitedEnergyCostText(); // Baþlangýçta sýnýrsýz enerji ürün maliyetini güncelle

        UpdateTouchItemCostText();

        UpdateItemCostText();
        UpdateEnergyFillSpeedupCostText();// Baþlangýçta ürün maliyetini güncelle

        // Geçen süreyi hesapla ve enerji seviyesini güncelle
        UpdateEnergyOnStart();

        UpdateScoreText();
        UpdateEnergyBar();
        UpdateLevelText(); // Baþlangýçta seviyeyi güncelle
        InitializeLevelBar();

        yetersizcoinpaneli.SetActive(false);
    }

    void Update()
    {
        // Geçen süreyi hesapla
        float timeElapsed = (float)(DateTime.Now - lastUpdateTime).TotalSeconds;

        // Enerji miktarýný zamana göre yeni kapasiteye göre güncelle
        float energyIncrease = (timeElapsed / fillDuration) * currentMaxEnergy;
        currentEnergy = Mathf.Clamp(currentEnergy + energyIncrease, 0, currentMaxEnergy);

        // Güncellenen enerji miktarýný bar üzerinde göster
        UpdateEnergyBar();

        // Güncellenmiþ son zaman kaydýný yap
        lastUpdateTime = DateTime.Now;
        PlayerPrefs.SetString(energyStartTimeKey, lastUpdateTime.ToString());
        PlayerPrefs.Save();

        if (isUnlimitedEnergyActive && DateTime.Now >= unlimitedEnergyEndTime)
        {
            isUnlimitedEnergyActive = false;
            PlayerPrefs.DeleteKey(unlimitedEnergyEndTimeKey); // Süre bittiðinde keyi sil
        }
        CheckAndUpdateLevel();
    }


    public void OnCoinClick()
    {
        if (isUnlimitedEnergyActive)
        {
            clicksayisi += touchClickValue;
            score += touchCoinValue;
            if (coinAnimator != null)
            {
                coinAnimator.SetTrigger(animationTriggerName);
            }
            else
            {
                Debug.LogError("Animator bileþeni atanmadý!");
            }
            UpdateScoreText();
            CheckAndUpdateLevel();
        }
        else if (currentEnergy >= energyDrainPerClick)
        {
            clicksayisi += touchClickValue;
            score += touchCoinValue;
            if (coinAnimator != null)
            {
                coinAnimator.SetTrigger(animationTriggerName);
            }
            else
            {
                Debug.LogError("Animator bileþeni atanmadý!");
            }
            UpdateScoreText();
            currentEnergy -= energyDrainPerClick;
            currentEnergy = Mathf.Clamp(currentEnergy, 0, currentMaxEnergy);
            UpdateEnergyBar();
            SaveEnergyState();
            CheckAndUpdateLevel();
        }
        else
        {
            Debug.Log("Yeterli enerji yok! Mevcut enerji: " + currentEnergy);
        }
    }

    public void BuyEnergyIncreaseItem()
    {
        // Ürünün maliyetini yükle veya varsayýlan maliyeti ata
        if (PlayerPrefs.HasKey(itemCostKey))
        {
            currentItemCost = PlayerPrefs.GetFloat(itemCostKey);
        }
        else
        {
            currentItemCost = initialItemCost;
        }

        // Yeterli coini olup olmadýðýný kontrol et
        if (score >= currentItemCost)
        {
            score -= currentItemCost; // Coin'lerden maliyeti düþ
            UpdateScoreText(); // Skoru güncelle

            float previousMaxEnergy = currentMaxEnergy;
            currentMaxEnergy += energyIncreaseStep; // Maksimum enerji kapasitesini artýr

            // Enerji miktarýný yeni kapasiteye göre güncelle
            currentEnergy = (currentEnergy / previousMaxEnergy) * currentMaxEnergy;

            SaveMaxEnergyState(); // Enerji kapasitesi durumunu kaydet

            currentItemCost *= 2; // Ürünün maliyetini iki katýna çýkar
            SaveItemCost(); // Ürün maliyetini kaydet

            UpdateItemCostText(); // Ürün maliyetini güncelle
            UpdateEnergyBar(); // Enerji barýný güncelle

            Debug.Log("Enerji kapasitesi arttýrýldý! Yeni maksimum kapasite: " + currentMaxEnergy);
        }
        else
        {
            yetersizcoinpaneli.SetActive(true);
            yetersizcoinanim.SetTrigger("Not");
            Debug.Log("Yeterli coininiz yok!");
        }

        CheckAndUpdateLevel();

        // Level text ve slider güncelleme
        UpdateLevelText();
        UpdateLevelBar(score);
    }


    public void BuyTouchIncreaseItem()
    {
        // Ürünün maliyetini yükle veya varsayýlan maliyeti ata
        if (PlayerPrefs.HasKey(touchItemCostKey))
        {
            currentTouchItemCost = PlayerPrefs.GetFloat(touchItemCostKey);
        }
        else
        {
            currentTouchItemCost = initialTouchItemCost;
        }

        // Yeterli coini olup olmadýðýný kontrol et
        if (score >= currentTouchItemCost)
        {
            score -= currentTouchItemCost; // Coin'lerden maliyeti düþ
            UpdateScoreText(); // Skoru güncelle

            touchCoinValue += 0.01f; // Týklama baþýna coin miktarýný iki katýna çýkar
            SaveTouchCoinValue(); // Týklama baþýna coin miktarýný kaydet

            currentTouchItemCost *= 2; // Ürünün maliyetini iki katýna çýkar
            SaveTouchItemCost(); // Ürün maliyetini kaydet

            UpdateTouchItemCostText(); // Ürün maliyetini güncelle

            Debug.Log("Týklama baþýna coin miktarý artýrýldý! Yeni miktar: " + touchCoinValue);
        }
        else
        {
            yetersizcoinpaneli.SetActive(true);
            yetersizcoinanim.SetTrigger("Not");
            Debug.Log("Yeterli coininiz yok!");
        }

        CheckAndUpdateLevel();

        // Level text ve slider güncelleme
        UpdateLevelText();
        UpdateLevelBar(score);
    }

    public void BuyEnergyFillSpeedupItem()
    {
        // Enerji doldurma süresini hýzlandýrma maliyetini yükle veya varsayýlan maliyeti ata
        if (PlayerPrefs.HasKey(energyFillSpeedupCostKey))
        {
            energyFillSpeedupCost = PlayerPrefs.GetFloat(energyFillSpeedupCostKey);
        }
        else
        {
            energyFillSpeedupCost = 0.5f; // Baþlangýç maliyeti
        }

        // Yeterli coin olup olmadýðýný kontrol et
        if (score >= energyFillSpeedupCost)
        {
            // Enerji doldurma süresini azalt
            fillDuration = Mathf.Max(fillDuration - 300f, 300f); // En az 300 saniyeye düþür, daha da düþürme

            // Coin'lerden maliyeti düþ
            score -= energyFillSpeedupCost;
            UpdateScoreText(); // Skoru güncelle

            // Maliyeti iki katýna çýkar
            energyFillSpeedupCost *= 2;
            SaveEnergyFillSpeedupCost(); // Enerji doldurma süresini hýzlandýrma maliyetini kaydet

            // UI'ý güncelle
            UpdateEnergyFillSpeedupCostText(); // Enerji doldurma süresini hýzlandýrma maliyetini güncelle

            Debug.Log("Enerji doldurma süresi hýzlandýrýldý! Yeni dolum süresi: " + fillDuration + " saniye");
        }
        else
        {
            yetersizcoinpaneli.SetActive(true);
            yetersizcoinanim.SetTrigger("Not");
            Debug.Log("Yeterli coininiz yok!");
        }

        CheckAndUpdateLevel();

        // Level text ve slider güncelleme
        UpdateLevelText();
        UpdateLevelBar(score);
    }

    public void BuyUnlimitedEnergyItem()
    {
        if (score >= unlimitedEnergyCost)
        {
            score -= unlimitedEnergyCost; // Coin'lerden maliyeti düþ
            UpdateScoreText(); // Skoru güncelle

            isUnlimitedEnergyActive = true;
            unlimitedEnergyEndTime = DateTime.Now.Add(unlimitedEnergyDuration); // 10 dakika ekle
            PlayerPrefs.SetString(unlimitedEnergyEndTimeKey, unlimitedEnergyEndTime.ToString());
            PlayerPrefs.Save();

            Debug.Log("Sýnýrsýz enerji aktif! Bitim zamaný: " + unlimitedEnergyEndTime);
        }
        else
        {
            yetersizcoinpaneli.SetActive(true);
            yetersizcoinanim.SetTrigger("Not");
            Debug.Log("Yeterli coininiz yok!");
        }

        CheckAndUpdateLevel();

        // Level text ve slider güncelleme
        UpdateLevelText();
        UpdateLevelBar(score);
    }


    private void SaveItemCost()
    {
        PlayerPrefs.SetFloat(itemCostKey, currentItemCost);
        PlayerPrefs.Save();
    }

    private void UpdateItemCostText()
    {
        itemCostText.text = $"{currentItemCost:F2}"; // Sadece sayý olarak maliyeti göster
    }

    private void UpdateTouchItemCostText()
    {
        touchItemCostText.text = $"{currentTouchItemCost:F2}"; // Sadece sayý olarak maliyeti göster
    }

    private void SaveEnergyFillSpeedupCost()
    {
        PlayerPrefs.SetFloat(energyFillSpeedupCostKey, energyFillSpeedupCost);
        PlayerPrefs.Save();
    }

    private void UpdateEnergyFillSpeedupCostText()
    {
        energyFillSpeedupCostText.text = $"{energyFillSpeedupCost:F2}"; // Sadece sayý olarak maliyeti göster
    }

    private void UpdateUnlimitedEnergyCostText()
    {
        unlimitedEnergyCostText.text = $"{unlimitedEnergyCost:F2}"; // Sadece sayý olarak maliyeti göster
    }

    private void UpdateScoreText()
    {
        scoreText.text = score.ToString("F2"); // Skoru iki ondalýk basamakla göster
        
    }

    private void UpdateEnergyBar()
    {
        float normalizedEnergy = currentEnergy / currentMaxEnergy; // Enerjiyi yeni kapasiteye göre normalize et
        float barWidth = normalizedEnergy * maxBarWidth;
        yellowBar.sizeDelta = new Vector2(barWidth, yellowBar.sizeDelta.y);
        UpdateEnergyText(); // Enerji seviyesini güncelle
    }


    private void UpdateEnergyText()
    {
        // Enerji seviyesini tam sayýya yuvarla
        int roundedCurrentEnergy = Mathf.RoundToInt(currentEnergy);
        int roundedMaxEnergy = Mathf.RoundToInt(currentMaxEnergy);
        // Text bileþenine enerji seviyesini yaz
        energyText.text = $"{roundedCurrentEnergy}/{roundedMaxEnergy}";
    }

    private void UpdateEnergyOnStart()
    {
        float timeElapsed = (float)(DateTime.Now - lastUpdateTime).TotalSeconds;
        float energyGained = (timeElapsed / fillDuration) * currentMaxEnergy;
        currentEnergy = Mathf.Clamp(currentEnergy + energyGained, 0, currentMaxEnergy);
        UpdateEnergyText(); // Enerji seviyesini güncelle
    }

    private void SaveMaxEnergyState()
    {
        PlayerPrefs.SetFloat(currentMaxEnergyKey, currentMaxEnergy);
        PlayerPrefs.Save();
    }


    private void SaveEnergyState()
    {
        PlayerPrefs.SetFloat(currentEnergyKey, currentEnergy);
        PlayerPrefs.SetString(energyStartTimeKey, DateTime.Now.ToString());
        PlayerPrefs.Save();
    }

    private void SaveTouchCoinValue()
    {
        PlayerPrefs.SetFloat(touchCoinValueKey, touchCoinValue);
        PlayerPrefs.Save();
        PlayerPrefs.SetFloat(clicksayisiPlayerPrefsKey, clicksayisi);
        PlayerPrefs.Save();

    }

    private void SaveTouchItemCost()
    {
        PlayerPrefs.SetFloat(touchItemCostKey, currentTouchItemCost);
        PlayerPrefs.Save();
    }


    private void InitializeLevelBar()
    {
        // Levelbar'ýn minimum ve maksimum deðerlerini ayarla
        levelBar.minValue = 0;
        levelBar.maxValue = 400; // Örneðin, maksimum deðeri 100 olarak düþünelim
    }

    private void CheckAndUpdateLevel()
    {
        int newLevel = currentLevel;

        // Level hesaplama mantýðýný buraya göre güncelleyin
        if (score >= 1e9f)
        {
            newLevel = 10;
        }
        else if (score >= 1e8f)
        {
            newLevel = 9;
        }
        else if (score >= 1e7f)
        {
            newLevel = 8;
        }
        else if (score >= 5e6f)
        {
            newLevel = 7;
        }
        else if (score >= 1e6f)
        {
            newLevel = 6;
        }
        else if (score >= 7.5e5f)
        {
            newLevel = 5;
        }
        else if (score >= 5e5f)
        {
            newLevel = 4;
        }
        else if (score >= 2.5e5f)
        {
            newLevel = 3;
        }
        else if (score >= 1e5f)
        {
            newLevel = 2;
        }

        if (newLevel != currentLevel)
        {
            currentLevel = newLevel;
            UpdateLevelText();
            SaveLevelState();
        }

        float currentLevelThreshold = levelThresholds[currentLevel - 1];
        float nextLevelThreshold = currentLevel < levelThresholds.Length ? levelThresholds[currentLevel] : currentLevelThreshold;
        // Score'a göre levelbar'ý güncelle
        UpdateLevelBar(score);
        levelclickText.text = clicksayisi.ToString("F0") + "/" + currentLevelThreshold.ToString();
    }

    private void UpdateLevelBar(float score)
    {
        // Geçerli level için maksimum skor deðerlerini al
        float maxScoreForCurrentLevel = GetMaxScoreForLevel(currentLevel);
        float maxScoreForNextLevel = GetMaxScoreForLevel(currentLevel + 1);

        // Geçerli leveldeki skor miktarýnýn yüzdesini hesapla
        float startScoreForCurrentLevel = maxScoreForCurrentLevel;
        float percentage = (score - startScoreForCurrentLevel) / (maxScoreForNextLevel - startScoreForCurrentLevel);

        // Slider'ý yüzde deðerine göre güncelle
        levelBar.value = Mathf.Lerp(levelBar.minValue, levelBar.maxValue, percentage);
    }



    private float GetMaxScoreForLevel(int level)
    {
        // Her level için maksimum score deðerlerini döndüren bir metod
        switch (level)
        {
            case 2:
                return 1e5f;
            case 3:
                return 2.5e5f;
            case 4:
                return 5e5f;
            case 5:
                return 7.5e5f;
            case 6:
                return 1e6f;
            case 7:
                return 5e6f;
            case 8:
                return 1e7f;
            case 9:
                return 1e8f;
            case 10:
                return 1e9f;
            default:
                return 0f; // Eðer level tanýmlý deðilse sýfýr döndür
        }
    }

    private void UpdateLevelText()
    {
        // Levelin yazýlý olduðu UI Text'i güncelle
        levelText.text = "Level"+currentLevel.ToString();
    }

    private void SaveLevelState()
    {
        PlayerPrefs.SetInt(currentLevelKey, currentLevel);
        PlayerPrefs.Save();
    }

    private void OnDestroy()
    {
        // Oyun kapatýldýðýnda skoru, enerji seviyesini ve enerji baþlangýç zamanýný kaydet
        PlayerPrefs.SetFloat(scorePlayerPrefsKey, score);
        SaveEnergyState();
        SaveLevelState();
        SaveTouchCoinValue(); // Týklama baþýna coin miktarýný kaydet
        SaveTouchItemCost(); // "Touch" ürünü maliyetini kaydet
        if (isUnlimitedEnergyActive)
        {
            PlayerPrefs.SetString(unlimitedEnergyEndTimeKey, unlimitedEnergyEndTime.ToString());
        }
        else
        {
            PlayerPrefs.DeleteKey(unlimitedEnergyEndTimeKey);
        }
    }




}
