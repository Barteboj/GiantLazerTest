using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StartMenuController : MonoBehaviour
{
    [SerializeField]
    private TMP_Dropdown evaluationModeDropdown;
    [SerializeField]
    private Button startGameButton;
    private EvaluationMode[] evaluationModes;

    private void Awake()
    {
        evaluationModes = Enum.GetValues(typeof(EvaluationMode)) as EvaluationMode[];
        foreach (EvaluationMode mode in evaluationModes)
        {
            evaluationModeDropdown.options.Add(new TMP_Dropdown.OptionData(mode.ToString()));
        }
    }

    private void OnEnable()
    {
        startGameButton.onClick.AddListener(OnStartGameButtonClicked);
    }

    private void OnDisable()
    {
        startGameButton.onClick.RemoveListener(OnStartGameButtonClicked);
    }

    private void OnStartGameButtonClicked()
    {
        GameController.Instance.SetupStartingEvaluationMode(evaluationModes[evaluationModeDropdown.value]);
        GameController.Instance.LoadGame();
    }
}