using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputTest : MonoBehaviour
{
    private GUIStyle style;
    private void Start()
    {
        style = new GUIStyle();
        style.fontSize = 50;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnGUI()
    {
        GUILayout.Label($"MousePos:{Input.mousePosition.x}", style);
    }
}
