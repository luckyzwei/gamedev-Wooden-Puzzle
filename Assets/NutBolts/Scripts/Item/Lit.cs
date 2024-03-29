using UnityEngine;

namespace NutBolts.Scripts.Item
{
    public class Lit : MonoBehaviour
    {
        public int iIndex;
        public GameObject lightObject;
        private Screw screw;
        private Hole hole;
        void Start()
        {
            TurnOff();
        }
        
        public void SetScrew(Screw sc)
        {
            screw = sc;
        }
        public void SetHole(Hole h)
        {
            hole = h;
        }
        public Screw GetScrew()
        {
            return screw;
        }
        public Hole GetHole()
        {
            return hole;
        }
        public void TurnOn()
        {
            lightObject.SetActive(true);
        }
        public void TurnOff()
        {
            lightObject.SetActive(false);
        }
    }
}