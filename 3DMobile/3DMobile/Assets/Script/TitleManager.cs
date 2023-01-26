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
    [SerializeField] private GameObject Login;
    [SerializeField] private GameObject NewGame;
    [SerializeField] private GameObject LoadingPanel;
    public TMP_InputField inputField;
    public TextMeshProUGUI text;
    public GameObject NameCheck;
    public GameObject NameError;
    public static string PlayerName;
    public static bool LoadDataflg = false;
    void Start()
    {
        //Componentを扱えるようにする
        inputField = inputField.GetComponent<TMP_InputField>();
        Login.SetActive(true);
        Login.GetComponent<Button>().onClick.AddListener(LoginClicked);
        NewGame.SetActive(true);
        NewGame.GetComponent<Button>().onClick.AddListener(NewGameClicked);
        NameCheck.SetActive(false);
        CancelButton.GetComponent<Button>().onClick.AddListener(NameCancel);
        TrueButton.GetComponent<Button>().onClick.AddListener(NameTrue);
        NameError.SetActive(false);

        
    }
    public void LoginClicked()
    {

        Login.SetActive(false);
        NewGame.SetActive(false);
        LoadingPanel.SetActive(true);
        LoadDataflg = true;
        SceneManager.LoadScene("MainGameScene");
    }
    public void NewGameClicked()
    {
        Login.SetActive(false);
        NewGame.SetActive(false);
        LoadDataflg = false;
        inputField.gameObject.SetActive(true);
    }
    public void InputText()
    {
        if(inputField.text.Length<=2||inputField.text.Length>15)
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
        LoadingPanel.SetActive(true);
        PlayerName = inputField.text;
        SceneManager.LoadScene("MainGameScene");
    }
}
