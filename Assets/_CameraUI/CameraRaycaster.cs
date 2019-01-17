using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System;
using RPG.Character;
namespace RPG.CameraUI
{
	public class CameraRaycaster : MonoBehaviour
	{
		const int WALKABLE_LAYER = 8;

		
		[SerializeField] Texture2D walkCursor = null;
		[SerializeField] Texture2D unknownCursor = null;
		[SerializeField] Texture2D targetCursor = null;
		[SerializeField] Vector2 cursorHotspot = new Vector2(0, 0);
		float maxRaycastDepth = 100f; // Hard coded value
		public Vector3 MoseInstaceposition;
		Rect screemRectOncontruction = new Rect(0, 0,Screen.width,Screen.height);//TODO determine for rescale window

		public delegate void OnMouseOverTerrain(Vector3 destination); // declare new delegate type
		public event OnMouseOverTerrain OnMouseOverPotetiallWalkable;

		public delegate void OnMouseOverEnemy(Enemy enemy); // declare new delegate type
		public event OnMouseOverEnemy onMouseOverEnemy;


	
		void Update() 
		{
			// Check if pointer is over an interactable UI element
			if (EventSystem.current.IsPointerOverGameObject())
			{
				//NotifyObserersIfLayerChanged(5);

			}
			else
			{
				PerformRayCast();
			}


		}

		void PerformRayCast()
		{
			if (screemRectOncontruction.Contains(Input.mousePosition))
			{

			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (RaycastFOrEnemy(ray)) { return; }
			if (RaycastForWalkable(ray)) { return; }
			}

		}
		private bool RaycastFOrEnemy(Ray ray)
		{
			
			RaycastHit hitInfo;
			Physics.Raycast(ray, out hitInfo, maxRaycastDepth);
			var gameObjectHit = hitInfo.collider.gameObject;
	
			var enemyHit = gameObjectHit.GetComponent<Enemy>();
			if(enemyHit)
			{
			
				Cursor.SetCursor(targetCursor, cursorHotspot, CursorMode.Auto);
				onMouseOverEnemy(enemyHit);
				return true;
			}

			return false;
		}

		private bool RaycastForWalkable(Ray ray)
		{
			RaycastHit hitInfo;

			LayerMask potentialyWalkableLayer = 1 << WALKABLE_LAYER;
			bool potentiallyWalkableHit = Physics.Raycast(ray, out hitInfo, maxRaycastDepth, potentialyWalkableLayer);
			MoseInstaceposition = hitInfo.point;
			if (potentiallyWalkableHit)
			{
				
				Cursor.SetCursor(walkCursor, cursorHotspot, CursorMode.Auto);
				OnMouseOverPotetiallWalkable(hitInfo.point);
				return true;
			}

			return false;

		}

	


		
	}
}
