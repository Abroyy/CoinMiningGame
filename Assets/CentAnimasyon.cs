using UnityEngine;

public class CentAnimasyon : MonoBehaviour
{
    public Animator coinAnimator; // Animator bile�enini atamak i�in bir de�i�ken
    public string animationTriggerName = "Drop"; // Animat�rdeki trigger parametre ad�

    // Bu fonksiyon butonun OnClick olay�na ba�lanacak
    public void PlayCoinAnimation()
    {
        if (coinAnimator != null)
        {
            coinAnimator.SetTrigger(animationTriggerName);
        }
        else
        {
            Debug.LogError("Animator bile�eni atanmad�!");
        }
    }
}
