using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kawaiiju.Traffic
{
	public class Road : NavSection 
	{
		[Header("Road")]
		public Transform[] pedestrianSpawns;
		
		public bool TryGetPedestrianSpawn(out Transform spawn)
		{
			if(pedestrianSpawns.Length > 0)
			{
				var index = UnityEngine.Random.Range(0, pedestrianSpawns.Length);
				spawn = pedestrianSpawns[index];
				return true;
			}
			spawn = null;
			return false;
		}
	}
}
