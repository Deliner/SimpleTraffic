using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kawaiiju.Traffic
{
	public class WaitZone : MonoBehaviour 
	{
		public TrafficType type;
		public WaitZone opposite;
		public bool canPass;
	}
}