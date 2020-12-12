using Assets.Scripts;
using Assets.Scripts.ScriptingUtilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavigationAgent : MonoBehaviour
{
    public MobileUnit unit;
    //front, back, left, right
    public float Facceleration;
    public float Bacceleration;
    public float Lacceleration;
    public float Racceleration;
    public float maxAccelerationMulti = 1.0f;
    public bool continousAcceleration = false;

    public float maxCombatSpeed;
    public float turnrate;
    public float turnDist;
    public float turnSpeedMulti = 1;
    public float turnTolerance;

    public Vector3? destination = null;

    public bool stopping = false;
    public bool stop = false;
    public bool facingTarget = false;

    public Rigidbody unitRigidbody;

    private float speedMulti = 1;
    private float distance;

    public virtual void UpdateDestination(Vector3 dest)
    {
        destination = dest;
        stop = false;
    }

    public virtual void MovementUpdate()
    {
        if (stopping & !stop)
        {
            StopUnit();
        }
        else if (destination != null)
        {
            if (!facingTarget)
            {
                CheckRotation();
                
            }
            
            Accelerate(AccDirections.Forward);
            
            
        }
    }

    public virtual void CheckRotation()
    {
        float angle = Vector3.Angle(transform.forward, (Vector3)destination - transform.position);
        if( angle < 0.01f)
        {
            facingTarget = true;

        }
        else
        {
            ScriptingUtilities.PointToTarget(this, (Vector3)destination);
        }
    }

    public virtual void Accelerate(AccDirections direction)
    {
        if (transform.InverseTransformDirection(unitRigidbody.velocity).x < maxCombatSpeed)
        {
            if (direction == AccDirections.Forward)
            {
                distance = Vector3.Distance((Vector3)destination, transform.position);

                var relativePos = (Vector3)destination - transform.position;
                var forward = transform.forward;
                var angle = Vector3.Angle(relativePos, forward);

                if (angle < turnTolerance)
                {
                    if (distance < turnDist)
                    {
                        speedMulti = distance / turnDist;
                    }
                    else
                    {
                        speedMulti = 1;
                    }
                }
                else
                {
                    speedMulti = 0;
                }


                unitRigidbody.AddForce(transform.forward * Facceleration * speedMulti * turnSpeedMulti, ForceMode.Impulse);
            }
        }
    }

    public virtual void ClearDestination()
    {
        facingTarget = false;
        destination = null;
    }

    public virtual void StopUnit()
    {

        //print("stopping");
        stop = true;
        stopping = false;

        //else
        //{
        //    unitRigidbody.AddForce(new Vector3(-Bacceleration, 0, 0));
        //}


    }

    public virtual void InitNavigationAgent()
    {
        unitRigidbody = unit.GetComponent<Rigidbody>();
    }
    public IEnumerator UnitMovementLoop()
    {
        while (true)
        {
            MovementUpdate();
            yield return new WaitForSeconds(GlobalSettings.GameSpeed);
        }
    }

    void Start()
    {
        //StartCoroutine(UnitMovementLoop());
        InitNavigationAgent();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!stop)
        {
            MovementUpdate();
        }
        
    }
}

public enum AccDirections
{
    Forward,
    Backward,
    Left,
    Right
}
