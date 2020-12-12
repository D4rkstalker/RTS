using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.ScriptingUtilities
{
	public static class ImageHelpers
	{
		public static Texture2D AlphaBlend(this Texture2D aBottom, Texture2D aTop)
		{
			if (aBottom.width != aTop.width || aBottom.height != aTop.height)
				throw new System.InvalidOperationException("AlphaBlend only works with two equal sized images");
			var bData = aBottom.GetPixels();
			var tData = aTop.GetPixels();
			int count = bData.Length;
			var rData = new Color[count];
			for (int i = 0; i < count; i++)
			{
				Color B = bData[i];
				Color T = tData[i];
				float srcF = T.a;
				float destF = 1f - T.a;
				float alpha = srcF + destF * B.a;
				Color R = (T * srcF + B * B.a * destF) / alpha;
				R.a = alpha;
				rData[i] = R;
			}
			var res = new Texture2D(aTop.width, aTop.height);
			res.SetPixels(rData);
			res.Apply();
			return res;
		}
	}
	public class ScriptingUtilities : MonoBehaviour
	{
		public static void PointToTarget(Turret turret, Unit target)
		{
			MobileUnit unit = turret.GetComponent<MobileUnit>();
			Vector3 interceptPoint = target.transform.position;
			if (turret.leadTarget)
			{
				interceptPoint = FirstOrderIntercept(
					turret.transform.position,
					 unit ? unit.agent.unitRigidbody.velocity : Vector3.zero,
					turret.projectileVelocity,
					target.transform.position,
					target.GetComponent<MobileUnit>() ? target.GetComponent<MobileUnit>().agent.unitRigidbody.velocity : Vector3.zero
				);

			}
			if (turret.mainGun && turret.tType == TurretTypes.Spinal)
			{
				PointToTarget(unit.agent,interceptPoint);
			}
			else
			{
				Vector3 _direction = (interceptPoint - turret.transform.position).normalized;

				Quaternion _lookRotation = Quaternion.LookRotation(_direction);

				turret.transform.rotation = Quaternion.Slerp(turret.transform.rotation, _lookRotation, Time.deltaTime * turret.turnRate);

			}
		}
		public static void PointToTarget(NavigationAgent agent, Vector3 target)
		{
			Vector3 _direction = (target - agent.transform.position).normalized;

			Quaternion _lookRotation = Quaternion.LookRotation(_direction);

			agent.unit.transform.rotation = Quaternion.Slerp(agent.unit.transform.rotation, _lookRotation, Time.deltaTime * agent.turnrate);


		}


		public static Player GetPlayerByID(int pid)
		{
			Player[] temp = (Player[])FindObjectsOfType(typeof(Player));
			foreach (Player player in temp)
			{
				if (pid == player.playerID)
				{
					return player;
				}
			}
			return null;
		}

		public static Vector3 FirstOrderIntercept
		(
			Vector3 shooterPosition,
			Vector3 shooterVelocity,
			float shotSpeed,
			Vector3 targetPosition,
			Vector3 targetVelocity
		)
		{
			Vector3 targetRelativePosition = targetPosition - shooterPosition;
			Vector3 targetRelativeVelocity = targetVelocity - shooterVelocity;
			float t = FirstOrderInterceptTime
			(
				shotSpeed,
				targetRelativePosition,
				targetRelativeVelocity
			);
			return targetPosition + t * (targetRelativeVelocity);
		}
		public static float FirstOrderInterceptTime(float shotSpeed, Vector3 targetRelativePosition, Vector3 targetRelativeVelocity)
		{
			float velocitySquared = targetRelativeVelocity.sqrMagnitude;
			if (velocitySquared < 0.001f)
				return 0f;

			float a = velocitySquared - shotSpeed * shotSpeed;

			//handle similar velocities
			if (Mathf.Abs(a) < 0.001f)
			{
				float t = -targetRelativePosition.sqrMagnitude /
				(
					2f * Vector3.Dot
					(
						targetRelativeVelocity,
						targetRelativePosition
					)
				);
				return Mathf.Max(t, 0f); //don't shoot back in time
			}

			float b = 2f * Vector3.Dot(targetRelativeVelocity, targetRelativePosition);
			float c = targetRelativePosition.sqrMagnitude;
			float determinant = b * b - 4f * a * c;

			if (determinant > 0f)
			{ //determinant > 0; two intercept paths (most common)
				float t1 = (-b + Mathf.Sqrt(determinant)) / (2f * a),
						t2 = (-b - Mathf.Sqrt(determinant)) / (2f * a);
				if (t1 > 0f)
				{
					if (t2 > 0f)
						return Mathf.Min(t1, t2); //both are positive
					else
						return t1; //only t1 is positive
				}
				else
					return Mathf.Max(t2, 0f); //don't shoot back in time
			}
			else if (determinant < 0f) //determinant < 0; no intercept path
				return 0f;
			else //determinant = 0; one intercept path, pretty much never happens
				return Mathf.Max(-b / (2f * a), 0f); //don't shoot back in time
		}
	}
}
