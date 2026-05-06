using System;
using UnityEngine;
using UnityEngine.SceneManagement;

//made to have anything in App assembly definition
public class GameController : MonoBehaviour
{
    [SerializeField]
    private int gameSceneBuildIndex;
    public EvaluationMode StartingEvaluationMode { get; private set; }

    public static GameController Instance { get; private set; }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject.transform.root.gameObject);
        Instance = this;
    }

    public void SetupStartingEvaluationMode(EvaluationMode mode)
    {
        StartingEvaluationMode = mode;
    }

    public void LoadGame()
    {
        SceneManager.LoadScene(gameSceneBuildIndex);
    }
}
