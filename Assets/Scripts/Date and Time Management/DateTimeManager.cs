using System;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class DateTimeManager : MonoBehaviour
{
    private Timer hourlyTimer;
    private Timer halfHourlyTimer;
    private DateTime now;
    private DateTime nextHalfHour;
    private DateTime nextHour;
    private TimeSpan timeUntilNextHour;
    private TimeSpan timeUntilNextHalfHour;

    private List<IUpdateOnHour> updateOnHourList;
    private List<IUpdateOnHalfHour> updateOnHalfHourList;

    public static DateTimeManager instance;

    public void SubscribeToHourlyUpdate(IUpdateOnHour observer)
    {
        updateOnHourList.Add(observer);
    }

    public void SubscribeToHalfHourlyUpdate(IUpdateOnHalfHour observer)
    {
        updateOnHalfHourList.Add(observer);
    }

    public void DetachFromHourlyUpdate(IUpdateOnHour observer)
    {
        updateOnHourList.Remove(observer);
    }

    public void DetachFromHalfHourlyUpdate(IUpdateOnHalfHour observer)
    {
        updateOnHalfHourList.Remove(observer);
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

        if (now.Hour == 23 && now.Minute >= 30)
            nextHalfHour = new DateTime(now.Year, now.Month, now.Day + 1, 0, 0, 0);
        else if (now.Minute >= 30)
            nextHalfHour = new DateTime(now.Year, now.Month, now.Day, now.Hour + 1, 0, 0);
        else
            nextHalfHour = new DateTime(now.Year, now.Month, now.Day, now.Hour, 30, 0);

        timeUntilNextHalfHour = nextHalfHour - now;
        halfHourlyTimer = new Timer(timeUntilNextHalfHour.TotalMilliseconds);
        halfHourlyTimer.Elapsed += OnHalfHourChanged;
        halfHourlyTimer.AutoReset = true;
        halfHourlyTimer.Start();

        updateOnHourList = new List<IUpdateOnHour>();
    }



    private void OnDestroy()
    {
        List<IUpdateOnHour> listCopy = new List<IUpdateOnHour>(updateOnHourList);

        foreach (IUpdateOnHour observer in listCopy)
        {
            if (updateOnHourList.Contains(observer))
                DetachFromHourlyUpdate(observer);
        }

        List<IUpdateOnHalfHour> halfListCopy = new List<IUpdateOnHalfHour>(updateOnHalfHourList);

        foreach (IUpdateOnHalfHour observer in halfListCopy)
        {
            if (updateOnHalfHourList.Contains(observer))
                DetachFromHalfHourlyUpdate(observer);
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

    private void OnHalfHourChanged(object sender, ElapsedEventArgs e)
    {
        UpdateOnHalfHour();

        now = DateTime.Now;
        nextHalfHour = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute >= 30 ? 30 : 0, 0).AddMinutes(30);
        timeUntilNextHalfHour = nextHalfHour - now;
        halfHourlyTimer.Interval = timeUntilNextHalfHour.TotalMilliseconds;
    }

    private void UpdateOnHour()
    {
        foreach (IUpdateOnHour observer in updateOnHourList)
            observer.UpdateOnHour();
    }

    private void UpdateOnHalfHour()
    {
        foreach (IUpdateOnHalfHour observer in updateOnHalfHourList)
            observer.UpdateOnHalfHour();
    }
}



