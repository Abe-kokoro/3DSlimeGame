using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class TitleManager : MonoBehaviour
{
    //オブジェクトと結びつける
    public TMP_InputField inputField;
    public TextMeshProUGUI text;
    public GameObject NameCheck;

    void Start()
    {
        //Componentを扱えるようにする
        inputField = inputField.GetComponent<TMP_InputField>();
        NameCheck.SetActive(false);

    }

    public void InputText()
    {
        //テキストにinputFieldの内容を反映
        text.text = inputField.text;
        NameCheck.SetActive(true);
    }
}
