using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D _rb;

    [SerializeField] private float moveSpeed, jumpForce;
    private float _moveHorizontal;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        _moveHorizontal = Input.GetAxis("Horizontal");
        
        if (_moveHorizontal > 0.1f || _moveHorizontal < -0.1f)
        {
            _rb.velocity = new Vector2(_moveHorizontal * moveSpeed, _rb.velocity.y);
        }
        
        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            _rb.AddForce(new Vector2(0,jumpForce), ForceMode2D.Impulse);
        }
    }
    
    private bool IsGrounded()
    {
        var position = transform.position;
        var groundCheck = Physics2D.Raycast(
            new Vector2(position.x, position.y - 0.5f), 
            Vector2.down, 0.2f);
        return groundCheck.collider != null && groundCheck.collider.CompareTag("Floor");
    }
}
