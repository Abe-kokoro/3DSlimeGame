using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class SettingText : MonoBehaviour
{
    [SerializeField] string TextVar;
    [SerializeField] Slider Slider;
    [SerializeField] TextMeshProUGUI TextValue;
    [SerializeField] int Value;
    [SerializeField] float volume = 1.0f;
    [SerializeField] bool isTextOnly;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        //var Label = GetComponent<TextMeshPro>();
        if (isTextOnly)
        {
            TextValue.text = TextVar;
        }
        else
        {
            TextValue.text = TextVar + Slider.value * volume;
        }

    }
}
