using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float sensitivity = 2;
    public float smoothing = 1.5f;
    Vector2 velocity;
    Vector2 frameVelocity;

    public Text vida;
    public int valorVida = 100;

    private int puntaje = 0;

    private RawImage mImage;
    public Texture[] mCaras = new Texture[6];

    public float moveSpeed = 1f;
    public float jumpForce = 3f;
    public float fireRange = 5f;
    public float rotationXSensitivity = 1f;

    public GameObject explosion;
    public GameObject explosionM;
    private Transform mShootPoint;

    private PlayerInputAction mInputAction;
    private InputAction mMovementAction;
    private InputAction mViewAction;
    private Rigidbody mRigidbody;
    private Transform mFirePoint;
    private Transform mCameraTransform;
    private Transform mWeapon;
    private bool jumpPressed = false;
    private bool onGround = true;

    public GameObject restart;

    private void Awake()
    {
        Time.timeScale = 1;
        mInputAction = new PlayerInputAction();
        mRigidbody = GetComponent<Rigidbody>();
        mFirePoint = transform.Find("FirePoint");
        mCameraTransform = transform.Find("Main Camera");
        mShootPoint = transform.Find("Main Camera").Find("ShootPoint");
        mWeapon = transform.Find("Main Camera").Find("AKM");
        mImage = GameObject.Find("Canvas").transform.Find("Cara").GetComponent<RawImage>();
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
    }

    private void Update()
    {
        #region Rotacion
        if (valorVida > 0)
        {
            Vector2 mouseDelta = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
            Vector2 rawFrameVelocity = Vector2.Scale(mouseDelta, Vector2.one * sensitivity);
            frameVelocity = Vector2.Lerp(frameVelocity, rawFrameVelocity, 1 / smoothing);
            velocity += frameVelocity;
            velocity.y = Mathf.Clamp(velocity.y, -90, 90);

            mCameraTransform.localRotation = Quaternion.AngleAxis(-velocity.y, Vector3.right);
            transform.localRotation = Quaternion.AngleAxis(velocity.x, Vector3.up);
        }
        #endregion

        #region Movimiento
        Vector2 targetVelocity = new Vector2(Input.GetAxis("Horizontal") * moveSpeed, Input.GetAxis("Vertical") * moveSpeed);
        mRigidbody.velocity = transform.rotation * new Vector3(targetVelocity.x, mRigidbody.velocity.y, targetVelocity.y);
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
        mShootPoint.GetComponent<ParticleSystem>().Play();
        if (Physics.Raycast(mFirePoint.position, mCameraTransform.forward, out hit, fireRange))
        {
            // Hubo colision
            //Debug.Log(hit.collider.name);
            if (hit.collider.tag == "EnemySmall" || hit.collider.tag == "EnemyBig")
            {
                GameObject newExplosion = Instantiate(explosionM, hit.point, transform.rotation);
                Destroy(newExplosion, 0.1f);
            }
            else
            {
                GameObject newExplosion = Instantiate(explosion, hit.point, transform.rotation);
                Destroy(newExplosion, 1f);
            }
        }
        Debug.DrawRay(mFirePoint.position, transform.forward * fireRange, Color.red, .25f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        

        if (collision.gameObject.CompareTag("EnemySmall"))
        {
            valorVida -= 5;
            Vector3 direction = collision.transform.position - transform.position;
            mRigidbody.AddForce(direction * 10f, ForceMode.Impulse);
        } else if (collision.gameObject.CompareTag("EnemyBig"))
        {
            valorVida -= 20;
            Vector3 direction = collision.transform.position - transform.position;
            mRigidbody.AddForce(direction * 10f, ForceMode.Impulse);
        }

        if (valorVida > 80)
        {
            mImage.texture = mCaras[0];
        } else if (valorVida > 60)
        {
            mImage.texture = mCaras[1];
        } else if (valorVida > 40)
        {
            mImage.texture = mCaras[2];
        } else if (valorVida > 20)
        {
            mImage.texture = mCaras[3];
        } else if (valorVida > 0)
        {
            mImage.texture = mCaras[4];
        }

        if (valorVida <= 0)
        {
            vida.text = "0%";
            mImage.texture = mCaras[5];
            mWeapon.gameObject.SetActive(false);
            Time.timeScale = 0;
            restart.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
        }
        if (valorVida > 0)
        {
            vida.text = valorVida.ToString() + "%";
        }
        onGround = true;
        jumpPressed = false;
    }

    public int getPuntaje()
    {
        return puntaje;
    }

    public void setPuntaje(int puntos)
    {
        puntaje += puntos;
    }
}
