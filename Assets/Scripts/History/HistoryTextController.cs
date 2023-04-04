using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HistoryTextController : MonoBehaviour
{
    [SerializeField] private TMP_Text slot0;
    [SerializeField] private TMP_Text slot1;
    [SerializeField] private TMP_Text slot2;
    [SerializeField] private TMP_Text slot3;
    [SerializeField] private TMP_Text slot4;

    private Queue<string> eventStrings;

    private void Awake()
    {
        if(eventStrings == null)
            eventStrings = new Queue<string>();
    }

    public void AddNewEvent(string eventString)
    {
        eventStrings.Enqueue(eventString);

        if(eventStrings.Count > 5)
            eventStrings.Dequeue();

        int loopCount = eventStrings.Count;

        for(int i = 0; i < loopCount; i++)
        {
            string currentString = eventStrings.Peek();

            switch (i)
            {
                case 0:
                    slot0.text = currentString;
                    eventStrings.Enqueue(eventStrings.Dequeue());
                    break;
                case 1:
                    slot1.text = currentString;
                    eventStrings.Enqueue(eventStrings.Dequeue());
                    break;
                case 2:
                    slot2.text = currentString;
                    eventStrings.Enqueue(eventStrings.Dequeue());
                    break;
                case 3:
                    slot3.text = currentString;
                    eventStrings.Enqueue(eventStrings.Dequeue());
                    break;
                case 4:
                    slot4.text = currentString;
                    eventStrings.Enqueue(eventStrings.Dequeue());
                    break;
                default:
                    break;
            }
        }
    }
}
