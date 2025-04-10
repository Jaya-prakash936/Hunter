using UnityEngine;
using Drakkar.GameUtils;
using System.Globalization;

namespace Drakkar.Examples
{
	public class AnimationEvents : MonoBehaviour
	{
		public DrakkarTrail Trail;

		public void StartTrail()
		{
			Trail.Begin();
		}

		public void StopTrail()
		{
			Trail.End();
		}
	}
}