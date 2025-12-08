using UnityEngine;
using UnityEngine.UI;
using TMPro;
using SmartRetailAR.Data;

namespace SmartRetailAR.Sprint2.UI
{
    /// <summary>
    /// Displays product information overlay in AR
    /// Sprint 2: AR data overlay
    /// </summary>
    public class ARProductOverlay : MonoBehaviour
    {
        [Header("UI Components")]
        [SerializeField] private Canvas overlayCanvas;
        [SerializeField] private TextMeshProUGUI productNameText;
        [SerializeField] private TextMeshProUGUI priceText;
        [SerializeField] private TextMeshProUGUI originText;
        
        [Header("Nutritional Panel")]
        [SerializeField] private GameObject nutritionalPanel;
        [SerializeField] private TextMeshProUGUI caloriesText;
        [SerializeField] private TextMeshProUGUI proteinText;
        [SerializeField] private TextMeshProUGUI carbsText;
        [SerializeField] private TextMeshProUGUI fatsText;
        
        [Header("Scores Panel")]
        [SerializeField] private GameObject scoresPanel;
        [SerializeField] private Image ecoScoreIndicator;
        [SerializeField] private TextMeshProUGUI ecoScoreText;
        [SerializeField] private Image healthScoreIndicator;
        [SerializeField] private TextMeshProUGUI healthScoreText;
        
        [Header("Tags")]
        [SerializeField] private GameObject bioIcon;
        [SerializeField] private GameObject veganIcon;
        [SerializeField] private GameObject localIcon;
        
        [Header("Settings")]
        [SerializeField] private float followSpeed = 5f;
        [SerializeField] private Vector3 offset = new Vector3(0, 0.1f, 0);
        
        private ProductData _currentProduct;
        private Transform _targetTransform;
        private bool _isVisible = false;
        
        void Start()
        {
            Hide();
        }
        
        void Update()
        {
            if (_isVisible && _targetTransform != null)
            {
                // Follow the target (tracked image or placed object)
                Vector3 targetPosition = _targetTransform.position + offset;
                transform.position = Vector3.Lerp(transform.position, targetPosition, 
                    Time.deltaTime * followSpeed);
                
                // Face the camera
                if (Camera.main != null)
                {
                    transform.LookAt(Camera.main.transform);
                    transform.Rotate(0, 180, 0);
                }
            }
        }
        
        public void DisplayProduct(ProductData product, Transform target)
        {
            _currentProduct = product;
            _targetTransform = target;
            
            UpdateProductInfo();
            Show();
        }
        
        void UpdateProductInfo()
        {
            if (_currentProduct == null) return;
            
            // Basic info
            if (productNameText != null)
                productNameText.text = _currentProduct.name;
            
            if (priceText != null)
                priceText.text = $"{_currentProduct.price:F2} €";
            
            if (originText != null)
                originText.text = _currentProduct.origin;
            
            // Nutritional info
            if (_currentProduct.nutritionalInfo != null)
            {
                if (caloriesText != null)
                    caloriesText.text = $"{_currentProduct.nutritionalInfo.calories:F0}";
                
                if (proteinText != null)
                    proteinText.text = $"{_currentProduct.nutritionalInfo.protein:F1}g";
                
                if (carbsText != null)
                    carbsText.text = $"{_currentProduct.nutritionalInfo.carbohydrates:F1}g";
                
                if (fatsText != null)
                    fatsText.text = $"{_currentProduct.nutritionalInfo.fats:F1}g";
            }
            
            // Scores
            UpdateScores();
            
            // Tags
            if (bioIcon != null)
                bioIcon.SetActive(_currentProduct.isBio);
            
            if (veganIcon != null)
                veganIcon.SetActive(_currentProduct.isVegan);
            
            if (localIcon != null)
                localIcon.SetActive(_currentProduct.isLocal);
        }
        
        void UpdateScores()
        {
            // Eco Score
            if (ecoScoreText != null)
            {
                ecoScoreText.text = $"{_currentProduct.ecoScore:F0}";
            }
            if (ecoScoreIndicator != null)
            {
                ecoScoreIndicator.fillAmount = _currentProduct.ecoScore / 100f;
                ecoScoreIndicator.color = GetScoreColor(_currentProduct.ecoScore);
            }
            
            // Health Score
            if (healthScoreText != null)
            {
                healthScoreText.text = $"{_currentProduct.healthScore:F0}";
            }
            if (healthScoreIndicator != null)
            {
                healthScoreIndicator.fillAmount = _currentProduct.healthScore / 100f;
                healthScoreIndicator.color = GetScoreColor(_currentProduct.healthScore);
            }
        }
        
        Color GetScoreColor(float score)
        {
            if (score >= 75f)
                return new Color(0.2f, 0.8f, 0.2f); // Green
            else if (score >= 50f)
                return new Color(1f, 0.8f, 0f); // Yellow
            else
                return new Color(0.9f, 0.2f, 0.2f); // Red
        }
        
        public void ToggleNutritionalPanel()
        {
            if (nutritionalPanel != null)
            {
                nutritionalPanel.SetActive(!nutritionalPanel.activeSelf);
            }
        }
        
        public void ToggleScoresPanel()
        {
            if (scoresPanel != null)
            {
                scoresPanel.SetActive(!scoresPanel.activeSelf);
            }
        }
        
        public void Show()
        {
            _isVisible = true;
            if (overlayCanvas != null)
            {
                overlayCanvas.enabled = true;
            }
            gameObject.SetActive(true);
        }
        
        public void Hide()
        {
            _isVisible = false;
            if (overlayCanvas != null)
            {
                overlayCanvas.enabled = false;
            }
            gameObject.SetActive(false);
        }
        
        public void SetTarget(Transform target)
        {
            _targetTransform = target;
        }
    }
}
