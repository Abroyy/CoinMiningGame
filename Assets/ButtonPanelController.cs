using UnityEngine;
using UnityEngine.UI;

public class ButtonPanelController : MonoBehaviour
{
    public GameObject[] panels; // Panel array'i, Unity Inspector'da atayacaðýz
    public Image[] panelFrames; // Panel çerçeve (frame) array'i, Unity Inspector'da atayacaðýz

    private void Start()
    {
        // Baþlangýçta 3. panelin aktif olmasýný ve çerçevesinin görünür olmasýný saðlýyoruz
        panels[2].SetActive(true); // 3. panel index olarak 2'dir (0'dan baþlayan index)
        panelFrames[2].enabled = true;

        // Diðer panelleri ve çerçeveleri baþlangýçta kapalý olarak ayarlýyoruz
        for (int i = 0; i < panels.Length; i++)
        {
            if (i != 2) // 3. panel hariç diðerleri kapalý
            {
                //panels[i].SetActive(false);
                panelFrames[i].enabled = false;
            }
        }
    }

    // Butonlara baðlý olarak panelleri ve çerçeveleri açan metod
    public void ActivatePanel(int panelIndex)
    {
        // Tüm panelleri kapat
        foreach (GameObject panel in panels)
        {
            panel.SetActive(false);
        }

        // Tüm çerçeveleri kapat
        foreach (Image frame in panelFrames)
        {
            frame.enabled = false;
        }

        // Ýstenilen paneli ve çerçeveyi aç
        panels[panelIndex].SetActive(true);
        panelFrames[panelIndex].enabled = true;
    }
}
