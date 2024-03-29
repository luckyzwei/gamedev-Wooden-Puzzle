using System.Collections;
using UnityEngine;

namespace NutBolts.Scripts.Assistant
{
    public class CameraChange : MonoBehaviour
    {  
        private Rect _rectField;
        private float _height;
        private float _width;
        private readonly float _maxY = 6.5f;
        private readonly float _minY = -6.5f;
        private readonly float _minX = -3.6f;
        private readonly float _maxX = 3.6f;
        private void OnEnable()
        {
            CLevelManager.OnEnterGame += ChangeAspect;
        }

        private void OnDisable()
        {
            CLevelManager.OnEnterGame -= ChangeAspect;
        }

        private void ChangeAspect()
        {
            StartCoroutine(DelayRoutine());
        }

        private IEnumerator DelayRoutine()
        {
            yield return new WaitWhile(() => !CLevelManager.THIS);
            if (CLevelManager.THIS.GameStatus == GameState.PrepareGame)
            {
                _height = _maxY - _minY ;
                _width = _maxX - _minX;
                _rectField = new Rect(_minX, _maxY, _width, _height);
                float width = (float)Screen.width;
                float height = (float)Screen.height;
                var h = 0.5f * _rectField.width * height / width;
                var w = (_rectField.height) / 2;
                var maxLength = Mathf.Max(h, w);
                Camera.main.orthographicSize = Mathf.Clamp(h, maxLength, maxLength);

            }
            else
                Camera.main.orthographicSize = 6.5f / Screen.width * Screen.height / 2f;

            transform.position = Vector3.down * 0;
        }

    }
}