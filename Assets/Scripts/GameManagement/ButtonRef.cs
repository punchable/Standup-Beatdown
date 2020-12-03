using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]

public class ButtonRef : MonoBehaviour
{
    public GameObject selectIndicP1;
    public bool selectedP1;

    public GameObject selectIndicP2;
    public bool selectedP2;

    public void Start()
    {
        selectIndicP1.SetActive(true);
        selectIndicP2.SetActive(true);
    }

    public void Update()
    {
        selectIndicP1.SetActive(selectedP1);
        selectIndicP2.SetActive(selectedP2);
    }
}
