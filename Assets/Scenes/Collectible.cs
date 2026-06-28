//* Author: Lee wei jun
//* Date: 14/6/2026
//* Description: The script for any of the collectables to call thier audio and animation, allows score gained to be customisable
using UnityEngine;

public class Collectible : MonoBehaviour
{
    /// <summary>
    /// custimisable in unity to set how much score each collectable gives
    /// </summary>
    public int score = 1; 
    public float hideDelay = 2f;
    public float timer;
    bool startTimer = false;

    void Start()
    {
        timer = hideDelay;
    }
    void Update()
    {
        if (startTimer)
        {
            timer -= Time.deltaTime;

            if (timer <= 0)
            {
                gameObject.SetActive(false);
            }
        }
    }
    public void Collect()
    {
        var audio = GetComponent<AudioSource>(); //play audio
        audio.Play();

        var collider = GetComponent<CapsuleCollider>(); //disable collider
        collider.enabled = false;

        startTimer = true;//hide game object after 2 seconds
        

        var animator = GetComponent<Animator>(); 
        animator.SetTrigger("Fly"); //play animation
    }
}
