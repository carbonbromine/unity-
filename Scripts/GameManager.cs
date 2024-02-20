using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int level = 1;
    public int food = 100;
    private bool sleepstep = true;
    public  bool win = false;
    public TextMeshProUGUI FoodText;
    public TextMeshProUGUI LevelText;
    private string UI_Food_string = "Food : &";
    private string UI_Level_string = "Level : &";
    public List<Enemy> EnemyList=new List<Enemy> ();
    public Rigidbody2D Player;
    public Animator Player_Animator;
    //������̬�࣬�Ա�ȫ������

    private static GameManager _gamemanager;
    public static GameManager Instance
    {
        get
        {
            return _gamemanager;
        }
    }




    // Start is called before the first frame update
    void Awake()
    {
        InitGame();
        DontDestroyOnLoad(this.gameObject);
        _gamemanager = this;
    }


    private void Update()
    {
        UIcontrol();
        lose();
    }






    public void NextLevel()
    {
        SceneManager.LoadScene(0);
    }


    private void OnLevelWasLoaded()
    {
        if(win)
        {
            level++;
            InitGame();
            win = false;
        }

    }

    void InitGame()
    {
        MapManager mapManager = GetComponent<MapManager>();
        mapManager.InitGame ();
        Player = null;
        Player_Animator = null;
        EnemyList.Clear();
    }

    //����UI

    void UIcontrol()
    {
        //����UI��ֵ

        string new_food=UI_Food_string.Replace("&", food.ToString());
        string new_level=UI_Level_string.Replace("&", level.ToString());

        //��string�������Ļ

        FoodText.text =new_food ;
        LevelText.text = new_level ;

    }

    //���Ƶ����ƶ�

    public void OnPlayerMove()
    {
        if(sleepstep )
        {
            sleepstep =false ;
        }
        else
        {
            foreach(Enemy enemy in EnemyList )
            {
                enemy.Move ();
            }
        }
    }
    
    void lose()
    {
        if(food<=0)
        {
            Destroy(this.gameObject);
            SceneManager.LoadScene(1);
        }
    }
}
