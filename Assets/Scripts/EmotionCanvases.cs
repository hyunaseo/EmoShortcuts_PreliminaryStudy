using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EmotionCanvases : MonoBehaviour
{
    public bool isStudyStarted = false;
    public GameObject[] emotionCanvases;
    public AudioSource beginningAudioSource;
    public AudioSource endingAudioSource;
    public GameObject startButton;

    private int currentCanvasIndex = 0;
    private Coroutine emotionCanvasCoroutine;
    private Timer timer;


    [HideInInspector] 
    public string currentEmotion;

    // Update is called once per frame
    void Update()
    {
        if (isStudyStarted && emotionCanvasCoroutine == null)
        {
            emotionCanvasCoroutine = StartCoroutine(ShowEmotionCanvases());
        }
    }

    private IEnumerator ShowEmotionCanvases()
    {
        while (currentCanvasIndex < emotionCanvases.Length)
        {
            emotionCanvases[currentCanvasIndex].SetActive(true);
            currentEmotion = emotionCanvases[currentCanvasIndex].transform.Find("Emotion Type")?.Find("Text")?.GetComponent<Text>()?.text;
            timer = emotionCanvases[currentCanvasIndex].transform.Find("Timer").GetComponent<Timer>();
            beginningAudioSource.Play();
            timer.StartTimer();
            yield return new WaitForSeconds(27);
            
            for (int i = 3; i >0; i--)
            {
                endingAudioSource.Play();
                yield return new WaitForSeconds(1);
            }

            emotionCanvases[currentCanvasIndex].SetActive(false);
            currentCanvasIndex++;
        }
    }

    public void StartStudy()
    {
        isStudyStarted = true;
        StartCoroutine(DeactivateStartButton());
    }

    private IEnumerator DeactivateStartButton()
    {
        yield return new WaitForSeconds(1);
        startButton.SetActive(false);
    }
}