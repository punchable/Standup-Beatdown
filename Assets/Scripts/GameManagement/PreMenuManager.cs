using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
[System.Serializable]

public class PreMenuManager : MonoBehaviour
{
    [SerializeField]
    public Master master;
    [SerializeField]
    public InputManager input;

    private float timer;
    private float inputDelay = 0.15f;

    private int activeElement1 = 0;
    public ButtonRef[] menuOptions;
    public ButtonRef[] controlOptions;
    public ButtonRef[] preControls;
    public ButtonRef[] campaignMap;

    [SerializeField]
    private GameObject controlsPanel;
    [SerializeField]
    private GameObject preMenuPanel;
    [SerializeField]
    private GameObject preControlsPanel;
    [SerializeField]
    private GameObject campaignPanel;

    [SerializeField]
    private GameObject controlsTextP1;
    [SerializeField]
    private GameObject controlsTextP2;

    [SerializeField]
    private Text p1Movement;
    [SerializeField]
    private Text p1Attacks;
    [SerializeField]
    private Text p2Movement;
    [SerializeField]
    private Text p2Attacks;


    private void Start()
    {
        master = FindObjectOfType<Master>();
        master.GameState = "preControls";
    }
   
    private void Update()
    {
        

        if (timer <= 0)
        {
            switch (master.GameState)
            {
                case "preControls":
                    preControls[activeElement1].selectedP1 = true;
                    preControls[activeElement1].selectedP2 = false;
                    if (input.horizP1KB > 0 || input.horizP1Joy < 0 || input.horizP2KB > 0)
                    {
                        preControls[activeElement1].selectedP1 = false;
                        if (activeElement1 > 0)
                        {
                            activeElement1--;
                        }
                        else
                        {
                            activeElement1 = preControls.Length - 1;
                        }
                    }

                    if (input.horizP1KB < 0 || input.horizP1Joy > 0 || input.horizP2KB < 0)
                    {
                        preControls[activeElement1].selectedP1 = false;
                        if (activeElement1 < preControls.Length - 1)
                        {
                            activeElement1++;
                        }
                        else
                        {
                            activeElement1 = 0;
                        }
                    }
                    timer = inputDelay;
                    break;

                case "preMenu":
                    menuOptions[activeElement1].selectedP1 = true;
                    menuOptions[activeElement1].selectedP2 = false;
                    if (input.vertP1KB > 0 || input.vertP1Joy < 0 || input.vertP2KB > 0)
                    {
                        menuOptions[activeElement1].selectedP1 = false;
                        if (activeElement1 > 0)
                        {
                            activeElement1--;
                        }
                        else
                        {
                            activeElement1 = menuOptions.Length - 1;
                        }
                    }

                    if (input.vertP1KB < 0 || input.vertP1Joy > 0 || input.vertP2KB < 0)
                    {
                        menuOptions[activeElement1].selectedP1 = false;
                        if (activeElement1 < menuOptions.Length - 1)
                        {
                            activeElement1++;
                        }
                        else
                        {
                            activeElement1 = 0;
                        }
                    }
                    timer = inputDelay;
                    break;

                case "controlsConfig":
                    controlOptions[activeElement1].selectedP1 = true;
                    controlOptions[activeElement1].selectedP2 = false;
                    if (input.horizP1KB < 0 || input.horizP1Joy < 0)
                    {
                        controlOptions[activeElement1].selectedP1 = false;
                        if (activeElement1 > 0)
                        {
                            activeElement1--;
                        }
                        else
                        {
                            activeElement1 = controlOptions.Length - 1;
                        }
                    }

                    if (input.horizP1KB > 0 || input.horizP1Joy > 0)
                    {
                        controlOptions[activeElement1].selectedP1 = false;
                        if (activeElement1 < controlOptions.Length - 1)
                        {
                            activeElement1++;
                        }
                        else
                        {
                            activeElement1 = 0;
                        }
                    }
                    timer = inputDelay;
                    break;

                case "campaignMap":
                    campaignMap[activeElement1].selectedP1 = true;
                    campaignMap[activeElement1].selectedP2 = false;
                    if (input.horizP1KB < 0 || input.horizP1Joy < 0 || input.horizP2KB < 0)
                    {
                        campaignMap[activeElement1].selectedP1 = false;
                        if (activeElement1 > 0)
                        {
                            activeElement1--;
                        }
                        else
                        {
                            activeElement1 = campaignMap.Length - 1;
                        }
                    }

                    if (input.horizP1KB > 0 || input.horizP1Joy > 0 || input.horizP2KB > 0)
                    {
                        campaignMap[activeElement1].selectedP1 = false;
                        if (activeElement1 < campaignMap.Length - 1)
                        {
                            activeElement1++;
                        }
                        else
                        {
                            activeElement1 = 0;
                        }
                    }
                    timer = inputDelay;
                    break;
            }
        }

        if (Input.GetButtonDown("P1KBSelect") || Input.GetButtonDown("JumpJoyP1"))
        {
            if (master.GameState == "preMenu")
            {
                HandleSelectedOption();
            }
            else if (master.GameState == "controlsConfig")
            {
                HandleControlOptions();
            }
            else if (master.GameState == "preControls")
            {
                switch (activeElement1)
                {
                    case 0:
                        master.ControlState = "controller";
                        master.ControlStateP2 = "controller";
                        SetControlsText(0);
                        SetControlsText(1);
                        master.GameState = "preMenu";
                        break;
                    case 1:
                        master.ControlState = "keyboard";
                        master.ControlStateP2 = "keyboard";
                        SetControlsText(0);
                        SetControlsText(1);
                        master.GameState = "preMenu";
                        break;
                }
            }
            else if (master.GameState == "campaignMap")
            {
                HandleAILevelSel(activeElement1);
            }
        }

        if (Input.GetButtonDown("P1KBCancel"))
        {
            master.GameState = "preMenu";
            menuOptions[2].selectedP1 = false;
            activeElement1 = 0;
        }

        if (master.GameState == "preMenu")
        {
            controlsPanel.SetActive(false);
            preControlsPanel.SetActive(false);
            preMenuPanel.SetActive(true);
        }
        else if (master.GameState == "controlsConfig")
        {
            controlsTextP1.GetComponent<Text>().text = "Current Device: " + master.ControlState;
            controlsTextP2.GetComponent<Text>().text = "Current Device: " + master.ControlStateP2;
            controlsPanel.SetActive(true);
            preControlsPanel.SetActive(false);
            preMenuPanel.SetActive(false);
        }
        else if (master.GameState == "preControls")
        {
            preControlsPanel.SetActive(true);
            preMenuPanel.SetActive(false);
            controlsPanel.SetActive(false);
        }

        void HandleSelectedOption()
        {
            switch (activeElement1)
            {
                case 0:
                    master.NumOfPlayers = 1;
                    master.GameMode = "AI";
                    LoadMenu();
                    break;
                case 1:
                    master.NumOfPlayers = 2;
                    master.GameMode = "Local";
                    LoadMenu();
                    break;
                case 2:
                    master.GameState = "controlsConfig";
                    activeElement1 = 0;
                    break;
            }
        }

        void HandleControlOptions()
        {
            switch (activeElement1)
            {
                case 0:
                    if (master.ControlState == "keyboard")
                    {
                        master.ControlState = "controller";
                    }
                    else if (master.ControlState == "controller")
                    {
                        master.ControlState = "keyboard";
                    }
                    SetControlsText(0);
                    break;
                case 1:
                    if (master.ControlStateP2 == "keyboard")
                    {
                        master.ControlStateP2 = "controller";
                    }
                    else if (master.ControlStateP2 == "controller")
                    {
                        master.ControlStateP2 = "keyboard";
                    }
                    SetControlsText(1);
                    break;
                case 2:
                    master.GameState = "preMenu";
                    break;
            }
        }

        void HandleAILevelSel(int level)
        {
            master.aiFighter = campaignMap[level].gameObject.name;
            campaignPanel.SetActive(false);
            master.GoToScene("MainMenu");
        }

        void LoadMenu()
        {
            master.GameState = "loading";
            if (master.GameMode == "AI")
            {
                campaignPanel.SetActive(true);
                master.GameState = "campaignMap";
            }
            else if (master.GameMode == "Local")
            {
                master.GoToScene("MainMenu");
            }

        }

        timer -= Time.deltaTime;
    }

    public void VsAI()
    {
        master.GameState = "loading";
        master.GameMode = "AI";
        master.NumOfPlayers = 1;
        master.GoToScene("MainMenu");
    }

    public void VsLocal()
    {
        master.GameState = "loading";
        master.GameMode = "Local";
        master.NumOfPlayers = 2;
        master.GoToScene("MainMenu");
    }

    public void OpenControls()
    {
        master.GameState = "controlsConfig";
    }

    public void CloseControls()
    {
        master.GameState = "preMenu";
    }

    public void ToggleControls(int player)
    {
        switch (player)
        {
            case 0:
                if (master.ControlState == "keyboard")
                {
                    master.ControlState = "controller";
                }
                else if (master.ControlState == "controller")
                {
                    master.ControlState = "keyboard";
                }
                break;
            case 1:
                if (master.ControlStateP2 == "keyboard")
                {
                    master.ControlStateP2 = "controller";
                }
                else if (master.ControlStateP2 == "controller")
                {
                    master.ControlStateP2 = "keyboard";
                }
                break;
        }
        SetControlsText(player);
    }

    public void PreControls(int ctrl)
    {
        switch (ctrl)
        {
            case 0:
                master.ControlState = "controller";
                master.ControlStateP2 = "controller";
                break;
            case 1:
                master.ControlState = "keyboard";
                master.ControlStateP2 = "keyboard";
                break;
        }
        master.GameState = "preMenu";
        SetControlsText(0);
        SetControlsText(1);
    }

    public void SetControlsText(int ID)
    {
        switch (ID)
        {
            case 0:
                if(master.ControlState == "keyboard")
                {
                    p1Movement.text = "Left/Right:  [A]/[S]" + "\r\n" + "Jump:  [W]";
                    p1Attacks.text = "Block: [spacebar]" + "\r\n" + "Medium Kick: [C]" + "\r\n" + "Medium Punch: [V]";
                }
                else
                {
                    p1Movement.text = "Left/Right:  Left Joystick" + "\r\n" + "Jump:  Up on Left Joystick";
                    p1Attacks.text = "Block: [Y]" + "\r\n" + "Medium Kick: [A]" + "\r\n" + "Medium Punch: [B]";
                }
                break;
            case 1:
                if (master.ControlState == "keyboard")
                {
                    p2Movement.text = "Left/Right:  [left/right arrows]" + "\r\n" + "Jump:  [up arrow]";
                    p2Attacks.text = "Block: [return]" + "\r\n" + "Medium Kick: [P]" + "\r\n" + "Medium Punch: [K]";
                }
                else
                {
                    p2Movement.text = "Left/Right:  Left Joystick" + "\r\n" + "Jump:  Up on Left Joystick";
                    p2Attacks.text = "Block: [Y]" + "\r\n" + "Medium Kick: [A]" + "\r\n" + "Medium Punch: [B]";
                }
                break;
        }
    }
}
