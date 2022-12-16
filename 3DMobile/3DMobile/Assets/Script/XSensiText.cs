using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class XSensiText : MonoBehaviour
{
    [SerializeField] Slider Slider;
    [SerializeField] TextMeshProUGUI TextValue;
    [SerializeField]int Value;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!Slider)
        {
            Slider = GameObject.FindObjectOfType<Slider>();
        }
        //var Label = GetComponent<TextMeshPro>();
        TextValue.text = "X:" + Slider.value;
       
    }
}
