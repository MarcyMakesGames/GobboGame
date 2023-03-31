using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SleepGameSpawner : MonoBehaviour
{
    [SerializeField] private SleepGameManager sleepingGameManager;

    private GameObject dreamPrefab;
    private GameObject nightmarePrefab;
    private int maxDreamCount = 10;
    private float spawnDelay = 2f;

    private float leftBound;
    private float rightBound;
    private float upperBound;
    private float lowerBound;

    private int dreamCount = 0;
    private float timer = 0f;
    private bool isSpawning = false;

    public void StartGame(GameObject newDreamPrefab, GameObject newNightmarePrefab, int newMaxDreamCount, float newSpawnDelay)
    {
        dreamPrefab = newDreamPrefab;
        nightmarePrefab = newNightmarePrefab;
        maxDreamCount = newMaxDreamCount;
        spawnDelay = newSpawnDelay;
        isSpawning = true;
        dreamCount = 0;
        timer = 0f;
    }

    private void Start()
    {
        Vector3 leftEdge = Camera.main.ViewportToWorldPoint(new Vector3(-0.1f, 0.5f, 0));
        Vector3 rightEdge = Camera.main.ViewportToWorldPoint(new Vector3(1.1f, 0.5f, 0));
        Vector3 upperEdge = Camera.main.ViewportToWorldPoint(new Vector3(0, 1.1f, 0));
        Vector3 lowerEdge = Camera.main.ViewportToWorldPoint(new Vector3(0, -0.1f, 0));

        leftBound = leftEdge.x;
        rightBound = rightEdge.x;
        upperBound = upperEdge.y;
        lowerBound = lowerEdge.y;
    }

    private void Update()
    {
        if (isSpawning && dreamCount < maxDreamCount)
        {
            timer += Time.deltaTime;

            if (timer >= spawnDelay)
            {
                SpawnDream();
                timer = 0f;
            }
        }
    }

    private void SpawnDream()
    {
        int dreamSelection = Random.Range(0, 2);
        int edgeSelection = Random.Range(0, 4);

        GameObject dreamObject = null;
        Vector3 spawnPosition = Vector3.zero;

        switch(dreamSelection)
        {
            case 0:
                dreamObject = dreamPrefab;
                dreamCount++;
                break;
            case 1:
                dreamObject = nightmarePrefab;
                break;
        }

        switch (edgeSelection)
        {
            case 0: 
                // Left edge
                spawnPosition = new Vector3(leftBound, Random.Range(lowerBound, upperBound), 0f);
                break;
            case 1: 
                // Right edge
                spawnPosition = new Vector3(rightBound, Random.Range(lowerBound, upperBound), 0f);
                break;
            case 2: 
                // Top edge
                spawnPosition = new Vector3(Random.Range(leftBound, rightBound), upperBound, 0f);
                break;
            case 3: 
                // Bottom edge
                spawnPosition = new Vector3(Random.Range(leftBound, rightBound), lowerBound, 0f);
                break;
        }

        GameObject newDream = Instantiate(dreamObject, spawnPosition, Quaternion.identity);
        sleepingGameManager.AddToActiveDreams(newDream);

        if (dreamCount >= maxDreamCount)
            sleepingGameManager.GameIsOver = true;
    }
}
