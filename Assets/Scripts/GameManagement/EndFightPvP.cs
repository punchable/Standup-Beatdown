using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndFightPvP : MonoBehaviour
{
    public GameObject fighter1;
    public GameObject fighter2;

    private Animator fighter1anim;
    private Animator fighter2anim;

    public Text winLoseTextP1;
    public Text winLoseTextP2;

    [SerializeField]
    private InputManager input;

    public ButtonRef[] menuOptions;

    private int activeElement1;
    private int activeElement2;

    private float timerDelay1;
    private float timerDelay2;
    private float inputDelay = 0.15f;

    void Start()
    {
        fighter1anim = fighter1.GetComponent<Animator>();
        fighter2anim = fighter2.GetComponent<Animator>();

        fighter1anim.runtimeAnimatorController = Resources.Load("Animators/" + Master.Instance.fighterP1) as RuntimeAnimatorController;
        fighter2anim.runtimeAnimatorController = Resources.Load("Animators/" + Master.Instance.fighterP2) as RuntimeAnimatorController;

        if (Master.Instance.player1win)
        {
            fighter1anim.SetBool("victory", true);
            fighter2anim.SetBool("death", true);
            winLoseTextP1.text = "WINNER";
            winLoseTextP2.text = "LOSER";
        }
        else
        {
            fighter2anim.SetBool("victory", true);
            fighter1anim.SetBool("death", true);
            winLoseTextP1.text = "LOSER";
            winLoseTextP2.text = "WINNER";
        }
    }


    void Update()
    {
        if (timerDelay1 <= 0)
        {
            switch (Master.Instance.ControlState)
            {
                case "controller":
                    if (input.vertP1Joy < 0)
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

                    if (input.vertP1Joy > 0)
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
                    if (input.vertP1KB < 0)
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

                    if (input.vertP1KB > 0)
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

    public void EndGame()
    {
        Master.Instance.gameState = "preMenu";
        Master.Instance.GoToScene("PreMenu");
    }

    public void ReMatch()
    {

    }
}
