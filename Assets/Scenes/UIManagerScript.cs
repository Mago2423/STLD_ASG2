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
    private bool isGameOver = false;

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
        StartButton.gameObject.SetActive(true);
        MainUI.SetActive(false);
        Injector.SetActive(false);
        KeyCard.SetActive(false);
        JointPlug.SetActive(false);
        AlertPanel.SetActive(false);
        RespawnButton.gameObject.SetActive(false);
        MenuPanel.gameObject.SetActive(false);

        if (MenuPanel.activeSelf)
        {
            Time.timeScale = 0f;  // Pause everything
        }
        else
        {
            Time.timeScale = 1f;  // Resume normal speed
        }
        //when menuepanel is visible, the cursor is visible and if menupanel is not visible, the cursor is locked
        Cursor.visible = MenuPanel.activeSelf;
        Cursor.lockState = MenuPanel.activeSelf ? CursorLockMode.None : CursorLockMode.Locked;
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
        ToggleAlert();
    }

    public void GeneratorDoorPanel() //toggle panel, Tell player they need to on the generator
    {

        print("you need to on the Generator");
        AlertText.text = "You Need to On the Generator";
        ToggleAlert();
    }

    public void CoinsCollected(int coinsCollected) //toggle panel, congradulates player when collected coins reach 10 or more
    {
        CoinsText.text = $"Coins Collected: {coinsCollected}/10";
        if (coinsCollected >= 10)
        {
            print("You have collected all the coins!");
            AlertText.text = "You have collected all the coins!";
            ToggleAlert();
        }
    }

    public void TogglePanel() //set menupanel and mainUI visible, lock cursor and pause
    {
        MenuPanel.SetActive(!MenuPanel.activeSelf);
        print($"MenuPanel is now: {MenuPanel.activeSelf}");

        MainUI.SetActive(!MainUI.activeSelf);
        print($"MainUI is now: {MainUI.activeSelf}");

        RespawnButton.gameObject.SetActive(false);
        GameOver.text = "Game";
        
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

    public void ToggleAlert() //set Alert panel and start button visible, lock cursor and pause
    {
        AlertPanel.SetActive(!AlertPanel.activeSelf);
        print($"AlertPanel is now: {AlertPanel.activeSelf}");
        StartButton.gameObject.SetActive(true);
        // Pause/unpause the game
        if (AlertPanel.activeSelf)
        {
            Time.timeScale = 0f;  // Pause everything
        }
        else
        {
            Time.timeScale = 1f;  // Resume normal speed
        }
        
        Cursor.visible = AlertPanel.activeSelf;
        Cursor.lockState = AlertPanel.activeSelf ? CursorLockMode.None : CursorLockMode.Locked;
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
        AlertPanel.SetActive(false);
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

    public void OnRespawnButton()
    {
        print("save loaded");
        checkPoint.LoadProgress();

        // Close game over menu
        MenuPanel.SetActive(false);

        // Show gameplay UI again
        MainUI.SetActive(true);

        // Hide respawn button
        RespawnButton.gameObject.SetActive(false);

        // Reset game over state
        isGameOver = false;
        GameOver.text = "Game";

        // Resume game
        Time.timeScale = 1f;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    public void OnStartButton2()
    {
        print("Start Button 2 Clicked");
        StartText.text = "Resume"; //change "start" to "resume"
        StartPanel.SetActive(false);
        MainUI.SetActive(true);
        TogglePanel(); //opens panel
        AlertPanel.SetActive(false);
    }
}
