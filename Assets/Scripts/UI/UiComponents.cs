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

    public ButtonRef[] pauseOptions;

    public GameObject PauseMenu { get => pauseMenu; }
    public Image P1Portrait { get => p1Portrait; set => p1Portrait = value; }
    public Image P2Portrait { get => p2Portrait; set => p2Portrait = value; }
    public Sprite MuSh1 { get => MuSh; }
    public Sprite BobBig1 { get => BobBig; }
    public GameObject P1PortraitObj { get => p1PortraitObj; }
    public GameObject P2PortraitObj { get => p2PortraitObj; }
    public Text P1HealthTxt { get => p1HealthTxt; set => p1HealthTxt = value; }
    public Text P2HealthTxt { get => p2HealthTxt; set => p2HealthTxt = value; }
}
