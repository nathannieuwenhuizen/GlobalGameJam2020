using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("round information")]
    [SerializeField]
    private int amountOfLaps = 3;
    private int currentLap = 1;

    private List<PlayerEntity> players;
    [SerializeField]
    private PlayerEntity[] playerObjects;
    [SerializeField]
    private int amountOfPlayers = 2;

    [Header("UI")]
    [SerializeField]
    private Text lapText;
    [SerializeField]
    private Text countDownText;

    [Header("Count down")]
    [SerializeField]
    private int countFrom = 3;
    [SerializeField]
    private float intervalBetweenCountDown = 0.5f;
    [SerializeField]
    private bool enableCountDown = true;

    private PlayerEntity leadingPlayer;

    [Header("SpawnPartsInfo")]
    [SerializeField]
    private int amountOfSpawnedParts = 10;
    [SerializeField]
    private float sizeOfPlayfield = 17f;

    [SerializeField]
    private List<GameObject> parts;

    public bool gameIsRunning = true;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    void Start()
    {
        CurrentLap = 1;

        ActivatesPlayers();
        SpawnRandomParts();

        if (enableCountDown)
        {
            StartCountDown();
        }
    }
    public void SpawnRandomParts()
    {
        parts = new List<GameObject>();
        foreach (PlayerEntity player in players)
        {
            parts.Add(player.partPrefab);
        }
        for (int i = 0; i < amountOfSpawnedParts; i++)
        {
            int randomIndex = Random.Range(0, parts.Count);
            Vector3 spawnPos = new Vector3(
                Random.Range(-sizeOfPlayfield, sizeOfPlayfield),
                0,
                Random.Range(-sizeOfPlayfield, sizeOfPlayfield)
                );
            Instantiate(parts[randomIndex]).transform.position = spawnPos;
        }
    }

    public void ActivatesPlayers()
    {
        List<PlayerEntity> foundPlayers = new List<PlayerEntity>(Transform.FindObjectsOfType<PlayerEntity>());
        players = new List<PlayerEntity>();

        for (int i = 0; i < foundPlayers.Count; i++)
        {
            if (i >= amountOfPlayers)
            {
                foundPlayers[i].gameObject.SetActive(false);
            } else
            {
                foundPlayers[i].gameObject.SetActive(true);
                foundPlayers[i].playerIndex = i + 1;
                players.Add(foundPlayers[i]);
            }
        }
    }

    public void NextLap(PlayerEntity player)
    {
        if (player.CurrentLap > CurrentLap)
        {
            leadingPlayer = player;
            CurrentLap = player.CurrentLap;
        }
    }
    public void End()
    {
        gameIsRunning = false;
        countDownText.text = "Finish! Player " + (players.IndexOf(leadingPlayer) + 1) + "wins!";
        foreach (PlayerEntity player in players)
        {
            player.canMove = false;
        }

    }

    public int CurrentLap
    {
        get { return currentLap; }
        set {
            currentLap = value;
            if (value > amountOfLaps)
            {
                End();
            } else
            {
                lapText.text = "Lap " + value + "/" + amountOfLaps;
            }
        }
    }
    public void StartCountDown()
    {
        gameIsRunning = false;
        foreach (PlayerEntity player in players)
        {
            player.canMove = false;
        }
        StartCoroutine(CountDown(countFrom));
    }
    IEnumerator CountDown(int number)
    {
        countDownText.text = number + "";
        if (number == 0)
        {
            countDownText.text = "GO!";
            foreach(PlayerEntity player in players)
            {
                player.canMove = true;
            }
            gameIsRunning = true;

            yield return new WaitForSeconds(intervalBetweenCountDown);
            countDownText.text = "";

        }
        else
        {
            yield return new WaitForSeconds(intervalBetweenCountDown);
            StartCoroutine(CountDown(number - 1));
        }

    }

    void Update()
    {
        
    }
}
