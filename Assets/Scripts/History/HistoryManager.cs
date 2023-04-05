using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class HistoryManager : MonoBehaviour, IUpdateOnQuarterHour
{
    public static HistoryManager instance;

    [SerializeField] private List<HistoricalEventSO> historicalEventSOs;
    [Space]
    [Header("Need Status Events")]
    [SerializeField] private HistoricalEventSO hungryEventSO;
    [SerializeField] private HistoricalEventSO tiredEventSO;
    [SerializeField] private HistoricalEventSO boredEventSO;
    [Space]
    [Header("Interaction Events")]
    [SerializeField] private HistoricalEventSO positiveFeedEventSO;
    [SerializeField] private HistoricalEventSO positiveRestedEventSO;
    [SerializeField] private HistoricalEventSO positivePlayedEventSO;
    [SerializeField] private HistoricalEventSO negativeFeedEventSO;
    [SerializeField] private HistoricalEventSO negativeRestedEventSO;
    [SerializeField] private HistoricalEventSO negativePlayedEventSO;
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

    public void PostInteractionEvent(NeedStatus interactionType, bool positiveInteraction)
    {
        switch (interactionType)
        {
            case NeedStatus.None:
                break;
            case NeedStatus.Hunger:
                if(positiveInteraction)
                    PostNewEvent(positiveFeedEventSO);
                else
                    PostNewEvent(negativeFeedEventSO);
                break;

            case NeedStatus.Sleep:
                if (positiveInteraction)
                    PostNewEvent(positiveRestedEventSO);
                else
                    PostNewEvent(negativeRestedEventSO);
                break;

            case NeedStatus.Entertainment:
                if(positiveInteraction)
                    PostNewEvent(positivePlayedEventSO);
                else
                    PostNewEvent(negativePlayedEventSO);
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
