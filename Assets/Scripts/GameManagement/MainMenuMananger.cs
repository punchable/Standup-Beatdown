using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]

public class MainMenuMananger : MonoBehaviour
{
    public Master master;

    [SerializeField]
    private UiManager manager;
    [SerializeField]
    private InputManager input;

    private int activeElement1 = 0;
    private int activeElement2 = 0;
    public ButtonRef[] menuOptions;

    private float timerDelay1;
    private float timerDelay2;
    private float inputDelay = 0.15f;

    [SerializeField]
    private GameObject p1Preview;
    [SerializeField]
    private GameObject p2Preview;

    private Animator p1PrevAnim;
    private Animator p2PrevAnim;

    public bool lockedIn1;
    public bool lockedIn2;

    public float timer1 = 0;
    public float timer2 = 0;

    public void Awake()
    {
        p1PrevAnim = p1Preview.GetComponent<Animator>();
        p2PrevAnim = p2Preview.GetComponent<Animator>();

        master = FindObjectOfType<Master>();
        master.GameState = "FighterSel";

        lockedIn1 = false;
        lockedIn2 = false;
    }

    public void Start()
    {

        if (master.GameMode == "AI")
        {
            GameObject aiFighter = GameObject.Find(master.aiFighter);
            aiFighter.GetComponent<ButtonRef>().selectedP2 = true;

            master.fighterP2 = master.aiFighter;
        }
    }

    public void Update()
    {
        menuOptions[activeElement1].selectedP1 = true;

        p1PrevAnim.runtimeAnimatorController = Resources.Load("Animators/" + menuOptions[activeElement1].name) as RuntimeAnimatorController;
        p2PrevAnim.runtimeAnimatorController = Resources.Load("Animators/" + menuOptions[activeElement2].name) as RuntimeAnimatorController;

        if (!lockedIn1)
        {
            if (timerDelay1 <= 0)
            {
                switch (Master.Instance.ControlState)
                {
                    case "controller":
                        if (input.horizP1Joy < 0)
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

                        if (input.horizP1Joy > 0)
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

                        timerDelay1 = inputDelay;
                        break;

                    case "keyboard":
                        if (input.horizP1KB < 0)
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

                        if (input.horizP1KB > 0)
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

                        timerDelay1 = inputDelay;
                        break;
                }
            }
        }

        if (Master.Instance.GameMode == "Local")
        {
            menuOptions[activeElement2].selectedP2 = true;

            if (!lockedIn2)
            {
                if (timerDelay2 <= 0)
                {
                    switch (Master.Instance.ControlStateP2)
                    {
                        case "controller":
                            if (input.horizP2Joy < 0)
                            {
                                menuOptions[activeElement2].selectedP2 = false;
                                if (activeElement2 > 0)
                                {
                                    activeElement2--;
                                }
                                else
                                {
                                    activeElement2 = menuOptions.Length - 1;
                                }
                            }

                            if (input.horizP2Joy > 0)
                            {
                                menuOptions[activeElement2].selectedP2 = false;
                                if (activeElement2 < menuOptions.Length - 1)
                                {
                                    activeElement2++;
                                }
                                else
                                {
                                    activeElement2 = 0;
                                }
                            }
                            timerDelay2 = inputDelay;
                            break;

                        case "keyboard":
                            if (input.horizP2KB < 0)
                            {
                                menuOptions[activeElement2].selectedP2 = false;
                                if (activeElement2 > 0)
                                {
                                    activeElement2--;
                                }
                                else
                                {
                                    activeElement2 = menuOptions.Length - 1;
                                }
                            }

                            if (input.horizP2KB > 0)
                            {
                                menuOptions[activeElement2].selectedP2 = false;
                                if (activeElement2 < menuOptions.Length - 1)
                                {
                                    activeElement2++;
                                }
                                else
                                {
                                    activeElement2 = 0;
                                }
                            }
                            timerDelay2 = inputDelay;
                            break;
                    }
                }
            }
        }

        timerDelay1 -= Time.deltaTime;
        timerDelay2 -= Time.deltaTime;

        if (Input.GetKeyUp(KeyCode.Return))
        {
            HandleSelectedOption(1);
            timer1 = 3.0f;
        }

        if (Input.GetButtonDown("P1KBSelect") || Input.GetKeyUp(KeyCode.Joystick1Button0))
        {
            HandleSelectedOption(0);
            timer1 = 3.0f;
        }

        if (timer1 < 0 && lockedIn1 && !lockedIn2)
        {
            timer1 = 0;
            SetFighterReady(1);
            Master.Instance.FighterSel2 = SetRandomFighter();
            master.GameState = "locked";
            timer2 = 2.0f;
        }
        else if (timer1 < 0 && lockedIn2 && !lockedIn1)
        {
            timer1 = 0;
            SetFighterReady(0);
            Master.Instance.FighterSel1.Equals(SetRandomFighter());
            master.GameState = "locked";
            timer2 = 2.0f;
        }
        else if (timer1 < 0 && lockedIn1 && lockedIn2)
        {
            master.GameState = "loading";
            timer1 = 0;
            timer2 = 1.0f;
        }

        
        if (master.GameState == "locked")
        {
            master.GameState = "loading";
            timer1 = 0;
            timer2 = 1.0f;
        }

        if (timer1 > 0)
        {
            timer1 -= Time.deltaTime;
        }

        if (timer2 > 0)
        {
            timer2 -= Time.deltaTime;
        }
        else if (timer2 < 0)
        {
            timer2 = 0;
            LoadLevel();
        }


        void HandleSelectedOption(int player)
        {
            switch (player)
            {
                case 0:
                    
                    SetFighterReady(0);
                    break;
                case 1:

                    SetFighterReady(1);
                    break;
            }
        }

        void LoadLevel()
        {
            master.GameState = "starting";
            master.GoToScene("FightStage");
        }
    }

    public string SetRandomFighter()
    {
        int fighter;
        string fighterName = "none";

        fighter = Random.Range(0, 5);

        if (fighter == 0)
        {
            fighterName = "BobBig";
        }
        else if (fighter == 1)
        {
            fighterName = "MuSh";
        }
        else if (fighter == 2)
        {
            fighterName = "TomDan";
        }
        else if (fighter == 3)
        {
            fighterName = "BertBelly";
        }
        else if (fighter == 4)
        {
            fighterName = "AndrewBaller";
        }


        if (fighterName != "none")
        {
            return fighterName;
        }
        else
        {
            return null;
        }
    }

    public void SetFighterReady(int player)
    {

        if (player == 0)
        {
            lockedIn1 = true;
            p1PrevAnim.SetBool("victory", true);
            Master.Instance.FighterSel1 = menuOptions[activeElement1].name;
        }
        else
        {
            lockedIn2 = true;
            p2PrevAnim.SetBool("victory", true);
            Master.Instance.FighterSel2 = menuOptions[activeElement2].name;
        }
    }
}
