using UnityEngine;

public class PlatformDetector : MonoBehaviour
{
    public GameManager myGameManager; 
    private string lastPlatformName = ""; 

    //void Update()
    //{
    //    RaycastHit hit;
        
    //    if (Physics.Raycast(transform.position, Vector3.down, out hit, 10.0f))
    //    {
    //        // Wir nutzen wieder den Namen des Eltern-Objekts ("Teleport Platform Blue")
    //        if (hit.collider.transform.parent != null)
    //        {
    //            string currentName = hit.collider.transform.parent.name;

    //            if (currentName != lastPlatformName && currentName.Contains("Teleport Platform"))
    //            {
    //                //Nur debuggen, um sicherzugehen
    //                Debug.Log("Ich stehe jetzt auf: " + currentName);

    //                lastPlatformName = currentName;

    //                if (myGameManager != null)
    //                {
    //                    // SCHRITT 1: Wir fragen den Manager: "War das richtig?"
    //                    myGameManager.CheckIfCorrect(currentName);

    //                    // SCHRITT 2: Danach erst holen wir uns eine neue Farbe
    //                    //myGameManager.PickNewColor(currentName);
    //                }
    //            }
    //        }
    //    }
    //}

}