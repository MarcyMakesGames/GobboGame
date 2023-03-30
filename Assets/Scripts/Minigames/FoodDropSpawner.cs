using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodDropSpawner : MonoBehaviour 
{
    [SerializeField] private FoodDropGameManager foodDropManager;
    [SerializeField] private GameObject garbageDisposalPrefab;

    private List<GameObject> foodPrefabs;
    private GameObject garbageDisposal;
    private int maxFoodCount = 10;
    private float spawnDelay = 2f;

    private float leftBound = -3f;
    private float rightBound = 3f;
    private float spawnHeight = 10f;

    private int foodCount = 0;
    private float timer = 0f; 
    private bool isSpawning = false;
    private int selectedFoodIndex;

    public void StartGame(List<GameObject> newFoodPrefabs, int newMaxFoodCount, float newSpawnDelay, int newSelectedFoodIndex)
    {
        foodPrefabs = newFoodPrefabs;
        maxFoodCount = newMaxFoodCount;
        spawnDelay = newSpawnDelay;
        selectedFoodIndex = newSelectedFoodIndex;
        isSpawning = true;
        foodCount = 0;
        timer = 0f;

        if(garbageDisposal == null)
        {
            Vector3 bottomEdge = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0));
            Instantiate(garbageDisposalPrefab, bottomEdge, Quaternion.identity);
        }
    }

    private void Start()
    {
        Vector3 leftEdge = Camera.main.ViewportToWorldPoint(new Vector3(0, 0.5f, 0));
        Vector3 rightEdge = Camera.main.ViewportToWorldPoint(new Vector3(1, 0.5f, 0));
        spawnHeight = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, 0)).y + 1f;

        leftBound = leftEdge.x;
        rightBound = rightEdge.x;
    }

    private void Update()
    {
        if (isSpawning && foodCount < maxFoodCount)
        {
            timer += Time.deltaTime;

            if (timer >= spawnDelay)
            {
                SpawnFood();
                timer = 0f;
            }
        }
    }

    private void SpawnFood()
    {
        int foodIndex = Random.Range(0, foodPrefabs.Count);
        GameObject foodPrefab = foodPrefabs[foodIndex];

        float spawnX = Random.Range(leftBound, rightBound);
        Vector3 spawnPosition = new Vector3(spawnX, spawnHeight, 0f);

        GameObject newFood = Instantiate(foodPrefab, spawnPosition, Quaternion.identity);
        foodDropManager.AddToTotalFood(newFood);

        if(selectedFoodIndex == foodIndex)
        {
            foodCount++;

            if(foodCount >= maxFoodCount)
            {
                foodDropManager.GameIsOver = true;
            }
        }
    }
}

