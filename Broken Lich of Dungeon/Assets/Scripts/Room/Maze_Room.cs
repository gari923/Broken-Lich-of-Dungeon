using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maze_Room : MonoBehaviour
{
    public static Maze_Room instance;//싱글톤

    public float way_delay;
    //활성화시킬 길의 번호
    int way_num;

    bool maze1_check = true;
    bool maze2_check = true;
    bool maze3_check = true;
    bool maze4_check = true;

    bool work = false;
    GameObject maze_way;
    GameObject maze;
    //각 구역의 클리어조건을 완료하면 통과할수 있다
    void Start()
    {
        maze_way = GameObject.Find("maze_Way");//게임오브젝트를 할당한다
        maze = GameObject.Find("maze");//게임오브젝트를 할당한다
        GameManager.instance.rock = true;//방의 클리어 조건을 완료시킨다
    }

    void Update()
    {
        Player.instance.ps = pState.Attack;//플레이어의 상태를 전투 상태로 바꾼다
        //만약 방리셋이 true라면
        if (GameManager.instance.reset == true)
        {
            way_delay = 0;//길을 렌덤으로 연다
            GameManager.instance.reset = false;//리셋이 더이상 안되도록한다
            work = true;
        }

        #region 길을 렌덤으로 여는 스크립트
        if (way_delay == 0)
        {
            for (int i = 0; i <= 4; i++)
            {
                maze_way.transform.GetChild(i).gameObject.SetActive(true);
            }
        }
        if (way_delay < 2)
        {
            way_delay += Time.deltaTime;
        }
        if (2 > way_delay && way_delay >= 1)
        {
            way_num = Random.Range(1, 5);
        }
        else if (way_delay >= 2)
        {
            switch (way_num)
            {
                case 1:
                    if (maze1_check == true && work == true)
                    {
                        maze_way.transform.GetChild(0).gameObject.SetActive(false);
                        maze.transform.GetChild(0).gameObject.SetActive(true);
                        maze1_check = false;
                        work = false;
                    }
                    else if(maze1_check == false && work == true)
                    {
                        way_num = Random.Range(1, 5);
                    }
                    break;
                case 2:
                    if (maze2_check == true && work == true)
                    {
                        maze_way.transform.GetChild(1).gameObject.SetActive(false);
                        maze.transform.GetChild(1).gameObject.SetActive(true);
                        maze2_check = false;
                        work = false;
                    }
                    else if (maze2_check == false && work == true)
                    {
                        way_num = Random.Range(1, 5);
                    }
                    break;
                case 3:
                    if (maze3_check == true && work == true)
                    {
                        maze_way.transform.GetChild(2).gameObject.SetActive(false);
                        maze_way.transform.GetChild(3).gameObject.SetActive(false);
                        maze.transform.GetChild(2).gameObject.SetActive(true);
                        maze3_check = false;
                        work = false;
                    }
                    else if (maze3_check == false && work == true)
                    {
                        way_num = Random.Range(1, 5);
                    }
                    break;
                case 4:
                    if (maze4_check == true && work == true)
                    {
                        maze_way.transform.GetChild(2).gameObject.SetActive(false);
                        maze_way.transform.GetChild(4).gameObject.SetActive(false);
                        maze.transform.GetChild(3).gameObject.SetActive(true);
                        maze4_check = false;
                        work = false;
                    }
                    else if (maze4_check == false && work == true)
                    {
                        way_num = Random.Range(1, 5);
                    }
                    break;
            }
        }
        #endregion
    }
}
