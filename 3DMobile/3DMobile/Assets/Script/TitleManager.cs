using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class TitleManager : MonoBehaviour
{
    //�I�u�W�F�N�g�ƌ��т���
    public TMP_InputField inputField;
    public TextMeshProUGUI text;
    public GameObject NameCheck;

    void Start()
    {
        //Component��������悤�ɂ���
        inputField = inputField.GetComponent<TMP_InputField>();
        NameCheck.SetActive(false);

    }

    public void InputText()
    {
        //�e�L�X�g��inputField�̓��e�𔽉f
        text.text = inputField.text;
        NameCheck.SetActive(true);
    }
}
