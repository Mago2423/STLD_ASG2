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
    public void Collect()
    {
        var audio = GetComponent<AudioSource>(); //play audio
        audio.Play();

        var collider = GetComponent<CapsuleCollider>(); //disable collider
        collider.enabled = false;

        Destroy(gameObject, 2);
        var animator = GetComponent<Animator>(); //destroy game object after 2 seconds
       
        animator.SetTrigger("Fly"); //play animation
    }
}
