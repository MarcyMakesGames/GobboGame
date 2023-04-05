using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SleepGameManager : MonoBehaviour
{
    [SerializeField] private GamePanelManager gamePanelManager;
    [SerializeField] private GameObject dreamPrefab;
    [SerializeField] private GameObject nightmarePrefab;
    [SerializeField] private TMP_Text instructionsText;
    [SerializeField] private int maxDreamCount = 10;
    [SerializeField] private float spawnDelay = 2f;
    [TextArea(1, 5)]
    [SerializeField] private string instructions;

    private SleepGameSpawner sleepGameSpawner;

    private List<GameObject> activeDreams;
    private int happyDreams = 0;
    private bool gameIsStarted = false;
    private bool gameIsOver = false;

    public bool GameIsOver { get => gameIsOver; set => gameIsOver = value; }

    public void AddToActiveDreams(GameObject dreamToAdd)
    {
        activeDreams.Add(dreamToAdd);
    }

    public void RemoveFromActiveDreams(GameObject dreamToRemove)
    {
        activeDreams.Remove(dreamToRemove);
    }

    public void ConsumeDream(SleepGameDreamController dreamController)
    {
        switch (dreamController.DreamType)
        {
            case DreamType.Dream:
                happyDreams++;
                AudioManager.instance.PlaySound(SoundType.Pickup);
                break;
            case DreamType.Nightmare:
                happyDreams--;
                PawnManager.instance.SetAnimation(AnimationEnums.Nightmare);
                AudioManager.instance.PlaySound(SoundType.Drop);
                break;
        }

        activeDreams.Remove(dreamController.gameObject);
    }


    public void StartNewGame()
    {
        sleepGameSpawner.StartGame(dreamPrefab, nightmarePrefab, maxDreamCount, spawnDelay);
        gameIsStarted = true;
        gameIsOver = false;
    }


    private void Start()
    {
        sleepGameSpawner = GetComponent<SleepGameSpawner>();
    }

    private void OnEnable()
    {
        activeDreams = new List<GameObject>();
        PawnMoveController.OnPawnReachedSleepPosition += StartNewGame;
        instructionsText.text = instructions;
    }

    private void OnDisable()
    {
        PawnMoveController.OnPawnReachedSleepPosition -= StartNewGame;
        PawnManager.instance.MovePawnToWanderPosition();
    }

    private void Update()
    {
        if (gameIsStarted && gameIsOver)
        {
            if (activeDreams.Count == 0)
                EndGame();
        }
    }

    private void EndGame()
    {
        gameIsOver = false;
        gameIsStarted = false;

        PawnManager.instance.PawnController.PawnStatusController.NeedsStatus.UpdateNeedsStatus(NeedStatus.Sleep, happyDreams);
        PawnManager.instance.InteractedWithPawn(ActivityStatus.Sleep);

        if (happyDreams > 0)
            HistoryManager.instance.PostInteractionEvent(NeedStatus.Sleep, true);
        else
            HistoryManager.instance.PostInteractionEvent(NeedStatus.Sleep, false);

        PawnManager.instance.MovePawnToWanderPosition();
        gamePanelManager.ChangeGamePanel(PanelType.Stats);
    }
}

