using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]

public class UiComponents
{
    [SerializeField]
    private GameObject pauseMenu;

    [SerializeField]
    private Text p1HealthTxt;
    [SerializeField]
    private Text p2HealthTxt;

    [SerializeField]
    private Image p1Portrait;
    [SerializeField]
    private Image p2Portrait;

    [SerializeField]
    private Sprite MuSh;
    [SerializeField]
    private Sprite BobBig;
    [SerializeField]
    private Sprite TomDan;
    [SerializeField]
    private Sprite BertBelly;
    [SerializeField]
    private Sprite AndrewBaller;

    public ButtonRef[] pauseOptions;

    public GameObject PauseMenu { get => pauseMenu; }
    public Image P1Portrait { get => p1Portrait; set => p1Portrait = value; }
    public Image P2Portrait { get => p2Portrait; set => p2Portrait = value; }
    public Sprite MuSh1 { get => MuSh; }
    public Sprite BobBig1 { get => BobBig; }
    public Sprite TomDan1 { get => TomDan; }
    public Sprite BertBelly1 { get => BertBelly; }
    public Sprite AndrewBaller1 { get => AndrewBaller; }

    public Text P1HealthTxt { get => p1HealthTxt; set => p1HealthTxt = value; }
    public Text P2HealthTxt { get => p2HealthTxt; set => p2HealthTxt = value; }
}