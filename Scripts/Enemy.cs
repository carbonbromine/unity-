using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("����")]
    public Rigidbody2D Enemys;
    public  Rigidbody2D Player;
    private Animator Enemy_Animator;
    private Animator Player_Animator;
    [Header("�ж����")]
    private int Number_0f_Calls = 0;
    [Header("��Ϸ����")]
    private float time = 0f;
    public float Attack_Time = 0.5f;
    [Header("��Ϣ")]
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



    //�����ƶ��������������Enemy��һ��

    public void Move()
    {
        Number_0f_Calls++;
        if (Number_0f_Calls % 2 == 0)
        {

            //���Enemy��Player��֮��x�������y�ᣬ��x�����ƶ�������y����

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
        //�ж��������˵�λ��

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

        //���Ƶ��˹�����ÿAttack_Time����һ��

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
            //�ж��ƶ�����������

            if (TargetPosition.x <= 0)//������
            {

                //�ж����������Ƿ����ڵ���
                RaycastHit2D Hit = Physics2D.Linecast(Enemys.position + new Vector2(0.5f, 0), Enemys.position + new Vector2(1, 0));
                if (Hit.collider == null)
                {
                    Enemys.MovePosition(Enemys.position + new Vector2(1, 0));
                }
                else if(Hit.collider .tag !="Player")//��������ϰ������y�����ƶ�
                {
                    Move_y();//ע����Move_y�м���һ��x�����ϰ����жϣ���ֹ���ޱջ�
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
            if (TargetPosition.y <= 0)//������
            {

                //�ж����������Ƿ����ڵ���
                RaycastHit2D Hit = Physics2D.Linecast(Enemys.position + new Vector2(0, 0.5f), Enemys.position + new Vector2(0, 1));
                if (Hit.collider == null)
                {
                    Enemys.MovePosition(Enemys.position + new Vector2(0, 1));
                }
                else if (Hit.collider.tag != "Player")//��������ϰ������x�����ƶ�
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
