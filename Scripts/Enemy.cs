using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("定义")]
    public Rigidbody2D Enemys;
    public  Rigidbody2D Player;
    private Animator Enemy_Animator;
    private Animator Player_Animator;
    [Header("判断相关")]
    private int Number_0f_Calls = 0;
    [Header("游戏参数")]
    private float time = 0f;
    public float Attack_Time = 0.5f;
    [Header("信息")]
    private Vector2 TargetPosition;
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.EnemyList.Add(this);
        Enemys = GetComponent<Rigidbody2D>();
        Enemy_Animator = GetComponent<Animator>();
        if(GameManager .Instance .Player !=null &&GameManager .Instance .Player_Animator !=null )
        {
            Player_Animator = GameManager.Instance.Player_Animator;
            Player = GameManager.Instance.Player;
        }
        
    }

    void Update()
    {
        Detect();
    }



    //控制移动，玩家走两步，Enemy走一步

    public void Move()
    {
        Number_0f_Calls++;
        if (Number_0f_Calls % 2 == 0)
        {

            //如果Enemy与Player的之间x轴间距大于y轴，则x方向移动，否则y方向

            if (Math.Abs(TargetPosition.x) >= Math.Abs(TargetPosition.y))
            {
                Move_x();
            }
            else
            {
                Move_y();
            }
        }
    }
        //判断玩家与敌人的位置

        void Detect()
        {
            TargetPosition = Enemys.transform.position - Player.transform.position;
            if (TargetPosition.magnitude < 1.1f)
            {
                time += Time.deltaTime;
                if (time > Attack_Time)
                {
                    Attack();
                }
            }
        }

        //控制敌人攻击，每Attack_Time攻击一次

        void Attack()
        {
            Enemy_Animator.SetTrigger("Attack");
            Player_Animator.SetTrigger("Hit");
            GameManager.Instance.food -= 15;
            GameManager.Instance.SendMessage("UIcontrol");
        time = 0f;
        }

        void Move_x()
        {
            //判断移动的正负方向

            if (TargetPosition.x <= 0)//正方向
            {

                //判定正方向上是否有遮挡物
                RaycastHit2D Hit = Physics2D.Linecast(Enemys.position + new Vector2(0.5f, 0), Enemys.position + new Vector2(1, 0));
                if (Hit.collider == null)
                {
                    Enemys.MovePosition(Enemys.position + new Vector2(1, 0));
                }
                else if(Hit.collider .tag !="Player")//如果存在障碍物，进行y方向移动
                {
                    Move_y();//注意在Move_y中加入一个x方向障碍物判断，防止无限闭环
                }
            }
            else
            {
                RaycastHit2D Hit = Physics2D.Linecast(Enemys.position + new Vector2(-0.5f, 0), Enemys.position + new Vector2(-1, 0));
                if (Hit.collider == null)
                {
                    Enemys.MovePosition(Enemys.position + new Vector2(-1, 0));
                }
                else if (Hit.collider.tag != "Player")
            {
                    Move_y();
                }
            }

        }

        void Move_y()
        {
            if (TargetPosition.y <= 0)//正方向
            {

                //判定正方向上是否有遮挡物
                RaycastHit2D Hit = Physics2D.Linecast(Enemys.position + new Vector2(0, 0.5f), Enemys.position + new Vector2(0, 1));
                if (Hit.collider == null)
                {
                    Enemys.MovePosition(Enemys.position + new Vector2(0, 1));
                }
                else if (Hit.collider.tag != "Player")//如果存在障碍物，进行x方向移动
            {
                    if (TargetPosition.x <= 0)
                    {
                        Hit = Physics2D.Linecast(Enemys.position + new Vector2(0.5f, 0), Enemys.position + new Vector2(1, 0));
                        if (Hit.collider == null)
                        {
                            Enemys.MovePosition(Enemys.position + new Vector2(1, 0));
                        }
                    }
                    else
                    {
                        Hit = Physics2D.Linecast(Enemys.position + new Vector2(-0.5f, 0), Enemys.position + new Vector2(-1, 0));
                        if (Hit.collider == null)
                        {
                            Enemys.MovePosition(Enemys.position + new Vector2(-1, 0));
                        }
                    }
                }
            }
            else
            {
                RaycastHit2D Hit = Physics2D.Linecast(Enemys.position + new Vector2(0, -0.5f), Enemys.position + new Vector2(0, -1));
                if (Hit.collider == null)
                {
                    Enemys.MovePosition(Enemys.position + new Vector2(0, -1));
                }
                else if (Hit.collider.tag != "Player")
            {
                    if (TargetPosition.x <= 0)
                    {
                        Hit = Physics2D.Linecast(Enemys.position + new Vector2(0.5f, 0), Enemys.position + new Vector2(1, 0));
                        if (Hit.collider == null)
                        {
                            Enemys.MovePosition(Enemys.position + new Vector2(1, 0));
                        }
                    }
                    else
                    {
                        Hit = Physics2D.Linecast(Enemys.position + new Vector2(-0.5f, 0), Enemys.position + new Vector2(-1, 0));
                        if (Hit.collider == null)
                        {
                            Enemys.MovePosition(Enemys.position + new Vector2(-1, 0));
                        }
                    }
                }
            }
        }
    }
