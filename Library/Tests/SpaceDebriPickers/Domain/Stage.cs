using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IdleLibrary;
using System;
using System.Linq;

namespace Pickers.Domain
{
    public interface IStage
    {
        void UpdateStage(float time);
        void Initialize();
    }
}

namespace Pickers.Domain.Test
{
    //Facade
    public class Stage : IStage
    {
        private StageManager stageManager;
        private DebriGenerator debriGenerator;
        private DebriInfo debriInfo;
        private DebriCollection debriCollection;
        private Picker picker;
        public Stage()
        {
            debriInfo = new DebriInfo();
            debriCollection = new DebriCollection();
            stageManager = new StageManager(() => debriInfo.IsDebriLeft(), (x) => { debriInfo.Clear(); });
            debriGenerator = new DebriGenerator(() => stageManager.currentStage, debriInfo);
            picker = new Picker(10, 1);
        }
        public void Initialize()
        {
            debriGenerator.GenerateDebri();
        }

        //とりあえずピッカーの処理を書いてみる
        public void UpdateStage(float time = 1.0f)
        {
            var count = time / Time.fixedDeltaTime;
            picker.MovePerSecond();
            debriInfo.GetDebris().ToList().ForEach(_ =>
            {
                if (_ is null) return;
                _.OnCollide(picker, debriCollection, debriInfo);
            });
            if (picker.currentPosition >= 60000) picker.currentPosition = 0;
        }

        public double GetDebriNum(int tier) { return debriCollection.GetCollectedDebri(tier); }
    }

    internal class StageManager
    {
        public long currentStage { get; private set; }
        public StageManager(Func<bool> canProgress, Action<long> onProgress)
        {
            this.canProgress = canProgress;
            this.onProgress = onProgress;

            onProgress(currentStage);
        }
        public void OnProgress()
        {
            if (!canProgress()) return;

            currentStage++;
            onProgress(currentStage);
        }

        private readonly Func<bool> canProgress;
        private readonly Action<long> onProgress;
    }

    internal class DebriGenerator
    {
        private Func<long> currentStage;
        private readonly DebriInfo debriInfo;
        public DebriGenerator(Func<long> currentStage, DebriInfo debriInfo)
        {
            this.currentStage = currentStage;
            this.debriInfo = debriInfo;
        }
        public void GenerateDebri()
        {
            Action action = currentStage() switch
            {
                0 => () =>
                {
                    var count = 0;
                    for (int i = 0; i < 700; i++)
                    {
                        debriInfo.AddDebri(new Debri(UnityEngine.Random.Range(1, 10), UnityEngine.Random.Range(0f, 1f), 1,count));
                        count++;
                    }
                    for (int i = 0; i < 250; i++)
                    {
                        debriInfo.AddDebri(new Debri(UnityEngine.Random.Range(10, 30), UnityEngine.Random.Range(0f, 1f), 1,count));
                        count++;
                    }
                    for (int i = 0; i < 50; i++)
                    {
                        debriInfo.AddDebri(new Debri(UnityEngine.Random.Range(30, 50), UnityEngine.Random.Range(0f, 1f),1,count));
                        count++;
                    }
                }
                ,
                _ => () =>
                {

                }
            };
            action();
        }    
    }

    internal class Debri
    {
        public Parameter requiredInhalePower { get; private set; }
        public float positionRate { get; private set; }
        public int id { get; private set; }
        public int tier { get; private set; }
        public Debri(double initialRequiredInhalePower, float positionRate, int tier, int id)
        {
            this.requiredInhalePower = new Parameter(initialRequiredInhalePower);
            this.positionRate = positionRate;
            this.tier = tier;
            this.id = id;
        }

        public void OnCollide(Picker picker, DebriCollection collection, DebriInfo debriInfo)
        {
            if (this.positionRate > picker.currentPosition / 60000) return;
            if (picker.inhalePower.Number < this.requiredInhalePower.Number * resistPower) return;

            Debug.Log("PickerをGetしたよ");
            Debug.Log($"Tier1 : {collection.GetCollectedDebri(1)}, Tier2 : {collection.GetCollectedDebri(2)}, Tier3 : {collection.GetCollectedDebri(3)}");
            collection.GetDebri(this.tier, this.requiredInhalePower.Number);
            debriInfo.RemoveDebri(this.id);
        }

        private double resistPower => tier switch
        {
            1 => 1.0,
            2 => 100,
            3 => 10000,
            _ => 1.0
        };
    }

    //デブリの情報と位置を全て格納する
    internal class DebriInfo
    {
        private List<Debri> debriInfos = new List<Debri>(1000);

        public void AddDebri(Debri debri) { debriInfos.Add(debri); }
        public void RemoveDebri(int id) { debriInfos[id] = null; }
        public void Clear() { debriInfos.Clear(); }
        public bool IsDebriLeft() => debriInfos.All(_ => _ == null);
        public Debri GetDebri(int id) => debriInfos[id];
        public IEnumerable<Debri> GetDebris() => debriInfos;
    }

    //デブリの取得数を格納する
    internal class DebriCollection
    {
        private Better.Dictionary<int, double> debrisCollected = new Better.Dictionary<int, double>();
        public DebriCollection()
        {
            debrisCollected[1] = 0;
            debrisCollected[2] = 0;
            debrisCollected[3] = 0;
        }

        public void GetDebri(int tier, double value) => debrisCollected[tier] += value;
        public double GetCollectedDebri(int tier) => debrisCollected[tier];
    }
}
