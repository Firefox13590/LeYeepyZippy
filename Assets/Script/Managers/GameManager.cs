using Data;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public static Action OnMinigameStart, OnMinigameEnd;

    public List<GameObject> Players { get; private set; }
    public int PlayerCount => Players.Count;
    public List<Camera> Cameras { get; private set; }
    public List<GameObject> MinigameBehaviours { get; private set; }

    [SerializeField, ReadOnly] SceneType sceneType;


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
            GeneralSpawner.Instance.SpawnObject(0, new Vector3(12, 11.5f, 0));
            GeneralSpawner.Instance.SpawnObject(0, 4);
        }

        Players = GameObject.FindGameObjectsWithTag("Player").ToList();
        Cameras = FindObjectsByType<Camera>().ToList();
        MinigameBehaviours = GameObject.FindGameObjectsWithTag("MinigameBehaviour").ToList();
        //foreach (var item in Players) Debug.Log(item, item);
        //foreach (var item in Cameras) Debug.Log(item, item);
        //foreach (var item in MinigameBehaviours) Debug.Log(item, item);

        MinigameStart();
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
                break;
            case InputDeviceChange.Disconnected:
                Debug.Log($"Device disconnected: {device.displayName}");
                break;
        }
    }
    void OnPlayerJoined(PlayerInput playerInput)
    {
        Debug.Log($"{playerInput} has joined");
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

    public void MinigameStart() => OnMinigameStart?.Invoke();
    public void MinigameEnd()
    {
        OnMinigameEnd?.Invoke();
        Invoke(nameof(ChangeScene), 5);
    }
}
