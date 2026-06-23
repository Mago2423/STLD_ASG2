//* Author: Lee wei jun
//* Date: 14/6/2026
//* Description: the player script that is responsible for Collision detection and additional player inputs
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class CollisionDetector : MonoBehaviour
{
    /// <summary>
    /// Increment score by this value when a coin is collected.
    /// </summary>
    int score = 0;  //unity overights this value
    /// <summary>
    /// Player Health
    /// </summary>
    public int health = 100; 
    /// <summary>
    /// keeps track of items collected
    /// </summary>
    public int itemsCollected = 0;
    /// <summary>
    ///  keeps track of coins collected
    /// </summary>
    public int coinsCollected = 0; 
    /// <summary>
    ///  the amount healed from using the injector
    /// </summary>
    public int healAmount = 20; 
    /// <summary>
    /// checks for whether the player has picked up the injector
    /// </summary>
    public bool HaveInjector = false; 
    /// <summary>
    /// checks for whether the player has picked up the KeyCard
    /// </summary>
    public bool HaveKeyCard = false; 
    /// <summary>
    /// checks for whether the player has picked up the JointPlug
    /// </summary>
    public bool HaveJointPlug = false; 
    /// <summary>
    /// checks for whether the power is on
    /// </summary>
    public bool HavePower = false;

    GameObject currentCollider;
    
    public UIManagerScript UIManagerScript;
    void OnMenu()
    {
        UIManagerScript.TogglePanel(); //toggle panel - pause screen
    }
    void OnCollisionEnter(Collision collision)
    {
        currentCollider = collision.gameObject;
        print($"Collided with {currentCollider.name}"); // check in console
    }

    void OnCollisionExit(Collision collision)
    {
        print($"Stopped colliding with {currentCollider.name}"); // check in console
        currentCollider = null;
    }

    void OnInteract(InputValue value) // code for interace - E
    {
        if (currentCollider == null)
        {
            return;
        }
        print("Interacted"); // check in console
        var Collectible = currentCollider.GetComponent<Collectible>();
        var collider = currentCollider.GetComponent<Collider>();
        var Door = currentCollider.GetComponent<Door>();
        if (Door != null)
        {
            if (currentCollider.CompareTag("Locked")) //checks for "locked" Tag
            {
                if (HaveKeyCard == true) // if Have key card the door is unlocked
                {
                    print("Door is now unlocked!");
                    Door.Interact();
                }
                else
                {
                    print("Door is locked. Find the Keycard to unlock."); //No keycard, its locked
                    UIManagerScript.KeyCardPanel();
                    return; // Exit the method to prevent interaction with the door
                }
            }
            else if (currentCollider.CompareTag("Power")) //checks for "power" Tag
            {
                if (HavePower == true) // if Have generator on the door is unlocked
                {
                    print("Door is now unlocked!");
                    Door.Interact();
                }
                else
                {
                    print("Door is locked. On the generator"); //No keycard, its locked
                    UIManagerScript.GeneratorDoorPanel();
                    return; // Exit the method to prevent interaction with the door
                }
            }
            else if (currentCollider.CompareTag("Unlocked")) //checks for "unlocked" Tag
            {
                print($"Interacted with {currentCollider.name}");
                Door.Interact(); //open or close the door using animation
                return;
            }
        }
        else if (currentCollider.CompareTag("Generator")) //checks for "generator" Tag
            {
                if (HaveJointPlug == true) // if Have key card the door is unlocked
                {
                    print("Power is now on!");
                    UIManagerScript.JointPlugUsed();
                    HavePower = true;
                }
                else
                {
                    print("Door is locked. Find the Keycard to unlock."); //No keycard, its locked
                    UIManagerScript.GeneratorPanel();
                    return; // Exit the method to prevent interaction with the door
                }
            }
        else if (currentCollider != null && currentCollider.CompareTag("Item")) //checks for the tag "item"
        {
            if (Collectible != null)
            {
                if(collider != null && !collider.enabled) // checks if item interacted with has already been collected
                {
                    print($"Already collected {currentCollider.name}"); 
                }
                else
                {
                    print($"Interacted with {currentCollider.name}");
                    itemsCollected += 1; //increase itemcollected by 1
                    Collectible.Collect(); //play audio and animation
                    UIManagerScript.ItemCollected(itemsCollected); //update UI
                }
            }
        }
        else if (currentCollider.CompareTag("Injector")) //checks for injector tag - healing item
        {
            HaveInjector = true; // player has injector
            UIManagerScript.InjectorCollected();
            if (Collectible != null)
            {
                Collectible.Collect(); //play audio and animation
            }
        }
        else if (currentCollider.CompareTag("KeyCard")) //checks for keycard tag
        {
            HaveKeyCard = true;
            UIManagerScript.KeyCardCollected();
            if (Collectible != null)
            {
                Collectible.Collect(); //play audio and animation
            }
        }
        else if (currentCollider.CompareTag("JointPlug")) //checks for jointPlug tag
        {
            HaveJointPlug = true;
            UIManagerScript.JointPlugCollected();
            if (Collectible != null)
            {
                Collectible.Collect(); //play audio and animation
            }
        }

        else if (currentCollider != null)
        {
            print($"Interacted with {currentCollider.name}");
            if (Collectible != null)
            {   
                if(collider != null && !collider.enabled) //checks if item has already been collected
                {
                    print($"Already collected {currentCollider.name}");
                }
                else
                {
                    print($"Interacted with {currentCollider.name}");
                    coinsCollected += 1; //increase coinscollected by 1
                    score += Collectible.score; //checks for the score asigned to the collectible which was changed in unity and adding score value to current score
                    print($"Score: {score}");
                    Collectible.Collect(); //play audio and animation
                    UIManagerScript.UpdateScore(score); //update score in the UI
                    UIManagerScript.CoinsCollected(coinsCollected); //update coinscollected in the UI
                }
            
            }
        }
    }

    void OnHeal(InputValue value) //the input to use the injector - Q
    {
        if (HaveInjector == true) //check if player has injector on them
        {
            print($"Healed for {healAmount} health");
            health += healAmount; //heals for the healamount (20), current health plus healing amount
            if (health > 100) health = 100;
            UIManagerScript.UpdateHealth(health); //updates current health after heal in UI
            UIManagerScript.InjectorUsed(); //Updates UI to remove the injector icon
            HaveInjector = false; //Player no injector anymore
        }
    }


}