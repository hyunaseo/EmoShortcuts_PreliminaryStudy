using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleTest : MonoBehaviour
{
    public GameObject cube;
    public GameObject button;
    bool isToggle = false;

    // Start is called before the first frame update
    void Start()
    {
        // isToggle = button.GetComponent<Toggle>().isOn;
    }

    // Update is called once per frame
    void Update()
    {
        isToggle = button.GetComponent<Toggle>().isOn;

        if (isToggle)
        {
            cube.GetComponent<Renderer>().material.color = Color.red;
        }
        else
        {
            cube.GetComponent<Renderer>().material.color = Color.blue;
        }
    }
}
