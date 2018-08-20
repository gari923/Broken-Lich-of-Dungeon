using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//일정 트리거안에 플레이어가 들어오면 클리어 시키는 스크립트
public class door : MonoBehaviour {
    private void OnTriggerEnter(Collider other)
    {
        //컬라이더에 들어온 오브젝트의 태그가 "Player"라면
        if (other.tag == "Player")
        {
            GameObject Maze = GameObject.FindWithTag("maze");//maze태그를 가진 오브젝트를 찾는다
            Maze.SetActive(false);//maze를 비활성화 시킨다.
            GameManager.instance.clear = true; //클리어조건을 완료시킨다
            Player.instance.ps = pState.Idle; //플레이어의 상태를 아이들로 만든다
            GameManager.instance.reset = true; //방을 리셋시킨다
        }
    }
}
