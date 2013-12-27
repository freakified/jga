using UnityEngine;
using System.Collections;


/// <summary>
/// http://answers.unity3d.com/questions/292031/how-to-display-a-rectangle-around-a-player.html?sort=oldest
/// </summary>
public class SelectionRect : MonoBehaviour {
	
	public float margin = 0;
	private Vector3[] pts = new Vector3[8];

	
	public void OnGUI () {
		//Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);

		//Construct a rect of the min and max positions and apply some margin
		
		//Render the box
		//GUI.Box (new Rect (pos.x, Screen.height - pos.y - 40, 150, 20), renderer.name);

		Bounds b = renderer.bounds;
		Camera cam = Camera.main;
		
		//The object is behind us
		if (cam.WorldToScreenPoint (b.center).z < 0) return;
		
		//All 8 vertices of the bounds
		pts[0] = cam.WorldToScreenPoint (new Vector3 (b.center.x + b.extents.x, b.center.y + b.extents.y, b.center.z + b.extents.z));
		pts[1] = cam.WorldToScreenPoint (new Vector3 (b.center.x + b.extents.x, b.center.y + b.extents.y, b.center.z - b.extents.z));
		pts[2] = cam.WorldToScreenPoint (new Vector3 (b.center.x + b.extents.x, b.center.y - b.extents.y, b.center.z + b.extents.z));
		pts[3] = cam.WorldToScreenPoint (new Vector3 (b.center.x + b.extents.x, b.center.y - b.extents.y, b.center.z - b.extents.z));
		pts[4] = cam.WorldToScreenPoint (new Vector3 (b.center.x - b.extents.x, b.center.y + b.extents.y, b.center.z + b.extents.z));
		pts[5] = cam.WorldToScreenPoint (new Vector3 (b.center.x - b.extents.x, b.center.y + b.extents.y, b.center.z - b.extents.z));
		pts[6] = cam.WorldToScreenPoint (new Vector3 (b.center.x - b.extents.x, b.center.y - b.extents.y, b.center.z + b.extents.z));
		pts[7] = cam.WorldToScreenPoint (new Vector3 (b.center.x - b.extents.x, b.center.y - b.extents.y, b.center.z - b.extents.z));
		
		//Get them in GUI space
		for (int i=0;i<pts.Length;i++) pts[i].y = Screen.height-pts[i].y;
		
		//Calculate the min and max positions
		Vector3 min = pts[0];
		Vector3 max = pts[0];
		for (int i=1;i<pts.Length;i++) {
			min = Vector3.Min (min, pts[i]);
			max = Vector3.Max (max, pts[i]);
		}
		
		//Construct a rect of the min and max positions and apply some margin
		Rect r = Rect.MinMaxRect (min.x,min.y,max.x,max.y);
		r.xMin -= margin;
		r.xMax += margin;
		r.yMin -= margin;
		r.yMax += margin;
		
		//Render the box
		GUI.Box (r,"This is a box covering the player");
	}

}