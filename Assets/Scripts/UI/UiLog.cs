﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
[System.Serializable]

public class UiLog
{
    public string fighterSel1;
    public string fighterSel2;

    private bool loadingLevel;
    private bool init;

 
    public bool LoadingLevel { get => loadingLevel; set => loadingLevel = value; }
    public bool Init { get => init; set => init = value; }
    public string FighterSel1 { get => fighterSel1; set => fighterSel1 = value; }
    public string FighterSel2 { get => fighterSel2; set => fighterSel2 = value; }
}
