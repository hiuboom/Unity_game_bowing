using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
/// <summary>
/// Player�����ڴ�����ҵ���Ϊ������
/// </summary>
public class player : MonoBehaviour
{
    [Header("�ٶ����")]
    /// <summary>
    /// ��ҵ��ƶ��ٶ�
    /// </summary>
    public float playerSpeed;

    /// <summary>
    /// ��ҵķ������
    /// </summary>
    public float rotateSpeed;
    [Header("�������")]
    public float rotation;
    public float N = 0;
    public float aboveRotation = 0;
    public float stopThere = 0;
    public float startThere = 0;


    /// <summary>
    /// ��ҵ�Y����
    /// </summary>
    public float Y_Position;
    public float ContinueTime;



    [Header("�������")]
    /// <summary>
    /// �������
    /// </summary>
    public Rigidbody2D playerRB;

    /// <summary>
    /// ���������
    /// </summary>
    public Transform playerTransform;

    /// <summary>
    /// �ϰ���Ԥ�Ƽ�
    /// </summary>
    public GameObject obstaclePrefab;

    /// <summary>
    /// ��Ѫ��ƵԴ
    /// </summary>
    public AudioSource BloodAudiio;

    /// <summary>
    /// �ϰ�����ƵԴ
    /// </summary>
    public AudioSource ObstacleAudio;
    

    /// <summary>
    /// ����Ϸ��ʼʱ��ʼ����ҵ�����
    /// </summary>
    void Start()
    {
        playerRB = GetComponent<Rigidbody2D>();
        playerTransform = GetComponent<Transform>();
        playerSpeed = 1f;
        rotateSpeed = 1.5f;
        playerRB.velocity = new Vector2(playerRB.velocity.x, playerSpeed);
        AudioSource[] arr = GetComponents<AudioSource>();
        BloodAudiio = arr[0];
        ObstacleAudio = arr[1];
        Y_Position = playerTransform.position.y;
    }

    
    /// <summary>
    /// ��ÿһ֡�������״̬
    /// </summary>
    void Update()
    {
        adjustStay();
        changeSpeed();
        AddSpeed();
    }

    /// <summary>
    /// �����ҵ�λ�ø���֮ǰ��λ��20����λ����������ҵ��ٶȺ���ת�ٶ�
    /// </summary>
    void AddSpeed()
    {
        if(playerTransform.position.y > Y_Position + 20)
        {
            playerSpeed += 0.05f;
            Y_Position = playerTransform.position.y;
            rotateSpeed += 0.07f;
        }
    }

    /// <summary>
    /// �ı�����ٶ�
    /// </summary>
    void changeSpeed()
    {
        float horizontal = Input.GetAxis("Horizontal");
        playerMove(horizontal);

    }

    /// <summary>
    /// ������ҷ���
    /// </summary>
    /// <param name="horizontal">ˮƽ���������ֵ</param>
    void playerMove(float horizontal)
    {
        

        
        if (horizontal != 0f && Mathf.Abs(N) <= Mathf.Abs(horizontal)) 
        {
            N = Mathf.Max(Mathf.Abs(N), Mathf.Abs(horizontal));
            // ���㴬����ת�Ƕ�
            rotation = -horizontal * 180 + aboveRotation;
            
            if(rotation < -70)
            {
                rotation = -70;
            }
            else if(rotation > 70)
            {
                rotation = 70;
            }
            // ���㴬���ٶȺ��ƶ�����
            Vector2 direction = new Vector2(Mathf.Cos((rotation+90) * Mathf.Deg2Rad), Mathf.Sin((rotation + 90) * Mathf.Deg2Rad));

            if(stopThere == 0)
                playerRB.velocity = direction * rotateSpeed;
            else playerRB.velocity = direction * rotateSpeed * startThere;
        }
        if(horizontal == 0f)
        {
            N = 0;
            aboveRotation = rotation;
        }
        
    }


    /// <summary>
    /// ���ô�����ת�Ƕ�
    /// </summary>
    private void FixedUpdate()
    {
        
        // ���ô�����ת�Ƕ�
        playerTransform.rotation = Quaternion.Euler(0, 0, rotation);
        

    }

    /// <summary>
    /// ��ײ�ж����������Ѫ����ײ��������Ӧ��Ч�����ϰ�����ײ��������Ӧ��Ч����ʹ���ͣ��һ��
    /// </summary>
    /// <param name="collision">��ײ��Ϣ</param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Obstacle")
        {
            ObstacleAudio.Play();
            playerRB.velocity *= 0;
            stopThere = 1;
        }
        if(collision.gameObject.tag == "Blood")
        {
            BloodAudiio.Play();
        }
        
    }

    /// <summary>
    /// ����ײ���ͣ�����𽥻ָ��ٶ�
    /// </summary>
    void adjustStay()
    {
        if(stopThere != 0)
        {
            startThere += Time.deltaTime;
            playerRB.velocity = new Vector2(playerRB.velocity.x, startThere);
            if(startThere > playerSpeed)
            {
                startThere = 0;
                stopThere = 0;
            }
        }
        
        
    }
}