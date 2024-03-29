using UnityEngine;

namespace NutBolts.Scripts.Item
{
    public class Circle : MonoBehaviour
    {
        public void Init(int index)
        {
            var spriteMask = GetComponent<SpriteMask>();
            spriteMask.frontSortingOrder = index+1;
            spriteMask.backSortingOrder = index - 1;
        }
    }
}
