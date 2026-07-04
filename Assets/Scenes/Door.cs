//* Author: Lee wei jun
//* Date: 14/6/2026
//* Description: The door script responsible for activating the animations of the door so that it can open and close
using UnityEngine;
using UnityEngine.UIElements;
public class Door : MonoBehaviour
{
    public Vector3 rotateAmount = new Vector3(0, 90, 0); //set the amount the door rotates
    bool isOpen = false; //the bool used in the animation
    int time = 1000; //time iterval untill door closes by itself
    bool isColliding = false; //checks if player is colliding with the door


    void Update()
    {
        if (!isColliding && isOpen)
        {
            time -= 1;
            if (time <= 0 && !isColliding)
            {
                print("Closing door");
                Interact();
                time = 1000; //1000 frames without coming back into contact to the door, the door closes by itself
            }
        }
    }
    public int Interact()
    {
        Debug.Log($"Before toggle: isOpen = {isOpen}");

        var animator = GetComponent<Animator>();
        var audio = GetComponent<AudioSource>();

        if (audio != null)
            audio.Play();

        isOpen = !isOpen;

        Debug.Log($"After toggle: isOpen = {isOpen}");

        animator.SetBool("IsOpen", isOpen);

        return 0;
    }

    void OnCollisionEnter(Collision collision) //checks if colliding with door
    {
        print($"Collided with {collision.gameObject.name}");
        isColliding = true;
    }
    void OnCollisionExit(Collision collision) //checks if stop colliding with door
    {
        isColliding = false;
        print($"Stopped colliding with {collision.gameObject.name}");
    }
}
