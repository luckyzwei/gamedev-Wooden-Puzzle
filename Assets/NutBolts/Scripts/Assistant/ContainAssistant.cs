using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainAssistant : MonoBehaviour
{
	public static ContainAssistant Instance;

	public List<ContainAssistantItem> cItems;
	public List<ContainAssistantItem> cSubItems;

	private Dictionary<string, GameObject> content = new Dictionary<string, GameObject>();

	private Transform front;
	private Transform quarantine;

	private GameObject zObj;

	void Awake()
	{
		Instance = this;
		content.Clear();
		foreach (ContainAssistantItem item in cItems)
			content.Add(item.item.name, item.item);
		foreach (ContainAssistantItem item in cSubItems)
			content.Add(item.item.name, item.item);
	}

	public T GetItem<T>(string key) where T : Component
	{
		return ((GameObject)Instantiate(content[key])).GetComponent<T>();
	}

	public GameObject GetItem(string key)
	{
		return (GameObject)Instantiate(content[key]);
	}

	public T GetItem<T>(string key, Vector3 position) where T : Component
	{
		zObj = GetItem(key);
		zObj.transform.position = position;
		return zObj.GetComponent<T>();
	}

	public GameObject GetItem(string key, Vector3 position)
	{
		zObj = GetItem(key);
		zObj.transform.position = position;
		return zObj;
	}

	public GameObject GetItem(string key, Vector3 position, Quaternion rotation)
	{
		zObj = GetItem(key, position);
		zObj.transform.rotation = rotation;
		return zObj;
	}


	[System.Serializable]
	public struct ContainAssistantItem
	{
		public GameObject item;
		public string category;
	}
}
