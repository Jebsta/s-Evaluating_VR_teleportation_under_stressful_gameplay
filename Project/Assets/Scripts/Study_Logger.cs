using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Study_Logger : MonoBehaviour
{

    public ConditionManager ConditionManager;
    public GameManager GameManager;
    public string logFileName = "interaction_logs.csv";

    // Datenstruktur für eine Interaktion
    public class InteractionData
    {
        public int participantID;
        public string teleportationType;  // "normal" oder "orientational"
        public int stressor;   // 0 "none", 1 "time", 2 "mismatch", 3 "shrinking"
        public int wrong;         // Anzahl Fehler
        public int correct;         // Anzahl korrekte Teleportationen
        public int skipped;         // Gesamtanzahl der Platformen
        public string completionTime;   // Zeit von erster Interaktion bis Drücken des "Fertig"-Buttons
    }

    // Liste, um alle Interaktionen zu speichern
    private List<InteractionData> interactionLogs = new List<InteractionData>();


    private void Start()
    {
        // Initialisiere Log Filename mit Timestamp
        string timestamp = System.DateTime.Now.ToString("yyyyMMdd_HHmmss");
        logFileName = $"{timestamp}_StudyLog.csv";
    }

    // Methode, um eine neue Interaktion zu loggen
    public void LogInteraction(int participantID, string teleportationType, int stressor, int wrong, int correct, int skipped, string completionTime)
    {
        // Welcher Stressor ist aktiv? 
        string currentStressor = ConditionManager.currentMode.ToString();

        //Timer setzen
        GameTimer timer = GameManager.myTimer;


        // Erstelle eine neue Interaktionsdateninstanz
        InteractionData newData = new InteractionData
        {
            participantID = participantID,
            teleportationType = teleportationType,
            stressor = stressor,
            wrong = wrong,
            correct = correct,
            skipped = skipped,
            completionTime = completionTime,
        };

        // Füge die Interaktionsdaten zur Liste hinzu
        interactionLogs.Add(newData);

        UnityEngine.Debug.Log("Interaktion geloggt für Teilnehmer: " + participantID);
    }

    // Methode, um die Daten in eine CSV-Datei zu speichern
    public void SaveToCSV()
    {
        // Verwende den persistenten Speicherpfad
        string filePath = Path.Combine(Application.persistentDataPath, logFileName);

        // Prüfe, ob die Datei schon existiert
        bool fileExists = File.Exists(filePath);

        using (StreamWriter writer = new StreamWriter(filePath, append: true))
        {
            // Schreibe den Header nur, wenn die Datei neu erstellt wird
            if (!fileExists)
            {
                writer.WriteLine("timestamp; pid; teleportID; stressorID; wrong; correct; skipped; totalTime");
            }

            // Schreibe alle Interaktionsdaten
            foreach (var data in interactionLogs)
            {
                writer.WriteLine(
                    $"{System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")};" +
                    $"{data.participantID};" +
                    $"{data.teleportationType};" +
                    $"{data.stressor};" +
                    $"{data.wrong};" +
                    $"{data.correct};" +
                    $"{data.skipped};" +
                    $"{data.completionTime}"
                );
            }

            UnityEngine.Debug.Log("CSV-Datei erfolgreich aktualisiert unter: " + filePath);
            UnityEngine.Debug.Log("Speicherort der CSV-Datei: " + Application.persistentDataPath);
            interactionLogs.Clear(); // Leere die Liste nach dem Speichern
        }
    }

}
