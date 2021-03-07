using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class OnEvents : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{

    public List<UnityAction> EnterEvents = new List<UnityAction>();
    public List<UnityAction> ExitEvents = new List<UnityAction>();
    public List<UnityAction> ClickEvents = new List<UnityAction>();
    public List<UnityAction> DisableEvents = new List<UnityAction>();

    public void OnPointerEnter(PointerEventData e)
    {
        DoActions(EnterEvents);
    }

    public void OnPointerExit(PointerEventData e)
    {
        DoActions(ExitEvents);
    }

    public void OnPointerClick(PointerEventData e)
    {
        DoActions(ClickEvents);
    }

    void OnDisable()
    {
        DoActions(DisableEvents);
    }

    void DoActions(IReadOnlyList<UnityAction> actions)
    {
        for (int i = 0; i < actions.Count; i++)
        {
            actions[i]?.Invoke();
        }
    }

    public void Copy(OnEvents original)
    {
        EnterEvents = original.EnterEvents;
        ExitEvents = original.ExitEvents;
        ClickEvents = original.ClickEvents;
        DisableEvents = original.DisableEvents;
    }
}
