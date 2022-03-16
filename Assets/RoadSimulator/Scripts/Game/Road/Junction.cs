using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kawaiiju.Traffic
{
	public class Junction : Road 
	{
		public enum PhaseType { Timed}

		[Header("Junction")]
		public PhaseType type = PhaseType.Timed;
		public Phase[] phases;
		public JunctionTrigger[] triggers;
		public float phaseInterval = 5;
		
		private Mesh m_Cube;
		
		float m_PhaseTimer;
		bool m_PhaseEnded;
		private int m_CurrentPhase;

		private Mesh cube
		{
			get
			{
				if(m_Cube == null)
				{
					var primitive = GameObject.CreatePrimitive(PrimitiveType.Cube);
					m_Cube = primitive.GetComponent<MeshFilter>().sharedMesh;
					DestroyImmediate(primitive);
				}
				return m_Cube;
			}
		}

		public override void Start()
		{
			base.Start();
			if(phases.Length > 0)
				phases[0].Enable();
			foreach(var jt in triggers)
				jt.junction = this;
		}

		private void Update()
		{
			if(type == PhaseType.Timed)
			{
				m_PhaseTimer += Time.deltaTime;
				if(!m_PhaseEnded && m_PhaseTimer > (phaseInterval * 0.5f))
					EndPhase();
				if(m_PhaseTimer > phaseInterval)
					ChangePhase();
			}
		}
		
		private void EndPhase()
		{
			m_PhaseEnded = true;
			phases[m_CurrentPhase].End();
		}

		private void ChangePhase()
		{
			m_PhaseTimer = 0;
			m_PhaseEnded = false;
			if(m_CurrentPhase < phases.Length - 1)
				m_CurrentPhase++;
			else
				m_CurrentPhase = 0;
			phases[m_CurrentPhase].Enable();
		}

        public void TryChangePhase()
        {
            if (!HasActiveTrains())
                ChangePhase();
        }
        
		private void OnDrawGizmos()
		{
			if(TrafficSystem.instance.drawGizmos)
			{
				var phase = phases[m_CurrentPhase];
				foreach(var zone in phase.positiveZones)
				{	
					Gizmos.color = zone.canPass ? Color.green : Color.red;
					DrawAreaGizmo(zone.transform);
				}
				Gizmos.color = Color.red;
				foreach(var zone in phase.negativeZones)
					DrawAreaGizmo(zone.transform);
			}
		}

		private void DrawAreaGizmo(Transform t)
		{
			var rotationMatrix = Matrix4x4.TRS(t.position - new Vector3(0, t.localScale.y * 0.5f, 0), t.rotation, Vector3.Scale(t.lossyScale, new Vector3(1f, 0.1f, 1f)));
			Gizmos.matrix = rotationMatrix;
			Gizmos.DrawWireMesh(cube, Vector3.zero, Quaternion.identity);
		}

		[Serializable]
		public class Phase
		{
			public TrafficLight[] positiveLights;
			public TrafficLight[] negativeLights;
			public WaitZone[] positiveZones;
			public WaitZone[] negativeZones;

			public void Enable()
			{
				foreach(var zone in positiveZones)
					zone.canPass = true;
				foreach(var light in positiveLights)
					light.SetLight(true);
				foreach(var zone in negativeZones)
					zone.canPass = false;
				foreach(var light in negativeLights)
					light.SetLight(false);
			}

			public void End()
			{
				foreach(var zone in positiveZones)
					zone.canPass = false;
			}
		}
	}
}
