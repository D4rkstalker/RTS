using UnityEngine;

public class CameraControl : MonoBehaviour
{
	public Transform camObj;
	public float panMulti = 1;
	public float zoomMulti = 1;
	private float oMousex = new float();
	private float oMousey = new float();
	public float minSize = 10;
	public float maxSize = 200;
	public float smoothTime = 0.3F;
	public float[] xBound = new float[] { -200, 200 };
	public float[] zBound = new float[] { -200, 200 };
	public float yBound;
	public int player;
	private float oHeight;
	private Vector3 velocity = Vector3.zero;
	// Start is called before the first frame update
	void Start()
    {
		oHeight = Input.GetAxis("Mouse ScrollWheel");
	}

	// Update is called once per frame
	void Update()
	{
		MoveCamera();
	}
	void MoveCamera()
	{
		float camSize = Camera.main.orthographicSize;
		if (oHeight != Input.GetAxis("Mouse ScrollWheel"))
		{
			float height = Input.GetAxis("Mouse ScrollWheel") ;
			
			Camera.main.orthographicSize = Mathf.Clamp(camSize - height * zoomMulti * Mathf.InverseLerp(minSize, maxSize, camSize) * 300, minSize * 2, maxSize);
			//if (true)
			//{
			//	Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			//	RaycastHit hit;
			//	if (Physics.Raycast(ray, out hit, 100))
			//	{
			//		Vector3 target = hit.point;
			//		target.y = camObj.position.y;
			//		camObj.Translate(target-camObj.transform.position);
			//	}
				oHeight = height;
			//}
		}
		if (Input.GetMouseButton(2))
		{
			if(oMousex == default || oMousey == default)
			{
				oMousex = Input.mousePosition.x;
				oMousey = Input.mousePosition.y;
			}
			Vector2 moveMent = new Vector2();
			moveMent.y = (Input.mousePosition.y - oMousey) * panMulti * Time.deltaTime * camSize * 0.1f;
			moveMent.x = (Input.mousePosition.x - oMousex) * panMulti * Time.deltaTime * camSize * 0.1f;
			camObj.Translate(moveMent);
		}
		else
		{
			oMousex = new float();
			oMousey = new float();
		}
		camObj.transform.position = new Vector3(Mathf.Clamp(camObj.transform.position.x, xBound[0], xBound[1]), yBound, Mathf.Clamp(camObj.transform.position.z, zBound[0], zBound[1]));

	}

}
