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
			agent = GetComponent<NavMeshAgent>();
			agent.angularSpeed = turnRate * 10;
			base.OnCreate();
		}

		public override void UpdateMarker(Marker marker)
		{
			if (marker is MarkerMove)
			{
				StopUnitAgent();
			}
			base.UpdateMarker(marker);
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

		public override void AttackUpdate()
		{
			base.AttackUpdate();
			if (Vector3.Distance(target.transform.position, transform.position) > mainGun.maxRange)
			{
				agent.isStopped = false;
				agent.destination = target.transform.position;
			}
			else
			{
				StopUnitAgent();
			}
		}

		public override void AssistUpdate()
		{
			base.AssistUpdate();
			if(builderType == BuilderTypes.none || assistTarget.isBuilt)
			{
				agent.destination = assistTarget.transform.position;
			}
			else if (builderType == BuilderTypes.engineer)
			{
				builder.AssistBuild(assistTarget);
			}
		}

		public override void StopOrder()
		{
			base.StopOrder();
			StopUnitAgent();
		}

		public virtual void StopUnitAgent()
		{
			agent.isStopped = true;
			agent.ResetPath();
		}

		public override void ToggleActive(bool toggle)
		{
			base.ToggleActive(toggle);
			agent.enabled = toggle;
		}
	}
}
