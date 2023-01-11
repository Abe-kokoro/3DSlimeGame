using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
public class PlayerHpBar : MonoBehaviourPunCallbacks
{
    public int PlayerMaxHp = 100;
    public int PlayerCurrentHp;
    public Slider slider;
    private bool CollisionFlg;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating(nameof(DelayMethod), 1.5f, 1.0f);
        slider.value = 1;
        PlayerCurrentHp = PlayerMaxHp;
      
    }

    // Update is called once per frame
    void Update()
    {
        


            if (!slider)
            {
                slider = FindObjectOfType<Slider>();
                PlayerCurrentHp = PlayerMaxHp;
            }
            else
            {
                slider.value = (float)PlayerCurrentHp / (float)PlayerMaxHp;

            }
        
    }

    //�G�L�����N�^�[�Ƃ̓����蔻��
    private void OnTriggerEnter(Collider collider)
    {
        //Enemy�^�O�̃I�u�W�F�N�g�ɐG���Ɣ���
        if (collider.gameObject.tag == "Enemy")
        {
            //  InvokeRepeating(nameof(DelayMethod), 1.5f, 1.0f);
            CollisionFlg = true;
        }
    }
    //�G�L�����N�^�[�Ƃ̓����蔻��
    private void OnTriggerExit(Collider collider)
    {
        //Enemy�^�O�̃I�u�W�F�N�g�ɐG���Ɣ���
        if (collider.gameObject.tag == "Enemy")
        {
            //  InvokeRepeating(nameof(DelayMethod), 1.5f, 1.0f);
            CollisionFlg = false;
        }
    }
    void DelayMethod()
    {
        if (CollisionFlg)
        {
            Debug.Log("Delay call");
            //�_���[�W��1�`100�̒��Ń����_���Ɍ��߂�B
            int damage = Random.Range(1, 10);
            Debug.Log("damage : " + damage);

            //���݂�HP����_���[�W������
            PlayerCurrentHp = PlayerCurrentHp - damage;
            Debug.Log("After currentHp : " + PlayerCurrentHp);
            if (PlayerCurrentHp < 0)
            {
                PlayerCurrentHp = 0;
            }
            //�ő�HP�ɂ����錻�݂�HP��Slider�ɔ��f�B
            //int���m�̊���Z�͏����_�ȉ���0�ɂȂ�̂ŁA
            //(float)������float�̕ϐ��Ƃ��ĐU���킹��B
            slider.value = (float)PlayerCurrentHp / (float)PlayerMaxHp;

            Debug.Log("slider.value : " + slider.value);
        }

    }

    private void OnDestroy()
    {
        // Destroy���ɓo�^����Invoke�����ׂăL�����Z��
        CancelInvoke();
    }
    public int GetPlayerHP()
    {
        return PlayerCurrentHp;
    }
    public int GetMaxPlayerHP()
    {
        return PlayerMaxHp;
    }

}
