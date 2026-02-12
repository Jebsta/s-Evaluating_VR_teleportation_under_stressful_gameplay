using System.Collections.Generic;
using TMPro;
using UnityEngine;
public class InputFieldManager : MonoBehaviour
{
    public int participantID = 1;
    public TextMeshProUGUI textField;
    
    public void Start()
    {
        // Load the participantID from PlayerPrefs if it exists
        participantID = PlayerPrefs.GetInt("ParticipantID", 1);
        updateTextField();
    }

    public void increaseID()
    { 
        participantID += 1;
        updateTextField();
    }

    public void decreaseID()
    {
        if (participantID > 1)
        {
            participantID -= 1;
        }
        updateTextField();
    }

    public int getID()
    {
        return participantID;
    }

    void updateTextField()
    {
        // Update the text field with the current participantID
        textField.text = participantID.ToString();
        PlayerPrefs.SetInt("ParticipantID", participantID);
    }
}
