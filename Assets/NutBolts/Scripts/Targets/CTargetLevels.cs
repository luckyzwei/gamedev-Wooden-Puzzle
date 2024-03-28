using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Malee;
using UnityEngine.Serialization;

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
	// amount is second if (type==CTarget.TIME)
	// amount is bar number if (type==CTarget.COLLECT)
	public int amount;
}
public enum CTarget
{
	COLLECT,
	TIME
}
