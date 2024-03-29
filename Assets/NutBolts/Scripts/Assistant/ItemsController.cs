using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace NutBolts.Scripts.Assistant
{
	public class ItemsController : MonoBehaviour
	{
		[Inject] private DiContainer _diContainer;
		[FormerlySerializedAs("cItems")] public List<Item> _mainItem;
		[FormerlySerializedAs("cSubItems")] public List<Item> _items;

		private Dictionary<string, GameObject> _itemMap = new();
		private GameObject _zPos;

		private void Awake()
		{
			_itemMap.Clear();
			foreach (Item item in _mainItem)
				_itemMap.Add(item._itemPrefab.name, item._itemPrefab);
			foreach (Item item in _items)
				_itemMap.Add(item._itemPrefab.name, item._itemPrefab);
		}

		public T TakeItem<T>(string key) where T : Component
		{
			return _diContainer.InstantiatePrefab(_itemMap[key]).GetComponent<T>();
		}

		public GameObject TakeItem(string key)
		{
			return _diContainer.InstantiatePrefab(_itemMap[key]);
		}
		
		public GameObject TakeItem(string key, Vector3 position)
		{
			_zPos = TakeItem(key);
			_zPos.transform.position = position;
			return _zPos;
		}

		public GameObject TakeItem(string key, Vector3 position, Quaternion rotation)
		{
			_zPos = TakeItem(key, position);
			_zPos.transform.rotation = rotation;
			return _zPos;
		}


		[System.Serializable]
		public struct Item
		{
			[FormerlySerializedAs("item")] public GameObject _itemPrefab;
			[FormerlySerializedAs("category")] public string _name;
		}
	}
}
