/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UpgradeLibrary;

public static class ICostExtension 
{
    public static double TotalCost(this ICost cost, int level, int upgradeNum)
    {
        int currentLevel = level;
        double temp = 0;
        for (int i = 0; i < upgradeNum; i++)
        {
            temp += cost.Cost(currentLevel);
            currentLevel++;
        }
        return temp;
    }
}
*/
