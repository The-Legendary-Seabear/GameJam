using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CGL.Inventory
{
	// melee ammo using overlap sphere for hit detection.
	// spawn at the weapon or hand position via animation event.
	public class MeleeAmmo : Ammo
	{
		// set by the weapon that spawns this ammo — used to exclude the owner from hit detection
		public Transform Owner { get; set; }
        private Transform punchPoint;

        public void SetPunchPoint(Transform point)
        {
            punchPoint = point;
        }

        private void Start()
		{
			if (ammoData == null)
			{
                Debug.LogError("MeleeAmmo has no ammoData assigned!", this);
                Destroy(gameObject);
				return;
			}

            Debug.Log("MeleeAmmo has ammoData: " + ammoData.name, this);
            FireMelee();
			Destroy(gameObject);
		}

		private void FireMelee()
		{
			Debug.Log("FireMelee was executed");
			Vector3 center = transform.position +
				transform.TransformDirection(ammoData.meleeOffset);

            //TEMP
            int enemyMask = LayerMask.GetMask("Agent");

            // detect all colliders in radius
            //Collider[] hits = Physics.OverlapSphere(center,
            Collider[] hits = Physics.OverlapSphere(punchPoint.position,
				//ammoData.meleeRadius, ammoData.hitLayerMask);
				ammoData.meleeRadius, enemyMask);
            Debug.Log($"Melee hit {hits.Length} colliders");

            Debug.DrawRay(punchPoint.position, Vector3.up * 0.5f, Color.red, 1f);

            /*
			foreach (Collider hit in hits)
			{
				// skip owner and all their children
				if (Owner != null && hit.transform.IsChildOf(Owner)) continue;

				ApplyDamage(hit.gameObject, center, Vector3.up);
				SpawnImpactEffect(hit.transform.position, Vector3.up);

                if (hit.GetComponentInParent<StateAgent>() is StateAgent agent)
                {
                    Debug.Log("Enemy was hit");
                    agent.stateMachine.SetState<AIDamageState>();
                }
            } */
            HashSet<StateAgent> hitAgents = new HashSet<StateAgent>();

            foreach (Collider hit in hits)
            {
                if (Owner != null && hit.transform.IsChildOf(Owner)) continue;

                ApplyDamage(hit.gameObject, center, Vector3.up);
                SpawnImpactEffect(hit.transform.position, Vector3.up);

                if (hit.GetComponentInParent<StateAgent>() is StateAgent agent && hitAgents.Add(agent))
                {
                    Debug.Log("Enemy was hit");
					agent.OnDamage(10);
                }
            }
        }

		// melee uses overlap sphere not physics collision
		protected override void OnCollisionEnter(Collision collision) { }
		protected override void OnCollisionStay(Collision collision) { }

#if UNITY_EDITOR
		private void OnDrawGizmosSelected()
		{
			if (ammoData == null) return;
			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere(
				transform.position + transform.TransformDirection(ammoData.meleeOffset),
				ammoData.meleeRadius);
		}
#endif
	}
}