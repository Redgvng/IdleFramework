using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InventoryLibrary;
using System;

[System.Serializable]
public class Save
{
    public string lastTime;
    public string birthDate;
    public bool isContinuePlay;
    public double allTime;

    public float SEVolume;
    public float BGMVolume;

    public long ascendPoint;
    //ミッション
    public int MissionClearNum;
    public int StoryMissionClearNum;
    public int[] MissionId;
    public int[] StoryMissionId;
    public bool[] isMissionCleared, isStoryMissionCleared;

    //アップグレード
    public int[] UpgradeLevels;
    /* libraryここまで */
    /* ここから永久に保存したい変数をpublicで宣言していく */
    /* 初期化はSave */

    public Item item = new Item();
}