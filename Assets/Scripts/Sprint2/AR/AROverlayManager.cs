using UnityEngine;
using UnityEngine.UI;
using SmartRetailAR.Data;
using TMPro;

namespace SmartRetailAR.AR
{
    public class AROverlayManager : MonoBehaviour
    {
        [Header("Overlay Elements")]
        public Canvas overlayCanvas;
        public TextMeshProUGUI productNameText;
        public TextMeshProUGUI priceText;
        public Image ecoScoreImage;
        public Image healthScoreImage;
        public GameObject bioTag;

        [Header("Animation")]
        public float fadeInDuration = 0.5f;
        public bool alwaysFaceCamera = true;

        private ProductData _currentProduct;
        private Camera _mainCamera;
        private CanvasGroup _canvasGroup;
        private float _fadeTimer;
        private bool _isFading;

        void Awake()
        {
            _mainCamera = Camera.main;
            if (overlayCanvas != null)
            {
                _canvasGroup = overlayCanvas.GetComponent<CanvasGroup>();
                if (_canvasGroup == null)
                    _canvasGroup = overlayCanvas.gameObject.AddComponent<CanvasGroup>();
                _canvasGroup.alpha = 0f;
            }
        }

        void Update()
        {
            if (alwaysFaceCamera && _mainCamera != null && overlayCanvas != null)
            {
                overlayCanvas.transform.LookAt(_mainCamera.transform);
                overlayCanvas.transform.Rotate(0, 180, 0);
            }

            if (_isFading)
            {
                UpdateFade();
            }
        }

        public void SetProductData(ProductData product)
        {
            _currentProduct = product;
            UpdateOverlayUI();
            StartFadeIn();
        }

        private void UpdateOverlayUI()
        {
            if (_currentProduct == null) return;

            if (productNameText != null)
                productNameText.text = _currentProduct.name;

            if (priceText != null)
                priceText.text = $"{_currentProduct.price:F2}€";

            if (bioTag != null)
                bioTag.SetActive(_currentProduct.flags.isBio);

            UpdateScoreImages();
        }

        private void UpdateScoreImages()
        {
            if (_currentProduct.scores != null)
            {
                if (ecoScoreImage != null)
                    ecoScoreImage.color = GetScoreColor(_currentProduct.scores.ecoScore);

                if (healthScoreImage != null)
                    healthScoreImage.color = GetScoreColor(_currentProduct.scores.healthScore);
            }
        }

        private Color GetScoreColor(string score)
        {
            switch (score)
            {
                case "A": return new Color(0.2f, 0.8f, 0.2f);
                case "B": return new Color(0.5f, 0.8f, 0.2f);
                case "C": return new Color(0.9f, 0.8f, 0.2f);
                case "D": return new Color(0.9f, 0.5f, 0.2f);
                case "E": return new Color(0.9f, 0.2f, 0.2f);
                default: return Color.gray;
            }
        }

        private void StartFadeIn()
        {
            _isFading = true;
            _fadeTimer = 0f;
        }

        private void UpdateFade()
        {
            _fadeTimer += Time.deltaTime;
            float alpha = Mathf.Clamp01(_fadeTimer / fadeInDuration);
            if (_canvasGroup != null)
                _canvasGroup.alpha = alpha;

            if (_fadeTimer >= fadeInDuration)
            {
                _isFading = false;
            }
        }

        public void Hide()
        {
            if (_canvasGroup != null)
                _canvasGroup.alpha = 0f;
        }

        public void Show()
        {
            StartFadeIn();
        }
    }

    public class AROverlayController : MonoBehaviour
    {
        private AROverlayManager _overlayManager;

        void Awake()
        {
            _overlayManager = GetComponent<AROverlayManager>();
        }

        public void SetProductData(ProductData product)
        {
            if (_overlayManager != null)
            {
                _overlayManager.SetProductData(product);
            }
        }
    }
}
