using UnityEngine;

public class CentAnimasyon : MonoBehaviour
{
    public Animator coinAnimator; // Animator bileþenini atamak için bir deðiþken
    public string animationTriggerName = "Drop"; // Animatördeki trigger parametre adý

    // Bu fonksiyon butonun OnClick olayýna baðlanacak
    public void PlayCoinAnimation()
    {
        if (coinAnimator != null)
        {
            coinAnimator.SetTrigger(animationTriggerName);
        }
        else
        {
            Debug.LogError("Animator bileþeni atanmadý!");
        }
    }
}
