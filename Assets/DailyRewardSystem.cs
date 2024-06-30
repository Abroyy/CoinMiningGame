using System;
using UnityEngine;
using UnityEngine.UI;

public class DailyRewardSystem : MonoBehaviour
{
    private const int TotalDays = 7; // Toplam gün sayýsý
    private const int TotalWeeks = 4; // Toplam hafta sayýsý
    private const int DaysPerWeek = 7; // Haftalýk gün sayýsý
    private const string LastRewardDateKey = "LastRewardDate";
    private const string RewardDayKeyPrefix = "RewardDay_";
    private const string WeeklyRewardClaimedKey = "WeeklyRewardClaimed";
    public float dailyRewardAmount = 0.05f;
    public float weeklyRewardAmount = 0.2f; // Haftalýk ödül miktarý
    public CoinClicker coinClicker;

    // Günlük ödüller
    private string[] dailyRewards = {
        "Reward 1",
        "Reward 2",
        "Reward 3",
        "Reward 4",
        "Reward 5",
        "Reward 6",
        "Reward 7"
    };

    // Haftalýk ödüller
    private float[] weeklyRewards = {
        0.1f, // Hafta 1 ödülü
        0.1f, // Hafta 2 ödülü
        0.1f, // Hafta 3 ödülü
        0.1f  // Hafta 4 ödülü
    };

    // UI elemanlarý
    public Button[] rewardButtons; // Günlük ödül butonlarý
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

        // Hangi gün olduðuna karar veriyoruz
        int dayIndex = (int)(currentDate - lastRewardDate.Date).TotalDays % TotalDays;
        if (dayIndex < 0)
        {
            dayIndex += TotalDays; // Negatif gün indeksi düzeltme
        }

        bool weeklyRewardClaimed = PlayerPrefs.GetInt(WeeklyRewardClaimedKey, 0) == 1;
        int weekIndex = ((int)DateTime.Now.Day / DaysPerWeek) -1 ;

        for (int i = 0; i < TotalDays; i++)
        {
            bool rewardClaimed = PlayerPrefs.GetInt(RewardDayKeyPrefix + i, 0) == 1;

            if (i == dayIndex && lastRewardDate.Date != currentDate.Date)
            {
                // Sadece o günün butonunu aktif yapýyoruz
                rewardButtons[i].interactable = true;
                rewardButtons[i].gameObject.SetActive(true);
            }
            else
            {
                // Diðer butonlarý pasif yapýyoruz
                rewardButtons[i].interactable = false;
            }

            int currentDayIndex = i; // Closure için lokal deðiþken
            rewardButtons[i].onClick.RemoveAllListeners();
            rewardButtons[i].onClick.AddListener(() => ClaimDailyReward(currentDayIndex));
        }

        if (weekIndex <= TotalWeeks && !weeklyRewardClaimed)
        {
            Debug.Log($"weekIndex {weekIndex}");
            // Haftalýk ödül zamaný geldi ve henüz talep edilmediyse
            weekButtons[weekIndex].interactable = true;
            weekButtons[weekIndex].gameObject.SetActive(true);
            weekButtons[weekIndex].onClick.RemoveAllListeners();
            weekButtons[weekIndex].onClick.AddListener(() => ClaimWeeklyReward(weekIndex));
        }
        else
        {
            // Haftalýk ödül butonunu pasif yap
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
                // Ödül alýnýyor
                coinClicker.score += dailyRewardAmount;
                GiveDailyReward(dayIndex);
                PlayerPrefs.SetInt(RewardDayKeyPrefix + dayIndex, 1);
                rewardButtons[dayIndex].interactable = false;

                // Son ödül alýnan günü güncelle
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
        Debug.Log("Haftalýk Ödül Talep Edildi!");
    }

    private void GiveDailyReward(int day)
    {
        if (day < dailyRewards.Length)
        {
            Debug.Log("Günlük Ödül: " + dailyRewards[day]);
            // Ödülü burada verin (örneðin, coin ekleme)
        }
    }
}
