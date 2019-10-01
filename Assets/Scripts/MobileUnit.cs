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

        public GameObject selectionIndicator;

        public int priority;

        // Start is called before the first frame update
        void Start()
        {
            agent = GetComponent<NavMeshAgent>();
        }

        // Update is called once per frame
        void Update()
        {
            UpdateUnit();
            //float facing = Vector3.Dot(transform.forward, (destination - transform.position).normalized);
            //Debug.Log(facing);
            if (Input.GetMouseButtonDown(1) && selected)
            {
                RightClick();
            }
            selectionIndicator.SetActive(selected);
        }

        public void RightClick()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100))
            {
                if (hit.collider.tag == "Ground")
                {
                    Move(hit.point);
                    
                }
            }
        }

        public void Move(Vector3 destination)
        {
            agent.destination = destination;
            Debug.Log("Changing Position!");
            task = Tasks.Moving;
        }
    }
}
