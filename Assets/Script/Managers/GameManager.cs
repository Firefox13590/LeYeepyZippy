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

    [SerializeField, ReadOnly] SceneType currentSceneType;


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
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneChange;
        InputSystem.onDeviceChange -= OnDeviceChange;
    }



    private void Init()
    {
        Cursor.lockState = CursorLockMode.Locked;

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
        currentSceneType = Enum.TryParse(scene.name, true, out SceneType result) ? result : SceneType.None;
        if (currentSceneType == SceneType.None)
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
