using UnityEngine;
using System.Collections;

public class ChickWalkControl : MonoBehaviour
{
    [Header("WALK")]
    public Sprite walk1;
    public Sprite walk2;
    public float walkFrameTime = 0.15f;

    [Header("SLIDE")]
    public Sprite slide1;
    public Sprite slide2;
    public float slideFrameTime = 0.15f;

    // 슬라이드 시 Y로 내려갈 정도
    public float slideYOffset = 0.25f;
    public float groundY = -1.94f; 

    [Header("JUMP")]
    public Sprite jumpSprite1;
    public Sprite jumpSprite2;
    public float jumpAnimTime = 0.1f;

    private SpriteRenderer sr;

    public bool isGrounded = true;
    private bool isWalking = true;
    private bool isSliding = false;
    private bool isJumping = false;

    public Vector2 slideSize = new Vector2(1.2f, 0.7f);
    public Vector2 slideOffset = new Vector2(0f, -0.2f);

    private Vector2 normalSize;
    private Vector2 normalOffset;
private AudioSource audioSource;
    public AudioClip jumpSound;
    
    public AudioClip slidingSound;
    private Rigidbody2D rb;
    private CapsuleCollider2D myCollider;

    private float startY; // 기본 y위치 저장

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
        // 슬라이드 종료 → 걷기
        else if (Input.GetKeyUp(KeyCode.DownArrow))
        {
           if(isGrounded) SetWalk();
        }
        // 걷기 상태 복귀
        else if (isGrounded && !isJumping && !isSliding && !isWalking)
        {
            SetWalk();
        }

        // 점프
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            SetJump();
        }
    }

    // --------------------------
    // WALK
    // --------------------------
    public void SetWalk()
    {
        if (isWalking) return;

        StopAllCoroutines();

        isWalking = true;
        isSliding = false;
        isJumping = false;

        // 콜라이더 원상복구
        myCollider.size = normalSize;
        myCollider.offset = normalOffset;

        // 슬라이드에서 올라오는 애니메이션
        StartCoroutine(SlideExit());
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

    // --------------------------
    // SLIDE
    // --------------------------
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

    // 슬라이드로 내려가는 동작 + 이미지 전환 동시에
    IEnumerator SlideEnter()
    {
        float duration = 0.08f;
        float elapsed = 0f;

        float fromY = transform.position.y;
        float toY = startY - slideYOffset;

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            transform.position = new Vector3(transform.position.x, Mathf.Lerp(fromY, toY, t), transform.position.z);
            sr.sprite = slide1;
            elapsed += Time.deltaTime;
            yield return null;
        }

        // 고정
        transform.position = new Vector3(transform.position.x, toY, transform.position.z);

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

    // --------------------------
    // SLIDE → WALK 올라오는 동작
    // --------------------------
    IEnumerator SlideExit()
    {
        float duration = 0.08f;
        float elapsed = 0f;

        float fromY = transform.position.y;
        float toY = startY;

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            transform.position = new Vector3(transform.position.x, Mathf.Lerp(fromY, toY, t), transform.position.z);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = new Vector3(transform.position.x, toY, transform.position.z);
        StartCoroutine(WalkAnimation());
    }

    // --------------------------
    // JUMP
    // --------------------------
    public void SetJump()
    {
        StopAllCoroutines();

        isJumping = true;
        isWalking = false;
        isSliding = false;
        isGrounded = false;

        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 13f);
 audioSource.PlayOneShot(jumpSound);
        StartCoroutine(JumpAnimation());
    }

    IEnumerator JumpAnimation()
    {
        sr.sprite = jumpSprite1;
        yield return new WaitForSeconds(jumpAnimTime);

        sr.sprite = jumpSprite2;
        yield return new WaitForSeconds(jumpAnimTime);
    }

    // --------------------------
    // 착지
    // --------------------------
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.collider.CompareTag("Ground"))
        {
            isGrounded = true;

            if (Input.GetKey(KeyCode.DownArrow))
                SetSlide();
            else
                SetWalk();
        }
    }
}
