using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UniRx;
using temp;
using TMPro;
using System;

namespace temp
{
    public class Cube
    {
        public int id { get; private set; }
        public IdleLibrary.NUMBER number { get; private set; }
        public Cube(int id)
        {
            this.id = id;
            number = new IdleLibrary.NUMBER();
        }
    }
}

namespace IdleLibrary.ResourceDistribution {
    public class ResourceDistribution : MonoBehaviour
    {
        [SerializeField] private UI ui;
        [SerializeField] private Transform canvas;
        [SerializeField] private TextMeshProUGUI cubeNumText;
        private List<UI> uis = new List<UI>();
        private List<Element> elements = new List<Element>();
        private Cube[] cubes = new Cube[] { new Cube(0), new Cube(1), new Cube(2), new Cube(3), new Cube(4), new Cube(5) };
        void Awake()
        {
            for (int i = 0; i < 6; i++)
            {
                uis.Add(Instantiate(ui, canvas));
            }
            uis[0].titleText.text = "White Cube";
            uis[1].titleText.text = "Blue Cube";
            uis[2].titleText.text = "Green Cube";
            uis[3].titleText.text = "Orange Cube";
            uis[4].titleText.text = "Gold Cube";
            uis[5].titleText.text = "Platina Cube";
            Observable.EveryUpdate().Subscribe(_ => cubeNumText.text = $"White : {cubes[0].number.Number:F0}  " +
                $" Blue : {cubes[1].number.Number:F0}" +
                $"  Green : {cubes[2].number.Number:F0}   " +
                $"Orange : {cubes[3].number.Number:F0} " +
                $" Gold : {cubes[4].number.Number:F0}  " +
                $" Platina : {cubes[5].number.Number:F0}");

            //cubeそれぞれ100個持ってるとする
            cubes.ToList().ForEach(x => x.number.IncrementFixNumber(100));

            elements.Add(new Element(cubes[0].number, cubes[1].number));
            elements.Add(new Element(cubes[1].number, cubes[2].number));
            elements.Add(new Element(cubes[2].number, cubes[3].number));
            elements.Add(new Element(cubes[3].number, cubes[4].number));
            elements.Add(new Element(cubes[4].number, cubes[5].number));
            elements.Add(new Element(cubes[5].number, cubes[0].number));

            uis.Select((u, i) => new { u, i }).ToList().ForEach(x => x.u.plusButton.onClick.AddListener(() => elements[x.i].Store()));
            uis.Select((u, i) => new { u, i }).ToList().ForEach(x => x.u.minusButton.onClick.AddListener(() => elements[x.i].Retrieve()));
            uis.Select((u, i) => new { u, i }).ToList().ForEach(x => x.u.convertButton.onClick.AddListener(() => elements[x.i].Convert()));

            elements.Select((e, i) => new { e, i }).ToList().ForEach(x => Observable.EveryFixedUpdate().Subscribe(_ => {
                uis[x.i].slider.value = x.e.CurrentProgressRatio();
                x.e.Update();
                }));
        }
    }

    class Element : ILevel
    {
        public long level { get; set; }
        private readonly NUMBER number, toConvert;
        private double stored;
        private double currentProgress;
        public Element(NUMBER number, NUMBER toConvert)
        {
            this.number = number;
            this.toConvert = toConvert;
            req = () => (level +1) * 10000;
        }
        public void Store()
        {
            stored += number.Number;
            number.ResetNumberToZero();
        }
        public void Retrieve()
        {
            number.IncrementFixNumber(stored);
            stored = 0;
        }
        public void Convert()
        {
            var increment = number.Number;
            toConvert.IncrementFixNumber(increment);
            number.Decrement(increment);
        }
        private Func<double> req;
        public void Update()
        {
            currentProgress += stored;
            if(currentProgress >= req())
            {
                currentProgress -= req();
                level++;
            }
        }
        public float CurrentProgressRatio() => (float)(currentProgress / req());
    }
}
