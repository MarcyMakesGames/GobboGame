using System;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class DateTimeManager : MonoBehaviour
{
    private Timer hourlyTimer;
    private Timer quarterHourlyTimer;
    private DateTime now;
    private DateTime nextQuarterHour;
    private DateTime nextHour;
    private TimeSpan timeUntilNextHour;
    private TimeSpan timeUntilNextQuarterHour;

    private List<IUpdateOnHour> updateOnHourList;
    private List<IUpdateOnQuarterHour> updateOnQuarterHourList;

    public static DateTimeManager instance;

    public void SubscribeToHourlyUpdate(IUpdateOnHour observer)
    {
        updateOnHourList.Add(observer);
    }

    public void SubscribeToQuarterHourlyUpdate(IUpdateOnQuarterHour observer)
    {
        updateOnQuarterHourList.Add(observer);
    }

    public void DetachFromHourlyUpdate(IUpdateOnHour observer)
    {
        updateOnHourList.Remove(observer);
    }

    public void DetachFromQuarterHourlyUpdate(IUpdateOnQuarterHour observer)
    {
        updateOnQuarterHourList.Remove(observer);
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        now = DateTime.Now;

        if (now.Hour == 23)
            nextHour = new DateTime(now.Year, now.Month, now.Day + 1, 0, 0, 0);
        else
            nextHour = new DateTime(now.Year, now.Month, now.Day, now.Hour + 1, 0, 0);

        timeUntilNextHour = nextHour - now;
        hourlyTimer = new Timer(timeUntilNextHour.TotalMilliseconds);
        hourlyTimer.Elapsed += OnHourChanged;
        hourlyTimer.AutoReset = true;
        hourlyTimer.Start();

        if (now.Hour == 23 && now.Minute >= 45)
            nextQuarterHour = new DateTime(now.Year, now.Month, now.Day + 1, 0, 0, 0);
        else if (now.Minute >= 45)
            nextQuarterHour = new DateTime(now.Year, now.Month, now.Day, now.Hour + 1, 0, 0);
        else if (now.Minute >= 30)
            nextQuarterHour = new DateTime(now.Year, now.Month, now.Day, now.Hour, 45, 0);
        else if (now.Minute >= 15)
            nextQuarterHour = new DateTime(now.Year, now.Month, now.Day, now.Hour, 30, 0);
        else
            nextQuarterHour = new DateTime(now.Year, now.Month, now.Day, now.Hour, 15, 0);

        timeUntilNextQuarterHour = nextQuarterHour - now;
        quarterHourlyTimer = new Timer(timeUntilNextQuarterHour.TotalMilliseconds);
        quarterHourlyTimer.Elapsed += OnQuarterHourChanged;
        quarterHourlyTimer.AutoReset = true;
        quarterHourlyTimer.Start();

        updateOnHourList = new List<IUpdateOnHour>();
        updateOnQuarterHourList = new List<IUpdateOnQuarterHour>();
    }

    private void OnDestroy()
    {
        List<IUpdateOnHour> listCopy = new List<IUpdateOnHour>(updateOnHourList);

        foreach (IUpdateOnHour observer in listCopy)
        {
            if (updateOnHourList.Contains(observer))
                DetachFromHourlyUpdate(observer);
        }

        List<IUpdateOnQuarterHour> quarterListCopy = new List<IUpdateOnQuarterHour>(updateOnQuarterHourList);

        foreach (IUpdateOnQuarterHour observer in quarterListCopy)
        {
            if (updateOnQuarterHourList.Contains(observer))
                DetachFromQuarterHourlyUpdate(observer);
        }
    }

    private void OnHourChanged(object sender, ElapsedEventArgs e)
    {
        UpdateOnHour();

        now = DateTime.Now;
        nextHour = new DateTime(now.Year, now.Month, now.Day, now.Hour + 1, 0, 0);
        timeUntilNextHour = nextHour - now;
        hourlyTimer.Interval = timeUntilNextHour.TotalMilliseconds;
    }

    private void OnQuarterHourChanged(object sender, ElapsedEventArgs e)
    {
        UpdateOnQuarterHour();

        now = DateTime.Now;
        nextQuarterHour = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute < 15 ? 0 : now.Minute < 30 ? 15 : now.Minute < 45 ? 30 : 45, 0).AddMinutes(15);
        timeUntilNextQuarterHour = nextQuarterHour - now;
        quarterHourlyTimer.Interval = timeUntilNextQuarterHour.TotalMilliseconds;
    }

    private void UpdateOnHour()
    {
        foreach (IUpdateOnHour observer in updateOnHourList)
            observer.UpdateOnHour();
    }

    private void UpdateOnQuarterHour()
    {
        foreach (IUpdateOnQuarterHour observer in updateOnQuarterHourList)
            observer.UpdateOnQuarterHour();
    }
}
