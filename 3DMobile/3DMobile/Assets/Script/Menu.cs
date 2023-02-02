using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using TMPro;

public class Menu : MonoBehaviour
{
    //�f�o�b�O
    [SerializeField] private UnityEngine.ShadowQuality shadowQuality;

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
    [SerializeField] private PlayerController Pcontroller;
    //GraphicSettings
    [SerializeField] private GameObject SaveSettingsPanel;
    [SerializeField] private Slider RenderScale;
    [SerializeField] private Toggle HDR;
    [SerializeField] private TMP_Dropdown Antialiasing;
    [SerializeField] private TMP_Dropdown Shadow;
    [SerializeField] private GameObject SaveTrue;
    [SerializeField] private GameObject SaveFalse;


    [SerializeField] bool PauseFlg = false;
    public static bool isMenu = false;
    bool isButtonUp = false;
    UniversalRenderPipelineAsset URPAsset;
    
    [SerializeField] int RenderingScale = 100;
    [SerializeField]bool isGranpicChange = false;
    
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
        SaveSettingsPanel.SetActive(false);
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
        //�`���b�g
        HintButton.GetComponent<Button>().onClick.AddListener(HintoMenu);
        ExitHintButton.GetComponent<Button>().onClick.AddListener(HintoMenuExit);
        ChatResumeButton.GetComponent<Button>().onClick.AddListener(HintoMenuExit);
        MapButton.GetComponent<Button>().onClick.AddListener(MapView);
        //�O���t�B�b�N�ݒ�
        SaveTrue.GetComponent<Button>().onClick.AddListener(SettingSave);
        SaveFalse.GetComponent<Button>().onClick.AddListener(DefaultSetting);

        URPAsset = (UniversalRenderPipelineAsset)GraphicsSettings.renderPipelineAsset;
        RenderScale.value = URPAsset.renderScale*100;
        HDR.isOn = URPAsset.supportsHDR;
        if (URPAsset.msaaSampleCount == 1)
        {
            Antialiasing.value = 0;
        }
        else if (URPAsset.msaaSampleCount == 2)
        {
            Antialiasing.value = 1;
        }
        else if (URPAsset.msaaSampleCount == 4)
        {
            Antialiasing.value = 2;
        }
        else if (URPAsset.msaaSampleCount == 8)
        {
            Antialiasing.value = 3;
        }
        Shadow.value=(int)QualitySettings.shadows;       
    }
    void Update()
    {
        shadowQuality = QualitySettings.shadows;
        if(isGranpicChange)
        {
            SaveSettingsPanel.SetActive(true);
        }
        else
        {
            SaveSettingsPanel.SetActive(false);
        }

        if(Input.GetKeyDown(KeyCode.DownArrow))
        {
            RenderingScale-=10;
            isGranpicChange = true;
        }
        if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            RenderingScale+=10;
            isGranpicChange = true;
        }
        RenderingScale=Mathf.Clamp(RenderingScale, 10, 200);
        
        if (Input.GetKeyDown("m")&&isGranpicChange)
        {
            ChangeGraphics();
        }
        

    }
    void Menue()
    {
        
        
    }
    public void Pause()
    {
        if(GameController.isPC)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        //Time.timeScale = 0;  // ���Ԓ�~
        pausePanel.SetActive(true);
        pauseButton.SetActive(false);
        resumeButton.SetActive(true);
        GameUIPanel.SetActive(false);
        ExitButton.SetActive(true);
        SettingsButton.SetActive(true);
        HintButton.SetActive(true);
        MapButton.SetActive(true);
        isMenu = true;
        //Gamecontroller.GetComponent<GameController>().SetIsMenu(true);
    }

    public void Resume()
    {
        if (GameController.isPC)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        //Time.timeScale = 1;  // �ĊJ
        pausePanel.SetActive(false);
        pauseButton.SetActive(true);
        resumeButton.SetActive(false);
        GameUIPanel.SetActive(true);
        ExitButton.SetActive(false);
        SettingsButton.SetActive(false);
        HintButton.SetActive(false);
        MapButton.SetActive(false);
        isMenu = false;
        if(GameController.isPC)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
       
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
        Pcontroller.MinePlayer.GetComponent<PlyerAnimator>().SavePlayerData();
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
        Pcontroller.MinePlayer.GetComponent<PlyerAnimator>().SavePlayerData();
    }
    public bool GetPauseFlg()
    {
        return PauseFlg;
    }
    public void GraphicValueChanged()
    {
        isGranpicChange = true;
    }
    public void ChangeGraphics()
    {
        
        //isGranpicChange = false;
        //URPAsset.renderScale = (float)RenderingScale/100;
        //GraphicsSettings.renderPipelineAsset = URPAsset;
        //Debug.Log("�O���t�B�b�N�ݒ���X�V���܂���");
    }
    private void SettingSave()
    {
        isGranpicChange = false;
        URPAsset.renderScale = (float)RenderScale.value / 100;
        URPAsset.supportsHDR = HDR.isOn;
        if (Antialiasing.value == 0)
        {
            URPAsset.msaaSampleCount = 1;
        }
        else if (Antialiasing.value == 1)
        {
            URPAsset.msaaSampleCount = 2;
        }
        else if (Antialiasing.value == 2)
        {
            URPAsset.msaaSampleCount = 4;
        }
        else if (Antialiasing.value == 3)
        {
            URPAsset.msaaSampleCount = 8;
        }
        QualitySettings.shadows = (UnityEngine.ShadowQuality)Shadow.value;
        if(Shadow.value == 2)
        {
            URPAsset.shadowDistance = 100;
        }
        else if (Shadow.value == 1)
        {
            URPAsset.shadowDistance = 30;
        }
        else if(Shadow.value == 0)
        {
            URPAsset.shadowDistance = 0;
        }
        GraphicsSettings.renderPipelineAsset = URPAsset;
        Debug.Log("�O���t�B�b�N�ݒ���X�V���܂���");
        
    }
    private void DefaultSetting()
    {
        RenderScale.value = URPAsset.renderScale * 100;
        HDR.isOn = URPAsset.supportsHDR;
        if (URPAsset.msaaSampleCount == 1)
        {
            Antialiasing.value = 0;
        }
        else if (URPAsset.msaaSampleCount == 2)
        {
            Antialiasing.value = 1;
        }
        else if (URPAsset.msaaSampleCount == 4)
        {
            Antialiasing.value = 2;
        }
        else if (URPAsset.msaaSampleCount == 8)
        {
            Antialiasing.value = 3;
        }
        Shadow.value = (int)QualitySettings.shadows;
    }
}
