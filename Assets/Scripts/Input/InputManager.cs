using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private Master master;

    public float vertP1Joy;
    public float horizP1Joy;
    public float vertP1KB;
    public float horizP1KB;

    public float vertP2Joy;
    public float horizP2Joy;
    public float vertP2KB;
    public float horizP2KB;

    public KeyCode P1KBJump = KeyCode.W;
    public KeyCode P2KBJump = KeyCode.UpArrow;

    public KeyCode P1KBBlock = KeyCode.Space;
    public KeyCode P1KBMedKick = KeyCode.C;
    public KeyCode P1KBMedPunch = KeyCode.V;
    public KeyCode P1JoyBlock = KeyCode.Joystick1Button2;
    public KeyCode P1JoyMedKick = KeyCode.Joystick1Button0;
    public KeyCode P1JoyMedPunch = KeyCode.Joystick1Button1;
    public KeyCode P1JoyPause = KeyCode.Joystick1Button7;

    public KeyCode P2KBBlock = KeyCode.Return;
    public KeyCode P2KBMedKick = KeyCode.K;
    public KeyCode P2KBMedPunch = KeyCode.P;
    public KeyCode P2JoyMedKick = KeyCode.Joystick2Button0;
    public KeyCode P2JoyBlock = KeyCode.Joystick2Button2;
    public KeyCode P2JoyMedPunch = KeyCode.Joystick2Button1;
    public KeyCode P2JoyPause = KeyCode.Joystick2Button7;

    void Start()
    {
        master = FindObjectOfType<Master>();
    }

    void Update()
    {
        if (master.GetScene().Equals("PreMenu"))
        {
            UpdateP1KBcontrols();
            UpdateP2KBcontrols();
            UpdateP1JoyControls();
            UpdateP2JoyControls();
        }
        else
        {
            if (Master.Instance.ControlState == "keyboard")
            {
                UpdateP1KBcontrols();
            }
            else if (Master.Instance.ControlState == "controller")
            {
                UpdateP1JoyControls();
            }

            if (Master.Instance.ControlStateP2 == "keyboard")
            {
                UpdateP2KBcontrols();
            }
            else if (Master.Instance.ControlStateP2 == "controller")
            {
                UpdateP2JoyControls();
            }
        }

        void UpdateP1KBcontrols()
        {
            vertP1KB = Input.GetAxis("VertKBP1");
            horizP1KB = Input.GetAxis("HorizKBP1");
        }

        void UpdateP2KBcontrols()
        {
            vertP2KB = Input.GetAxis("VertKBP2");
            horizP2KB = Input.GetAxis("HorizKBP2");
        }

        void UpdateP1JoyControls()
        {
            vertP1Joy = Input.GetAxis("VertJoyP1");
            horizP1Joy = Input.GetAxis("HorizJoyP1");
        }

        void UpdateP2JoyControls()
        {
            vertP2Joy = Input.GetAxis("VertJoyP2");
            horizP2Joy = Input.GetAxis("HorizJoyP2");
        }
    }

    //starting to config re-configurable controls but got distracted as this isn't critical atm
    public void SetKey(int id, string input, KeyCode key)
    {
        switch (id)
        {
            case 0:
                if (master.ControlState == "controller")
                {
                    switch (input)
                    {
                        case "P1KBJump":
                            P1KBJump = key;
                            break;
                    }
                }
                else
                {

                }
                break;
            case 1:
                if (master.ControlState == "keyboard")
                {

                }
                else
                {

                }
                break;
        }
    }

    public void ShowKeyOptions()
    {

    }
}
