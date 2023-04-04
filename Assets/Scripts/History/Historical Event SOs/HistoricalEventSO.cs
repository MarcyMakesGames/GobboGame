using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewHistoricalEvent", menuName = "History/New Historical Event")]
public class HistoricalEventSO : ScriptableObject
{
    [SerializeField] private string eventText;
    [SerializeField] private NeedStatus eventEffect;
    [SerializeField] private int effectMagnitude;

    public string EventText { get => eventText; }
    public NeedStatus EventEffect { get => eventEffect; }
    public int EffectMagnitude { get => effectMagnitude; }
}
