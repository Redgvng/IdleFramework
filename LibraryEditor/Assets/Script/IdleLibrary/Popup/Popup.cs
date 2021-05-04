using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;
using UniRx;
using UniRx.Triggers;
using static IdleLibrary.UsefulMethod;
namespace IdleLibrary.UI
{
    public interface IPopup
    {

    }
    public class Popup : MonoBehaviour, IPopup
    {
        Func<bool> showCondition;
        GameObject windowObject;
        public Popup(Func<bool> showCondition, GameObject windowObject)
        {
            this.showCondition = showCondition;
            this.windowObject = windowObject;
            ShowWindow();
        }
        //public bool isShowCondition
        //{
        //    get => showCondition();
        //    set
        //    {
        //        if (showCondition() && !value) setFalse(windowObject);
        //        else if (!showCondition() && value) setActive(windowObject);
        //        showCondition = () => value;
        //    }
        //}
        async void ShowWindow()
        {
            bool tempBool = false;
            while (true)
            {
                if (showCondition() && !tempBool)
                {
                    setActive(windowObject);
                    tempBool = true;
                }
                else if (!showCondition() && tempBool)
                {
                    setFalse(windowObject);
                    tempBool = false;
                }
                await UniTask.DelayFrame(1);
            }
        }
    }
}
public class Selectable<T>
{
    private T mValue;   // 選択中の値

    /// <summary>
    /// <para>値を取得または設定します</para>
    /// <para>値の設定後に mChanged イベントが呼び出されます</para>
    /// </summary>
    public T Value
    {
        get { return mValue; }
        set
        {
            mValue = value;
            OnChanged(mValue);
        }
    }

    /// <summary>
    /// 値が変更された時に呼び出されます
    /// </summary>
    public Action<T> mChanged;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public Selectable()
    {
        mValue = default(T);
    }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public Selectable(T value)
    {
        mValue = value;
    }

    /// <summary>
    /// <para>値を設定します</para>
    /// <para>値の設定後に mChanged イベントは呼び出されません</para>
    /// </summary>
    public void SetValueWithoutCallback(T value)
    {
        mValue = value;
    }

    /// <summary>
    /// <para>値を設定します</para>
    /// <para>値が変更された場合にのみ mChanged イベントが呼び出されます</para>
    /// </summary>
    public void SetValueIfNotEqual(T value)
    {
        if (mValue.Equals(value))
        {
            return;
        }
        mValue = value;
        OnChanged(mValue);
    }

    /// <summary>
    /// 値が変更された時に呼び出されます
    /// </summary>
    private void OnChanged(T value)
    {
        var onChanged = mChanged;
        if (onChanged == null)
        {
            return;
        }
        onChanged(value);
    }
}
