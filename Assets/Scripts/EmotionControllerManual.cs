using System.Collections;
using System.Collections.Generic;

using TMPro;

using UnityEngine.Playables;
using UnityEngine.Animations;
using UnityEngine.UI;
using UnityEngine;

public class EmotionControllerManual : MonoBehaviour
{
    // Make a class that has three Gameobjects, each is low, mid, high
    [System.Serializable]   
    public class Emotion
    {
        public string EmotionName;
        public GameObject EmotionType;
        public GameObject ButtonLow;
        public GameObject ButtonMid;
        public GameObject ButtonHigh;

        public AnimationClip[] LowAnimations;
        public AnimationClip[] MidAnimations;       
        public AnimationClip[] HighAnimations;
    }

    [Header("Emotions")]
    public Emotion[] Emotions;


    [Header("Avatar")]
    public GameObject avatar;
    private Animator animator;
    private PlayableGraph playableGraph;
    private AnimationMixerPlayable mixer;
    private AnimationPlayableOutput output;

    // [Header("Test")]
    // public GameObject cube;

    private AnimationClip currentAnimation;
    private GameObject currentButton;
    private AnimationClip lastClip;
    private float transitionDuration = 1f;
    private int currentIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        // cube.GetComponent<Renderer>().material.color = Color.blue;
       foreach (var emotion in Emotions)
       {
            // Set every emotion's button's toggle to false at the beginning
            emotion.ButtonLow.GetComponent<Toggle>().isOn = false;
            emotion.ButtonMid.GetComponent<Toggle>().isOn = false;  
            emotion.ButtonHigh.GetComponent<Toggle>().isOn = false;
            
            AddToggleListener(emotion.ButtonLow, emotion);
            AddToggleListener(emotion.ButtonMid, emotion);
            AddToggleListener(emotion.ButtonHigh, emotion);
       }
    }

    void AddToggleListener(GameObject button, Emotion emotion)
    {
        Toggle toggle = button.GetComponent<Toggle>();
        if (toggle != null)
        {
            toggle.onValueChanged.AddListener((isOn) => {
                if (isOn)
                {
                    currentAnimation = GetCurrentAnimation(button);
                    UpdateAnimation(currentAnimation);
                    UpdateLableColor(button, Color.white);
                    UpdateEmotionTypeColor(emotion, Color.white);
                    SetOtherTogglesOff(button);
                    currentButton = button;

                    // cube.GetComponent<Renderer>().material.color = Color.red;
                }
                else
                {
                    UpdateLableColor(button, Color.black);
                    if (!emotion.ButtonLow.GetComponent<Toggle>().isOn &&
                        !emotion.ButtonMid.GetComponent<Toggle>().isOn &&
                        !emotion.ButtonHigh.GetComponent<Toggle>().isOn)
                    {
                        UpdateEmotionTypeColor(emotion, Color.black);
                    }
                }
            });
        }
    }

    void SetOtherTogglesOff(GameObject activeButton)
    {
        foreach (var emotion in Emotions)
        {
            if (emotion.ButtonLow != activeButton)
            {
                Toggle toggle = emotion.ButtonLow.GetComponent<Toggle>();
                if (toggle != null && toggle.isOn)
                {
                    toggle.isOn = false;
                }
            }
            if (emotion.ButtonMid != activeButton)
            {
                Toggle toggle = emotion.ButtonMid.GetComponent<Toggle>();
                if (toggle != null && toggle.isOn)
                {
                    toggle.isOn = false;
                }
            }
            if (emotion.ButtonHigh != activeButton)
            {
                Toggle toggle = emotion.ButtonHigh.GetComponent<Toggle>();
                if (toggle != null && toggle.isOn)
                {
                    toggle.isOn = false;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
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
    }

    AnimationClip GetCurrentAnimation(GameObject button)
    {
        foreach (var emotion in Emotions)
        {
            if (button == emotion.ButtonLow)
            {   
                return emotion.LowAnimations[Random.Range(0, emotion.LowAnimations.Length)];
            }
            else if (button == emotion.ButtonMid)
            {
                return emotion.MidAnimations[Random.Range(0, emotion.MidAnimations.Length)];
            }
            else if (button == emotion.ButtonHigh)
            {
                return emotion.HighAnimations[Random.Range(0, emotion.HighAnimations.Length)];
            }
        }
        return null;
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
    void UpdateLableColor(GameObject button, Color color)
    {
        TextMeshProUGUI labelText = button.GetComponentInChildren<TextMeshProUGUI>(true); 
        if (labelText != null)
        {
            labelText.color = color;
        }
    }

    void UpdateEmotionTypeColor(Emotion emotion, Color color)
    {
        TextMeshProUGUI emotionTypeText = emotion.EmotionType.GetComponentInChildren<TextMeshProUGUI>(true); 
        if (emotionTypeText != null)
        {
            emotionTypeText.color = color;
        }
    }
}
