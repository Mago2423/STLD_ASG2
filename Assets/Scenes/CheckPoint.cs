using UnityEngine;
using UnityEngine.Events;

public class CheckPoint : MonoBehaviour

{
    public CollisionDetector collisionDetector;
    /// <summary>
    /// Increment score by this value when a coin is collected.
    /// </summary>
    public int savescore = 100;  //unity overights this value
    /// <summary>
    /// Player Health
    /// </summary>
    public int savehealth = 100; 
    /// <summary>
    /// keeps track of items collected
    /// </summary>
    public int saveitemsCollected = 0;
    /// <summary>
    ///  keeps track of coins collected
    /// </summary>
    public int savecoinsCollected = 0; 
    /// <summary>
    ///  the amount healed from using the injector
    /// </summary>
    public int savehealAmount = 20; 
    /// <summary>
    /// checks for whether the player has picked up the injector
    /// </summary>
    public bool saveHaveInjector = false; 
    /// <summary>
    /// checks for whether the player has picked up the KeyCard
    /// </summary>
    public bool saveHaveKeyCard = false; 
    /// <summary>
    /// checks for whether the player has picked up the JointPlug
    /// </summary>
    public bool saveHaveJointPlug = false; 
    /// <summary>
    /// checks for whether the power is on
    /// </summary>
    public bool saveHavePower = false;






    public Transform startposition;
    private Vector3 spawnpoint;
    public UnityEvent Respawn;
    bool Checkpoint = false;
    public UIManagerScript UIManagerScript;

    void Start()
    {
        spawnpoint = startposition.position;
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("player"))
        {
            collision.gameObject.transform.root.position = spawnpoint;
            print("checkpoint updated");
            Checkpoint = true;
            SaveProgress();

        }
    }
    void SaveProgress()
    {
        savescore = collisionDetector.score;
        /// <summary>
        /// Player Health
        /// </summary>
        savehealth = collisionDetector.health; 
        /// <summary>
        /// keeps track of items collected
        /// </summary>
        saveitemsCollected = collisionDetector.itemsCollected;
        /// <summary>
        ///  keeps track of coins collected
        /// </summary>
        savecoinsCollected = collisionDetector.coinsCollected; 
        /// <summary>
        ///  the amount healed from using the injector
        /// </summary>
        savehealAmount = collisionDetector.healAmount; 
        /// <summary>
        /// checks for whether the player has picked up the injector
        /// </summary>
        saveHaveInjector = collisionDetector.HaveInjector; 
        /// <summary>
        /// checks for whether the player has picked up the KeyCard
        /// </summary>
        saveHaveKeyCard = collisionDetector.HaveKeyCard; 
        /// <summary>
        /// checks for whether the player has picked up the JointPlug
        /// </summary>
        saveHaveJointPlug = collisionDetector.HaveJointPlug; 
        /// <summary>
        /// checks for whether the power is on
        /// </summary>
        saveHavePower = collisionDetector.HavePower;

    }

    public void LoadProgress()
    {
        UIManagerScript.ItemCollected(saveitemsCollected);
    }

    

}
