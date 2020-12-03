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

    public string controllerP1;
    public string controllerP2;

    public KeyCode P1KBJump = KeyCode.Space;
    public KeyCode P1JoyJump = KeyCode.Joystick1Button0;

    public KeyCode P2KBJump = KeyCode.Return;
    public KeyCode P2JoyJump = KeyCode.Joystick2Button0;

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
}
