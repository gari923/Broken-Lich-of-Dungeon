using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop_Manager : MonoBehaviour {
    //대화하기 창 변수
    public GameObject dialog;

    //플레이어 변수
    public Transform player;
    //유니티짱의 변수
    public Transform unitychan;
    
    //대화하기 창이 열려있을때 상호작용키를 누르면 상점창을 띄운다
    //대화하기 창이 열려있는지 확인하는 변수
    bool dialog_state = false;
    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        //유니티짱에 레이가 닿았을 경우 유니티짱이 플레이어를 바라보고 대화하기 창을 활성화 시키고 싶다
        //유니티짱이 레이에 닿았을경우
        if (Player.instance.rayObject.name == "unitychan")
        {
            Quaternion newRotation = Quaternion.LookRotation(player.position - unitychan.position - new Vector3(0,1,0));//플레이어의 방향을 가져온다
            unitychan.rotation = Quaternion.Slerp(unitychan.rotation, newRotation, 2 * Time.deltaTime);//플레이어의 방향을 바라본다

            dialog.SetActive(true);//대화하기 창을 활성화 시킨다
            dialog_state = true;//대화하기 창이 열려있다고 알려준다
        }
        else if(Player.instance.rayObject.name != "unitychan")
        {
            dialog.SetActive(false);//대화하기 창을 비활성화 시킨다
            dialog_state = false;//대화하기 창이 안열려있다고 알려준다

            Quaternion newRotation = Quaternion.LookRotation(new Vector3(0, 0, 0));//(0,0,0)방향을 가져온다
            unitychan.rotation = Quaternion.Slerp(unitychan.rotation, newRotation, 2 * Time.deltaTime);//(0,0,0)방향을 바라본다

        }
        //만약 플레이어가 유니티짱을 클릭했고 대화하기 창이 열려있다면
        //if(Player.instance.rayObject.name == "unitychan"&& Player.instance.buttonClicked == true && dialog_state == true)
        //{
        //    dialog.SetActive(false);
        //    print("상점을 엽니다");
        //}
    }   
}
