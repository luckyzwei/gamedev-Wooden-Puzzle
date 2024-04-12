using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace NutBolts.Scripts.UI.UIMenu
{
    public class LevelButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private TMP_Text _levelText;
        [SerializeField] private GameObject _lockImage;
        private int _sceneIndex;

        public Button Button => _button;

        public void Assign(int sceneIndex)
        {
            _sceneIndex = sceneIndex;
            _levelText.text = _sceneIndex.ToString();
            _levelText.gameObject.SetActive(true);
            _lockImage.SetActive(false);
        }
        
        private void OnDestroy()
        {
            _button.onClick.RemoveAllListeners();
        }
    }
}