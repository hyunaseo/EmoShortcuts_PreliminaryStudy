using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class BluetoothKeyboardChecker : MonoBehaviour
{
    public GameObject cube;
    private Material cubeMaterial;
    
    public Color connectedColor = Color.blue;
    public Color disconnectedColor = Color.red;

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

        // 일정 시간 동안 키 입력이 없으면 연결이 끊긴 것으로 간주
        if (!isKeyboardConnected || !Input.anyKey)
        {
            cubeMaterial.color = disconnectedColor;
        }
        else
        {
            cubeMaterial.color = connectedColor;
        }
    }
}