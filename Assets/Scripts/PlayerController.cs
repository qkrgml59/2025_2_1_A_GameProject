using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [Header("이동 설정")]
    public float walkSpeed = 3f;
    public float runSpeed = 6f;
    public float rotationSpeed = 10;

    [Header("점프 설정")]
    public float jumpHeight = 2f;                     //점프 높이
    public float gravity = -9.81f;                   //중력 속도 추가
    public float landingDuration = 0.3f;             //착지 후 착지 지속 시간


    [Header("공격 설정")]
    public float attackDuration = 0.8f;               //공격 지속시간
    public bool canMoveWhileAttacking = false;        // 공격중 이동 가능 여부

    [Header("컴포넌트")]
    public Animator animator;

    private CharacterController controller;
    private Camera playerCamera;


    //현재 상태
    private float currentSpeed;
    private bool isAttacking = false;               //공격상태 체크
    private bool isLanding = false;                 //착지중인지확인
    private float landingTimer;                       //착지 타이머

    private Vector3 velocity;
    private bool isGrounded;
    private bool wasGrounded;                      //이전 프레임에 땅이었는지
    private float attackTimer;



    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        playerCamera = Camera.main;

    }

    // Update is called once per frame
    void Update()
    {
        CheckGrounded();
        HandleLanding();
        HandleMovement();
        HandileJump();
        HandleAttack();
        UpdateAnimatior();

    }

    void CheckGrounded()
    {
        //이전 상태 저장
        wasGrounded = isGrounded;
        isGrounded = controller.isGrounded;                              //캐릭터 컨트롤러에서 받아온다.

        if (!isGrounded && wasGrounded)                                  //땅에서 떨어졌을 때 ( 지금 프레임은 땅이 아니고, 이전 프레임은 땅 )
        {
            Debug.Log("떨어지기 시작");

        }

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;

            //착지 모션 트리거 및 착지 상태 시작
            if (!wasGrounded && animator != null)
            {
                //animator.SetTrigger("landTrigger");
                isLanding = true;
                landingTimer = landingDuration;
                Debug.Log("착지");
            }
        }
    }

    void HandleLanding()
    {
        if (isLanding)
        {
            landingTimer -= Time.deltaTime;                             //랜딩 타이머 시간 만큼 못 움직임
            if (landingTimer <= 0)
            {
                isLanding = false;                                      //착지 완료
            }
        }
    }

    void HandleAttack()
    {
        if (isAttacking)                                               //공격중일때
        {
            attackTimer -= Time.deltaTime;                            //타이머를 감소시킨다.
            if (attackTimer <= 0)
            {
                isAttacking = false;

            }
          
        }
        if (Input.GetKeyDown(KeyCode.Alpha1) && !isAttacking)          //공격 중이 아닐 때 키를 누르면 공격
        {
            isAttacking = true;                                         //공격 중 표시
            attackTimer = attackDuration;                             //타이머 리필

            if (animator != null)
            {
                animator.SetTrigger("attackTrigger");
            }
        }
    }

    void HandileJump()
    {
        if(Input.GetButtonDown("Jump")&& isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

            if (animator != null)
            {
                animator.SetTrigger("jumpTrigger");
            }
        }
        
        if(!isGrounded)
        {
            velocity.y += gravity * Time.deltaTime;
        }

        controller.Move(velocity * Time.deltaTime);
    }

    void HandleMovement()          //이동 함수 제작
    {
        if((isAttacking && !canMoveWhileAttacking) || isLanding)
        {
            currentSpeed = 0;
            return;
        }

        float horizontal = Input.GetAxis("Horizontal");
        float verical = Input.GetAxis("Vertical");

        if (horizontal != 0 || verical != 0)                    //둘중 하나라도 입력이 있을 때
        {
            //카메라가 보는 방향이 앞쪽으로 되게 설정
            Vector3 cameraForward = playerCamera.transform.forward;
            Vector3 cameraRight = playerCamera.transform.right;
            cameraForward.y = 0;
            cameraRight.y = 0;
            cameraForward.Normalize();
            cameraRight.Normalize();

            Vector3 moveDirection = cameraForward * verical + cameraRight * horizontal;           //이동 방향 설정

            if (Input.GetKey(KeyCode.LeftShift))                  //왼쪽 쉬프트를 눌러서 런 모드르 변경
            {
                currentSpeed = runSpeed;

            }
            else
            {
                currentSpeed = walkSpeed;
            }

            controller.Move(moveDirection * currentSpeed * Time.deltaTime);                 //캐릭터 컨트롤러의 이동 입력

            // 이동 진행 방향을 바라보면서 이동
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        }
        else
        {
            currentSpeed = 0;              //이동이 아닐 경우 스피드 0
        }

    }

    void UpdateAnimatior()
    {
        if (controller != null)
        {
            //전체 최대 속도 (runSpeed) 기준으로 0~1 계산
            float animatorSpeed = Mathf.Clamp01(currentSpeed / runSpeed);
            animator.SetFloat("Speed", animatorSpeed);
            animator.SetBool("isGrounded", isGrounded);

            bool isFalling = !isGrounded && velocity.y < -0.1f;                                  //캐릭터의 y 축 속도가 음수로 넘어가면 떨어지고 있다고 판단.
            animator.SetBool("isFalling", isFalling);                           
            animator.SetBool("isLanding", isLanding);

        }

    }
}
