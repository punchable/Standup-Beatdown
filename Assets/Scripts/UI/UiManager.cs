using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[Serializable]

public class UiManager : MonoBehaviour
{
    public static UiManager Instance;

    [SerializeField]
    private Master master;
    [SerializeField]
    private UiComponents comp;

    [SerializeField]
    private UiLog log;
    [SerializeField]
    private InputManager input;

    private int activeElement = 0;
    private float timer;
    private float inputDelay = 0.25f;

    [SerializeField]
    private GameObject p1PortraitObj;
    [SerializeField]
    private GameObject p2PortraitObj;
    [SerializeField]
    private Text p1HealthTxt;
    [SerializeField]
    private Text p2HealthTxt;


    private Image p1Portrait;
    private Image p2Portrait;

    [SerializeField]
    private Sprite MuSh;
    [SerializeField]
    private Sprite BobBig;

    private UiUtilities util;
    public Master Master { get => master; }
    public UiComponents Comp { get => comp; }
    public UiLog Log { get => log; }
    public UiUtilities Util { get => util; }

    public void Awake()
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

        util = new UiUtilities(this);

        log.FighterSel1 = Master.Instance.FighterSel1;
        log.FighterSel2 = Master.Instance.FighterSel2;
    }

    public void Start()
    {

    }

    public void Update()
    {
        if (master.GetScene().Equals("FightStage"))
        {
            if (Master.Instance.GameState == "paused")
            {
                comp.PauseMenu.SetActive(true);
            }
            else
            {
                comp.PauseMenu.SetActive(false);
            }
        }

        if (Master.Instance.GameState == "paused")
        {
            comp.pauseOptions[activeElement].selectedP1 = true;
            if (timer <= 0)
            {
                if (input.vertP1KB > 0 || input.vertP1Joy < 0)
                {
                    comp.pauseOptions[activeElement].selectedP1 = false;
                    if (activeElement > 0)
                    {
                        activeElement--;
                    }
                    else
                    {
                        activeElement = comp.pauseOptions.Length - 1;
                    }
                }

                if (input.vertP1KB < 0 || input.vertP1Joy > 0)
                {
                    comp.pauseOptions[activeElement].selectedP1 = false;
                    if (activeElement < comp.pauseOptions.Length - 1)
                    {
                        activeElement++;
                    }
                    else
                    {
                        activeElement = 0;
                    }
                }
                timer = inputDelay;
            }
            timer -= Time.deltaTime;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                HandleOption();
            }
        }
        

        void HandleOption()
        {
            switch (activeElement)
            {
                case 0:
                    Master.Instance.GameState = "fighting";
                    break;
                case 1:
                    master.GoToScene("PreMenu");
                    break;
            }
        }
    }

    public void SetUpUI()
    {
        if (master.GetScene().Equals("FightScene"))
        {
            if (log.FighterSel1 == "MuSh")
            {
                p1PortraitObj.GetComponent<Image>().sprite = comp.MuSh1;
            }
            else if (log.FighterSel1 == "BobBig") 
            {
                p1PortraitObj.GetComponent<Image>().sprite = comp.BobBig1;
            }

            if (log.FighterSel2 == "MuSh")
            {
                p2PortraitObj.GetComponent<Image>().sprite = comp.MuSh1;
            }
            else if (log.FighterSel2 == "BobBig")
            {
                p2PortraitObj.GetComponent<Image>().sprite = comp.BobBig1;
            }
        }
    }

    public void UpdateHealth(int ID, Player player)
    {
        switch (ID)
        {
            case 0:
                p1HealthTxt.text = player.Log.CurrHealth.ToString() + " / " + player.Log.MaxHealth.ToString();
                break;
            case 1:
                p2HealthTxt.text = player.Log.CurrHealth.ToString() + " / " + player.Log.MaxHealth.ToString();
                break;
        }
    }
}
