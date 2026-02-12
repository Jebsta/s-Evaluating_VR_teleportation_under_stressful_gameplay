using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static ConditionManager;

public class StudyManager : MonoBehaviour
{
    public ConditionManager ConditionManager;
    public GameManager GameManager;

    public GameObject practiceButtonX;

    public GameObject studyButtonX0;
    public GameObject studyButtonX1;
    public GameObject studyButtonX2;
    public GameObject studyButtonX3;

    public GameObject practiceButtonY;

    public GameObject studyButtonY0;
    public GameObject studyButtonY1;
    public GameObject studyButtonY2;
    public GameObject studyButtonY3;

    public TextMeshProUGUI headerText;

    public Color todoColor = new Color(0.7843137f, 0.7843137f, 0.7843137f, 1f); // Light gray
    public Color doneColor = new Color(0.5f, 0.75f, 0.5f, 1f); // Light green

    public void SetupButtons(List<TrialCondition> conditionsList)
    {
        // set button texts

        practiceButtonX.GetComponentInChildren<TextMeshProUGUI>().text = $"Practice {conditionsList[0].ScenarioId()}\n{conditionsList[0].Scenario}";
        studyButtonX0.GetComponentInChildren<TextMeshProUGUI>().text = $"Study {conditionsList[0].ScenarioId()} - {conditionsList[0].StressorId()}\n{conditionsList[0].Stressor}";
        studyButtonX1.GetComponentInChildren<TextMeshProUGUI>().text = $"Study {conditionsList[1].ScenarioId()} - {conditionsList[1].StressorId()}\n{conditionsList[1].Stressor}";
        studyButtonX2.GetComponentInChildren<TextMeshProUGUI>().text = $"Study {conditionsList[2].ScenarioId()} - {conditionsList[2].StressorId()}\n{conditionsList[2].Stressor}";
        studyButtonX3.GetComponentInChildren<TextMeshProUGUI>().text = $"Study {conditionsList[3].ScenarioId()} - {conditionsList[3].StressorId()}\n{conditionsList[3].Stressor}";

        practiceButtonY.GetComponentInChildren<TextMeshProUGUI>().text = $"Practice {conditionsList[4].ScenarioId()}\n{conditionsList[4].Scenario}";
        studyButtonY0.GetComponentInChildren<TextMeshProUGUI>().text = $"Study {conditionsList[4].ScenarioId()} - {conditionsList[4].StressorId()}\n{conditionsList[4].Stressor}";
        studyButtonY1.GetComponentInChildren<TextMeshProUGUI>().text = $"Study {conditionsList[5].ScenarioId()} - {conditionsList[5].StressorId()}\n{conditionsList[5].Stressor}";
        studyButtonY2.GetComponentInChildren<TextMeshProUGUI>().text = $"Study {conditionsList[6].ScenarioId()} - {conditionsList[6].StressorId()}\n{conditionsList[6].Stressor}";
        studyButtonY3.GetComponentInChildren<TextMeshProUGUI>().text = $"Study {conditionsList[7].ScenarioId()} - {conditionsList[7].StressorId()}\n{conditionsList[7].Stressor}";
    
        // set button actions
        practiceButtonX.GetComponentInChildren<Button>().onClick.AddListener(delegate { ConditionManager.SetTailCondition(new TrialCondition(conditionsList[0].Scenario, StressMode.None)); GameManager.StartTheWholeGame(true); });

        studyButtonX0.GetComponentInChildren<Button>().onClick.AddListener(delegate { ConditionManager.SetTailCondition(conditionsList[0]); GameManager.StartTheWholeGame(false); });
        studyButtonX1.GetComponentInChildren<Button>().onClick.AddListener(delegate { ConditionManager.SetTailCondition(conditionsList[1]); GameManager.StartTheWholeGame(false); });
        studyButtonX2.GetComponentInChildren<Button>().onClick.AddListener(delegate { ConditionManager.SetTailCondition(conditionsList[2]); GameManager.StartTheWholeGame(false); });
        studyButtonX3.GetComponentInChildren<Button>().onClick.AddListener(delegate { ConditionManager.SetTailCondition(conditionsList[3]); GameManager.StartTheWholeGame(false); });

        practiceButtonY.GetComponentInChildren<Button>().onClick.AddListener(delegate { ConditionManager.SetTailCondition(new TrialCondition(conditionsList[4].Scenario, StressMode.None)); GameManager.StartTheWholeGame(true); });
        studyButtonY0.GetComponentInChildren<Button>().onClick.AddListener(delegate { ConditionManager.SetTailCondition(conditionsList[4]); GameManager.StartTheWholeGame(false); });
        studyButtonY1.GetComponentInChildren<Button>().onClick.AddListener(delegate { ConditionManager.SetTailCondition(conditionsList[5]); GameManager.StartTheWholeGame(false); });
        studyButtonY2.GetComponentInChildren<Button>().onClick.AddListener(delegate { ConditionManager.SetTailCondition(conditionsList[6]); GameManager.StartTheWholeGame(false); });
        studyButtonY3.GetComponentInChildren<Button>().onClick.AddListener(delegate { ConditionManager.SetTailCondition(conditionsList[7]); GameManager.StartTheWholeGame(false); });

        UpdateButtonState();
    }

    public void UpdateButtonState()
    {

        // set header text
        headerText.text = $"Participant ID: {GameManager.GetParticipantID()}";

        int currentTrialIndex = GameManager.GetCurrentTrialIndex();

        ColorBlock buttonColors = practiceButtonX.GetComponentInChildren<Button>().colors;

        ColorBlock todoButtonColors = buttonColors;
        todoButtonColors.disabledColor = todoColor;

        ColorBlock doneButtonColors = buttonColors;
        doneButtonColors.disabledColor = doneColor;


        practiceButtonX.GetComponentInChildren<Button>().interactable = currentTrialIndex <= 3;

        studyButtonX0.GetComponentInChildren<Button>().interactable = currentTrialIndex == 0;
        studyButtonX1.GetComponentInChildren<Button>().interactable = currentTrialIndex == 1;
        studyButtonX2.GetComponentInChildren<Button>().interactable = currentTrialIndex == 2;
        studyButtonX3.GetComponentInChildren<Button>().interactable = currentTrialIndex == 3;

        studyButtonX0.GetComponentInChildren<Button>().colors = currentTrialIndex > 0 ? doneButtonColors : todoButtonColors;
        studyButtonX1.GetComponentInChildren<Button>().colors = currentTrialIndex > 1 ? doneButtonColors : todoButtonColors;
        studyButtonX2.GetComponentInChildren<Button>().colors = currentTrialIndex > 2 ? doneButtonColors : todoButtonColors;
        studyButtonX3.GetComponentInChildren<Button>().colors = currentTrialIndex > 3 ? doneButtonColors : todoButtonColors;


        practiceButtonY.GetComponentInChildren<Button>().interactable = currentTrialIndex <= 7 && currentTrialIndex >=4;
        studyButtonY0.GetComponentInChildren<Button>().interactable = currentTrialIndex == 4;
        studyButtonY1.GetComponentInChildren<Button>().interactable = currentTrialIndex == 5;
        studyButtonY2.GetComponentInChildren<Button>().interactable = currentTrialIndex == 6;
        studyButtonY3.GetComponentInChildren<Button>().interactable = currentTrialIndex == 7;

        studyButtonY0.GetComponentInChildren<Button>().colors = currentTrialIndex > 4 ? doneButtonColors : todoButtonColors;
        studyButtonY1.GetComponentInChildren<Button>().colors = currentTrialIndex > 5 ? doneButtonColors : todoButtonColors;
        studyButtonY2.GetComponentInChildren<Button>().colors = currentTrialIndex > 6 ? doneButtonColors : todoButtonColors;
        studyButtonY3.GetComponentInChildren<Button>().colors = currentTrialIndex > 7 ? doneButtonColors : todoButtonColors;

        if (currentTrialIndex > 7)
        {
            headerText.text = $"Participant ID: {GameManager.GetParticipantID()} - All trials completed!";
            PlayerPrefs.SetInt("ParticipantID", GameManager.GetParticipantID() + 1);
        }
    }

}
