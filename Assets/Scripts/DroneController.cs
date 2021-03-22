using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneController : MonoBehaviour
{
    enum DroneState
    {
        DRONE_STATE_IDLE,
        DRONE_STATE_START_TAKINGOFF,
        DRONE_STATE_TAKINGOFF,
        DRONE_STATE_MOVING_UP,
        DRONE_STATE_FLYING,
        DRONE_STATE_START_LANDING,
        DRONE_STATE_LANDING,
        DRONE_STATE_LANDED,
        DRONE_STATE_WAIT_ENGINE_STOP,
    }

    DroneState _State;

    Animator _anim;
    Vector3 _Speed = new Vector3(0.0f, 0.0f, 0.0f);

    public float _SpeedMultipler = 1f;

    public bool IsIdle()
    {
        return (_State == DroneState.DRONE_STATE_IDLE);
    }

    public void TakeOff()
    {
        _State = DroneState.DRONE_STATE_START_TAKINGOFF;
    }

    public bool IsFlying()
    {
        return (_State == DroneState.DRONE_STATE_FLYING);
    }

    public void Land()
    {
        _State = DroneState.DRONE_STATE_START_LANDING;
    }

    private void Start()
    {
        _anim = GetComponent<Animator>();

        _State = DroneState.DRONE_STATE_IDLE;
    }

    public void Move(float _speedX,float _speedZ)
    {
        _Speed.x = _speedX;
        _Speed.z = _speedZ;

        UpdateDrone();
    }

    void UpdateDrone()
    {
        switch (_State)
        {
            case DroneState.DRONE_STATE_IDLE:
                break;
            case DroneState.DRONE_STATE_START_TAKINGOFF:
                _anim.SetBool("TakeOff", true);
                _State = DroneState.DRONE_STATE_TAKINGOFF;
                break;
            case DroneState.DRONE_STATE_TAKINGOFF:
                if (_anim.GetBool("TakeOff") == false)
                {
                    _State = DroneState.DRONE_STATE_MOVING_UP;
                }
                break;
            case DroneState.DRONE_STATE_MOVING_UP:
                if(_anim.GetBool("MoveUp") == false)
                {
                    _State = DroneState.DRONE_STATE_FLYING;
                }
                break;
            case DroneState.DRONE_STATE_FLYING:
                float angleZ = -30.0f * _Speed.x * 60.0f * Time.deltaTime;
                float angleX = 30.0f * _Speed.z * 60.0f * Time.deltaTime;

                Vector3 rotation = transform.localRotation.eulerAngles;
                transform.localPosition += _Speed * Time.deltaTime * _SpeedMultipler;
                transform.localRotation = Quaternion.Euler(angleX, rotation.y, angleZ);
                break;

            case DroneState.DRONE_STATE_START_LANDING:
                _anim.SetBool("MoveDown", true);
                _State = DroneState.DRONE_STATE_LANDING;
                break;
            case DroneState.DRONE_STATE_LANDING:
                if (_anim.GetBool("MoveDown") == false)
                {
                    _State = DroneState.DRONE_STATE_LANDED;
                }
                break;
            case DroneState.DRONE_STATE_LANDED:
                _anim.SetBool("Land", true);
                _State = DroneState.DRONE_STATE_WAIT_ENGINE_STOP;
                break;
            case DroneState.DRONE_STATE_WAIT_ENGINE_STOP:
                if(_anim.GetBool("Land") == false)
                {
                    _State = DroneState.DRONE_STATE_IDLE;
                }
                break;
        }
    }
}
