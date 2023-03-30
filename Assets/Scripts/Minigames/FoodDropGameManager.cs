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
        if (stackObject.GetComponent<StackItemController>().FoodType == selectedFoodType)
            foodStackController.AddObjectToStack(stackObject);
        else
            DropStack();
    }

    public void AddToTotalFood(GameObject foodToAdd)
    {
        activeFoodList.Add(foodToAdd);
    }

    public void RemoveFromTotalFood(GameObject foodToRemove)
    {
        activeFoodList.Remove(foodToRemove);
    }

    [ContextMenu("Drop Stack")]
    public void DropStack()
    {
        foodStackController.DropStack();
    }

    [ContextMenu("StartGame")]
    public void StartNewGame()
    {
        int selectedFoodIndex = Random.Range(0, foodPrefabs.Count);
        selectedFoodType = foodPrefabs[selectedFoodIndex].GetComponent<StackItemController>().FoodType;

        instructionsText.text = instructions + selectedFoodType.ToString() + " foods.";

        foodStackController.StartNewGame(stackOffset);
        foodDropSpawner.StartGame(foodPrefabs, maxFoodCount, spawnDelay, selectedFoodIndex);

        gameIsStarted = true;
        gameIsOver = false;
    }


    private void Start()
    {
        foodDropSpawner = GetComponent<FoodDropSpawner>();
        foodStackController =  new FoodStackController();
        StartNewGame();
    }

    private void OnEnable()
    {
        activeFoodList = new List<GameObject>();
        PawnMoveController.OnPawnReachedCenter += StartNewGame;
        PawnManager.instance.LockPawnToCenter(true);

    }

    private void OnDisable()
    {
        PawnMoveController.OnPawnReachedCenter -= StartNewGame;
        PawnManager.instance.LockPawnToCenter(false);
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

        PawnManager.instance.Controller.PawnStatusController.NeedsStatus.UpdateNeedsStatus(NeedStatus.Hunger, foodStackController.StackCount);
        PawnManager.instance.LockPawnToCenter(false);

        gamePanelManager.ChangeGamePanel(PanelType.Stats);
    }
}
