using UnityEngine;
using System.Collections;
public class EggController : MonoBehaviour
{
    [Header("Sprites")]
    public Sprite runSprite;      // 달리기 이미지
    public Sprite jumpSprite1; 
    public Sprite jumpSprite2;    // 점프 이미지
    public Sprite slideSprite;    // 슬라이딩 이미지

    
    public Sprite brokenSprite1;
    public Sprite brokenSprite2;
    public Sprite brokenSprite3;
    public Sprite brokenSprite4;

    public Sprite jump1Broken1;
    public Sprite jump1Broken2;
    public Sprite jump1Broken3;
    public Sprite jump1Broken4;
    public Sprite jump2Broken1;
    public Sprite jump2Broken2;
    public Sprite jump2Broken3;
    public Sprite jump2Broken4;

    public Sprite slideBroken1;
    public Sprite slideBroken2;
    public Sprite slideBroken3;
    public Sprite slideBroken4;

    [Header("Physics")]
    public float jumpForce = 12f;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private CapsuleCollider2D myCollider;
    private AudioSource audioSource;
    public AudioClip jumpSound;
    
    public AudioClip slidingSound;
    public AudioClip itemSound;
    public AudioClip brokenSound;

    [Header("State")]
    public bool isGrounded = true;
    public bool isSliding = false;
    public float groundY = -2.5764f; 
    public int brokenGauge=-1;

    // 콜리더 설정을 위한 변수들
    private Vector2 normalSize;
    private Vector2 normalOffset;
    public Vector2 slideSize = new Vector2(1.2f, 0.5f);   // 슬라이딩 시 납작한 크기
    public Vector2 slideOffset = new Vector2(0f, 0f); // 슬라이딩 시 낮은 중심점

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        myCollider = GetComponent<CapsuleCollider2D>();

        // 시작할 때의 콜리더 크기를 기억해둡니다.
        normalSize = myCollider.size;
        normalOffset = myCollider.offset;
    }

void Awake(){
    audioSource = GetComponent<AudioSource>();
}
   void Update()
{
    // 1. 점프 로직
    if (Input.GetKeyDown(KeyCode.Space) && isGrounded && !isSliding)
    {
        SetJump();
    }

    // 2. 슬라이딩 로직 (isGrounded 조건을 삭제해서 공중에서도 입력을 받게 함)
    if (Input.GetKeyDown(KeyCode.DownArrow))
    {
        if (!isSliding) 
        {
            audioSource.PlayOneShot(slidingSound);
        }
        SetSlide();
    }

    // 3. 슬라이딩 해제 로직 (GetKeyUp 추가)
    if (Input.GetKeyUp(KeyCode.DownArrow))
    {
        SetStand();
    }
}

    public void SetJump()
    {
        isGrounded = false;
        rb.linearVelocity = Vector2.up * jumpForce;
        
        StopAllCoroutines();        // 혹시 다른 애니메이션이 돌고 있으면 종료
    StartCoroutine(JumpAnimation());
   audioSource.PlayOneShot(jumpSound);

   

    }
    IEnumerator JumpAnimation()
{
    SetJumpImage1();
    yield return new WaitForSeconds(0.1f);

   SetJumpImage2();
    yield return new WaitForSeconds(0.1f);
 //brokenGauge ++;
    SetImage();
}
public void SetImage(){
    
  if (brokenGauge >= 15) spriteRenderer.sprite = brokenSprite4;
    else if (brokenGauge >= 8) spriteRenderer.sprite = brokenSprite3;
    else if (brokenGauge >= 3) spriteRenderer.sprite = brokenSprite2;
    else if (brokenGauge >= 1) spriteRenderer.sprite = brokenSprite1;
    else spriteRenderer.sprite = runSprite;


}

public void SetJumpImage1(){
   if (brokenGauge >= 15) spriteRenderer.sprite = jump1Broken4;
    else if (brokenGauge >= 8) spriteRenderer.sprite = jump1Broken3;
    else if (brokenGauge >= 3) spriteRenderer.sprite = jump1Broken2;
    else if (brokenGauge >= 1) spriteRenderer.sprite = jump1Broken1;
    else spriteRenderer.sprite = jumpSprite1;
}
public void SetJumpImage2(){
if (brokenGauge >= 15) spriteRenderer.sprite = jump2Broken4;
    else if (brokenGauge >= 8) spriteRenderer.sprite = jump2Broken3;
    else if (brokenGauge >= 3) spriteRenderer.sprite = jump2Broken2;
    else if (brokenGauge >= 1) spriteRenderer.sprite = jump2Broken1;
    else spriteRenderer.sprite = jumpSprite2;
}
  public void SetSlide()
{
    // [수정] 정말로 공중에 떠 있는 '순간'에만 착지 효과 적용
    // y속도가 0보다 작거나 큰 '공중' 상태일 때만 실행되도록 보강
    if (!isGrounded && Mathf.Abs(rb.linearVelocity.y) > 0.1f)
    {
        audioSource.PlayOneShot(brokenSound); 
        brokenGauge++; 
    }

    isSliding = true;
    isGrounded = true; // 강제 착지 처리
    rb.linearVelocity = Vector2.zero;
    
    Vector3 pos = transform.position;
    pos.y = groundY;
    transform.position = pos;

    SetSlideImage();
    myCollider.size = slideSize;
    myCollider.offset = slideOffset;
}

    public void SetSlideImage(){
      if (brokenGauge >= 15) spriteRenderer.sprite = slideBroken4;
    else if (brokenGauge >= 8) spriteRenderer.sprite = slideBroken3;
    else if (brokenGauge >= 3) spriteRenderer.sprite = slideBroken2;
    else if (brokenGauge >= 1) spriteRenderer.sprite = slideBroken1;
    else spriteRenderer.sprite = slideSprite;
    }

    public void SetStand()
    {
        isSliding = false;
       SetImage();
        // 콜리더를 원래대로 복구합니다.
        myCollider.size = normalSize;
        myCollider.offset = normalOffset;
    }

    // 바닥 착지 체크
    private void OnCollisionEnter2D(Collision2D collision)
{
    if (collision.gameObject.CompareTag("Ground"))
    {
        // [수정] 공중에 떠 있다가 바닥에 닿는 '순간'에만 소리 재생
        if (!isGrounded) 
        {
            audioSource.PlayOneShot(brokenSound); // 착지 시 깨지는 소리
            brokenGauge++;
            isGrounded = true;
            
            if (!isSliding) 
            {
                SetImage();
            }
        }
    }
}
}