using System.Collections.Generic;
using UnityEngine;
namespace IdleLibrary
{
    public interface IObserver
    {
        void _Update(ISubject subject);
    }
    public interface ISubject
    {
        void Attach(IObserver observer);
        void Detach(IObserver observer);
        void Notify();
    }
    public abstract class Subject : MonoBehaviour ,ISubject
    {
        public void Attach(IObserver observer)
        {
            observers.Add(observer);
        }
        public void Detach(IObserver observer)
        {
            observers.Remove(observer);
        }
        public void Notify()
        {
            if (observers.Count == 0) return;
            foreach (var item in observers)
            {
                item._Update(this);
            }
        }
        protected List<IObserver> observers = new List<IObserver>();
    }
}