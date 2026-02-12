using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TeleportLogger : MonoBehaviour
{

    public class TeleportInteractionData
    {
        public int participantID;
        public string teleportationType;  // "normal" oder "orientational"
        public int stressor;   // "none", "time", "mismatch", "shrinking"
        public string startColor;
        public string targetColor;
        public string teleportedColor;
        public bool correct;
        public float moveTime;   // Sekunden
        public float aimTime;    // Sekunden
        public float totalTime;  // Sekunden
    }

    public string teleportLogFileName = "TeleportLog.csv";

    private List<TeleportInteractionData> teleportLogs = new List<TeleportInteractionData>();


    private void Start()
    {
        // Initialisiere Log Filename mit Timestamp
        string timestamp = System.DateTime.Now.ToString("yyyyMMdd_HHmmss");
        teleportLogFileName = $"{timestamp}_TeleportLog.csv";
    }

    // Teleportation loggen
    public void LogTeleport(
        int participantID,
        string teleportationType,  // A "directional", B "normal"
        int stressor,   // "none", "time", "mismatch", "shrinking"
        string startColor,
        string targetColor,
        string teleportedColor,
        bool correct,
        float moveTime,
        float aimTime)
    {
        TeleportInteractionData data = new TeleportInteractionData
        {
            participantID = participantID,
            teleportationType = teleportationType,
            stressor = stressor,
            startColor = startColor.Replace("Teleport Platform ",""),
            targetColor = targetColor,
            teleportedColor = teleportedColor.Replace("Teleport Platform ", ""),
            correct = correct,
            moveTime = moveTime,
            aimTime = aimTime,
            totalTime = moveTime + aimTime
        };

        teleportLogs.Add(data);

        Debug.Log($"Teleport geloggt | PID {participantID} | {startColor} → {teleportedColor}");
    }

    public void SaveTeleportsToCSV()
    {
        string filePath = Path.Combine(Application.persistentDataPath, teleportLogFileName);
        bool fileExists = File.Exists(filePath);

        using (StreamWriter writer = new StreamWriter(filePath, append: true))
        {
            if (!fileExists)
            {
                writer.WriteLine(
                    "timestamp;" +
                    "pid;" +
                    "teleportID;" +
                    "stressorID;" +
                    "startColor;" +
                    "targetColor;" +
                    "teleportedColor;" +
                    "correct;" +
                    "moveTime;" +
                    "aimTime;" +
                    "totalTime"
                );
            }

            foreach (var data in teleportLogs)
            {
                writer.WriteLine(
                    $"{System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")};" +
                    $"{data.participantID};" +
                    $"{data.teleportationType};" +
                    $"{data.stressor};" +
                    $"{data.startColor};" +
                    $"{data.targetColor};" +
                    $"{data.teleportedColor};" +
                    $"{data.correct};" +
                    $"{data.moveTime:F3};" +
                    $"{data.aimTime:F3};" +
                    $"{data.totalTime:F3}"
                );
            }
        }

        Debug.Log("Teleport-CSV gespeichert unter: " + filePath);
        teleportLogs.Clear();
    }
}

