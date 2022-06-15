using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float sensitivity = 2;
    public float smoothing = 1.5f;
    Vector2 velocity;
    Vector2 frameVelocity;

    public float moveSpeed = 1f;
    public float jumpForce = 3f;
    public float fireRange = 5f;
    public float rotationXSensitivity = 1f;
    public GameObject explosion;

    private PlayerInputAction mInputAction;
    private InputAction mMovementAction;
    private InputAction mViewAction;
    private Rigidbody mRigidbody;
    private Transform mFirePoint;
    private Transform mCameraTransform;
    private float mRotationX = 0f;
    private bool jumpPressed = false;
    private bool onGround = true;

    private void Awake()
    {
        mInputAction = new PlayerInputAction();
        mRigidbody = GetComponent<Rigidbody>();
        mFirePoint = transform.Find("FirePoint");
        mCameraTransform = transform.Find("Main Camera");

        Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnEnable()
    {
        // Codigo que se ejecutara al habilitar un Game Object
        mInputAction.Player.Jump.performed += DoJump;
        mInputAction.Player.Jump.Enable();

        mInputAction.Player.Fire.performed += DoFire;
        mInputAction.Player.Fire.Enable();

        mViewAction = mInputAction.Player.View;
        mViewAction.Enable();

        mMovementAction = mInputAction.Player.Movement;
        mMovementAction.Enable();
    }

    private void OnDisable()
    {
        // Codigo que se ejecutara al deshabilitar un Game Object
        mInputAction.Player.Jump.Disable();
        mMovementAction.Disable();
        mInputAction.Disable();
        //mInputAction.Player.View.Disable();
    }

    private void Update()
    {
        #region Rotacion
        /*
        Vector2 deltaPos = mViewAction.ReadValue<Vector2>();
        transform.Rotate(Vector3.up * deltaPos.x * Time.deltaTime * 10);
        
        mRotationX -= deltaPos.y * rotationXSensitivity;
        mCameraTransform.localRotation = Quaternion.Euler(Mathf.Clamp(mRotationX, -90f, 90f), 0f, 0f);*/
        Vector2 mouseDelta = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        Vector2 rawFrameVelocity = Vector2.Scale(mouseDelta, Vector2.one * sensitivity);
        frameVelocity = Vector2.Lerp(frameVelocity, rawFrameVelocity, 1 / smoothing);
        velocity += frameVelocity;
        velocity.y = Mathf.Clamp(velocity.y, -90, 90);

        mCameraTransform.localRotation = Quaternion.AngleAxis(-velocity.y, Vector3.right);
        transform.localRotation = Quaternion.AngleAxis(velocity.x, Vector3.up);
        #endregion

        #region Movimiento
        /*
        Vector2 movement = Vector2.ClampMagnitude(mMovementAction.ReadValue<Vector2>(), 1f);
        mRigidbody.velocity = movement.x * transform.right * moveSpeed + movement.y * transform.forward * moveSpeed + transform.up * mRigidbody.velocity.y;*/
        Vector2 targetVelocity = new Vector2(Input.GetAxis("Horizontal") * moveSpeed, Input.GetAxis("Vertical") * moveSpeed);
        mRigidbody.velocity = transform.rotation * new Vector3(targetVelocity.x, mRigidbody.velocity.y, targetVelocity.y);
        //mRigidbody.velocity = new Vector3(movement.x * moveSpeed, mRigidbody.velocity.y, movement.y * moveSpeed);
        #endregion

        #region Salto
        if (jumpPressed && onGround)
        {
            mRigidbody.velocity += Vector3.up * jumpForce;
            jumpPressed = false;
            onGround = false;
        }
        #endregion
    }

    private void DoJump(InputAction.CallbackContext obj)
    {
        jumpPressed = true;
    }

    private void DoFire(InputAction.CallbackContext obj)
    {
        // Lanzar un raycast
        RaycastHit hit;

        if (Physics.Raycast(mFirePoint.position, mCameraTransform.forward, out hit, fireRange))
        {
            // Hubo colision
            Debug.Log(hit.collider.name);
            GameObject newExplosion = Instantiate(explosion, hit.point, transform.rotation);
            Destroy(newExplosion, 1f);
        }
        Debug.DrawRay(mFirePoint.position, transform.forward * fireRange, Color.red, .25f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("collision");
        onGround = true;
        jumpPressed = false;
    }
}
