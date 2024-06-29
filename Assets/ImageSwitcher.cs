using UnityEngine;
using UnityEngine.UI;

public class ImageSwitcherWithSingleCheckmark : MonoBehaviour
{
    public Image targetImage; // Resmi göstereceðimiz Image bileþeni
    public Sprite[] sprites;  // Butonlara atanacak resimler
    public Button[] buttons;  // Butonlar
    public Image[] checkmarks; // Her butonun Checkmark görselleri

    public Sprite checkmarkSprite; // Checkmark (normal) görseli
    public Sprite tickSprite; // Tik (seçili) görseli

    private const string SelectedImageKey = "SelectedImageIndex";
    private const string SelectedButtonKey = "SelectedButtonIndex";
    private int selectedIndex = -1;

    private void Start()
    {
        LoadSelectedImageAndButton();
    }

    public void ChangeImage(int index)
    {
        if (index >= 0 && index < sprites.Length)
        {
            targetImage.sprite = sprites[index];
            UpdateCheckmarks(index);
            SaveSelectedImageAndButton(index);
        }
    }

    private void UpdateCheckmarks(int index)
    {
        for (int i = 0; i < checkmarks.Length; i++)
        {
            if (i == index)
            {
                // Sadece týklanan butonun Checkmark'ýný tik yap ve -90 derece döndür
                checkmarks[i].sprite = tickSprite;
                checkmarks[i].rectTransform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else
            {
                // Diðer butonlarýn Checkmark'larýný normal yap ve döndürmeyi sýfýrla
                checkmarks[i].sprite = checkmarkSprite;
                checkmarks[i].rectTransform.rotation = Quaternion.identity;
            }
        }
    }

    private void SaveSelectedImageAndButton(int index)
    {
        PlayerPrefs.SetInt(SelectedImageKey, index);
        PlayerPrefs.SetInt(SelectedButtonKey, index);
        PlayerPrefs.Save();
    }

    private void LoadSelectedImageAndButton()
    {
        if (PlayerPrefs.HasKey(SelectedImageKey))
        {
            selectedIndex = PlayerPrefs.GetInt(SelectedImageKey);
            if (selectedIndex >= 0 && selectedIndex < sprites.Length)
            {
                targetImage.sprite = sprites[selectedIndex];
                UpdateCheckmarks(selectedIndex);
            }
        }
    }
}
