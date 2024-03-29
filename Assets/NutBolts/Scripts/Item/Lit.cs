using UnityEngine;

namespace NutBolts.Scripts.Item
{
    public class Lit : MonoBehaviour
    {
        public int iIndex;
        public GameObject lightObject;
        public Screw Screw { get; set; }
        public bool IsActive { get => lightObject.activeSelf; set => lightObject.SetActive(value); }

        private void Start()
        {
            IsActive = false;
        }
        
        public void TurnOff()
        {
            lightObject.SetActive(false);
        }
    }
}