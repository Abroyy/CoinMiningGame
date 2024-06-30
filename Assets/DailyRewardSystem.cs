using System;
using UnityEngine;
using UnityEngine.UI;

public class DailyRewardSystem : MonoBehaviour
{
    private const int TotalDays = 7; // Toplam g�n say�s�
    private const int TotalWeeks = 4; // Toplam hafta say�s�
    private const int DaysPerWeek = 7; // Haftal�k g�n say�s�
    private const string LastRewardDateKey = "LastRewardDate";
    private const string RewardDayKeyPrefix = "RewardDay_";
    private const string WeeklyRewardClaimedKey = "WeeklyRewardClaimed";
    public float dailyRewardAmount = 0.05f;
    public float weeklyRewardAmount = 0.2f; // Haftal�k �d�l miktar�
    public CoinClicker coinClicker;

    // G�nl�k �d�ller
    private string[] dailyRewards = {
        "Reward 1",
        "Reward 2",
        "Reward 3",
        "Reward 4",
        "Reward 5",
        "Reward 6",
        "Reward 7"
    };

    // Haftal�k �d�ller
    private float[] weeklyRewards = {
        0.1f, // Hafta 1 �d�l�
        0.1f, // Hafta 2 �d�l�
        0.1f, // Hafta 3 �d�l�
        0.1f  // Hafta 4 �d�l�
    };

    // UI elemanlar�
    public Button[] rewardButtons; // G�nl�k �d�l butonlar�
    public Button[] weekButtons;

    private DateTime lastRewardDate;

    void Start()
    {
        LoadData();
        UpdateUI();
    }

    private void LoadData()
    {
        lastRewardDate = PlayerPrefs.HasKey(LastRewardDateKey)
            ? DateTime.FromBinary(Convert.ToInt64(PlayerPrefs.GetString(LastRewardDateKey)))
            : DateTime.MinValue;
    }

    private void SaveData()
    {
        PlayerPrefs.SetString(LastRewardDateKey, lastRewardDate.ToBinary().ToString());
    }

    private void UpdateUI()
    {
        DateTime currentDate = DateTime.Today;

        // Hangi g�n oldu�una karar veriyoruz
        int dayIndex = (int)(currentDate - lastRewardDate.Date).TotalDays % TotalDays;
        if (dayIndex < 0)
        {
            dayIndex += TotalDays; // Negatif g�n indeksi d�zeltme
        }

        bool weeklyRewardClaimed = PlayerPrefs.GetInt(WeeklyRewardClaimedKey, 0) == 1;
        int weekIndex = ((int)DateTime.Now.Day / DaysPerWeek) -1 ;

        for (int i = 0; i < TotalDays; i++)
        {
            bool rewardClaimed = PlayerPrefs.GetInt(RewardDayKeyPrefix + i, 0) == 1;

            if (i == dayIndex && lastRewardDate.Date != currentDate.Date)
            {
                // Sadece o g�n�n butonunu aktif yap�yoruz
                rewardButtons[i].interactable = true;
                rewardButtons[i].gameObject.SetActive(true);
            }
            else
            {
                // Di�er butonlar� pasif yap�yoruz
                rewardButtons[i].interactable = false;
            }

            int currentDayIndex = i; // Closure i�in lokal de�i�ken
            rewardButtons[i].onClick.RemoveAllListeners();
            rewardButtons[i].onClick.AddListener(() => ClaimDailyReward(currentDayIndex));
        }

        if (weekIndex <= TotalWeeks && !weeklyRewardClaimed)
        {
            Debug.Log($"weekIndex {weekIndex}");
            // Haftal�k �d�l zaman� geldi ve hen�z talep edilmediyse
            weekButtons[weekIndex].interactable = true;
            weekButtons[weekIndex].gameObject.SetActive(true);
            weekButtons[weekIndex].onClick.RemoveAllListeners();
            weekButtons[weekIndex].onClick.AddListener(() => ClaimWeeklyReward(weekIndex));
        }
        else
        {
            // Haftal�k �d�l butonunu pasif yap
            weekButtons[weekIndex].interactable = false;
        }
    }

    public void ClaimDailyReward(int dayIndex)
    {
        DateTime currentDate = DateTime.Today;

        if (lastRewardDate.Date != currentDate.Date)
        {
            bool rewardClaimed = PlayerPrefs.GetInt(RewardDayKeyPrefix + dayIndex, 0) == 1;

            if (!rewardClaimed)
            {
                // �d�l al�n�yor
                coinClicker.score += dailyRewardAmount;
                GiveDailyReward(dayIndex);
                PlayerPrefs.SetInt(RewardDayKeyPrefix + dayIndex, 1);
                rewardButtons[dayIndex].interactable = false;

                // Son �d�l al�nan g�n� g�ncelle
                lastRewardDate = currentDate;
                SaveData();
            }
        }
    }

    public void ClaimWeeklyReward(int weekIndex)
    {
        coinClicker.score += weeklyRewards[weekIndex];
        PlayerPrefs.SetInt(WeeklyRewardClaimedKey, 1);
        weekButtons[weekIndex].interactable = false;
        Debug.Log("Haftal�k �d�l Talep Edildi!");
    }

    private void GiveDailyReward(int day)
    {
        if (day < dailyRewards.Length)
        {
            Debug.Log("G�nl�k �d�l: " + dailyRewards[day]);
            // �d�l� burada verin (�rne�in, coin ekleme)
        }
    }
}
