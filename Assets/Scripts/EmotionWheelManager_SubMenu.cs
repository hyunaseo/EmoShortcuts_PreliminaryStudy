using System.Collections;
using System.Collections.Generic;

using UnityEngine.Playables;
using UnityEngine.Animations;
using UnityEngine;
using UnityEngine.UI;

public class EmotionWheelManager_SubMenu : MonoBehaviour
{   
    [System.Serializable] 
    public class Emotion{
        public string name;
        [HideInInspector] public string colorCode;
        [HideInInspector] public UltimateRadialButtonInfo buttonInfo;
        public AnimationClip[] lowAnimations;
        public AnimationClip[] midAnimations;
        public AnimationClip[] highAnimations;
    }

    [System.Serializable]
    public class Intensity{
        public string name;
        [HideInInspector] public string colorCode;
        [HideInInspector] public UltimateRadialSubButtonInfo subButtonInfo;
    }

    [Header("Radial Menus")]
    public GameObject menuCanvas;
    public string radialMenuName = "EmotionWheel";
    UltimateRadialMenu radialMenu;
    UltimateRadialSubmenu subMenu;

    [Header("Emotion Lists")]
    public Emotion[] Emotions;
    public Intensity[] Intensities;

    private readonly Dictionary<string, string[]> emotionColors = new Dictionary<string, string[]>()
    {
        { "Anger", new[] {"#F9ADB8", "#DE8792", "#AA6770"}},      
        { "Disgust", new[] {"#A9E4D8", "#42CDAE", "#30957E"}},    
        { "Joy", new[] {"#F1CC9F", "#F6B052", "#BE8840"}},        
        { "Fear", new[] {"#B6B6E8", "#8685CE", "#5F5F92"}},       
        { "Sad", new[] {"#8BD9E7", "#3BB1CB", "#2C8396"}},        
        { "Surprise", new[] {"#F1B9A9", "#ED8E73", "#B36B57"}},   
        { "Neutral", new[] {"#AEAEAE", "#7F7F7F", "#525252"}}     
    };

    private string currentEmotion;
    private string currentIntensity;
    
    private GameObject currentIntensityButton;
    private Transform currentIntensityButtonTransform;

    void Start()
    {
        // Store the radial and sub menus.
        radialMenu = UltimateRadialMenu.ReturnComponent(radialMenuName);
        subMenu = UltimateRadialSubmenu.ReturnComponent(radialMenuName);

        RegisterEmotionButtons();
        SetRadialButtonColor();
    }

    void Update()
    {
        Debug.Log($"Current Emotion: {currentEmotion}, Intensity: {currentIntensity}");
        UpdateButtonSelection();
    }

    // Update the button activation state. Called in Update(). 
    private void UpdateButtonSelection()
    {
        for (int i=0; i<Emotions.Length; i++){
            if(currentEmotion == Emotions[i].name && currentIntensity != null) 
            {
                Emotions[i].buttonInfo.Selected = true;

                for (int j=0; j<Intensities.Length; j++)
                {
                    if(currentIntensity == Intensities[j].name) Intensities[j].subButtonInfo.Selected = true;
                    else Intensities[j].subButtonInfo.Selected = false;
                }
            }
            else Emotions[i].buttonInfo.Selected = false;
        }

        if(currentIntensityButton != null) currentIntensityButton.SetActive(true);
    } 

    public void UpdateEmotionType(int id){
        Debug.Log($"Hyuna: {Emotions[id].name} is clicked.");

        // Select this button exclusively on the radial menu.
        Emotions[id].buttonInfo.SelectButton(true);
        currentEmotion = Emotions[id].name;
        currentIntensity = null;
        Destroy(currentIntensityButton);
        currentIntensityButton = null;
        currentIntensityButtonTransform = null;

         // Clear the sub menu.
        subMenu.ClearMenu();

        // Loop through all the intensity options.
        for( int i = 0; i < Intensities.Length; i++ )
        {   
            // Debug.Log($"Hyuna: {Intensities[i].name} is added.");
            
            // Store the id of this option into the button info.
            Intensities[i].subButtonInfo.id = i;
            Intensities[i].subButtonInfo.name = Intensities[i].name;
            Intensities[i].subButtonInfo.key = Intensities[i].name;
            subMenu.RegisterButton(UpdateIntensity, Intensities[i].subButtonInfo);

            if(currentIntensity==Intensities[i].name){
                Intensities[i].subButtonInfo.Selected = true;
            }
            else{
                Intensities[i].subButtonInfo.Selected = false;
            }
        }
        
        SetSubButtonColor();

        subMenu.Enable();
    }

    public void UpdateIntensity(int id){
        currentIntensity = Intensities[id].name;

        UpdateAnimation(id);

        Intensities[id].subButtonInfo.SelectButton(true);

        // Clone sub menu button.
        foreach (Transform buttonTransform in subMenu.transform)
        {
            Text buttonText = buttonTransform.GetComponentInChildren<Text>();
            if(buttonText.text == currentIntensity) 
            {
                Destroy(currentIntensityButton);
                currentIntensityButton = Instantiate(buttonTransform.gameObject, menuCanvas.transform);
                currentIntensityButtonTransform = currentIntensityButton.transform;
                currentIntensityButtonTransform.position = buttonTransform.position;
                currentIntensityButtonTransform.rotation = buttonTransform.rotation;
            }
        }
    }

    public void UpdateAnimation(int id){
        
    }


    // DEFAULT SETTING METHODS

    // Register the radial button with the input information. Called in Start().
    private void RegisterEmotionButtons()
    {
        for (int i=0; i<Emotions.Length; i++)
        {
            var emotion = Emotions[i];
            emotion.buttonInfo.id = i;

            if (emotionColors.TryGetValue(emotion.name, out string[] colorList))
            {
                emotion.colorCode = colorList[1];
            }

            emotion.buttonInfo.name = emotion.name;
            emotion.buttonInfo.key = emotion.name;
            radialMenu.RegisterButton(UpdateEmotionType, emotion.buttonInfo);
        }
    }

    // Set the color of radial button. Called in Start().
    private void SetRadialButtonColor()
    {
        foreach (Transform buttonTransform in radialMenu.transform)
        {
            Image buttonImage = buttonTransform.GetComponent<Image>();
            if (buttonImage == null){
                continue;
            }
                
            Text buttonText = buttonTransform.GetComponentInChildren<Text>();
            if (buttonText == null){
                continue;
            }

            string textValue = buttonText.text;
            Color newColor = Color.gray;

            foreach (var emotion in Emotions)
            {
                if (textValue.Contains(emotion.name))
                {
                    // Debug.Log($"Hyuna: {emotion.name}'s ColorCode is {emotion.colorCode}.");
                    ColorUtility.TryParseHtmlString(emotion.colorCode, out newColor);
                    break; 
                }
            }

            buttonImage.color = newColor;
        }
    }

    // Set the color of "sub" menu button. Called in UpdateEmotionType().
    void SetSubButtonColor()
    {
        string[] colorList;
        emotionColors.TryGetValue(currentEmotion, out colorList);

        string lowColor = colorList[0];
        string midColor = colorList[1];
        string highColor = colorList[2];

        foreach (Transform buttonTransform in subMenu.transform)
        {
            Image buttonImage = buttonTransform.GetComponent<Image>();
            if (buttonImage == null){
                continue;
            }

            Text buttonText = buttonTransform.GetComponentInChildren<Text>();
            if (buttonText == null){
                continue;
            }

            string textValue = buttonText.text;
            Color newColor= Color.gray;

            if(textValue == "L") ColorUtility.TryParseHtmlString(lowColor, out newColor);
            if(textValue == "M") ColorUtility.TryParseHtmlString(midColor, out newColor);
            if(textValue == "H") ColorUtility.TryParseHtmlString(highColor, out newColor);
            
            buttonImage.color = newColor;
        }
    }
}
