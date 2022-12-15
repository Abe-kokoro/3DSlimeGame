using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHpBar : MonoBehaviour
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

    //敵キャラクターとの当たり判定
    private void OnTriggerEnter(Collider collider)
    {
        //Enemyタグのオブジェクトに触れると発動
        if (collider.gameObject.tag == "Enemy")
        {
            //  InvokeRepeating(nameof(DelayMethod), 1.5f, 1.0f);
            CollisionFlg = true;
        }
    }
    //敵キャラクターとの当たり判定
    private void OnTriggerExit(Collider collider)
    {
        //Enemyタグのオブジェクトに触れると発動
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
            //ダメージは1～100の中でランダムに決める。
            int damage = Random.Range(1, 10);
            Debug.Log("damage : " + damage);

            //現在のHPからダメージを引く
            PlayerCurrentHp = PlayerCurrentHp - damage;
            Debug.Log("After currentHp : " + PlayerCurrentHp);
            if (PlayerCurrentHp < 0)
            {
                PlayerCurrentHp = 0;
            }
            //最大HPにおける現在のHPをSliderに反映。
            //int同士の割り算は小数点以下は0になるので、
            //(float)をつけてfloatの変数として振舞わせる。
            slider.value = (float)PlayerCurrentHp / (float)PlayerMaxHp;

            Debug.Log("slider.value : " + slider.value);
        }

    }

    private void OnDestroy()
    {
        // Destroy時に登録したInvokeをすべてキャンセル
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
