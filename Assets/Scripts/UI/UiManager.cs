﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[Serializable]

public class UiManager : MonoBehaviour
{
    //public static UiManager Instance;

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

    private float startingTimer = 2f;

    [SerializeField]
    private Text p1HealthTxt;
    [SerializeField]
    private Text p2HealthTxt;

    private UiUtilities util;
    public Master Master { get => master; }
    public UiComponents Comp { get => comp; }
    public UiLog Log { get => log; }
    public UiUtilities Util { get => util; }

    public void Awake()
    {
        /*
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
        */

        util = new UiUtilities(this);

        log.FighterSel1 = Master.Instance.FighterSel1;
        log.FighterSel2 = Master.Instance.FighterSel2;

        SetUpUIP1();
        SetUpUIP2();
    }

    public void Start()
    {

    }

    public void Update()
    {
        if (Master.Instance.GameState == "fighting" && (log.P1Health <= 0 || log.P2Health <= 0))
        {
            EndGame(log.Winner);
        }

        if (Master.Instance.GameState == "paused")
        {
            comp.PauseMenu.SetActive(true);
        }
        else
        {
            comp.PauseMenu.SetActive(false);
        }

        if (Master.Instance.GameState == "starting")
        {
            comp.VsPanel.SetActive(true);
            comp.P1Fighter.text = log.FighterSel1;
            comp.P2Fighter.text = log.FighterSel2;
        }
        else
        {
            comp.VsPanel.SetActive(false);
        }

        if (startingTimer > 0 && Master.Instance.GameState == "starting")
        {
            startingTimer -= Time.deltaTime;
        }
        else if (startingTimer < 0 && Master.Instance.GameState == "starting")
        {
            master.GoToScene("FightStage");
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
                    Master.Instance.GameState = "preMenu";
                    master.GoToScene("PreMenu");
                    break;
            }
        }
    }

    public void SetUpUIP1()
    {
        switch (log.FighterSel1)
        {
            case "MuSh":
                comp.P1Portrait.sprite = comp.MuSh1;
                break;
            case "BobBig":
                comp.P1Portrait.sprite = comp.BobBig1;
                break;
            case "TomDan":
                comp.P1Portrait.sprite = comp.TomDan1;
                break;
            case "BertBelly":
                comp.P1Portrait.sprite = comp.BertBelly1;
                break;
            case "AndrewBaller":
                comp.P1Portrait.sprite = comp.AndrewBaller1;
                break;
        }
    }

    public void SetUpUIP2()
    {
        switch (log.FighterSel2)
        {
            case "MuSh":
                comp.P2Portrait.sprite = comp.MuSh1;
                break;
            case "BobBig":
                comp.P2Portrait.sprite = comp.BobBig1;
                break;
            case "TomDan":
                comp.P2Portrait.sprite = comp.TomDan1;
                break;
            case "BertBelly":
                comp.P2Portrait.sprite = comp.BertBelly1;
                break;
            case "AndrewBaller":
                comp.P2Portrait.sprite = comp.AndrewBaller1;
                break;
        }
    }

    public void UpdateHealth(int ID, Player player)
    {
        switch (ID)
        {
            case 0:
                p1HealthTxt.text = player.Log.CurrHealth.ToString() + " / " + player.Log.MaxHealth.ToString();
                log.P1Health = player.Log.CurrHealth;

                if (log.P2Health <= 0)
                {
                    player.State.SetState(PLAYERSTATE.VICTORY);
                    log.Winner = log.FighterSel1;
                    log.Loser = log.FighterSel2;
                    log.player1win = true;
                    break;
                }
                break;
            case 1:
                p2HealthTxt.text = player.Log.CurrHealth.ToString() + " / " + player.Log.MaxHealth.ToString();
                log.P2Health = player.Log.CurrHealth;

                if (log.P1Health <= 0)
                {
                    player.State.SetState(PLAYERSTATE.VICTORY);
                    log.Winner = log.FighterSel2;
                    log.Loser = log.FighterSel1;
                    log.player1win = false;
                    break;
                }
                break;
        }
    }

    public void EndGame(string winner)
    {
        if (log.player1win)
        {
            Master.Instance.player1win = true;
        }
        else
        {
            Master.Instance.player1win = false;
        }

        Master.Instance.GameState = "gameOver";
    }
}
