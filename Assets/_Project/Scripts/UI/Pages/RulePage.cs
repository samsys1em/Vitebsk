using DG.Tweening;
using Scripts.Core.Systems.UI;
using Scripts.Core.Systems.UI.Pages;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI.Pages
{
    public class RulePage : BasePage
    {
        [SerializeField] private Button _backButton;
        [SerializeField] private Button _ruleFirst;
        [SerializeField] private Button _ruleSecond;
        [SerializeField] private Image _shadowFirst;
        [SerializeField] private Image _shadowSecond;
        [SerializeField] private float _maxFade = 0.4f;
        [SerializeField] private float _fadeDuration = 1f;
        [SerializeField] private ScrollRect _scrollRect;


        public override string Id => PageId.Rule;


        protected override void OnInitialization()
        {
            _backButton.onClick.AddListener(OnBackButtonDown);
            _ruleFirst.onClick.AddListener(OnRuleFirstDown);
            _ruleSecond.onClick.AddListener(OnRuleSecondDown);
        }

        protected override void OnBeforeOpen(IUIOpenParam openParam)
        {
            _ruleFirst.transform.SetAsLastSibling();

            _shadowFirst.DOKill();
            _shadowSecond.DOKill();

            _shadowFirst.color = new Color(0, 0, 0, 0);
            _shadowSecond.color = new Color(0, 0, 0, _maxFade);

            _scrollRect.verticalNormalizedPosition = 1;
        }

        private void OnRuleFirstDown()
        {
            _ruleFirst.transform.SetAsLastSibling();

            _shadowFirst.DOKill();
            _shadowSecond.DOKill();

            _shadowFirst.DOFade(0, _fadeDuration);
            _shadowSecond.DOFade(_maxFade, _fadeDuration);
        }

        private void OnRuleSecondDown()
        {
            _ruleSecond.transform.SetAsLastSibling();

            _shadowSecond.DOKill();
            _shadowFirst.DOKill();

            _shadowSecond.DOFade(0, _fadeDuration);
            _shadowFirst.DOFade(_maxFade, _fadeDuration);
        }

        private void OnBackButtonDown() => UIProvider.OpenPage(PageId.MainPage);
    }
}