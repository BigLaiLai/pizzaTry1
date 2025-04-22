using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class MultiplayerManager : MonoBehaviour
{
    public static MultiplayerManager Instance;

    [Header("Player Prefabs")]
    public Transform playerParent;

    [Header("Countdown UI")]
    public GameObject countdownPanel;
    public TMPro.TextMeshProUGUI countdownText;

    public List<PlayerSelector> activePlayers = new List<PlayerSelector>();
    private HashSet<int> lockedCharacters = new HashSet<int>();
    private int confirmedCount = 0;
    private int maxPlayers = 4;
    private bool countdownStarted = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        PlayerInputManager.instance.onPlayerJoined += OnPlayerJoined;
        countdownPanel.SetActive(false);
    }

    public void OnPlayerJoined(PlayerInput playerInput)
    {
        int index = activePlayers.Count;

        if (index >= maxPlayers)
        {
            Destroy(playerInput.gameObject);
            return;
        }

        playerInput.transform.SetParent(this.transform);
        PlayerSelector selectorScript = playerInput.GetComponent<PlayerSelector>();
        RegisterPlayer(selectorScript);
    }

    public void RegisterPlayer(PlayerSelector player)
    {
        if (!activePlayers.Contains(player) && activePlayers.Count < maxPlayers)
        {
            activePlayers.Add(player);

            if (activePlayers.Count == maxPlayers && !countdownStarted)
            {
                countdownStarted = true;
                ShowCountdownPanel();
            }
        }
    }

    public void UpdateReady()
    {
        foreach (var player in activePlayers)
        {
            if (!player.isReady)
                return;
        }

        ShowCountdownPanel();
    }

    private void ShowCountdownPanel()
    {
        countdownPanel.SetActive(true);
        StartCoroutine(StartCountdown());
    }

    private IEnumerator StartCountdown()
    {
        int count = 3;
        while (count > 0)
        {
            countdownText.text = count.ToString();
            yield return new WaitForSeconds(1f);
            count--;
        }

        countdownText.text = "GO!";
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("GameScene");
    }

    public void ConfirmPlayerSelection(int playerIndex, int characterIndex)
    {
        if (lockedCharacters.Contains(characterIndex)) return;

        lockedCharacters.Add(characterIndex);
        confirmedCount++;
    }

    public bool IsCharacterTaken(int characterIndex)
    {
        return lockedCharacters.Contains(characterIndex);
    }
}
