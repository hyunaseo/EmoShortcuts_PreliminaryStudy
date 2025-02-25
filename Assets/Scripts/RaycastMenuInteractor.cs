using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RaycastMenuInteractor : MonoBehaviour
{
    public UltimateRadialMenu radialMenu;
    public Transform leftFingerTip, leftFingerBase, rightFingerTip, rightFingerBase;

    LayerMask worldSpaceMask;

    void Awake()
    {
        worldSpaceMask = LayerMask.GetMask("UI");
    }
    void Update ()
    {
        // If any of the components are unassigned, warn the user and return.
        if( radialMenu == null || leftFingerTip == null || leftFingerBase == null || rightFingerTip == null || rightFingerBase == null)
        {
            return;
        }

        // Send in the finger tip and base positions to be calculated on the menu.
        // radialMenu.inputManager.SendRaycastInput(leftFingerTip.position, leftFingerBase.position);
        radialMenu.inputManager.SendRaycastInput(rightFingerTip.position, rightFingerBase.position);
    }
}
