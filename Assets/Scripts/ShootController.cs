using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShootController : MonoBehaviour
{
    private Transform mShootPoint;
    private PlayerInputAction mInputAction;

    private void Awake()
    {
        mInputAction = new PlayerInputAction();
        mShootPoint = transform.Find("ShootPoint");
    }

    private void OnEnable()
    {
        mInputAction.Player.Fire.performed += DoShoot;
        mInputAction.Player.Fire.Enable();
    }

    private void DoShoot(InputAction.CallbackContext obj)
    {
        mShootPoint.GetComponent<ParticleSystem>().Play();
    }
}
