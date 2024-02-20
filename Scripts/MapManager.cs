using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    [Header ("��ͼ�������")]
    public int size = 1;
    float lenth = 16;
    float width = 9;
    private List<Vector2> PositionList=new List<Vector2> ();
    [Header("��ͼԪ��")]
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
    [Header("����")]
    public GameObject[] Enemys;
    public GameObject Player;
    [Header ("��Ϸ����")]
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
        //�ڳ����д������������

        Map = new GameObject("Map").transform ;
        Floor = new GameObject("Floor").transform;
        OutWalls = new GameObject("OutWalls").transform;

        //����ǽ��ذ�����ΪMap��������
        
        Floor.transform.SetParent(Map);
        OutWalls.transform.SetParent(Map);

        //������ͼ����СΪ16*9��size�������У�(1,1)Ϊ��ҳ����أ�(16*size-2,9*size-2)Ϊ�յ�

        for(int x=0;x<size*lenth;x++)
        {
            for (int y=0;y<size*width;y++)
            {

                //������ǽ
                
                if(x==0||y==0||x==size*lenth -1||y==size*width -1)
                {
                    int ran = Random.Range(0, Outwalls.Length);
                    GameObject go=Instantiate(Outwalls[ran], new Vector3(x, y, 0), Quaternion.identity) as GameObject ;
                    go.transform.SetParent(OutWalls);
                }

                //�����յ�

                else if(x== size * lenth - 2&&y == size * width - 2)
                {
                   GameObject go= Instantiate(Exit, new Vector3(x, y, 0), Quaternion.identity) as GameObject ;
                    go.transform.SetParent(Map);
                }

                //������ǽ

                else
                {
                    int ran = Random.Range(0, Floors .Length);
                   GameObject go= Instantiate(Floors [ran], new Vector3(x, y, 0), Quaternion.identity)as GameObject ;
                    go.transform.SetParent(Floor );
                }
            }
        }

        //������ң������������ΪPlayer�Ŀ������������

        GameObject Go=Instantiate(Player, new Vector3(1, 1, 0), Quaternion.identity)as GameObject ;
        Go.transform.SetParent(GameObject.Find ("WholeMap").transform);

        //����size��ֵ�趨���λ�ú���Ұ
        Camera.transform.position   = new Vector3(size * lenth / 2 - 0.5f, size * width / 2 - 0.5f, -10f);
        Camera.orthographicSize = size * 4.5f;

        //����������"Destroyed Obstacles"�����ݻٺ���ϰ����Ϊ��������

        DestroyerObstacles = new GameObject("Destroyed Obstacles").transform;
    }
    void SetPositionList()
    {
        PositionList.Clear();//��ʼ��PositionList,�˺������ʼ��������ԭ�б�����λ���ظ�����ʳ�����ɿ��ظ����γ��ϰ������ʳ��Ļ��ƣ�
        
        //����PositionList,��ΧΪ������ͼ

        for (int x = 2; x < size * lenth - 2; x++)
        {
            for (int y = 2; y < size * width - 2; y++)
            {
                PositionList.Add(new Vector2(x, y));
            }
        }
    }

    //���ɵ���

    void EnemySommon()
    {
        Enemy = new GameObject("Enemys").transform;
        RandomPrefabs(Enemys, enemynum_min, enemynum_max, Enemy);
    }

    //����ʳ��

    void FoodSommon()
    {
        Foods = new GameObject("Foods").transform;
        RandomPrefabs(Food, food_num_min, food_num_max, Foods);
    }

    //�����ϰ���ɴݻ����壩

    void ObstacleSommon()
    {
        ObstaclesHolder = new GameObject("Obstacles").transform;
        RandomPrefabs(Obstacles, obstacle_num_min, obstacle_num_max, ObstaclesHolder);
    }

    //���������������������Ԥ����

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

    //���������������Ϊ��WholeMap���������壬���������һ�س�ʼ����ͼʱɾ��

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
