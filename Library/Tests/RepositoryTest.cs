using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using IdleLibrary;
using IdleLibrary.Upgrade;
using System.Linq;
using System;
using Zenject;

public class Cookie : NUMBER
{
    public override double Number { get => record.cookie; set => record.cookie = value; }
    private readonly Record record;
    public Cookie(Record record)
    {
        this.record = record;
    }
}

//これをtest上でセーブできるようにする
public class RepositoryTest
{
    Upgrade[] upgrades;
    //Needs to save
    NUMBER cookie;
    Level[] levels { get => record.upgradeLevels; set => record.upgradeLevels = value; }

    private void Initialize()
    {
        record.Initialize();
        cookie = new Cookie(record);
        upgrades = new Upgrade[] { new Upgrade(levels[0], cookie, new LinearCost(0,0,levels[0])) , new Upgrade(levels[1], cookie, new LinearCost(0, 0, levels[1])) };
        Load();
    }

    [Test]
    public void RepositoryTestSimplePasses()
    {
        Initialize();
        cookie.Increment(100);
        upgrades[0].Pay();
        upgrades[1].Pay();
        //ここでセーブ
        Save();
        Initialize();
        //ここでロード
        Debug.Log(JsonUtility.ToJson(record));
    }

    //一番いいのは...
    //個別にセーブとロードを書く

    //↓ここにセーブする
    private Record record = new Record();
    public void Save()
    {
        Record.SetObject<Record>("Record", record);
    }
    public void Load()
    {
        var record = Record.GetObject<Record>("Record");
        if (record == null) this.record = new Record();
    }
}

//ここにセーブするべきものをデータベースとして用意する？
[Serializable]
public class Record
{
    public void Initialize()
    {
        upgradeLevels = Enumerable.Range(0, 2).Select(_ => new Level()).ToArray();
    }
    public double cookie;
    public Level[] upgradeLevels;

    public static void SetObject<T>(string key, T obj)
    {
        var json = JsonUtility.ToJson(obj);
        PlayerPrefs.SetString(key, json);
    }

    public static T GetObject<T>(string key)
    {
        var json = PlayerPrefs.GetString(key);
        var obj = JsonUtility.FromJson<T>(json);

        return obj;
    }
}

