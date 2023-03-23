using System;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class DateTimeManager : MonoBehaviour
{
    private Timer timer;
    private DateTime now;
    private DateTime nextHour;
    private TimeSpan timeUntilNextHour;
    private List<IUpdateOnHour> updateOnHourList;

    public static DateTimeManager instance;

    public void SubscribeToHourUpdate(IUpdateOnHour observer)
    {
        updateOnHourList.Add(observer);
    }

    public void Detach(IUpdateOnHour observer)
    {
        updateOnHourList.Remove(observer);
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
        nextHour = new DateTime(now.Year, now.Month, now.Day, now.Hour + 1, 0, 0);
        timeUntilNextHour = nextHour - now;
        timer = new Timer(timeUntilNextHour.TotalMilliseconds);
        timer.Elapsed += OnHourChanged;
        timer.AutoReset = true;
        timer.Start();

        updateOnHourList = new List<IUpdateOnHour>();
    }

    private void OnDestroy()
    {
        List<IUpdateOnHour> listCopy = new List<IUpdateOnHour>(updateOnHourList);

        foreach (IUpdateOnHour observer in listCopy)
        {
            if (updateOnHourList.Contains(observer))
                Detach(observer);
        }
    }

    private void OnHourChanged(object sender, ElapsedEventArgs e)
    {
        UpdateOnHour();

        now = DateTime.Now;
        nextHour = new DateTime(now.Year, now.Month, now.Day, now.Hour + 1, 0, 0);
        timeUntilNextHour = nextHour - now;
        timer.Interval = timeUntilNextHour.TotalMilliseconds;
    }


    private void UpdateOnHour()
    {
        foreach (IUpdateOnHour observer in updateOnHourList)
        {
            observer.UpdateOnHour();
        }
    }
}



