using UnityEngine;

namespace RoadSimulator.Scripts.Game.Simulation.RoadSystem
{
	public class TrafficLight : MonoBehaviour 
	{
		public MeshRenderer Renderer;
		
		private static readonly int PropertyId = Shader.PropertyToID("_Color");

		public void SetLight(bool input)
		{
			Renderer.material.SetColor(PropertyId, input ? Color.green : Color.red );
		}
	}
}
