using Malee;
using UnityEngine;

namespace NutBolts.Scripts.Targets
{
	[CreateAssetMenu(fileName = "TargetLevel", menuName = "TargetLevel")]
	public class CLevels : ScriptableObject
	{
		[Reorderable]
		public CTargetList targets;

		[System.Serializable]
		public class CTargetList : ReorderableArray<CTarget>
		{
		}
	}
	[System.Serializable]
	public class CTarget
	{
		public CTargetTypes type;
		public int amount;
	}
	public enum CTargetTypes
	{
		COLLECT,
		TIME
	}
}