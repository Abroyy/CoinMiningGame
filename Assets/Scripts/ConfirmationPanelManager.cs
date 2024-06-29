using UnityEngine;

public class ConfirmationPanelManager : MonoBehaviour
{
    public GameObject[] panels; // Panellerin GameObject dizisi
    private int openPanelIndex = -1; // Açýk olan panelin indeksi, -1 ise hiçbiri açýk deðil demektir

    void Start()
    {
        // Baþlangýçta tüm panelleri kapalý yap
        foreach (GameObject panel in panels)
        {
            panel.SetActive(false);
        }
    }

    public void OpenPanel(int panelIndex)
    {
        // Eðer bir panel zaten açýksa yeni paneli açmayýz
        if (openPanelIndex != -1)
        {
            Debug.Log("Zaten bir panel açýk, önce mevcut paneli kapatýn.");
            return;
        }

        // Paneli aç
        panels[panelIndex].SetActive(true);
        Animator animator = panels[panelIndex].GetComponent<Animator>();
        if (animator != null)
        {
            animator.SetTrigger("OpenTrigger");
        }

        openPanelIndex = panelIndex;
    }

    public void ClosePanel()
    {
        if (openPanelIndex != -1)
        {
            // Açýk olan paneli kapat
            Animator animator = panels[openPanelIndex].GetComponent<Animator>();
            if (animator != null)
            {
                animator.SetTrigger("CloseTrigger");
            }

            openPanelIndex = -1;
        }
    }
}
