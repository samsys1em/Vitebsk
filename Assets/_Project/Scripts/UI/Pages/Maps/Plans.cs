using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI.Pages.Maps
{
    public class Plans : MonoBehaviour
    {
        [SerializeField] private Image _image;
        [SerializeField] private float _fadeDuration = 1f;
        [SerializeField] private Image _maskImage;
        [SerializeField] private float _maskFadeDuration = 1f;

        private void OnEnable()
        {
            _image.DOKill();
            _image.color = new Color(1f, 1f, 1f, 0);
            _image.DOFade(1, _fadeDuration);

            if (_maskImage == null)
                return;
            
            _maskImage.DOKill();
            _maskImage.fillAmount = 0;
            _maskImage.DOFillAmount(1, _maskFadeDuration);
        }
    }
}