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
        void UpdateStage();
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
        public Stage()
        {
            debriInfo = new DebriInfo();
            stageManager = new StageManager(() => debriInfo.IsDebriLeft(), (x) => { debriInfo.Clear(); });
            debriGenerator = new DebriGenerator(() => stageManager.currentStage, debriInfo);
        }
        public void Initialize()
        {
            debriGenerator.GenerateDebri();
        }
        public void UpdateStage()
        {

        }
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
        public Func<long> currentStage;
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
                        debriInfo.AddDebri(new Debri(UnityEngine.Random.Range(1, 100), UnityEngine.Random.Range(0f, 1f), count));
                        count++;
                    }
                    for (int i = 0; i < 250; i++)
                    {
                        debriInfo.AddDebri(new Debri(UnityEngine.Random.Range(100, 300), UnityEngine.Random.Range(0f, 1f), count));
                        count++;
                    }
                    for (int i = 0; i < 50; i++)
                    {
                        debriInfo.AddDebri(new Debri(UnityEngine.Random.Range(300, 1000), UnityEngine.Random.Range(0f, 1f), count));
                        count++;
                    }
                }
                ,
                _ => () =>
                {

                }
            };
        }
    }

    internal class Debri
    {
        public Parameter requiredInhalePower { get; private set; }
        public float positionRate { get; private set; }
        public int id { get; private set; }
        public Debri(double initialRequiredInhalePower, float positionRate, int id)
        {
            this.requiredInhalePower = new Parameter(initialRequiredInhalePower);
            this.positionRate = positionRate;
            this.id = id;
        }
    }

    //デブリの情報と位置を全て格納する
    internal class DebriInfo
    {
        private List<Debri> debriInfos = new List<Debri>(1000);

        public void AddDebri(Debri debri) { debriInfos.Add(debri); }
        public void RemoveDebri(int id) { debriInfos[id] = null; }
        public void Clear() { debriInfos.Clear(); }
        public bool IsDebriLeft() => debriInfos.All(_ => _ == null);
    }
}
