using Data;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public static Action OnMinigameStart, OnMinigameEnd;

    public List<GameObject> Players { get; private set; }
    public int PlayerCount => Players.Count;
    public List<Camera> Cameras { get; private set; }
    public List<GameObject> MinigameBehaviours { get; private set; }

    [SerializeField, ReadOnly] SceneType sceneType;

    bool minigameStarted = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        for (int i = 0; i < InputSystem.devices.Count; i++)
        {
            Debug.Log(InputSystem.devices[i].displayName);
        }
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneChange;
        InputSystem.onDeviceChange += OnDeviceChange;
        PlayerInputManager.instance.onPlayerJoined += OnPlayerJoined;
        PlayerInputManager.instance.onPlayerLeft += OnPlayerLeft;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneChange;
        InputSystem.onDeviceChange -= OnDeviceChange;
        PlayerInputManager.instance.onPlayerJoined -= OnPlayerJoined;
        PlayerInputManager.instance.onPlayerLeft -= OnPlayerLeft;
    }



    private void Init()
    {
        Cursor.lockState = CursorLockMode.Locked;

        if (sceneType == SceneType.MarioParty)
        {
            //GeneralSpawner.Instance.SpawnObject(0, new Vector3(12, 11.5f, 0));
            //GeneralSpawner.Instance.SpawnObject(0, new Vector3(Random.Range(-3, 4), 3, Random.Range(-3, 4)));
        }

        Players = GameObject.FindGameObjectsWithTag("Player").ToList();
        Cameras = FindObjectsByType<Camera>().ToList();
        MinigameBehaviours = GameObject.FindGameObjectsWithTag("MinigameBehaviour").ToList();
        //foreach (var item in Players) Debug.Log(item, item);
        //foreach (var item in Cameras) Debug.Log(item, item);
        foreach (var item in MinigameBehaviours)
        {
            //Debug.Log(item, item);
            item.SetActive(false);
        }

        //MinigameStart();
    }

    void OnSceneChange(Scene scene, LoadSceneMode _ = 0)
    {
        sceneType = Enum.TryParse(scene.name, true, out SceneType result) ? result : SceneType.None;
        if (sceneType == SceneType.None)
        {
            Debug.LogError("Le nom de la sc�ne n'est pas reconnue. Assurez-vous que le nom match les valeurs du Enum SceneType sauf SceneType.None");
            return;
        }

        Init();
    }
    void OnDeviceChange(InputDevice device, InputDeviceChange change)
    {
        switch (change)
        {
            case InputDeviceChange.Added:
            case InputDeviceChange.Reconnected:
                Debug.Log($"Device connected: {device.displayName}");
                Debug.Log(device.deviceId);
                Debug.Log(device.layout);
                break;
            case InputDeviceChange.Disconnected:
                Debug.Log($"Device disconnected: {device.displayName}");
                break;
        }
    }
    void OnPlayerJoined(PlayerInput playerInput)
    {
        Debug.Log($"{playerInput} has joined");
        Debug.Log(PlayerInput.all.Count);
        Debug.Log(playerInput.name);
        Debug.Log(playerInput.playerIndex);
        Debug.Log(playerInput.currentControlScheme);
        //Debug.Log(playerInput.actions);
        //foreach (var item in playerInput.actions)
        //{
        //    Debug.Log(item);
        //}
        //Debug.Log(playerInput.actionEvents);
        //for (int i = 0; i < playerInput.actionEvents.Count; i++)
        //{
        //    Debug.Log(playerInput.actionEvents[i]);
        //}
        //Debug.Log(playerInput.currentActionMap);
        //foreach (var item in playerInput.currentActionMap)
        //{
        //    Debug.Log(item);
        //}
        Debug.Log(playerInput.devices);
        for (int i = 0; i < playerInput.devices.Count; i++)
        {
            Debug.Log(playerInput.devices[i]);
        }

        if (!minigameStarted) MinigameStart();
    }
    void OnPlayerLeft(PlayerInput playerInput)
    {
        Debug.Log($"{playerInput} has left");
    }

    void ChangeScene(int scene)
    {
        SceneManager.LoadScene(scene);
    }
    void ChangeScene()
    {
        SceneManager.LoadScene(0);
    }

    public void MinigameStart()
    {
        OnMinigameStart?.Invoke();
        foreach (var item in MinigameBehaviours)
        {
            item.SetActive(true);
        }
    }
    public void MinigameEnd()
    {
        OnMinigameEnd?.Invoke();
        Invoke(nameof(ChangeScene), 5);
    }

    public void SwitchPlayer(GameObject player)
    {
        PlayerInput pi = player.GetComponent<PlayerInput>();
        PlayerInputManager.instance.JoinPlayer(pi.playerIndex, controlScheme: pi.currentControlScheme, pairWithDevices: pi.devices.ToArray());
    }
}
