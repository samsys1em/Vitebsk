using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI.Pages.Components
{
    public class UILine : MonoBehaviour
    {
        [SerializeField] private RectTransform _pointA;
        [SerializeField] private RectTransform _pointLeft;
        [SerializeField] private RectTransform _pointRight;
        [SerializeField] private GameObject _leftSubLine;
        [SerializeField] private GameObject _rightSubLine;
        [SerializeField] private bool _isLeft = true;
        [SerializeField] private Image _image;
        [SerializeField] private float _offset = 10f;


        [ContextMenu("Set Line With Offset")]
        public void SetLineWithOffset()
        {
            var pointB = _isLeft ? _pointLeft : _pointRight;
            _leftSubLine.SetActive(_isLeft);
            _rightSubLine.SetActive(!_isLeft);

            var dir = pointB.position - _pointA.position;
            var dist = dir.magnitude;

            if (dist < 0.001f)
                return;

            var offsetPos = _pointA.position + dir.normalized * _offset;
            var newDist = Mathf.Max(0, dist - _offset);

            var rt = _image.rectTransform;
            rt.position = offsetPos;
            rt.sizeDelta = new Vector2(newDist, rt.sizeDelta.y);
            rt.right = dir;
        }
    }
}