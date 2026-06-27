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
    public float savelapsedTime = 0f; //time spent while the game is running






    public Transform startposition;
    private Vector3 spawnpoint;
    public UnityEvent Respawn;
    public UIManagerScript UIManagerScript;

    void Start()
    {
        spawnpoint = startposition.position;
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.transform.root.CompareTag("Player"))
        {
            print("checkpoint updated");
            spawnpoint = transform.position;
            print("checkpoint updated");
            SaveProgress();

        }
    }
    public void SaveProgress()
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
        savelapsedTime = UIManagerScript.elapsedTime; //time spent while the game is running

    }

    public void LoadProgress()
    {
        Debug.Log(collisionDetector.gameObject.name);
        Debug.Log("Spawnpoint: " + spawnpoint);
        Debug.Log("Player before: " + collisionDetector.transform.position);

        collisionDetector.transform.position = spawnpoint;

        Debug.Log("Player after: " + collisionDetector.transform.position);
        //Move player to saved position
        collisionDetector.transform.position = spawnpoint;

        CharacterController cc =
        collisionDetector.GetComponent<CharacterController>();

        Rigidbody rb =
            collisionDetector.GetComponent<Rigidbody>();

        if (cc != null)
            cc.enabled = false;

        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;
        }

        collisionDetector.transform.position = spawnpoint;

        if (rb != null)
            rb.isKinematic = false;

        if (cc != null)
            cc.enabled = true;


        // Restore player values
        collisionDetector.score = savescore;
        collisionDetector.health = savehealth;
        collisionDetector.itemsCollected = saveitemsCollected;
        collisionDetector.coinsCollected = savecoinsCollected;
        collisionDetector.healAmount = savehealAmount;
        collisionDetector.HaveInjector = saveHaveInjector;
        collisionDetector.HaveKeyCard = saveHaveKeyCard;
        collisionDetector.HaveJointPlug = saveHaveJointPlug;
        collisionDetector.HavePower = saveHavePower;

        // Restore timer
        UIManagerScript.elapsedTime = savelapsedTime;

        // Update UI
        UIManagerScript.UpdateScore(savescore);
        UIManagerScript.UpdateHealth(savehealth);
        UIManagerScript.ItemCollected(saveitemsCollected);
        UIManagerScript.CoinsCollected(savecoinsCollected);

        // Restore icons
        if (saveHaveInjector)
        {
            UIManagerScript.InjectorCollected();
        }
        else
        {
            UIManagerScript.InjectorUsed();
        }
        if (saveHaveKeyCard)
            UIManagerScript.KeyCardCollected();
        else
        {
            UIManagerScript.KeyCardUsed();
        }

        if (saveHaveJointPlug)
            UIManagerScript.JointPlugCollected();
        else
        {
            UIManagerScript.JointPlugUsed();
        }
    }

    

}
