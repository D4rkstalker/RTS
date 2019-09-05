using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts
{
    class MobileUnit : Unit
    {

        private NavMeshAgent agent;
        private Vector3 destination;
        public float health;
        public float accelerationMulti;
        public float turnRate;
        public bool autoPilot;
        public float speed;

        // Start is called before the first frame update
        void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            agent.acceleration *= accelerationMulti;
        }

        // Update is called once per frame
        void Update()
        {
            float facing = Vector3.Dot(transform.forward, (destination - transform.position).normalized);
            //Debug.Log(facing);
            if (Input.GetMouseButtonDown(1) && selected)
            {
                RightClick();
            }
            if (!autoPilot)
            {
                if (facing > 0.99f)
                {
                    //agent.isStopped = true;
                    agent.destination = destination;
                    autoPilot = true;
                }
                else
                {
                    float distance = Vector3.Distance(destination, transform.position);
                    //if (distance > 0.1)
                    //{
                        Vector3 direction = destination - transform.position;
                        transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, direction, turnRate, 0.0f));
                        Vector3 movement = transform.forward * Time.deltaTime * speed;
                        agent.Move(movement);
                    //}
                }
            }
        }

        public void RightClick()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100))
            {
                if (hit.collider.tag == "Ground")
                {
                    destination = hit.point;
                    autoPilot = false;
                    Debug.Log("Changing Position!");
                }
            }
        }
    }
}
