using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField]
    private bool useSettings = false;

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
    private TextMeshProUGUI lapText;
    [SerializeField]
    private Text countDownText;

    [Header("Count down")]
    [SerializeField]
    private int countFrom = 3;
    [SerializeField]
    private float intervalBetweenCountDown = 1.0f;
    [SerializeField]
    private bool enableCountDown = true;

    private PlayerEntity leadingPlayer;

    [Header("SpawnPartsInfo")]
    [SerializeField]
    private int amountOfSpawnedParts = 10;
    [SerializeField]
    private float minRingSize = 5f;
    [SerializeField]
    private float maxRingSize = 7f;


    [SerializeField]
    private List<GameObject> parts;

    [Header("in game objects")]
    [SerializeField]
    private FinishLine finishLine;
    [SerializeField]
    private FancyCam fancyCam;

    [Header("overig")]
    [SerializeField]
    private Material dottedMaterial;
    [SerializeField]
    private float dotLineSpeed = 2f;

    [SerializeField]
    private GameObject PauseScreen;
    private bool paused = false;
    [SerializeField]
    private GameObject resultScreen;
    [SerializeField]
    private Text resultText;

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
        if (useSettings)
        {
            amountOfPlayers = Globals.AMOUNT_PLAYERS;
            amountOfLaps = Globals.AMOUNT_LAPS;
        }
        CurrentLap = 1;

        if (finishLine != null)
        {
            finishLine.SpawnRings(amountOfLaps);
        }

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
            float angle = Random.Range(0, Mathf.PI * 2);
            float amplitude = Random.Range(minRingSize, maxRingSize);

            Vector3 spawnPos = new Vector3(
                Mathf.Cos(angle) * amplitude,
                0,
                Mathf.Sin(angle) * amplitude
                );

            int randomIndex = Random.Range(0, parts.Count);

            Instantiate(parts[randomIndex]).transform.position = spawnPos;
        }
    }
    public void Pause(bool val)
    {
        if (!gameIsRunning)
        {
            return;
        }

        paused = val;
        Time.timeScale = val ? 0 : 1;
        PauseScreen.SetActive(val ? true : false);
    }
    public void ActivatesPlayers()
    {
        //find the players
        List<PlayerEntity> foundPlayers = new List<PlayerEntity>(Transform.FindObjectsOfType<PlayerEntity>());
        players = new List<PlayerEntity>();

        //disable and enable players
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

        //apply active players to fancy cam
        List<GameObject> targets = new List<GameObject>();

        for (int i = 0; i < fancyCam.targets.Length; i++)
        {
            targets.Add(fancyCam.targets[i]);
        }
        for (int i = 0; i < players.Count; i++)
        {
            targets.Add(players[i].gameObject);
        }

        if (fancyCam != null)
        {
            fancyCam.targets = targets.ToArray();
        }
    }

    public void NextLap(PlayerEntity player)
    {
        if (player.CurrentLap > CurrentLap)
        {
            leadingPlayer = player;
            CurrentLap = player.CurrentLap;
            if (finishLine != null)
            {
                finishLine.RemoveRing();
            }
        }
    }
    public void End()
    {
        if (fancyCam != null)
        {
            fancyCam.targets = new List<GameObject> { leadingPlayer.gameObject }.ToArray();
        }
        gameIsRunning = false;
        resultScreen.SetActive(true);
        resultText.text =  "Player " + (players.IndexOf(leadingPlayer) + 1) + "wins!";

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
        FMODUnity.RuntimeManager.PlayOneShot("event:/Sound/countdown");
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
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown("Pause") )
        {
            Pause(!paused);
        }
        dottedMaterial.SetTextureOffset("_BaseMap", new Vector2(-Time.time * dotLineSpeed, 0));
        //dottedMaterial.mainTextureOffset = new Vector2(-Time.time * dotLineSpeed, 0);
    }
}
