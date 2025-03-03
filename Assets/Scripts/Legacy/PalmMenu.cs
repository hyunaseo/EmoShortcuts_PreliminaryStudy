/*
* Copyright (c) Meta Platforms, Inc. and affiliates.
* All rights reserved.
*
* Licensed under the Oculus SDK License Agreement (the "License");
* you may not use the Oculus SDK except in compliance with the License,
* which is provided at the time of installation or download, or which
* otherwise accompanies this software in either electronic or hard copy form.
*
* You may obtain a copy of the License at
*
* https://developer.oculus.com/licenses/oculussdk/
*
* Unless required by applicable law or agreed to in writing, the Oculus SDK
* distributed under the License is distributed on an "AS IS" BASIS,
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
* See the License for the specific language governing permissions and
* limitations under the License.
*/

using TMPro;
using UnityEngine;
using Oculus.Interaction.Samples.PalmMenu;


/// <summary>
/// Example of a bespoke behavior created to react to a particular palm menu. This controls the state
/// of the object that responds to the menu, but also parts of the menu itself, specifically those
/// which depend on the state of the controlled object (swappable icons, various text boxes, etc.).
/// </summary>
public class PalmMenu : MonoBehaviour
{

    [SerializeField]
    private TMP_Text _emotionNameText;

    [SerializeField]
    private string[] _emotionNames;

    [SerializeField]
    private GameObject[] _emotionCanvases;
    private int _currentEmotionIdx;

    private void Start()
    {
        _currentEmotionIdx = _emotionCanvases.Length;
        CycleEmotion(true);
    }

    private void Update()
    {

    }

    /// <summary>
    /// Change the shape of the controlled object to the next or previous in the list of allowed shapes, depending on the requested direction, looping beyond the bounds of the list.
    /// Set the text to display the name of the current shape.
    /// </summary>
    public void CycleEmotion(bool cycleForward)
    {
        Debug.Assert(_emotionNames.Length == _emotionCanvases.Length);

        _currentEmotionIdx += cycleForward ? 1 : -1;
        if (_currentEmotionIdx >= _emotionCanvases.Length)
        {
            _currentEmotionIdx = 0;
        }
        else if (_currentEmotionIdx < 0)
        {
            _currentEmotionIdx = _emotionCanvases.Length - 1;
        }

        _emotionNameText.text = _emotionNames[_currentEmotionIdx];

        foreach (var canvas in _emotionCanvases)
        {
            canvas.SetActive(false);
        }
        _emotionCanvases[_currentEmotionIdx].SetActive(true);
    }
}
