//* Author: Lee wei jun
//* Date: 14/6/2026
//* Description: The UI ManagerScript that is responsible for the visibility of the in game UI and manages updating the value of varius variables
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManagerScript : MonoBehaviour
{
    public CheckPoint checkPoint;
    //the text, buttons, game objects in the UI
    public TMP_Text ScoreText;
    public TMP_Text HealthText;
    public TMP_Text GameOver;
    public Button StartButton;
    public Button RespawnButton;
    public TMP_Text ItemText;
    public TMP_Text CoinsText;
    public TMP_Text StartText;
    public TMP_Text TimerText;
    public TMP_Text AlertText;

    public GameObject Injector;
    public GameObject KeyCard;
    public GameObject JointPlug;
    public GameObject MenuPanel;
    public GameObject MainUI;
    public GameObject AlertPanel;
    public GameObject StartPanel;
    public GameObject OptionsPage;
    public GameObject CreditPage;
    public GameObject HowToPlayPage;
    public GameObject Crosshair;
    public GameObject Items;
    public GameObject SpaceSuit;
    public GameObject MiniJet;
    public GameObject ToolBox;
    private bool isGameOver = false;
    bool menuOpen = false;
    
    

        public float elapsedTime = 0f; //time spent while the game is running
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //setting all the UI value to thier devualt value and text
        ScoreText.text = "Score: 0";
        HealthText.text = "Health: 100";
        ItemText.text = "Items Collected: 0";
        CoinsText.text = "Coins Collected: 0";
        GameOver.text = "Game";
        StartText.text = "Start";
        TimerText.text = "Time: 0";
        AlertText.text = "Alert";
        //hiding the panels and buttons
        StartButton.gameObject.SetActive(false);
        MainUI.SetActive(false);
        Injector.SetActive(false);
        KeyCard.SetActive(false);
        JointPlug.SetActive(false);
        AlertPanel.SetActive(false);
        RespawnButton.gameObject.SetActive(false);
        MenuPanel.SetActive(false);
        StartPanel.SetActive(true);
        OptionsPage.SetActive(false);
        CreditPage.SetActive(false);
        HowToPlayPage.SetActive(false);
        Crosshair.SetActive(false);
        Items.SetActive(false);
        SpaceSuit.SetActive(false);
        MiniJet.SetActive(false);
        ToolBox.SetActive(false);

        // evaluate the actual menu state after activating StartPanel
        menuOpen = MenuPanel.activeSelf || StartPanel.activeSelf;

        // when a menu panel is visible, show the cursor and pause the game
        if (menuOpen)
        {
            Time.timeScale = 0f;  // Pause everything
        }
        else
        {
            Time.timeScale = 1f;  // Resume normal speed
        }

        Cursor.visible = menuOpen;
        Cursor.lockState = menuOpen ? CursorLockMode.None : CursorLockMode.Locked;
    }

    void Update()
    {
        if (Time.timeScale > 0)  // Only update if game is running
        {
            elapsedTime += Time.deltaTime;
        }
        TimerText.text = $"Time: {Mathf.FloorToInt(elapsedTime)}s"; //update the UI text every frame to keep track of time elasped
    }
    // Update is called once per frame
    public void UpdateScore(int score)
    {
        ScoreText.text = $"Score: {score}"; //update scoretext with current score
    }

    public void UpdateHealth(int health) //update healthtext with current health
    {
        HealthText.text = $"Health: {health}";
        if (health <= 0 && !isGameOver)
            {   
                isGameOver = true;
                HealthText.text = $"Health: 0";
                print("Game Over");
                Gameover();
            }
    }

    public void ItemCollected(int collected) //toggle panel, congradulates when collected items reach 10 or more
    {
        ItemText.text = $"Items Collected: {collected}/10";
        if (collected >= 10)
        {
            print("You have collected all the items!");
            AlertText.text = "You Have collected all the Joint Plugs, On the Generator";
            TogglePanel();
        }
    }

    public void KeyCardPanel() //toggle panel, Tell player they need a key card
    {

        print("You Need a Key card");
        AlertText.text = "You Need a Key Card";
        StartText.text = "Resume";
        ToggleAlert();
    }

    public void GeneratorPanel() //toggle panel, Tell player they need a Joint plug
    {

        print("you need a joint plug");
        AlertText.text = "You Need a joint Plug";
        ShowAlert();
    }

    public void GeneratorDoorPanel() //toggle panel, Tell player they need to on the generator
    {

        print("you need to on the Generator");
        AlertText.text = "You Need to On the Generator";
        ShowAlert();
    }

    public void CoinsCollected(int coinsCollected) //toggle panel, congradulates player when collected coins reach 10 or more
    {
        CoinsText.text = $"Coins Collected: {coinsCollected}/10";
        if (coinsCollected >= 10)
        {
            print("You have collected all the coins!");
            AlertText.text = "You have collected all the coins!";
            ShowAlert();
        }
    }

    public void TogglePanel() //set menupanel and mainUI visible, lock cursor and pause
    {
        MenuPanel.SetActive(!MenuPanel.activeSelf);
        print($"MenuPanel is now: {MenuPanel.activeSelf}");

        MainUI.SetActive(!MainUI.activeSelf);
        print($"MainUI is now: {MainUI.activeSelf}");

        Crosshair.SetActive(!MenuPanel.activeSelf);
        Items.SetActive(!MenuPanel.activeSelf);
        
        RespawnButton.gameObject.SetActive(false);
        GameOver.text = "Game";
        StartButton.gameObject.SetActive(true);
        
        // Pause/unpause the game
        if (MenuPanel.activeSelf)
        {
            Time.timeScale = 0f;  // Pause everything
        }
        else
        {
            Time.timeScale = 1f;  // Resume normal speed
        }
        

        Cursor.visible = MenuPanel.activeSelf;
        Cursor.lockState = MenuPanel.activeSelf ? CursorLockMode.None : CursorLockMode.Locked;
    }

    public void ToggleAlert() //toggle Alert panel visibility, lock cursor and pause
    {
        bool willShow = !AlertPanel.activeSelf;
        AlertPanel.SetActive(willShow);
        print($"AlertPanel is now: {AlertPanel.activeSelf}");
        StartButton.gameObject.SetActive(true);
        if (willShow)
        {
            MenuPanel.SetActive(false);
            MainUI.SetActive(false);
            Time.timeScale = 0f;  // Pause everything
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Time.timeScale = 1f;  // Resume normal speed
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            MainUI.SetActive(true);
        }
    }

    public void ShowAlert()
    {
        MenuPanel.SetActive(false);
        MainUI.SetActive(false);
        AlertPanel.SetActive(true);
        Crosshair.SetActive(false);
        Items.SetActive(!AlertPanel.activeSelf);
        print($"AlertPanel is now: {AlertPanel.activeSelf}");
        StartButton.gameObject.SetActive(true);
        Time.timeScale = 0f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void HideAlert()
    {
        AlertPanel.SetActive(false);
        print($"AlertPanel is now: {AlertPanel.activeSelf}");
        Crosshair.SetActive(true);
        Time.timeScale = 1f;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }


    public void Gameover() //toggle game over screen
    {
        RespawnButton.gameObject.SetActive(true);
        GameOver.text = "Game Over"; //change big text to say "game over"
        StartButton.gameObject.SetActive(false); //remove start button so players cant resume

        //toggle UI
        MenuPanel.SetActive(true);
        MainUI.SetActive(false);

        Time.timeScale = 0f;
        //make cursor visible and unlock cursor
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void Restart() //restart button on click restarts the scene
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OnStartButtonClick() //start button on click
    {
        print("Start Button Clicked");
        StartText.text = "Resume"; //change "start" to "resume"
        TogglePanel(); //closes panel
        HideAlert();
    }
    public void InjectorCollected() //make Icon for injector visible
    {
        print("Injector Obtained");
        Injector.SetActive(true);
    }
    public void KeyCardCollected() //make icon for key card visible
    {
        print("Key Card Obtained");
        KeyCard.SetActive(true);
    }
    
    public void JointPlugCollected() //make icon for joint plug visible
    {
        print("Joint Plug Obtained");
        JointPlug.SetActive(true);
    }
    public void InjectorUsed() //make icon for injector dissapear
    {
        print("Injector Used");
        Injector.SetActive(false);
    }
    public void JointPlugUsed() //make icon for JointPlug dissapear
    {
        print("Joint Plug Used");
        JointPlug.SetActive(false);
    }
    public void KeyCardUsed() //make icon for key card not visible
    {
        print("Key Card Used");
        KeyCard.SetActive(false);
    }

    public void SpaceSuitUsed() //make icon for space suit not visible
    {
        print("Space Suit Used");
        SpaceSuit.SetActive(false);
    }
    public void MiniJetUsed() //make icon for mini jet not visible
    {
        print("Mini Jet Used");
        MiniJet.SetActive(false);
    }
    public void ToolBoxUsed() //make icon for tool box not visible
    {
        print("Tool Box Used");
        ToolBox.SetActive(false);
    }
    public void SpaceSuitCollected() //make icon for space suit visible
    {
        print("Space Suit Obtained");
        SpaceSuit.SetActive(true);
    }
    public void MiniJetCollected() //make icon for mini jet visible
    {
        print("Mini Jet Obtained");
        MiniJet.SetActive(true);
    }
    public void ToolBoxCollected() //make icon for tool box visible
    {
        print("Tool Box Obtained");
        ToolBox.SetActive(true);
    }

    public void OnRespawnButton() //for when respawn button is pressed, load Saved progress if any by changing the UI and item's setactive
    {
        print("save loaded");
        checkPoint.LoadProgress(); //load Saved progress
        StartPanel.SetActive(false);//hide startpanel
        // Close game over menu
        MenuPanel.SetActive(false);//hide Menu Panel
        // Show gameplay UI again
        MainUI.SetActive(true);//show main UI
        // Hide respawn button
        RespawnButton.gameObject.SetActive(false);//hide Respawn button
        // Reset game over state
        isGameOver = false; //reset player death state
        GameOver.text = "Game"; //Game Main title
        // Resume game
        Time.timeScale = 1f;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    public void OnStartButton2()
    {
        print("Start Button 2 Clicked");
        StartButton.gameObject.SetActive(true);//unhide start button
        StartPanel.SetActive(false);//hide start panel
        MainUI.SetActive(true);//show main UI
        TogglePanel(); //opens panel
        // Only hide alert (and change cursor) if the alert panel is actually shown
        if (AlertPanel.activeSelf)
        {
            HideAlert(); //Hide Alert Panel
        }
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        Debug.Log($"Cursor Visible: {Cursor.visible}");
        Debug.Log($"Cursor Lock State: {Cursor.lockState}");
    }
    public void OnOptionsClick()
    {
        OptionsPage.SetActive(true); //hide Options Page
        StartPanel.SetActive(false); //Hide start panel
        Time.timeScale = 0f;  // Pause everything
    }
    public void OnCreditClick()
    {
        CreditPage.SetActive(true); // show credits page
        StartPanel.SetActive(false); // hide start panel
        Time.timeScale = 0f;  // Pause everything
    }
    public void OnHowToPlayClick()
    {
        HowToPlayPage.SetActive(true); //show how to play page
        StartPanel.SetActive(false); //hide start panel
        Time.timeScale = 0f;  // Pause everything
    }
    public void OnBackToStartPanel()
    {
        StartPanel.SetActive(true); //show start panel
        HowToPlayPage.SetActive(false); //hide how to play page
        CreditPage.SetActive(false); //hide credit page
        OptionsPage.SetActive(false);// options page
        MenuPanel.SetActive(false);//hide menu panel
    }
    public void OnQuit()
    {
        print("Quit");
        Application.Quit(); //quit
    }
    public void OnClose()
    {
        HideAlert(); //hide alert panel
    }
}
