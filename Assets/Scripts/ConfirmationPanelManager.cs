using UnityEngine;

public class ConfirmationPanelManager : MonoBehaviour
{
    public GameObject[] panels; // Panellerin GameObject dizisi
    private int openPanelIndex = -1; // A��k olan panelin indeksi, -1 ise hi�biri a��k de�il demektir

    void Start()
    {
        // Ba�lang��ta t�m panelleri kapal� yap
        foreach (GameObject panel in panels)
        {
            panel.SetActive(false);
        }
    }

    public void OpenPanel(int panelIndex)
    {
        // E�er bir panel zaten a��ksa yeni paneli a�may�z
        if (openPanelIndex != -1)
        {
            Debug.Log("Zaten bir panel a��k, �nce mevcut paneli kapat�n.");
            return;
        }

        // Paneli a�
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
            // A��k olan paneli kapat
            Animator animator = panels[openPanelIndex].GetComponent<Animator>();
            if (animator != null)
            {
                animator.SetTrigger("CloseTrigger");
            }

            openPanelIndex = -1;
        }
    }
}
