using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Lightbug.LaserMachine
{



	public class LaserMachine : MonoBehaviour
	{

		struct LaserElement
		{
			public Transform transform;
			public LineRenderer lineRenderer;
			public GameObject sparks;
			public bool impact;
		};

		List<LaserElement> elementsList = new List<LaserElement>();


		[Header("External Data")]

		public GameObject m_laserSparks;
		public Material m_laserMaterial;

		[Tooltip("This variable is true by default, all the inspector properties will be overridden.")]
		[SerializeField] bool m_overrideExternalProperties = true;

		[SerializeField] LaserProperties m_currentProperties = new LaserProperties();

		float m_time = 0;
		bool m_active = true;
		bool m_assignLaserMaterial;
		bool m_assignSparks;



		public void SetupLaser(float beamLength)
		{
			m_currentProperties.m_initialTimingPhase = Mathf.Clamp01(m_currentProperties.m_initialTimingPhase);

			float angleStep = m_currentProperties.m_angularRange / m_currentProperties.m_raysNumber;

			m_assignSparks = m_laserSparks != null;
			m_assignLaserMaterial = m_laserMaterial != null;

			for (int i = 0; i < m_currentProperties.m_raysNumber; i++)
			{
				LaserElement element = new LaserElement();

				GameObject newObj = new GameObject("lineRenderer_" + i.ToString());

				if (m_currentProperties.m_physicsType == LaserProperties.PhysicsType.Physics2D)
					newObj.transform.position = (Vector2)transform.position;
				else
					newObj.transform.position = transform.position;

				newObj.transform.rotation = transform.rotation;
				newObj.transform.Rotate(Vector3.up, i * angleStep);
				newObj.transform.position += newObj.transform.forward * m_currentProperties.m_minRadialDistance;

				newObj.AddComponent<LineRenderer>();

				if (m_assignLaserMaterial)
					newObj.GetComponent<LineRenderer>().material = m_laserMaterial;

				newObj.GetComponent<LineRenderer>().receiveShadows = false;
				newObj.GetComponent<LineRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
				newObj.GetComponent<LineRenderer>().startWidth = m_currentProperties.m_rayWidth;
				newObj.GetComponent<LineRenderer>().useWorldSpace = true;
				newObj.GetComponent<LineRenderer>().SetPosition(0, newObj.transform.position);
				newObj.GetComponent<LineRenderer>().SetPosition(1, newObj.transform.position + transform.forward * beamLength);
				newObj.transform.SetParent(transform);

				if (m_assignSparks)
				{
					GameObject sparks = Instantiate(m_laserSparks);
					sparks.transform.SetParent(newObj.transform);
					sparks.SetActive(false);
					element.sparks = sparks;
				}

				element.transform = newObj.transform;
				element.lineRenderer = newObj.GetComponent<LineRenderer>();
				element.impact = false;

				elementsList.Add(element);
			}

		}


		public IEnumerator Fire(float beamDuration, float damage, int player, float beamLength)
		{
			m_time = 0.0f;
			while (m_time < beamDuration)
			{
				m_time += Time.deltaTime;
				RaycastHit hitInfo3D;

				foreach (LaserElement element in elementsList)
				{
					if (m_currentProperties.m_rotate)
					{
						if (m_currentProperties.m_rotateClockwise)
							element.transform.RotateAround(transform.position, transform.up, Time.deltaTime * m_currentProperties.m_rotationSpeed);    //rotate around Global!!
						else
							element.transform.RotateAround(transform.position, transform.up, -Time.deltaTime * m_currentProperties.m_rotationSpeed);
					}


					element.lineRenderer.enabled = true;
					element.lineRenderer.SetPosition(0, element.transform.position);

					if (m_currentProperties.m_physicsType == LaserProperties.PhysicsType.Physics3D)
					{
						Physics.Linecast(
							element.transform.position,
							element.transform.position + element.transform.forward * beamLength,
							out hitInfo3D,
							m_currentProperties.m_layerMask
						);


						if (hitInfo3D.collider)
						{
							element.lineRenderer.SetPosition(1, hitInfo3D.point);

							if (m_assignSparks)
							{
								element.sparks.transform.position = hitInfo3D.point; //new Vector3(rhit.point.x, rhit.point.y, transform.position.z);
								element.sparks.transform.rotation = Quaternion.LookRotation(hitInfo3D.normal);
							}

							/*
							EXAMPLE : In this line you can add whatever functionality you want, 
							for example, if the hitInfoXD.collider is not null do whatever thing you wanna do to the target object.
							DoAction();
							*/

						}
						else
						{
							element.lineRenderer.SetPosition(1, element.transform.position + element.transform.forward * beamLength);

						}

						if (m_assignSparks)
							element.sparks.SetActive(hitInfo3D.collider != null);
					}

				}
				yield return null;
			}
			foreach (LaserElement element in elementsList)
			{
				element.lineRenderer.enabled = false;

				if (m_assignSparks)
					element.sparks.SetActive(false);

			}

			yield return null;
		}

		/*
		EXAMPLE : 
		void DoAction()
		{

		}
		*/


	}


}
