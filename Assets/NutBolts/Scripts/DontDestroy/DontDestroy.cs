using UnityEngine;

namespace NutBolts.Scripts.DontDestroy
{
    public class DontDestroy : MonoBehaviour
    {
        void Awake()
        {
            DontDestroyOnLoad(this);
        }
    }
}
