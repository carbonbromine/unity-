using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    [Header ("地图生成相关")]
    public int size = 1;
    float lenth = 16;
    float width = 9;
    private List<Vector2> PositionList=new List<Vector2> ();
    [Header("地图元素")]
    public GameObject[] Outwalls;
    public GameObject[] Floors;
    public GameObject[] Obstacles;
    public GameObject[] Food;
    public GameObject Exit;
    private GameObject WholeMap;
    private Transform  Map;
    private Transform Floor;
    private Transform OutWalls;
    private Transform ObstaclesHolder;
    private Transform Enemy;
    private Transform Foods;
    public Transform DestroyerObstacles;
    public Camera Camera;
    [Header("人物")]
    public GameObject[] Enemys;
    public GameObject Player;
    [Header ("游戏参数")]
    public int mode = 1;
    int enemynum_min= 1;
    int enemynum_max = 3;
    int obstacle_num_min = 2;
    int obstacle_num_max = 8;
    int food_num_min = 0;
    int food_num_max = 2;
    // Start is called before the first frame update
    public void InitGame()
    {
        Camera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        InitMap();
        SetPositionList();
        EnemySommon();
        FoodSommon();
        ObstacleSommon();
        Parent();
    }

    public void InitMap()
    {
        //在场景中创建空物体归类

        Map = new GameObject("Map").transform ;
        Floor = new GameObject("Floor").transform;
        OutWalls = new GameObject("OutWalls").transform;

        //将外墙与地板设置为Map的子物体
        
        Floor.transform.SetParent(Map);
        OutWalls.transform.SetParent(Map);

        //创建地图，大小为16*9的size倍。其中，(1,1)为玩家出生地，(16*size-2,9*size-2)为终点

        for(int x=0;x<size*lenth;x++)
        {
            for (int y=0;y<size*width;y++)
            {

                //创建外墙
                
                if(x==0||y==0||x==size*lenth -1||y==size*width -1)
                {
                    int ran = Random.Range(0, Outwalls.Length);
                    GameObject go=Instantiate(Outwalls[ran], new Vector3(x, y, 0), Quaternion.identity) as GameObject ;
                    go.transform.SetParent(OutWalls);
                }

                //创建终点

                else if(x== size * lenth - 2&&y == size * width - 2)
                {
                   GameObject go= Instantiate(Exit, new Vector3(x, y, 0), Quaternion.identity) as GameObject ;
                    go.transform.SetParent(Map);
                }

                //创建外墙

                else
                {
                    int ran = Random.Range(0, Floors .Length);
                   GameObject go= Instantiate(Floors [ran], new Vector3(x, y, 0), Quaternion.identity)as GameObject ;
                    go.transform.SetParent(Floor );
                }
            }
        }

        //生成玩家，并将其归入名为Player的空物体的子物体

        GameObject Go=Instantiate(Player, new Vector3(1, 1, 0), Quaternion.identity)as GameObject ;
        Go.transform.SetParent(GameObject.Find ("WholeMap").transform);

        //根据size的值设定相机位置和视野
        Camera.transform.position   = new Vector3(size * lenth / 2 - 0.5f, size * width / 2 - 0.5f, -10f);
        Camera.orthographicSize = size * 4.5f;

        //创建空物体"Destroyed Obstacles"，将摧毁后的障碍物归为其子物体

        DestroyerObstacles = new GameObject("Destroyed Obstacles").transform;
    }
    void SetPositionList()
    {
        PositionList.Clear();//初始化PositionList,此后无需初始化，沿用原列表，避免位置重复，（食物生成可重复，形成障碍物里藏食物的机制）
        
        //创建PositionList,范围为整个地图

        for (int x = 2; x < size * lenth - 2; x++)
        {
            for (int y = 2; y < size * width - 2; y++)
            {
                PositionList.Add(new Vector2(x, y));
            }
        }
    }

    //生成敌人

    void EnemySommon()
    {
        Enemy = new GameObject("Enemys").transform;
        RandomPrefabs(Enemys, enemynum_min, enemynum_max, Enemy);
    }

    //生成食物

    void FoodSommon()
    {
        Foods = new GameObject("Foods").transform;
        RandomPrefabs(Food, food_num_min, food_num_max, Foods);
    }

    //生成障碍物（可摧毁物体）

    void ObstacleSommon()
    {
        ObstaclesHolder = new GameObject("Obstacles").transform;
        RandomPrefabs(Obstacles, obstacle_num_min, obstacle_num_max, ObstaclesHolder);
    }

    //生产随机数量，随机种类的预制体

    void RandomPrefabs(GameObject[] Prefabs,int min,int max,Transform Parent)
    {
        int prefabs_num = Random.Range(min, max+1);
        for(int i=0;i<prefabs_num;i++)
        {
            int ListRan = Random.Range(0, PositionList.Count);
            GameObject go = Instantiate(Prefabs[Random.Range(0, Prefabs.Length)], PositionList[ListRan], Quaternion.identity) as GameObject;
            go.transform.SetParent(Parent);
            if(Prefabs!=Food)
            {
                PositionList.Remove(PositionList[ListRan]);
            }
        }
    }

    //将所有生成物体归为“WholeMap”的子物体，方便进入下一关初始化地图时删除

    void Parent()
    {
        WholeMap = GameObject.Find ("WholeMap");
        Map.SetParent(WholeMap.transform);
        DestroyerObstacles.SetParent(WholeMap.transform);
        Enemy.SetParent(WholeMap.transform);
        Foods.SetParent(WholeMap.transform);
        ObstaclesHolder.SetParent(WholeMap.transform);
    }
}
