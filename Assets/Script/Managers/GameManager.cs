using Data;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Linq;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public static Action OnMinigameStart, OnMinigameEnd;

    public int PlayerCount => players.Count;

    [SerializeField, ReadOnly] SceneType currentSceneType;

    List<GameObject> players, cameras;

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

        OnSceneChange(SceneManager.GetActiveScene());
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneChange;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneChange;
    }



    private void Init()
    {
        Cursor.lockState = CursorLockMode.Locked;
        players = GameObject.FindGameObjectsWithTag("Player").ToList();

        MinigameStart();
    }

    void OnSceneChange(Scene scene, LoadSceneMode _ = 0)
    {
        currentSceneType = Enum.TryParse(scene.name, true, out SceneType result) ? result : SceneType.None;
        if (currentSceneType == SceneType.None)
        {
            Debug.LogError("Le nom de la scène n'est pas reconnue. Assurez-vous que le nom match les valeurs du Enum SceneType sauf SceneType.None");
            return;
        }

        Init();
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
