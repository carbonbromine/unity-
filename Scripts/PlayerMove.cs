using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [Header ("����")]
    public Rigidbody2D Player;
    private Animator animator;
    [Header ("�ƶ�����")]
    private Vector2 NextPosition;
    public float PlayermoveSpeed;
    public float RestTime = 0.5f;
    public float time = 0f;
    private float h;
    private float v;
    [Header ("�ж����")]
    public bool Detection = true;
    public bool Moving = false;
    private bool IfHit = true;
    [Header("��Ϣ")]
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
        if(time>RestTime &&Detection )//�������
        {
            h = Input.GetAxisRaw("Horizontal");
            v = Input.GetAxisRaw("Vertical");
            if(h!=0f)
            {
                v = 0f;
            }
            if(h!=0||v!=0)//��⵽����
            {
                GameManager.Instance.OnPlayerMove();
                GameManager.Instance.food -= 1;
                NextPosition = Player.position + new Vector2(h, v);//����Ŀ��λ��
                Detect();//�������߼��
                if(!IfHit )//���߼��۲�ǰ��������ײ����
                {
                    Detection = false;//�رռ��
                    Moving = true;//�����ƶ�
                }
            }
        }
        if(Moving )//�ƶ�
        {
            Player.MovePosition(Vector2.Lerp(Player.position, NextPosition, PlayermoveSpeed));//��ֵ���㣺Vector2.Lerp((x1,y1),(x2,y2),t),��t=0ʱ�����أ�x1,y1)����t>1ʱ������(x2,y2)����0<t<=1ʱ�����أ�x1+(x2-x1)*t,y1+(y2-y1)*t)��
            if((NextPosition -Player .position ).sqrMagnitude <0.001 )//�ƶ����ӽ�Ŀ��λ�ú󣬹ر��ƶ���ʱ����㣬�������
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
        if(Hit.collider ==null||Hit.collider .tag =="Food"||Hit .collider .tag =="Exit")//���û�м�⵽���壬��ǰ����ʳ����ڣ���Ӱ���ƶ�
        {
            IfHit = false;
        }
        else//�����򲻽����ƶ���������ʳ��
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
