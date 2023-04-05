using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HistoryManager : MonoBehaviour, IUpdateOnQuarterHour
{
    public static HistoryManager instance;

    [SerializeField] private List<HistoricalEventSO> historicalEventSOs;
    [SerializeField] private HistoricalEventSO hungryEventSO;
    [SerializeField] private HistoricalEventSO tiredEventSO;
    [SerializeField] private HistoricalEventSO boredEventSO;
    [Space]
    [SerializeField] private HistoryTextController historyTextController;

    public void UpdateOnQuarterHour()
    {
        PostNewEvent();
    }

    public void PostNewEvent(HistoricalEventSO eventSO = null)
    {
        HistoricalEventSO currentEvent;

        if (eventSO == null)
            currentEvent = GetRandomHistoricalEvent();
        else
            currentEvent = eventSO;

        PawnManager.instance.UpdateStatus(currentEvent.EventEffect, currentEvent.EffectMagnitude);

        historyTextController.AddNewEvent(currentEvent.EventText);
    }

    public void PostNeedStatusEvent(NeedStatus needStatus)
    {
        switch (needStatus)
        {
            case NeedStatus.None:
                break;
            case NeedStatus.Hunger:
                PostNewEvent(hungryEventSO);
                break;
            case NeedStatus.Sleep:
                PostNewEvent(tiredEventSO);
                break;
            case NeedStatus.Entertainment:
                PostNewEvent(boredEventSO);
                break;
            case NeedStatus.Happiness:
                break;
        }
    }

    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }


    private HistoricalEventSO GetRandomHistoricalEvent()
    {
        return historicalEventSOs[Random.Range(0, historicalEventSOs.Count)];
    }
}
