using UnityEngine;
using System.Collections;

public class ChickenWalkController : MonoBehaviour
{
    [Header("WALK")]
    public Sprite walk1;
    public Sprite walk2;
    public float walkFrameTime = 0.15f;

    [Header("SLIDE")]
    public Sprite slide1;
    public Sprite slide2;
    public float slideFrameTime = 0.15f;
      public float slideYOffset = 0.25f;
    public float groundY = -1.94f; 

    [Header("JUMP")]
    public Sprite jumpSprite1;
    public Sprite jumpSprite2;
    public float jumpAnimTime = 0.1f;
private AudioSource audioSource;
    public AudioClip jumpSound;
    
    public AudioClip slidingSound;
    private SpriteRenderer sr;
    private CapsuleCollider2D myCollider;
    public bool isGrounded = true;
    private bool isWalking = true;
    private bool isSliding = false;
    private bool isJumping = false;

     private Vector2 normalSize;
    private Vector2 normalOffset;
private float startY;
    public Vector2 slideSize = new Vector2(1.2f, 0.2f);
    public Vector2 slideOffset = new Vector2(0f, -0.45f);

    private Rigidbody2D rb;
private int jumpCount = 0; // 현재 점프 횟수
public int maxJumpCount = 2; // 최대 허용 점프 횟수 (2단 점프면 2)
    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        myCollider = GetComponent<CapsuleCollider2D>();
         audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        normalSize = myCollider.size;
        normalOffset = myCollider.offset;
        
        startY = transform.position.y;
        StartCoroutine(WalkAnimation());
    }

    void Update()
    {
        HandleInput();
    }

    void HandleInput()
    {
        // 슬라이딩
        if (Input.GetKey(KeyCode.DownArrow))
        {
            if (!isSliding) 
        {
            audioSource.PlayOneShot(slidingSound);
        }
            SetSlide();
        }
        else if (isGrounded && !isJumping)
        {
            SetWalk();
        }

        // 점프
        if (Input.GetKeyDown(KeyCode.Space))
    {
        // 바닥에 있거나, 공중이더라도 점프 횟수가 남아있다면 실행
        if (isGrounded || jumpCount < maxJumpCount)
        {
            SetJump();
        }
    }
    }

    //---------------------------------------------------
    // WALK
    //---------------------------------------------------
    public void SetWalk()
    {
        if (isWalking) return;

        StopAllCoroutines();

        isWalking = true;
        isSliding = false;
        isJumping = false;

         myCollider.size = normalSize;
        myCollider.offset = normalOffset;

        StartCoroutine(WalkAnimation());
    }

    IEnumerator WalkAnimation()
    {
        while (isWalking)
        {
            sr.sprite = walk1;
            yield return new WaitForSeconds(walkFrameTime);

            sr.sprite = walk2;
            yield return new WaitForSeconds(walkFrameTime);
        }
    }

    //---------------------------------------------------
    // SLIDE
    //---------------------------------------------------
     public void SetSlide()
{
    if (isSliding) return;

    StopAllCoroutines();

    isSliding = true;
    isWalking = false;
    isJumping = false;
    
    // 추가: 공중에서 슬라이드 시 즉시 바닥으로 이동 및 속도 제거
    isGrounded = true; 
    rb.linearVelocity = Vector2.zero;   

    // 캐릭터 위치를 강제로 바닥(groundY) 또는 슬라이드 위치로 고정
    Vector3 pos = transform.position;
    pos.y = startY - slideYOffset; // 슬라이드 높이로 즉시 이동
    transform.position = pos;

    // 콜라이더 설정
    myCollider.size = slideSize;
    myCollider.offset = slideOffset;

    // 바로 슬라이드 애니메이션 시작 (내려가는 연출 생략하고 즉시 전환)
    StartCoroutine(SlideAnimation());
}

    IEnumerator SlideAnimation()
    {
        while (isSliding)
        {
            sr.sprite = slide1;
            yield return new WaitForSeconds(slideFrameTime);

            sr.sprite = slide2;
            yield return new WaitForSeconds(slideFrameTime);
        }
    }

    //---------------------------------------------------
    // JUMP (물리 적용 + 점프 애니메이션 2프레임)
    //---------------------------------------------------
    public void SetJump()
{
    StopAllCoroutines();

    isJumping = true;
    isGrounded = false; // 점프하는 순간 공중 상태
    isWalking = false;
    isSliding = false;

    jumpCount++; // 점프 횟수 증가!

    // 점프 힘 적용 (기존 11f 사용)
    rb.linearVelocity = new Vector2(rb.linearVelocity.x, 11f);  
 audioSource.PlayOneShot(jumpSound);
    StartCoroutine(JumpAnimation());
}

    IEnumerator JumpAnimation()
    {
        sr.sprite = jumpSprite1;
        yield return new WaitForSeconds(jumpAnimTime);

        sr.sprite = jumpSprite2;
        yield return new WaitForSeconds(jumpAnimTime);

        // 점프 중엔 이미지 유지 → 착지하면 Run으로 돌아옴
    }

    //---------------------------------------------------
    // 착지
    //---------------------------------------------------
    private void OnCollisionEnter2D(Collision2D col)
{
    if (col.collider.CompareTag("Ground"))
    {
        isGrounded = true;
        jumpCount = 0; // ★ 땅에 닿으면 점프 횟수 초기화!

        if (Input.GetKey(KeyCode.DownArrow))
            SetSlide();
        else
            SetWalk();
    }
}
}
