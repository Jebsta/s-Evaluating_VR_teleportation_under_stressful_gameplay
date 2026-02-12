using System.Collections.Generic;
using UnityEngine;

public class ConditionManager : MonoBehaviour
{
    // 1. Wir definieren die Typen, damit andere Skripte sie lesen können
    public enum StressMode { None, TimeLimit, ColorMismatch, Shrinking }
    public enum ScenarioType { Directional, Normal }

    // 2. Diese Variable speichert den aktuellen Zustand
    public StressMode currentMode = StressMode.None;

    [Header("Stressor Objects")]
    public GameObject noStressor;
    public GameObject timeLimitStressor;
    public GameObject colorTextMismatchStressor;
    public GameObject shrinkingStressor;

    [Header("Teleportation Types")]
    public TeleportationModeController teleportationController;

    void DisableAll()
    {
        noStressor?.SetActive(false);
        timeLimitStressor?.SetActive(false);
        colorTextMismatchStressor?.SetActive(false);
        shrinkingStressor?.SetActive(false);
    }

    // Ich habe hier "StressMode newMode" hinzugefügt
    void ActivateStressor(GameObject stressor, StressMode newMode)
    {
        DisableAll();
        
        if (stressor != null) stressor.SetActive(true);

        // 3. Hier speichern wir den neuen Modus!
        currentMode = newMode;
        
        Debug.Log("Stressor gesetzt auf: " + newMode);
    }


    // Balanced Latin Square (4 conditions)
    private static readonly StressMode[][] balancedLatinSquare =
    {
        new[] { StressMode.None, StressMode.TimeLimit, StressMode.ColorMismatch, StressMode.Shrinking },
        new[] { StressMode.TimeLimit, StressMode.Shrinking, StressMode.None, StressMode.ColorMismatch },
        new[] { StressMode.ColorMismatch, StressMode.None, StressMode.Shrinking, StressMode.TimeLimit },
        new[] { StressMode.Shrinking, StressMode.ColorMismatch, StressMode.TimeLimit, StressMode.None }
    };

    public struct TrialCondition
    {
        public ScenarioType Scenario;
        public StressMode Stressor;

        public TrialCondition(ScenarioType scenario, StressMode stressor)
        {
            Scenario = scenario;
            Stressor = stressor;
        }

        public override string ToString()
        {
            return $"Scenario: {Scenario}, Stressor: {Stressor}";
        }

        public string ScenarioId()
        {
            return Scenario == ScenarioType.Directional ? "A" : "B";
        }

        public string StressorId()
        {
            return Stressor switch
            {
                StressMode.None => "0",
                StressMode.TimeLimit => "1",
                StressMode.ColorMismatch => "2",
                StressMode.Shrinking => "3",
                _ => "Unknown"
            };
        }
    }

    public List<TrialCondition> GenerateLatinSquareConditionList(int participantID)
    {
        var conditions = new List<TrialCondition>();

        // Szenario-Reihenfolge: ungerade = A zuerst, gerade = B zuerst
        ScenarioType first = (participantID % 2 == 1)
            ? ScenarioType.Directional
            : ScenarioType.Normal;

        ScenarioType second = (first == ScenarioType.Directional)
            ? ScenarioType.Normal
            : ScenarioType.Directional;

        // Balanced Latin Square Sequenz bestimmen
        int seqIndex = ((participantID - 1) / 2) % 4;
        StressMode[] sequence = balancedLatinSquare[seqIndex];

        // Block 1 (4 Trials)
        foreach (var stress in sequence)
            conditions.Add(new TrialCondition(first, stress));

        // Block 2 (4 Trials)
        foreach (var stress in sequence)
            conditions.Add(new TrialCondition(second, stress));

        return conditions;
    }



    // --- Wrapper Methods for Buttons ---

    public void NoStress() =>
        ActivateStressor(noStressor, StressMode.None);

    public void Time() =>
        ActivateStressor(timeLimitStressor, StressMode.TimeLimit);

    public void Mismatch() =>
        ActivateStressor(colorTextMismatchStressor, StressMode.ColorMismatch);

    public void Shrinking() =>
        ActivateStressor(shrinkingStressor, StressMode.Shrinking);

    public void SetOrietationalTeleportation(bool setTrue)
    {
        Debug.Log(setTrue);
        if (setTrue) {
            teleportationController.SetOrientationalTeleportation();
            Debug.Log("Teleportation mit Rotation!");
        }
        else
        {
            teleportationController.SetNormalTeleportation();
            Debug.Log("Teleportation ohne Rotation!");
        }
    }

    public void SelectStressMode(int stressID)
    {
        Debug.Log((StressMode)stressID + " " + stressID);
        switch((StressMode) stressID)
        {
            case StressMode.None:
                NoStress();
                break;
            case StressMode.TimeLimit:
                Time();
                break;
            case StressMode.ColorMismatch:
                Mismatch();
                break;
            case StressMode.Shrinking:
                Shrinking();
                break;
            default:
                DisableAll();
                break;
        }
    }

    public void SetTailCondition(TrialCondition trailCondition)
    {
        switch (trailCondition.Stressor)
        {
            case StressMode.None:
                NoStress();
                break;
            case StressMode.TimeLimit:
                Time();
                break;
            case StressMode.ColorMismatch:
                Mismatch();
                break;
            case StressMode.Shrinking:
                Shrinking();
                break;
            default:
                DisableAll();
                break;
        }

        switch (trailCondition.Scenario)
        {
            case ScenarioType.Directional:
                teleportationController.SetOrientationalTeleportation();
                break;
            case ScenarioType.Normal:
                teleportationController.SetNormalTeleportation();
                break;
        }
    }

    public int GetCurrentStressModeID()
    {
        return (int)currentMode;
    }
}