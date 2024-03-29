using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace NutBolts.Scripts.UI.UIMenu
{
    public class LevelButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private TMP_Text _levelText;
        private int _sceneIndex;

        public Button Button => _button;

        public void Assign(int sceneIndex)
        {
            _sceneIndex = sceneIndex;
            _levelText.text = _sceneIndex.ToString();
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveAllListeners();
        }
    }
}