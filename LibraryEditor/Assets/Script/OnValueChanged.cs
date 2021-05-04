using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IdleLibrary
{
    public class ChangeValue<T>
    {
        private T value;
        private Action action;

        public ChangeValue(T value, Action action)
        {
            this.value = value;
            this.action = action;
        }
        public T Value
        {
            get => value;
            set
            {
                this.value = value;
                OnChanged();
            }
        }
        public void OnChanged()
        {
            action();
        }
    }
}
