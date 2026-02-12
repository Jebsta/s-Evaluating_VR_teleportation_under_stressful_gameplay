using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using static ConditionManager;


public class GameManager : MonoBehaviour
{
    [Header("Verbindungen")]
    public GameTimer myTimer; 
    public GameObject settingWindow;
    public GameObject taskWindow;
    public GameObject experimentWindow;
    public TextMeshProUGUI colorDisplay;
    public TextMeshProUGUI roundTimerText;
    public TextMeshProUGUI taskDescriptionText;
    public GameObject centerPlatform;
    public InputFieldManager inputFieldManager;

    // NEU: Verbindung zum Condition Manager
    public ConditionManager myConditionManager;

    //Verbindung zu Study_Logger
    public Study_Logger Study_Logger;

    //Verbindung zu Teleportation Controller
    public TeleportationModeController TeleportationModeController;
    
    //Verbindung zu TeleportLogger
    public TeleportLogger TeleportLogger;
    public XRRayInteractor XRRayInteractor;
    public StudyManager StudyManager;

    // Plattform-Farben
    string[] possibleColors = { "Blue", "Yellow", "Cyan", "Green", "Red", "Purple" };
    // Farben, die im aktuellen Durchlauf noch übrig sind
    private List<string> remainingColors;

    public bool isGameRunning = false;
    public bool isPracticeRun = false;

    // Spieler-Performance
    public int scoreCorrect = 0;
    public int scoreWrong = 0;
    public int scoreSkipped = 0;
    public int totalPlatforms => scoreCorrect + scoreWrong + scoreSkipped;

    public string currentTargetColor = "";

    float roundTimer = 0f;
    public float timePerRound = 5f;   // z.B. 5 Sekunden 
    public enum EndCondition { TimeLimit, RoundsCompleted, TimeOrRounds, TimeAndRounds }
    public EndCondition endCondition;
    public int platformMax = 6; // z.B. 6 Runden
    public int timeMax = 180; // z.B. 180 Sekunden

    //Variablen für TeleportLogger
    float teleportMoveTime = 0f;
    float teleportAimTime = 0f;
    string teleportStartColor = "Base";

    // Variablen für Study Progress
    int currentTrialIndex = 0;

    private string lastPlatformName = "";

    void Start()
    {
        // Initialisierung
        isGameRunning = false;
        scoreCorrect = 0;
        scoreWrong = 0;
        scoreSkipped = 0;
        currentTargetColor = "";
        if (settingWindow != null)
        {
            settingWindow.SetActive(true);
        }
        if (experimentWindow != null)
        {
            experimentWindow.SetActive(false);
        }
        if (centerPlatform != null)
        {
            centerPlatform.SetActive(true);
        }
        if (taskWindow != null)
        {
            taskWindow.SetActive(false);
        }
    }

    // Diese Funktion prüft STÄNDIG (jedes Frame), ob die Zeit um ist
    void Update()
    {
        // Globaler Spieltimer
        if (isGameRunning && myTimer != null)
        {
            switch (endCondition)
            {
                case EndCondition.TimeLimit:
                    if (myTimer.currentTime >= timeMax)
                    {
                        EndGame();
                    }
                    break;
                case EndCondition.RoundsCompleted:
                    if (totalPlatforms == platformMax)
                    {
                        EndGame();
                    }
                    break;
                case EndCondition.TimeOrRounds:
                    if (myTimer.currentTime >= timeMax || totalPlatforms == platformMax)
                    {
                        EndGame();
                    }
                    break;
                case EndCondition.TimeAndRounds:
                    if (myTimer.currentTime >= timeMax && totalPlatforms == platformMax)
                    {
                        EndGame();
                    }
                    break;
            }
        }

        // Time-Limit-Stressor
        if (isGameRunning
            && myConditionManager != null
            && myConditionManager.currentMode == ConditionManager.StressMode.TimeLimit)
        {
            roundTimer += Time.deltaTime;

            float timeLeft = Mathf.Max(0f, timePerRound - roundTimer);

            // UI aktualisieren
            if (roundTimerText != null)
            {
                roundTimerText.text = timeLeft.ToString("F1") + " s";
            }

            if (roundTimer >= timePerRound)
            {
                Debug.Log("Rundenzeit abgelaufen! Neue Farbe wird gesetzt.");

                PickNewColor();

                scoreSkipped++;

                roundTimer = 0f;
            }
        }
        else
        {
            // Falls kein TimeLimit aktiv → Anzeige ausblenden
            if (roundTimerText != null)
            {
                roundTimerText.text = "";
            }
        }

        if (!isGameRunning || XRRayInteractor == null)
            return;

        if (XRRayInteractor.isActiveAndEnabled)
        {
            teleportAimTime += Time.deltaTime;
        }
        else
        {
            teleportMoveTime += Time.deltaTime;
        }

    }

    public void SetupExperiment()
    {
        int participant_ID = inputFieldManager.getID();
        List<TrialCondition> conditionsList = myConditionManager.GenerateLatinSquareConditionList(participant_ID);

        if (settingWindow != null)
        {
            settingWindow.SetActive(false);
        }

        // Debug-Ausgabe der conditionsList sicher und aussagekräftig
        if (conditionsList == null)
        {
            Debug.Log($"SetupExperiment: participant_ID={participant_ID} -> conditionsList ist null");
            return;
        }

        var parts = new List<string>();
        foreach (TrialCondition item in conditionsList)
        {
            parts.Add(item.ScenarioId() + "-" + item.StressorId());
        }

        string listAsString = parts.Count == 0 ? "(leer)" : string.Join(", ", parts);
        Debug.Log($"SetupExperiment: participant_ID={participant_ID} | conditionsList Count={parts.Count} -> {listAsString}");

        currentTrialIndex = 0;

        StudyManager.SetupButtons(conditionsList);

        if (experimentWindow != null)
        {
            experimentWindow.SetActive(true);
        }
    }
    public void StartTheWholeGame(bool practice = false)
    {

        Debug.Log("Das Spiel startet jetzt!");

        taskDescriptionText.SetText("Teleport to Platform according to text-color");

        isGameRunning = true;
        isPracticeRun = practice;
        if (practice)
        {
            Debug.Log("Dies ist ein Übungsdurchlauf.");
        }

        scoreCorrect = 0;
        scoreWrong = 0;
        scoreSkipped = 0;

        teleportMoveTime = 0f;
        teleportAimTime = 0f;
        teleportStartColor = "Base";

        roundTimer = 0f;

        ResetColorPool();

        Debug.Log("Modus: " + (myConditionManager != null ? myConditionManager.currentMode.ToString() : "Kein ConditionManager verbunden"));

        // 1. Timer starten
        if(myTimer != null) 
        {
            myTimer.StartRunning();
        }

        if (myTimer != null && myTimer.timerText != null)
        {
            bool showGlobalTimer =
                myConditionManager.currentMode != ConditionManager.StressMode.TimeLimit;

            myTimer.SetVisibility(showGlobalTimer);
        }

        // 2. Elemente ausblenden
        if (experimentWindow != null)
        {
            experimentWindow.SetActive(false);
        }
        if (centerPlatform != null)
        {
            centerPlatform.SetActive(false);
        }

        // 3. Farbtext setzten
        PickNewColor();

        // 4. Task Window Einblenden

        if (taskWindow != null)
        {
            taskWindow.SetActive(true);
        }
    }

    public void EndGame()
    {
        isGameRunning = false;

        taskDescriptionText.SetText("Fill out the questionair and then select the next task");

        if (colorDisplay != null)
        {
            colorDisplay.text = "Fertig!";
            colorDisplay.color = Color.white; 
        }

        if (myTimer != null)
        {
            myTimer.StopTimer();
        }


        // 2. Menü einblenden
        if (centerPlatform != null)
        {
            centerPlatform.SetActive(true);
        }

        //Daten die geloggt werden
        int currentStressor = myConditionManager.GetCurrentStressModeID();
        int participant_ID = inputFieldManager.getID();
        string teleportationType;
        if (TeleportationModeController.IsOrientationalTeleportaionActive())
        {
            teleportationType = "A";
        }
        else
        {
            teleportationType = "B";
        }
        if (!isPracticeRun)
        {
            currentTrialIndex++;
            Study_Logger.LogInteraction(participant_ID, teleportationType, currentStressor, scoreWrong, scoreCorrect, scoreSkipped, myTimer.GetFormattedTime());
            Study_Logger.SaveToCSV();

            if (myConditionManager.currentMode == StressMode.TimeLimit)
            {
                TeleportLogger.LogTeleport(inputFieldManager.getID(), teleportationType, currentStressor, teleportStartColor, currentTargetColor, "None", false, teleportMoveTime, teleportAimTime);
                TeleportLogger.SaveTeleportsToCSV();
            }
        }

        Debug.Log(myTimer.GetFormattedTime());

        Debug.Log("Spielzeit abgelaufen! Endergebnis - Richtig: " + scoreCorrect);

        StudyManager.UpdateButtonState();
    }

    public int GetCurrentTrialIndex()
    {
        return currentTrialIndex;
    }

    public void PickNewColor(string colorToAvoid = "")
    {
        if (!isGameRunning || colorDisplay == null) return;


        if (remainingColors == null || remainingColors.Count == 0)
        {
            ResetColorPool();
        }


        while (remainingColors[0] == colorToAvoid)
        {
            // Farbe vermeiden, die gerade benutzt wurde
            remainingColors.Add(remainingColors[0]);
            remainingColors.RemoveAt(0);
        }


        // Nächste Farbe aus der gemischten Liste
        string newColor = remainingColors[0];
        remainingColors.RemoveAt(0);

        currentTargetColor = newColor;
        colorDisplay.color = GetRealColor(newColor);

        // Color Mismatch Stressor
        if (myConditionManager != null &&
            myConditionManager.currentMode == ConditionManager.StressMode.ColorMismatch)
        {
            string wrongColorName = newColor;
            while (wrongColorName == newColor)
            {
                wrongColorName = possibleColors[Random.Range(0, possibleColors.Length)];
            }
            colorDisplay.text = wrongColorName;
        }
        else
        {
            colorDisplay.text = newColor;
        }
    }


    void ResetColorPool()
    {
        remainingColors = new List<string>(possibleColors);
        ShuffleList(remainingColors);

        Debug.Log("Farbpool zurückgesetzt für neue Runde.");
    }

    public int GetParticipantID()
    {
        return inputFieldManager.getID();
    }

    void ShuffleList(List<string> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int rnd = Random.Range(i, list.Count);
            string temp = list[i];
            list[i] = list[rnd];
            list[rnd] = temp;
        }
    }

    public void CheckIfCorrect(string platformName)
    {
        if (!isGameRunning || currentTargetColor == "") return;

        bool isCorrect = platformName.Contains(currentTargetColor);

        if (isCorrect)
        {
            scoreCorrect++;
            Debug.Log("RICHTIG! Richtig: " + scoreCorrect + " | Falsch: " + scoreWrong);
        }
        else
        {
            scoreWrong++;
            Debug.Log("FALSCH! Richtig: " + scoreCorrect + " | Falsch: " + scoreWrong);
        }

        roundTimer = 0f;

        int currentStressor = myConditionManager.GetCurrentStressModeID();
        int participant_ID = inputFieldManager.getID();
        string teleportationType;
        if (TeleportationModeController.IsOrientationalTeleportaionActive())
        {
            teleportationType = "A";
        }
        else
        {
            teleportationType = "B";
        }
        if (!isPracticeRun)
        {
            TeleportLogger.LogTeleport(inputFieldManager.getID(), teleportationType, currentStressor, teleportStartColor, currentTargetColor, platformName, isCorrect, teleportMoveTime, teleportAimTime);
            TeleportLogger.SaveTeleportsToCSV();
        }
        // Reset
        teleportMoveTime = 0f;
        teleportAimTime = 0f;
        teleportStartColor = platformName;


        PickNewColor(platformName);
    }

    public void OnPlatformTeleport(string platformName)
    {
        if (platformName != lastPlatformName)
        {
            //Nur debuggen, um sicherzugehen
            Debug.Log("Ich stehe jetzt auf: " + platformName);
            lastPlatformName = platformName;
            CheckIfCorrect(platformName);
        }
    }

    Color GetRealColor(string colorName)
    {
        switch (colorName)
        {
            case "Blue": return Color.blue;
            case "Yellow": return Color.yellow;
            case "Cyan": return Color.cyan;
            case "Green": return Color.green;
            case "Red": return Color.red;
            case "Purple": return Color.magenta; 
            default: return Color.white;
        }
    }
}