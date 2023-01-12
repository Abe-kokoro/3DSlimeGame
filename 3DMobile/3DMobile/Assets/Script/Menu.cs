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
    [SerializeField] private GameObject MapButton;
    //SettingMenu
    [SerializeField] private GameObject SettingPanel;
    [SerializeField] private GameObject SettingsExitButton;
    [SerializeField] private GameObject ResumeButton2;
    //ExitMenu
    [SerializeField] private GameObject ExitPanel;
    [SerializeField] private GameObject ExitButton2;
    [SerializeField] private GameObject ResumeButton3;
    [SerializeField] private GameObject ExitCancelButton;
    [SerializeField] private GameObject ExitTrueButton;
    [SerializeField] bool PauseFlg = false;
    bool isMenu = false;
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
        HintButton.GetComponent<Button>().onClick.AddListener(HintoMenu);
        MapButton.GetComponent<Button>().onClick.AddListener(MapView);
    }
    void Update()
    {
        
       

    }
    void Menue()
    {
        if (PauseFlg)
        {

            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
    private void Pause()
    {
        Time.timeScale = 0;  // ���Ԓ�~
        pausePanel.SetActive(true);
        pauseButton.SetActive(false);
        resumeButton.SetActive(true);
        GameUIPanel.SetActive(false);
        ExitButton.SetActive(true);
        SettingsButton.SetActive(true);
        HintButton.SetActive(true);
        MapButton.SetActive(true);
        PauseFlg = true;
    }

    private void Resume()
    {
        Time.timeScale = 1;  // �ĊJ
        pausePanel.SetActive(false);
        pauseButton.SetActive(true);
        resumeButton.SetActive(false);
        GameUIPanel.SetActive(true);
        ExitButton.SetActive(false);
        SettingsButton.SetActive(false);
        HintButton.SetActive(false);
        MapButton.SetActive(false);
        PauseFlg = false;
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
        UnityEditor.EditorApplication.isPlaying = false;//�Q�[���v���C�I��
#else
        Application.Quit();//�Q�[���v���C�I��
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

    }
    private void MapView()
    {

    }
    public bool GetPauseFlg()
    {
        return PauseFlg;
    }
}
