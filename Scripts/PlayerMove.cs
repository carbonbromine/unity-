using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [Header ("定义")]
    public Rigidbody2D Player;
    private Animator animator;
    [Header ("移动参数")]
    private Vector2 NextPosition;
    public float PlayermoveSpeed;
    public float RestTime = 0.5f;
    public float time = 0f;
    private float h;
    private float v;
    [Header ("判断相关")]
    public bool Detection = true;
    public bool Moving = false;
    private bool IfHit = true;
    [Header("信息")]
    public RaycastHit2D Hit;
    // Start is called before the first frame update
    void Start()
    {
        Player=GetComponent<Rigidbody2D>();
        NextPosition = Player.position;
        animator = GetComponent<Animator>();
        GameManager.Instance.Player = Player;
        GameManager.Instance.Player_Animator = animator;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }
    void Move()
    {
        time += Time.deltaTime;
        if(time>RestTime &&Detection )//开启检测
        {
            h = Input.GetAxisRaw("Horizontal");
            v = Input.GetAxisRaw("Vertical");
            if(h!=0f)
            {
                v = 0f;
            }
            if(h!=0||v!=0)//检测到输入
            {
                GameManager.Instance.OnPlayerMove();
                GameManager.Instance.food -= 1;
                NextPosition = Player.position + new Vector2(h, v);//计算目标位置
                Detect();//开启射线检测
                if(!IfHit )//射线检测观察前方有无碰撞物体
                {
                    Detection = false;//关闭检测
                    Moving = true;//开启移动
                }
            }
        }
        if(Moving )//移动
        {
            Player.MovePosition(Vector2.Lerp(Player.position, NextPosition, PlayermoveSpeed));//插值计算：Vector2.Lerp((x1,y1),(x2,y2),t),当t=0时，返回（x1,y1)，当t>1时，返回(x2,y2)，当0<t<=1时，返回（x1+(x2-x1)*t,y1+(y2-y1)*t)。
            if((NextPosition -Player .position ).sqrMagnitude <0.001 )//移动到接近目标位置后，关闭移动，时间归零，开启检测
            {
                Player.position = NextPosition;
                Moving = false;
                Detection = true;
                time = 0f;
            }
        }
    }
    void  Detect()
    {
        RaycastHit2D Hit= Physics2D.Linecast(Player.position +new Vector2 (h,v)*0.48f, NextPosition);
        if(Hit.collider ==null||Hit.collider .tag =="Food"||Hit .collider .tag =="Exit")//如果没有检测到物体，或前方是食物，出口，则不影响移动
        {
            IfHit = false;
        }
        else//其他则不进行移动，但减少食物
        {
            IfHit = true;
            time = 0f;
            switch (Hit.collider .tag)
            {
                case"OutWall":
                    break;
                case "Obstacles":
                    {
                        Hit.collider.SendMessage("IsHit");
                        animator.SetTrigger("Attack");
                    }
                    break;
                case "Enemys":
                    {
                        GameManager.Instance.food -= 15;
                        animator.SetTrigger("Hit");
                    }
                    break;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag =="Food")
        {
            GameManager.Instance.food += 10;
            Destroy(collision.gameObject);
        }
        if (collision.tag == "Exit")
        {
            GameManager.Instance.NextLevel();
            GameManager.Instance.win = true;
        }
    }
}
