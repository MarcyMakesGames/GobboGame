using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FoodDropGameManager : MonoBehaviour
{
    [SerializeField] private GamePanelManager gamePanelManager;
    [SerializeField] private List<GameObject> foodPrefabs;
    [SerializeField] private TMP_Text instructionsText;
    [SerializeField] private int maxFoodCount = 10;
    [SerializeField] private float spawnDelay = 2f;
    [SerializeField] private Vector3 stackOffset;
    [TextArea(1,5)]
    [SerializeField] private string defaultInstructions;
    [TextArea(1, 5)]
    [SerializeField] private string instructions;

    private FoodDropSpawner foodDropSpawner;
    private FoodStackController foodStackController;

    private List<GameObject> activeFoodList;
    private FoodType selectedFoodType;
    private bool gameIsStarted = false;
    private bool gameIsOver = false;

    public bool GameIsOver { get => gameIsOver; set => gameIsOver = value; }

    public void AddFoodToStack(GameObject stackObject)
    {
        if (stackObject.GetComponent<FoodItemController>().FoodType == selectedFoodType)
        {
            foodStackController.AddObjectToStack(stackObject);
            AudioManager.instance.PlaySound(SoundType.Pickup);
        }
        else
        {
            foodStackController.AddObjectToStack(stackObject);
            AudioManager.instance.PlaySound(SoundType.Drop);
            DropStack();
        }
    }

    public void AddToTotalFood(GameObject foodToAdd)
    {
        activeFoodList.Add(foodToAdd);
    }

    public void RemoveFromTotalFood(GameObject foodToRemove)
    {
        activeFoodList.Remove(foodToRemove);
    }

    public void DropStack()
    {
        foodStackController.DropStack();
    }

    public void StartNewGame()
    {
        int selectedFoodIndex = Random.Range(0, foodPrefabs.Count);
        selectedFoodType = foodPrefabs[selectedFoodIndex].GetComponent<FoodItemController>().FoodType;

        switch (selectedFoodType)
        {
            case FoodType.Fish:
                instructionsText.text = instructions + selectedFoodType.ToString() + ".";
                break;
            case FoodType.Hotdog:
                instructionsText.text = instructions + selectedFoodType.ToString() + "s.";
                break;
            case FoodType.Chicken:
                instructionsText.text = instructions + selectedFoodType.ToString() + ".";
                break;
            case FoodType.Burger:
                instructionsText.text = instructions + selectedFoodType.ToString() + "s.";
                break;
        }

        foodStackController.StartNewGame(stackOffset);
        foodDropSpawner.StartGame(foodPrefabs, maxFoodCount, spawnDelay, selectedFoodIndex);

        gameIsStarted = true;
        gameIsOver = false;
    }


    private void Start()
    {
        foodDropSpawner = GetComponent<FoodDropSpawner>();
        foodStackController =  new FoodStackController();
    }

    private void OnEnable()
    {
        activeFoodList = new List<GameObject>();
        PawnMoveController.OnPawnReachedEatPosition += StartNewGame;
        instructionsText.text = defaultInstructions;
    }

    private void OnDisable()
    {
        PawnMoveController.OnPawnReachedEatPosition -= StartNewGame;
        PawnManager.instance.MovePawnToWanderPosition();
    }

    private void Update()
    {
        if(gameIsStarted && gameIsOver)
        {
            if(activeFoodList.Count == foodStackController.StackCount)
                EndGame();
        }
    }

    private void EndGame()
    {
        gameIsOver = false;
        gameIsStarted = false;

        if (foodStackController.StackCount > 0)
            AudioManager.instance.PlaySound(SoundType.Celebration);
        else
            AudioManager.instance.PlaySound(SoundType.Failure);

        PawnManager.instance.Controller.PawnStatusController.NeedsStatus.UpdateNeedsStatus(NeedStatus.Hunger, foodStackController.StackCount);
        PawnManager.instance.InteractedWithPawn(ActivityStatus.Eat);
        foodStackController.EatStack();

        PawnManager.instance.MovePawnToWanderPosition();
        gamePanelManager.ChangeGamePanel(PanelType.Stats);
    }
}
