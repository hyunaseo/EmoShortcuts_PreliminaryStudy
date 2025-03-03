using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class BluetoothKeyboardChecker : MonoBehaviour
{
    public GameObject cube;
    private Material cubeMaterial;
    
    public Color blueColor = Color.blue;
    public Color redColor = Color.red;
    public Color greenColor = Color.green;

    private bool isKeyboardConnected = false;

    void Start()
    {
        if (cube != null)
        {
            cubeMaterial = cube.GetComponent<Renderer>().material;
        }
    }

    void Update()
    {
        if (cubeMaterial == null) return;

        // 키보드 입력이 감지되면 연결된 것으로 간주
        if (Input.anyKeyDown)
        {
            isKeyboardConnected = true;
        }

        // 특정 키 입력에 따라 색상 변경
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            cubeMaterial.color = blueColor; 
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            cubeMaterial.color = redColor; 
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            cubeMaterial.color = greenColor; // Green
        }
    }
}