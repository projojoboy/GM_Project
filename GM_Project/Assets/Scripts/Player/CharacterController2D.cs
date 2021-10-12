using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Rigidbody2D))]
public class CharacterController2D : MonoBehaviour
{
    [SerializeField] private float _jumpForce = 400f;
    [SerializeField] private float _moveSpeed = 40f;
    [SerializeField] private float _fallMultiplier = 2.5f;
    [SerializeField] private float _lowJumpMultiplier = 2f;

    [Range(0, .3f)] [SerializeField] private float _movementSmoothing = .05f;

    [SerializeField] private LayerMask _whatIsGround;
    [SerializeField] private LayerMask _shadowDashLayers;

    [SerializeField] private Transform _groundCheck;

    [SerializeField] private ParticleSystem _shadowDashParticle;

    const float _groundedRadius = .2f;

    private Rigidbody2D _rb;

    private Vector3 _velocity = Vector3.zero;
    private Vector2 positionToDashTo = Vector2.zero;

    private bool _facingRight = true;
    private bool canDash = false;

    public float horizontalMove = 0;

    public bool jump = false;
    public bool grounded = false;
    public bool crouch = false;

    [Header("Events")]
    [Space]

    public UnityEvent OnLandEvent;

    [System.Serializable]
    public class BoolEvent : UnityEvent<bool> { }

    public BoolEvent OnCrouchEvent;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();

        if (OnLandEvent == null)
            OnLandEvent = new UnityEvent();

        if (OnCrouchEvent == null)
            OnCrouchEvent = new BoolEvent();
    }

    private void Update()
    {
        horizontalMove = Input.GetAxis("Horizontal") * _moveSpeed;

        if (Input.GetKeyDown(KeyCode.Space))
            jump = true;

        JumpModifier();
        ShadowDash();
    }

    private void FixedUpdate()
    {
        GroundCheck();


        Move(horizontalMove * Time.fixedDeltaTime, jump);
        jump = false;
    }

    private void JumpModifier()
    {
        if (_rb.velocity.y < 0)
            _rb.velocity += Vector2.up * Physics2D.gravity.y * (_fallMultiplier - 1) * Time.deltaTime;
        else if (_rb.velocity.y > 0 && !Input.GetKey(KeyCode.Space))
            _rb.velocity += Vector2.up * Physics2D.gravity.y * (_lowJumpMultiplier - 1) * Time.deltaTime;
    }

    private void GroundCheck()
    {
        bool wasGrounded = grounded;
        grounded = false;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(_groundCheck.position, _groundedRadius, _whatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                grounded = true;
                if (!wasGrounded)
                    OnLandEvent.Invoke();
            }
        }
    }

    private void Move(float move, bool jump)
    {
        Vector3 targetVelocity = new Vector2(move * 10f, _rb.velocity.y);
        _rb.velocity = Vector3.SmoothDamp(_rb.velocity, targetVelocity, ref _velocity, _movementSmoothing);

        if (move > 0 && !_facingRight)
            Flip();
        else if (move < 0 && _facingRight)
            Flip();

        if (grounded && jump)
        {
            grounded = false;
            _rb.velocity = new Vector2(_rb.velocity.x, 0);
            _rb.AddForce(new Vector2(0f, _jumpForce));
        }
    }

    private void Flip()
    {
        _facingRight = !_facingRight;

        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    private void ShadowDash()
    {
        if (Input.GetMouseButton(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = (Vector2)((mousePos - transform.position));
            direction.Normalize();

            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 5f, _shadowDashLayers);

            if (hit.collider != null)
            {
                Side hitSide = SideHit(hit);
                positionToDashTo = hit.point;

                Debug.DrawRay(transform.position, direction * 5, Color.green);

                switch (hitSide)
                {
                    case Side.Top:
                        positionToDashTo.y += GetComponent<CircleCollider2D>().bounds.size.y;
                        break;
                    case Side.Bottom:
                        positionToDashTo.y -= GetComponent<CircleCollider2D>().bounds.size.y;
                        break;
                    case Side.Left:
                        positionToDashTo.x -= GetComponent<CircleCollider2D>().bounds.size.x;
                        break;
                    case Side.Right:
                        positionToDashTo.x += GetComponent<CircleCollider2D>().bounds.size.x;
                        break;
                    case Side.TopLeft:
                        positionToDashTo.x += GetComponent<CircleCollider2D>().bounds.size.x / 2;
                        positionToDashTo.y += GetComponent<CircleCollider2D>().bounds.size.y;
                        break;
                    case Side.TopRight:
                        positionToDashTo.x -= GetComponent<CircleCollider2D>().bounds.size.x / 2;
                        positionToDashTo.y += GetComponent<CircleCollider2D>().bounds.size.y;
                        break;
                }

                Debug.Log(hitSide);
                canDash = true;
            }
            else
                Debug.DrawRay(transform.position, direction * 5, Color.red);
        }
        else if (canDash)
        {
            transform.position = positionToDashTo;
            canDash = false;
            Debug.Log("Dash");
        }
    }

    private Side SideHit(RaycastHit2D hit)
    {
        Vector2 hitGoPos = hit.collider.gameObject.transform.position;
        Vector2 hitGoCollSize = hit.collider.gameObject.GetComponent<Collider2D>().bounds.size;

        if (hit.point.y - .01f <= (hitGoPos.y + (hitGoCollSize.y / 2)) &&
            hit.point.y + .01f >= (hitGoPos.y + (hitGoCollSize.y / 2)))
            return Side.Top;
        else
        if (hit.point.y - .01f <= (hitGoPos.y - (hitGoCollSize.y / 2)) &&
            hit.point.y + .01f >= (hitGoPos.y - (hitGoCollSize.y / 2)))
            return Side.Bottom;
        else
        if (hit.point.x - .01f <= (hitGoPos.x + (hitGoCollSize.x / 2)) &&
            hit.point.x + .01f >= (hitGoPos.x + (hitGoCollSize.x / 2)) &&
            hit.point.y >= (hitGoPos.y + (hitGoCollSize.y / 2) - .4f))
            return Side.TopRight;
        else
        if (hit.point.x - .01f <= (hitGoPos.x + (hitGoCollSize.x / 2)) &&
            hit.point.x + .01f >= (hitGoPos.x + (hitGoCollSize.x / 2)))
            return Side.Right;
        else
        if (hit.point.x - .01f <= (hitGoPos.x - (hitGoCollSize.x / 2)) &&
            hit.point.x + .01f >= (hitGoPos.x - (hitGoCollSize.x / 2)) &&
            hit.point.y >= (hitGoPos.y + (hitGoCollSize.y / 2) - .4f))
            return Side.TopLeft;
        else
        if (hit.point.x - .01f <= (hitGoPos.x - (hitGoCollSize.x / 2)) &&
            hit.point.x + .01f >= (hitGoPos.x - (hitGoCollSize.x / 2)))
            return Side.Left;

        return Side.Top;
    }

    private enum Side { Top, Bottom, Left, Right, TopLeft, TopRight };
}
