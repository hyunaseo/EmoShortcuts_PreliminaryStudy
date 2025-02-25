using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Oculus.Avatar2.Experimental;

public class TrackingToAnimation : MonoBehaviour
{
    public GameObject mirroredAvatar;
    private OvrAvatarAnimationBehavior animationBehavior;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animationBehavior = mirroredAvatar.GetComponent<OvrAvatarAnimationBehavior>();
        StartCoroutine(SetCustomAnimationOnAfterDelay(10f));
        StartCoroutine(SetCustomAnimationOffAfterDelay(20f));
    }

    
    void Update()
    {
        if (animator == null) // Keep searching until we find an Animator
        {
            FindAndAssignAnimator();
        }
    }

    private IEnumerator SetCustomAnimationOnAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        mirroredAvatar.transform.localScale = new Vector3(1, 1, 1);
        animationBehavior.enabled = true;
    }

    private IEnumerator SetCustomAnimationOffAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        // mirroredAvatar.transform.localScale = new Vector3(1, 1, -1);
        animator.enabled = false;
    }


    void FindAndAssignAnimator()
    {
        // Find all GameObjects in the scene
        GameObject[] allObjects = FindObjectsOfType<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            // Check if the GameObject has an Animator component
            Animator foundAnimator = obj.GetComponent<Animator>();
            if (foundAnimator != null)
            {
                animator = foundAnimator; // Store the found Animator
                break; // Stop searching after finding the first one
            }
        }
    }
}

