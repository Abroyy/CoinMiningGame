using UnityEngine;
using UnityEngine.UI;
using System;

public class CoinClicker : MonoBehaviour
{
    public Text scoreText; // Skoru g�steren Text bile�eni
    public Text energyText; // Enerji seviyesini g�steren Text bile�eni
    public Text levelText;
    public Slider levelBar; // Levelbar i�in UI Slider referans�// Oyuncunun seviyesini g�steren Text bile�eni
    public Text itemCostText; // Enerji kapasitesini art�ran �r�n�n maliyetini g�steren Text bile�eni
    public Text touchItemCostText; // "Touch" �r�n� maliyetini g�steren Text bile�eni
    public Text levelclickText;

    private float clicksayisi;
    private float score; // Skoru tutan de�i�ken
    public Animator coinAnimator; // Coin animat�r�
    public string animationTriggerName = "Clicked"; // Animasyon tetikleyicisi
    private string scorePlayerPrefsKey = "PlayerScore"; // Skor i�in PlayerPrefs anahtar�
    private string clicksayisiPlayerPrefsKey = "Playerclicksayisi";

    public RectTransform yellowBar; // Sar� bar�n RectTransform'u
    public float maxBarWidth = 850f; // Sar� bar�n y�zde 100 dolu oldu�undaki geni�li�i
    public float energyDrainPerClick = 1f; // Her t�klamada harcanacak enerji oran�
    private float currentEnergy; // Mevcut enerji miktar�
    private float fillDuration = 3600.0f; // Dolum s�resi (saniye cinsinden)

    private string energyStartTimeKey = "EnergyStartTime"; // Enerji ba�lang�� zaman� i�in PlayerPrefs anahtar�
    private string currentEnergyKey = "CurrentEnergy"; // Mevcut enerji i�in PlayerPrefs anahtar�
    private DateTime lastUpdateTime; // Son g�ncelleme zaman�

    private string currentLevelKey = "CurrentLevel"; // Oyuncunun seviyesi i�in PlayerPrefs anahtar�
    private int currentLevel = 1; // Oyuncunun ba�lang�� seviyesi
    public float[] levelThresholds; // Level s�n�rlar�

    private string currentMaxEnergyKey = "CurrentMaxEnergy"; // Maksimum enerji kapasitesi i�in PlayerPrefs anahtar�
    private float currentMaxEnergy = 1000f; // Ba�lang��ta enerji kapasitesi 1000
    private float energyIncreaseStep = 500f; // Enerji art�� ad�m�

    private string itemCostKey = "ItemCost"; // Enerji kapasitesi art�ran �r�n�n maliyeti i�in PlayerPrefs anahtar�
    private float initialItemCost = 0.5f; // Enerji kapasitesi art�ran �r�n�n ba�lang�� maliyeti
    private float currentItemCost; // Mevcut �r�n maliyeti

    private float touchClickValue = 1f;
    private float touchCoinValue = 0.01f; // T�klama ba��na ba�lang�� coini
    private string touchCoinValueKey = "TouchCoinValue"; // T�klama ba��na coin i�in PlayerPrefs anahtar�
    private string touchClickValueKey = "TouchClickValue";

    private string touchItemCostKey = "TouchItemCost"; // "Touch" �r�n� maliyeti i�in PlayerPrefs anahtar�
    private float initialTouchItemCost = 0.5f; // "Touch" �r�n� ba�lang�� maliyeti
    private float currentTouchItemCost; // Mevcut "Touch" �r�n� maliyeti

    public Text energyFillSpeedupCostText; // Enerji doldurma s�resini h�zland�rma maliyetini g�steren Text bile�eni
    private string energyFillSpeedupCostKey = "EnergyFillSpeedupCost"; // Enerji doldurma s�resini h�zland�rma maliyeti i�in PlayerPrefs anahtar�
    private float energyFillSpeedupCost = 0.5f;

    public Text unlimitedEnergyCostText; // S�n�rs�z enerji �r�n�n�n maliyetini g�steren Text bile�eni
    private float unlimitedEnergyCost = 1f; // S�n�rs�z enerji �r�n�n�n maliyeti
    private TimeSpan unlimitedEnergyDuration = TimeSpan.FromMinutes(10); // S�n�rs�z enerji s�resi (10 dakika)
    private DateTime unlimitedEnergyEndTime; // S�n�rs�z enerjinin biti� zaman�
    private bool isUnlimitedEnergyActive = false; // S�n�rs�z enerjinin aktif olup olmad���n� belirten bayrak
    private string unlimitedEnergyEndTimeKey = "UnlimitedEnergyEndTime"; // S�n�rs�z enerji biti� zaman� i�in PlayerPrefs anahtar�

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
            clicksayisi = 0f; // E�er kaydedilmi� skor yoksa varsay�lan olarak 0 kabul edilir
        }
        // Oyun ba�lad���nda kaydedilmi� skoru y�kle
        if (PlayerPrefs.HasKey(scorePlayerPrefsKey))
        {
            score = PlayerPrefs.GetFloat(scorePlayerPrefsKey);
        }
        else
        {
            score = 0f; // E�er kaydedilmi� skor yoksa varsay�lan olarak 0 kabul edilir
        }

        // Enerjinin ba�lang�� zaman�n� kaydet veya y�kle
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

        // Kaydedilmi� mevcut enerji seviyesini y�kle
        if (PlayerPrefs.HasKey(currentEnergyKey))
        {
            currentEnergy = PlayerPrefs.GetFloat(currentEnergyKey);
        }
        else
        {
            currentEnergy = 1000f; // Ba�lang��ta enerji dolu
        }

        // Kaydedilmi� mevcut seviyeyi y�kle
        if (PlayerPrefs.HasKey(currentLevelKey))
        {
            currentLevel = PlayerPrefs.GetInt(currentLevelKey);
        }
        else
        {
            currentLevel = 1; // Ba�lang��ta seviye 1
        }

        if (PlayerPrefs.HasKey(currentMaxEnergyKey))
        {
            currentMaxEnergy = PlayerPrefs.GetFloat(currentMaxEnergyKey);
        }
        else
        {
            currentMaxEnergy = 1000f; // Ba�lang��ta enerji kapasitesi y�zde 100
        }

        // Mevcut �r�n maliyetini y�kle
        if (PlayerPrefs.HasKey(itemCostKey))
        {
            currentItemCost = PlayerPrefs.GetFloat(itemCostKey);
        }
        else
        {
            currentItemCost = initialItemCost; // Ba�lang�� maliyeti
        }

        if (PlayerPrefs.HasKey(touchCoinValueKey))
        {
            touchCoinValue = PlayerPrefs.GetFloat(touchCoinValueKey);
        }
        else
        {
            touchCoinValue = 0.01f; // Ba�lang�� de�eri
        }

        if (PlayerPrefs.HasKey(touchClickValueKey))
        {
            touchClickValue = PlayerPrefs.GetFloat(touchClickValueKey);
        }
        else
        {
            touchClickValue = 1f; // Ba�lang�� de�eri
        }

        // Mevcut "Touch" �r�n� maliyetini y�kle
        if (PlayerPrefs.HasKey(touchItemCostKey))
        {
            currentTouchItemCost = PlayerPrefs.GetFloat(touchItemCostKey);
        }
        else
        {
            currentTouchItemCost = initialTouchItemCost; // Ba�lang�� maliyeti
        }

        if (PlayerPrefs.HasKey(energyFillSpeedupCostKey))
        {
            energyFillSpeedupCost = PlayerPrefs.GetFloat(energyFillSpeedupCostKey);
        }
        else
        {
            energyFillSpeedupCost = 0.5f; // Ba�lang�� maliyeti
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

        UpdateUnlimitedEnergyCostText(); // Ba�lang��ta s�n�rs�z enerji �r�n maliyetini g�ncelle

        UpdateTouchItemCostText();

        UpdateItemCostText();
        UpdateEnergyFillSpeedupCostText();// Ba�lang��ta �r�n maliyetini g�ncelle

        // Ge�en s�reyi hesapla ve enerji seviyesini g�ncelle
        UpdateEnergyOnStart();

        UpdateScoreText();
        UpdateEnergyBar();
        UpdateLevelText(); // Ba�lang��ta seviyeyi g�ncelle
        InitializeLevelBar();

        yetersizcoinpaneli.SetActive(false);
    }

    void Update()
    {
        // Ge�en s�reyi hesapla
        float timeElapsed = (float)(DateTime.Now - lastUpdateTime).TotalSeconds;

        // Enerji miktar�n� zamana g�re yeni kapasiteye g�re g�ncelle
        float energyIncrease = (timeElapsed / fillDuration) * currentMaxEnergy;
        currentEnergy = Mathf.Clamp(currentEnergy + energyIncrease, 0, currentMaxEnergy);

        // G�ncellenen enerji miktar�n� bar �zerinde g�ster
        UpdateEnergyBar();

        // G�ncellenmi� son zaman kayd�n� yap
        lastUpdateTime = DateTime.Now;
        PlayerPrefs.SetString(energyStartTimeKey, lastUpdateTime.ToString());
        PlayerPrefs.Save();

        if (isUnlimitedEnergyActive && DateTime.Now >= unlimitedEnergyEndTime)
        {
            isUnlimitedEnergyActive = false;
            PlayerPrefs.DeleteKey(unlimitedEnergyEndTimeKey); // S�re bitti�inde keyi sil
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
                Debug.LogError("Animator bile�eni atanmad�!");
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
                Debug.LogError("Animator bile�eni atanmad�!");
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
        // �r�n�n maliyetini y�kle veya varsay�lan maliyeti ata
        if (PlayerPrefs.HasKey(itemCostKey))
        {
            currentItemCost = PlayerPrefs.GetFloat(itemCostKey);
        }
        else
        {
            currentItemCost = initialItemCost;
        }

        // Yeterli coini olup olmad���n� kontrol et
        if (score >= currentItemCost)
        {
            score -= currentItemCost; // Coin'lerden maliyeti d��
            UpdateScoreText(); // Skoru g�ncelle

            float previousMaxEnergy = currentMaxEnergy;
            currentMaxEnergy += energyIncreaseStep; // Maksimum enerji kapasitesini art�r

            // Enerji miktar�n� yeni kapasiteye g�re g�ncelle
            currentEnergy = (currentEnergy / previousMaxEnergy) * currentMaxEnergy;

            SaveMaxEnergyState(); // Enerji kapasitesi durumunu kaydet

            currentItemCost *= 2; // �r�n�n maliyetini iki kat�na ��kar
            SaveItemCost(); // �r�n maliyetini kaydet

            UpdateItemCostText(); // �r�n maliyetini g�ncelle
            UpdateEnergyBar(); // Enerji bar�n� g�ncelle

            Debug.Log("Enerji kapasitesi artt�r�ld�! Yeni maksimum kapasite: " + currentMaxEnergy);
        }
        else
        {
            yetersizcoinpaneli.SetActive(true);
            yetersizcoinanim.SetTrigger("Not");
            Debug.Log("Yeterli coininiz yok!");
        }

        CheckAndUpdateLevel();

        // Level text ve slider g�ncelleme
        UpdateLevelText();
        UpdateLevelBar(score);
    }


    public void BuyTouchIncreaseItem()
    {
        // �r�n�n maliyetini y�kle veya varsay�lan maliyeti ata
        if (PlayerPrefs.HasKey(touchItemCostKey))
        {
            currentTouchItemCost = PlayerPrefs.GetFloat(touchItemCostKey);
        }
        else
        {
            currentTouchItemCost = initialTouchItemCost;
        }

        // Yeterli coini olup olmad���n� kontrol et
        if (score >= currentTouchItemCost)
        {
            score -= currentTouchItemCost; // Coin'lerden maliyeti d��
            UpdateScoreText(); // Skoru g�ncelle

            touchCoinValue += 0.01f; // T�klama ba��na coin miktar�n� iki kat�na ��kar
            SaveTouchCoinValue(); // T�klama ba��na coin miktar�n� kaydet

            currentTouchItemCost *= 2; // �r�n�n maliyetini iki kat�na ��kar
            SaveTouchItemCost(); // �r�n maliyetini kaydet

            UpdateTouchItemCostText(); // �r�n maliyetini g�ncelle

            Debug.Log("T�klama ba��na coin miktar� art�r�ld�! Yeni miktar: " + touchCoinValue);
        }
        else
        {
            yetersizcoinpaneli.SetActive(true);
            yetersizcoinanim.SetTrigger("Not");
            Debug.Log("Yeterli coininiz yok!");
        }

        CheckAndUpdateLevel();

        // Level text ve slider g�ncelleme
        UpdateLevelText();
        UpdateLevelBar(score);
    }

    public void BuyEnergyFillSpeedupItem()
    {
        // Enerji doldurma s�resini h�zland�rma maliyetini y�kle veya varsay�lan maliyeti ata
        if (PlayerPrefs.HasKey(energyFillSpeedupCostKey))
        {
            energyFillSpeedupCost = PlayerPrefs.GetFloat(energyFillSpeedupCostKey);
        }
        else
        {
            energyFillSpeedupCost = 0.5f; // Ba�lang�� maliyeti
        }

        // Yeterli coin olup olmad���n� kontrol et
        if (score >= energyFillSpeedupCost)
        {
            // Enerji doldurma s�resini azalt
            fillDuration = Mathf.Max(fillDuration - 300f, 300f); // En az 300 saniyeye d���r, daha da d���rme

            // Coin'lerden maliyeti d��
            score -= energyFillSpeedupCost;
            UpdateScoreText(); // Skoru g�ncelle

            // Maliyeti iki kat�na ��kar
            energyFillSpeedupCost *= 2;
            SaveEnergyFillSpeedupCost(); // Enerji doldurma s�resini h�zland�rma maliyetini kaydet

            // UI'� g�ncelle
            UpdateEnergyFillSpeedupCostText(); // Enerji doldurma s�resini h�zland�rma maliyetini g�ncelle

            Debug.Log("Enerji doldurma s�resi h�zland�r�ld�! Yeni dolum s�resi: " + fillDuration + " saniye");
        }
        else
        {
            yetersizcoinpaneli.SetActive(true);
            yetersizcoinanim.SetTrigger("Not");
            Debug.Log("Yeterli coininiz yok!");
        }

        CheckAndUpdateLevel();

        // Level text ve slider g�ncelleme
        UpdateLevelText();
        UpdateLevelBar(score);
    }

    public void BuyUnlimitedEnergyItem()
    {
        if (score >= unlimitedEnergyCost)
        {
            score -= unlimitedEnergyCost; // Coin'lerden maliyeti d��
            UpdateScoreText(); // Skoru g�ncelle

            isUnlimitedEnergyActive = true;
            unlimitedEnergyEndTime = DateTime.Now.Add(unlimitedEnergyDuration); // 10 dakika ekle
            PlayerPrefs.SetString(unlimitedEnergyEndTimeKey, unlimitedEnergyEndTime.ToString());
            PlayerPrefs.Save();

            Debug.Log("S�n�rs�z enerji aktif! Bitim zaman�: " + unlimitedEnergyEndTime);
        }
        else
        {
            yetersizcoinpaneli.SetActive(true);
            yetersizcoinanim.SetTrigger("Not");
            Debug.Log("Yeterli coininiz yok!");
        }

        CheckAndUpdateLevel();

        // Level text ve slider g�ncelleme
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
        itemCostText.text = $"{currentItemCost:F2}"; // Sadece say� olarak maliyeti g�ster
    }

    private void UpdateTouchItemCostText()
    {
        touchItemCostText.text = $"{currentTouchItemCost:F2}"; // Sadece say� olarak maliyeti g�ster
    }

    private void SaveEnergyFillSpeedupCost()
    {
        PlayerPrefs.SetFloat(energyFillSpeedupCostKey, energyFillSpeedupCost);
        PlayerPrefs.Save();
    }

    private void UpdateEnergyFillSpeedupCostText()
    {
        energyFillSpeedupCostText.text = $"{energyFillSpeedupCost:F2}"; // Sadece say� olarak maliyeti g�ster
    }

    private void UpdateUnlimitedEnergyCostText()
    {
        unlimitedEnergyCostText.text = $"{unlimitedEnergyCost:F2}"; // Sadece say� olarak maliyeti g�ster
    }

    private void UpdateScoreText()
    {
        scoreText.text = score.ToString("F2"); // Skoru iki ondal�k basamakla g�ster
        
    }

    private void UpdateEnergyBar()
    {
        float normalizedEnergy = currentEnergy / currentMaxEnergy; // Enerjiyi yeni kapasiteye g�re normalize et
        float barWidth = normalizedEnergy * maxBarWidth;
        yellowBar.sizeDelta = new Vector2(barWidth, yellowBar.sizeDelta.y);
        UpdateEnergyText(); // Enerji seviyesini g�ncelle
    }


    private void UpdateEnergyText()
    {
        // Enerji seviyesini tam say�ya yuvarla
        int roundedCurrentEnergy = Mathf.RoundToInt(currentEnergy);
        int roundedMaxEnergy = Mathf.RoundToInt(currentMaxEnergy);
        // Text bile�enine enerji seviyesini yaz
        energyText.text = $"{roundedCurrentEnergy}/{roundedMaxEnergy}";
    }

    private void UpdateEnergyOnStart()
    {
        float timeElapsed = (float)(DateTime.Now - lastUpdateTime).TotalSeconds;
        float energyGained = (timeElapsed / fillDuration) * currentMaxEnergy;
        currentEnergy = Mathf.Clamp(currentEnergy + energyGained, 0, currentMaxEnergy);
        UpdateEnergyText(); // Enerji seviyesini g�ncelle
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
        // Levelbar'�n minimum ve maksimum de�erlerini ayarla
        levelBar.minValue = 0;
        levelBar.maxValue = 400; // �rne�in, maksimum de�eri 100 olarak d���nelim
    }

    private void CheckAndUpdateLevel()
    {
        int newLevel = currentLevel;

        // Level hesaplama mant���n� buraya g�re g�ncelleyin
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
        // Score'a g�re levelbar'� g�ncelle
        UpdateLevelBar(score);
        levelclickText.text = clicksayisi.ToString("F0") + "/" + currentLevelThreshold.ToString();
    }

    private void UpdateLevelBar(float score)
    {
        // Ge�erli level i�in maksimum skor de�erlerini al
        float maxScoreForCurrentLevel = GetMaxScoreForLevel(currentLevel);
        float maxScoreForNextLevel = GetMaxScoreForLevel(currentLevel + 1);

        // Ge�erli leveldeki skor miktar�n�n y�zdesini hesapla
        float startScoreForCurrentLevel = maxScoreForCurrentLevel;
        float percentage = (score - startScoreForCurrentLevel) / (maxScoreForNextLevel - startScoreForCurrentLevel);

        // Slider'� y�zde de�erine g�re g�ncelle
        levelBar.value = Mathf.Lerp(levelBar.minValue, levelBar.maxValue, percentage);
    }



    private float GetMaxScoreForLevel(int level)
    {
        // Her level i�in maksimum score de�erlerini d�nd�ren bir metod
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
                return 0f; // E�er level tan�ml� de�ilse s�f�r d�nd�r
        }
    }

    private void UpdateLevelText()
    {
        // Levelin yaz�l� oldu�u UI Text'i g�ncelle
        levelText.text = "Level"+currentLevel.ToString();
    }

    private void SaveLevelState()
    {
        PlayerPrefs.SetInt(currentLevelKey, currentLevel);
        PlayerPrefs.Save();
    }

    private void OnDestroy()
    {
        // Oyun kapat�ld���nda skoru, enerji seviyesini ve enerji ba�lang�� zaman�n� kaydet
        PlayerPrefs.SetFloat(scorePlayerPrefsKey, score);
        SaveEnergyState();
        SaveLevelState();
        SaveTouchCoinValue(); // T�klama ba��na coin miktar�n� kaydet
        SaveTouchItemCost(); // "Touch" �r�n� maliyetini kaydet
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
