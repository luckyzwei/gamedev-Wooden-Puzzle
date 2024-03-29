using Malee;
using UnityEngine;

namespace NutBolts.Scripts.Targets
{
	[CreateAssetMenu(fileName = "TargetLevel", menuName = "TargetLevel")]
	public class CTargetLevels : ScriptableObject
	{
		[Reorderable]
		public CTargetList targets;

		[System.Serializable]
		public class CTargetList : ReorderableArray<CTargetObject>
		{
		}
	}
	[System.Serializable]
	public class CTargetObject
	{
		public CTarget type;
		public int amount;
	}
	public enum CTarget
	{
		COLLECT,
		TIME
	}
}