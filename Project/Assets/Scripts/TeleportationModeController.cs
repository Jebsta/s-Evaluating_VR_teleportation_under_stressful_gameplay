using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
//using System.Diagnostics;


public class TeleportationModeController : MonoBehaviour
{
    [Header("Right Controller Teleportation Interactor")]
    [SerializeField] public GameObject teleportInteractor;

    private XRRayInteractor rightHandInteractor
    {
        get
        {
            if (teleportInteractor != null)
            {
                return teleportInteractor.GetComponent<XRRayInteractor>();
            }
            return null;
        }
    }

    private bool orientationalTeleportationActive;

    public void SetNormalTeleportation()
    {
        if (rightHandInteractor != null)
            rightHandInteractor.manipulateAttachTransform = false;

        orientationalTeleportationActive = false;
    }

    public void SetOrientationalTeleportation()
    {
        if (rightHandInteractor != null)
            rightHandInteractor.manipulateAttachTransform = true;

        orientationalTeleportationActive = true;
    }

    public bool IsOrientationalTeleportaionActive()
    {
        return orientationalTeleportationActive;
    }
}
