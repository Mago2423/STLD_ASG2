using UnityEngine;

public class CoinRing : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public int score = 5;

    public int Interact()
    {
        var audio = GetComponent<AudioSource>();
        audio.Play();

        Destroy(gameObject, 2);
        var animator = GetComponent<Animator>();
       
        animator.SetTrigger("Fly");
        return score;

        
    }

}
