using UnityEngine;
using UnityEngine.UI;

public class ButtonPanelController : MonoBehaviour
{
    public GameObject[] panels; // Panel array'i, Unity Inspector'da atayaca��z
    public Image[] panelFrames; // Panel �er�eve (frame) array'i, Unity Inspector'da atayaca��z

    private void Start()
    {
        // Ba�lang��ta 3. panelin aktif olmas�n� ve �er�evesinin g�r�n�r olmas�n� sa�l�yoruz
        panels[2].SetActive(true); // 3. panel index olarak 2'dir (0'dan ba�layan index)
        panelFrames[2].enabled = true;

        // Di�er panelleri ve �er�eveleri ba�lang��ta kapal� olarak ayarl�yoruz
        for (int i = 0; i < panels.Length; i++)
        {
            if (i != 2) // 3. panel hari� di�erleri kapal�
            {
                //panels[i].SetActive(false);
                panelFrames[i].enabled = false;
            }
        }
    }

    // Butonlara ba�l� olarak panelleri ve �er�eveleri a�an metod
    public void ActivatePanel(int panelIndex)
    {
        // T�m panelleri kapat
        foreach (GameObject panel in panels)
        {
            panel.SetActive(false);
        }

        // T�m �er�eveleri kapat
        foreach (Image frame in panelFrames)
        {
            frame.enabled = false;
        }

        // �stenilen paneli ve �er�eveyi a�
        panels[panelIndex].SetActive(true);
        panelFrames[panelIndex].enabled = true;
    }
}
