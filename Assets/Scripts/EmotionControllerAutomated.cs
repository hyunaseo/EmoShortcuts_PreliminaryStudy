using System.Collections;
using System.Collections.Generic;

using UnityEngine.Playables;
using UnityEngine.Animations;
using UnityEngine.UI;
using UnityEngine.XR;
using UnityEngine;
public class EmotionControllerAutomated : MonoBehaviour
{
    public EmotionCanvases emotionCanvases;
    private string currentEmotion;

    [System.Serializable]
    public class Emotion{
        public string name;
        public AnimationClip[] lowAnimations;
        public AnimationClip[] midAnimations;
        public AnimationClip[] highAnimations;
    }

    [Header("Emotions")]
    public Emotion[] Emotions;
    
    [Header("Avatar")]
    public GameObject avatar;
    private Animator animator;
    private PlayableGraph playableGraph;
    private AnimationMixerPlayable mixer;
    private AnimationPlayableOutput output;

    private AnimationClip currentAnimation;
    private Emotion neutralEmotionObject = null;
    private Emotion currentEmotionObject = null;  
    private AnimationClip lastClip;
    private float transitionDuration = 1f;
    private int currentIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        foreach (Emotion emotion in Emotions)
        {
            if (emotion.name == "Neutral")
            {
                neutralEmotionObject = emotion;
                break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        currentEmotion = emotionCanvases.currentEmotion;

         // find a child object from the avatar object that has animator 
        if (animator == null)
        {
            animator = avatar.GetComponentInChildren<Animator>();

            // Initialize PlayableGraph
            playableGraph = PlayableGraph.Create("AnimationGraph");
            playableGraph.SetTimeUpdateMode(DirectorUpdateMode.GameTime);

            // Create Mixer
            mixer = AnimationMixerPlayable.Create(playableGraph, 2); // 2 slots for blending
            output = AnimationPlayableOutput.Create(playableGraph, "AnimationOutput", animator);
            output.SetSourcePlayable(mixer);

            // Play Default Animation
            if (animator.runtimeAnimatorController != null)
            {
                AnimatorControllerPlayable controllerPlayable = AnimatorControllerPlayable.Create(playableGraph, animator.runtimeAnimatorController);
                playableGraph.Connect(controllerPlayable, 0, mixer, 0);
                mixer.SetInputWeight(0, 1);
                playableGraph.Play();
            }
        }

        // find Emotions's element whose name is currentEmotion 
        currentEmotionObject = null;
        foreach (Emotion emotion in Emotions)
        {
            if (emotion.name == currentEmotion)
            {
                currentEmotionObject = emotion;
                break;
            }
        } 

        currentAnimation = null;
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            // turn on low intensity animation
            currentAnimation = currentEmotionObject.lowAnimations[Random.Range(0, currentEmotionObject.lowAnimations.Length)];
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            // turn on mid intensity animation
            currentAnimation = currentEmotionObject.midAnimations[Random.Range(0, currentEmotionObject.midAnimations.Length)];
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            // turn on high intensity animation
            currentAnimation = currentEmotionObject.highAnimations[Random.Range(0, currentEmotionObject.highAnimations.Length)];
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            // turn on neutral 1 animation
            currentAnimation = neutralEmotionObject.lowAnimations[Random.Range(0, neutralEmotionObject.lowAnimations.Length)];
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            // turn on neutral 2 animation
            currentAnimation = neutralEmotionObject.midAnimations[Random.Range(0, neutralEmotionObject.midAnimations.Length)];
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            // turn on neutral 3 animation
            currentAnimation = neutralEmotionObject.highAnimations[Random.Range(0, neutralEmotionObject.highAnimations.Length)];
        }

        if(currentAnimation != null)
        {
            UpdateAnimation(currentAnimation);
        }
    }

    private void UpdateAnimation(AnimationClip newClip)
    {
        if(newClip == null || animator == null) return;

        var newClipPlayable = AnimationClipPlayable.Create(playableGraph, newClip);

        if(currentIndex>=0)
        {
            int newIndex = (currentIndex + 1) % 2;
            mixer.DisconnectInput(newIndex);
            mixer.ConnectInput(newIndex, newClipPlayable, 0);
            mixer.SetInputWeight(newIndex, 1);
        
            StartCoroutine(BlendAnimations(currentIndex, newIndex, transitionDuration));
        }
        else
        {
            mixer.ConnectInput(0, newClipPlayable, 0);
            mixer.SetInputWeight(0, 1);
            currentIndex = 0;
        }

        lastClip = newClip;
    }

    private IEnumerator BlendAnimations(int fromIndex, int toIndex, float duration)
    {
        float time = 0;
    
        while(time < duration)
        {
            float weight = time / duration;
            mixer.SetInputWeight(fromIndex, 1 - weight);
            mixer.SetInputWeight(toIndex, weight);
            time += Time.deltaTime;
            yield return null;
        }
        mixer.SetInputWeight(fromIndex, 0);
        mixer.SetInputWeight(toIndex, 1);
        currentIndex = toIndex;
    }
}
