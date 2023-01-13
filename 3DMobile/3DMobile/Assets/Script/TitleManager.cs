using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
public class TitleManager : MonoBehaviour
{
    //オブジェクトと結びつける
    [SerializeField] private GameObject CancelButton;
    [SerializeField] private GameObject TrueButton;
    public TMP_InputField inputField;
    public TextMeshProUGUI text;
    public GameObject NameCheck;
    public GameObject NameError;
    public static string PlayerName;
    void Start()
    {
        //Componentを扱えるようにする
        inputField = inputField.GetComponent<TMP_InputField>();
        NameCheck.SetActive(false);
        CancelButton.GetComponent<Button>().onClick.AddListener(NameCancel);
        TrueButton.GetComponent<Button>().onClick.AddListener(NameTrue);
        NameError.SetActive(false);
    }

    public void InputText()
    {
        if(inputField.text.Length<=3||inputField.text.Length>15)
        {
            NameError.SetActive(true);
            inputField.text = "";
        }
        else
        {
            NameError.SetActive(false);
            //テキストにinputFieldの内容を反映
            text.text = inputField.text;
            NameCheck.SetActive(true);

        }
    }
    private void NameCancel()
    {
        NameCheck.SetActive(false);
        inputField.text = "";
    }
    private void NameTrue()
    {
        PlayerName = inputField.text;
        SceneManager.LoadScene("MainGameScene");
    }
}
