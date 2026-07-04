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
    public int score = 0;  //unity overights this value
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

    public float interactDistance = 6f;
    //public LayerMask interactLayer;

    public GameObject objectToSpawn;
    public GameObject explosion;
    bool touched = false;
    [Header("Debug")]
    public bool debugUseKey = true; // allow testing interact with keyboard E
    [Header("References")]
    public Camera cameraToUse; // assign in inspector to ensure correct camera is used
    public RectTransform crosshair; // optional UI crosshair position for raycast origin
    
    public UIManagerScript UIManagerScript;
    public void OnMenu(InputValue value)
    {
        UIManagerScript.TogglePanel(); //toggle panel - pause screen
    }
    // Support PlayerInput Send Messages (parameterless) behavior
    public void OnMenu()
    {
        UIManagerScript.TogglePanel();
    }
    public void OnInteract(InputValue value)
    {
        HandleInteract();
    }

    // Support PlayerInput Send Messages (parameterless) behavior
    public void OnInteract()
    {
        HandleInteract();
    }

    private void HandleInteract()
    {
        Debug.Log("HandleInteract: starting diagnostics");
        if (cameraToUse == null && Camera.main == null)
        {
            Debug.Log("HandleInteract: No camera available (cameraToUse and Camera.main are null)");
            return;
        }

        // prefer inspector cameraToUse if assigned
        var cam = cameraToUse != null ? cameraToUse : Camera.main;
        Ray ray;
        if (crosshair != null)
        {
            Vector3 screenPoint = RectTransformUtility.WorldToScreenPoint(cam, crosshair.position);
            ray = cam.ScreenPointToRay(screenPoint);
            Debug.Log($"Using crosshair-based ray. ScreenPoint: {screenPoint}, origin: {ray.origin}, dir: {ray.direction}, maxDist: {interactDistance}");
            Debug.DrawRay(ray.origin, ray.direction * interactDistance, Color.red, 2f);
        }
        else
        {
            Vector3 origin = cam.transform.position + cam.transform.forward * cam.nearClipPlane;
            ray = new Ray(origin, cam.transform.forward);
            Debug.Log($"Using forward ray. Origin: {origin}, dir: {cam.transform.forward}, maxDist: {interactDistance}");
            Debug.DrawRay(origin, cam.transform.forward * interactDistance, Color.red, 2f);
        }

        // Gather all hits (including triggers) and pick the nearest non-player, non-self hit
        var allHits = Physics.RaycastAll(ray, interactDistance, ~0, QueryTriggerInteraction.Collide);
        Debug.Log($"RaycastAll hits count: {allHits.Length}");

        if (allHits.Length == 0)
        {
            Debug.Log("No RaycastAll hits");
            return;
        }

        System.Array.Sort(allHits, (a, b) => a.distance.CompareTo(b.distance));

        RaycastHit? chosen = null;
        foreach (var h in allHits)
        {
            if (h.collider == null) continue;
            Debug.Log($"  RaycastAll hit: {h.collider.name}, tag={h.collider.tag}, isTrigger={h.collider.isTrigger}, distance={h.distance}");

            // Skip hits that are part of this player (self or children)
            if (h.collider.transform.IsChildOf(this.transform) || h.collider.gameObject == this.gameObject)
            {
                Debug.Log($"  Skipping hit on self: {h.collider.name}");
                continue;
            }

            // Skip objects explicitly tagged as Player
            if (h.collider.CompareTag("Player"))
            {
                Debug.Log($"  Skipping hit by tag Player: {h.collider.name}");
                continue;
            }

            chosen = h;
            break;
        }

        if (!chosen.HasValue)
        {
            Debug.Log("No valid hits after filtering player/self");
            return;
        }

        RaycastHit hit = chosen.Value;
        Debug.Log($"Hit: {hit.collider.name}, Tag: {hit.collider.tag}, isTrigger={hit.collider.isTrigger}");

        GameObject currentCollider = hit.collider.gameObject;

        Debug.Log($"Looking at {currentCollider.name}");

        print("Interacted"); // check in console
        var Collectible = currentCollider.GetComponent<Collectible>();//checks for the collectible script on the object
        var collider = currentCollider.GetComponent<Collider>();//checks for the collider on the object
        var Door = currentCollider.GetComponent<Door>();//checks for the door script on the object
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

        else if (currentCollider.CompareTag("coinSpawn") && !touched)
        {
            var spawnedObject = Instantiate(objectToSpawn,currentCollider.transform.position + new Vector3(0,1,0), currentCollider.transform.rotation);
            var explosionObject = Instantiate(explosion,currentCollider.transform.position + new Vector3(0,1,0), currentCollider.transform.rotation, spawnedObject.transform);
            touched = true;
            Destroy(currentCollider.gameObject);
            Destroy(explosionObject,2);
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
        else if (currentCollider.CompareTag("Item")) //checks for the tag "item"
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

        else if (currentCollider.CompareTag("Coin")) //checks for coin tag
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

    public void OnHeal(InputValue value) //the input to use the injector - Q
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
    // Support PlayerInput Send Messages (parameterless) behavior
    public void OnHeal()
    {
        if (HaveInjector == true)
        {
            print($"Healed for {healAmount} health");
            health += healAmount;
            if (health > 100) health = 100;
            UIManagerScript.UpdateHealth(health);
            UIManagerScript.InjectorUsed();
            HaveInjector = false;
        }
    }
    private void Start()
    {
        Debug.Log($"CollisionDetector Start on {gameObject.name}, enabled={enabled}");
        Debug.Log($"MainCamera: {(Camera.main!=null?Camera.main.name:"null")}, interactDistance={interactDistance}");
        var pi = GetComponent<UnityEngine.InputSystem.PlayerInput>();
        Debug.Log($"PlayerInput on same GameObject: {(pi!=null?"found":"none")}");

        // If no camera was assigned in inspector, pick the enabled camera with highest depth (likely the gameplay follow camera)
        if (cameraToUse == null)
        {
            Camera[] cams = Camera.allCameras;
            Camera best = null;
            float bestDepth = float.MinValue;
            foreach (var c in cams)
            {
                if (!c.enabled || !c.gameObject.activeInHierarchy) continue;
                if (c.depth > bestDepth)
                {
                    best = c;
                    bestDepth = c.depth;
                }
            }
            if (best != null)
            {
                cameraToUse = best;
                Debug.Log($"Auto-selected cameraToUse: {cameraToUse.name} (depth={cameraToUse.depth})");
            }
            else
            {
                Debug.Log("No enabled cameras found; cameraToUse remains null and will fall back to Camera.main at interact time.");
            }
        }
    }

    private void Update()
    {
#if ENABLE_INPUT_SYSTEM
        if (debugUseKey && UnityEngine.InputSystem.Keyboard.current != null && UnityEngine.InputSystem.Keyboard.current.eKey.wasPressedThisFrame)
        {
            Debug.Log("Debug key E pressed (Input System) - calling HandleInteract()");
            HandleInteract();
        }
#else
        if (debugUseKey && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Debug key E pressed (Legacy Input) - calling HandleInteract()");
            HandleInteract();
        }
#endif
    }


}