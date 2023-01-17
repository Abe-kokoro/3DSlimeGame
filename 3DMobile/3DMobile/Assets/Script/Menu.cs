using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class Menu : MonoBehaviour
{
    [SerializeField] private GameObject pauseButton;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject resumeButton;
    [SerializeField] private GameObject GameUIPanel;
    [SerializeField] private GameObject ExitButton;
    [SerializeField] private GameObject SettingsButton;
    [SerializeField] private GameObject HintButton;
    [SerializeField] private GameObject ExitHintButton;
    [SerializeField] private GameObject ChatResumeButton;

    [SerializeField] private GameObject MapButton;
    //SettingMenu
    [SerializeField] private GameObject SettingPanel;
    [SerializeField] private GameObject SettingsExitButton;
    [SerializeField] private GameObject ResumeButton2;
    //ExitMenu
    [SerializeField] private GameObject ExitPanel;
    [SerializeField] private GameObject ChatPanel;
    [SerializeField] private GameObject ExitButton2;
    [SerializeField] private GameObject ResumeButton3;
    [SerializeField] private GameObject ExitCancelButton;
    [SerializeField] private GameObject ExitTrueButton;
    [SerializeField] private GameObject Gamecontroller;
    [SerializeField] bool PauseFlg = false;
    public static bool isMenu = false;
    bool isButtonUp = false;
    void Start()
    {
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;

        pausePanel.SetActive(false);
        pauseButton.SetActive(true);
        resumeButton.SetActive(false);
        GameUIPanel.SetActive(true);
        ExitButton.SetActive(false);
        SettingsButton.SetActive(false);
        HintButton.SetActive(false);
        MapButton.SetActive(false);
        SettingPanel.SetActive(false);
        ExitPanel.SetActive(false);
        ChatPanel.SetActive(false);
        pauseButton.GetComponent<Button>().onClick.AddListener(Pause);
        resumeButton.GetComponent<Button>().onClick.AddListener(Resume);
        //SettingMenu
        SettingsButton.GetComponent<Button>().onClick.AddListener(SettingsMenu);
        SettingsExitButton.GetComponent<Button>().onClick.AddListener(SettingsExit);
        ResumeButton2.GetComponent<Button>().onClick.AddListener(SettingsExit);
        //ExitMenu
        ExitButton.GetComponent<Button>().onClick.AddListener(ExitMenu);
        ExitButton2.GetComponent<Button>().onClick.AddListener(ExitMenuExit);
        ResumeButton3.GetComponent<Button>().onClick.AddListener(ExitMenuExit);
        ExitCancelButton.GetComponent<Button>().onClick.AddListener(ExitMenuExit);
        ExitTrueButton.GetComponent<Button>().onClick.AddListener(ExitGame);
        //チャット
        HintButton.GetComponent<Button>().onClick.AddListener(HintoMenu);
        ExitHintButton.GetComponent<Button>().onClick.AddListener(HintoMenuExit);
        ChatResumeButton.GetComponent<Button>().onClick.AddListener(HintoMenuExit);
        MapButton.GetComponent<Button>().onClick.AddListener(MapView);
    }
    void FixedUpdate()
    {
        
        

        

    }
    void Menue()
    {
        
        
    }
    public void Pause()
    {
        //Time.timeScale = 0;  // 時間停止
        pausePanel.SetActive(true);
        pauseButton.SetActive(false);
        resumeButton.SetActive(true);
        GameUIPanel.SetActive(false);
        ExitButton.SetActive(true);
        SettingsButton.SetActive(true);
        HintButton.SetActive(true);
        MapButton.SetActive(true);
        isMenu = true;
        Gamecontroller.GetComponent<GameController>().SetIsMenu(true);
    }

    public void Resume()
    {
        //Time.timeScale = 1;  // 再開
        pausePanel.SetActive(false);
        pauseButton.SetActive(true);
        resumeButton.SetActive(false);
        GameUIPanel.SetActive(true);
        ExitButton.SetActive(false);
        SettingsButton.SetActive(false);
        HintButton.SetActive(false);
        MapButton.SetActive(false);
        isMenu = false;
        Gamecontroller.GetComponent<GameController>().SetIsMenu(false);
    }
    private void ExitMenu()
    {
        ExitPanel.SetActive(true);
        pausePanel.SetActive(false);
    }
    private void ExitMenuExit()
    {
        ExitPanel.SetActive(false);
        pausePanel.SetActive(true);
    }
    private void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;//ゲームプレイ終了
#else
        Application.Quit();//ゲームプレイ終了
#endif
    }

    private void SettingsMenu()
    {
        SettingPanel.SetActive(true);
        pausePanel.SetActive(false);
    }
    private void SettingsExit()
    {
        SettingPanel.SetActive(false);
        pausePanel.SetActive(true);
    }
    private void HintoMenu()
    {
        pausePanel.SetActive(false);
        ChatPanel.SetActive(true);
    }
    private void HintoMenuExit()
    {
        pausePanel.SetActive(true);
        ChatPanel.SetActive(false);
    }

    private void MapView()
    {

    }
    public bool GetPauseFlg()
    {
        return PauseFlg;
    }
}
