using UnityEngine;

namespace NutBolts.Scripts.Item
{
    public class CircleRound : MonoBehaviour
    {
        public void Construct(int id)
        {
            var spriteMask = GetComponent<SpriteMask>();
            spriteMask.frontSortingOrder = id+1;
            spriteMask.backSortingOrder = id - 1;
        }
    }
}
