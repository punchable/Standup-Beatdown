using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
[System.Serializable]

public class Master : MonoBehaviour
{
    public static Master Instance;

    public string fighterP1 = "none";
    public string fighterP2 = "none";

    public string aiFighter = "none";

    private int wins;
    private int losses;
    private int ties;
    private int numOfPlayers;

    private string controlStateP1 = "keyboard";
    private string controlStateP2 = "keyboard";
    private string gameState = "none";
    private string gameMode = "none";

    public string FighterSel1 { get => fighterP1; set => fighterP1 = value; }
    public string FighterSel2 { get => fighterP2; set => fighterP2 = value; }

    public int Wins { get => wins; set => wins = value; }
    public int Losses { get => losses; set => losses = value; }
    public int Ties { get => ties; set => ties = value; }

    public string GameState { get => gameState; set => gameState = value; }
    public int NumOfPlayers { get => numOfPlayers; set => numOfPlayers = value; }
    public string ControlState { get => controlStateP1; set => controlStateP1 = value; }
    public string ControlStateP2 { get => controlStateP2; set => controlStateP2 = value; }
    public string GameMode { get => gameMode; set => gameMode = value; }

    void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GoToScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public string GetScene()
    {
        return SceneManager.GetActiveScene().name;
    }

    public void LoadLevel()
    {
        SceneManager.LoadSceneAsync("MainMenu", LoadSceneMode.Single);
    }
}

// GameStates - "preMenu", "preControls", "controlsConfig", "campaignMap", "loading", 
// GameStates - "FighterSel", "starting"

// GameModes - "AI", "Local"