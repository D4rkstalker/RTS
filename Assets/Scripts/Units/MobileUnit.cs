using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts
{
    public class MobileUnit : Unit
    {
		[System.NonSerialized]
		public NavMeshAgent agent;

        public int priority;

		// Start is called before the first frame update
		public override void OnCreate()
		{
			base.OnCreate();
			agent = GetComponent<NavMeshAgent>();
			agent.angularSpeed = turnRate * 50;
		}
        // Update is called once per frame
		public override void UpdateUnit()
		{
			base.UpdateUnit();
			if (!isBeingbuilt)
			{
				if (target != null && (task == Tasks.Attacking || task == Tasks.Idle))
				{
					AttackUpdate();
				}

			}
		}

		public override void CheckMarker(Marker marker)
		{
			base.CheckMarker(marker);
			if(marker is MarkerMove)
			{
				if (target)
				{
					target = null;
				}
				agent.isStopped = false;
				agent.destination = marker.transform.position;
				task = Tasks.Moving;
			}
		}

		public void AttackUpdate()
		{

			if (Vector3.Distance(target.transform.position, transform.position) > mainGun.maxRange)
			{
				agent.isStopped = false;
				agent.destination = target.transform.position;
			}
			else
			{
				agent.isStopped = true;
			}
		}

		public override void StopOrder()
		{
			base.StopOrder();
			agent.isStopped = true;
		}
	}
}
