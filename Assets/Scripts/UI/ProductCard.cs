using UnityEngine;
using UnityEngine.UI;
using TMPro;
using SmartRetailAR.Data;

namespace SmartRetailAR.UI
{
    /// <summary>
    /// Product Card UI component for displaying product in lists
    /// </summary>
    public class ProductCard : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private TextMeshProUGUI productNameText;
        [SerializeField] private TextMeshProUGUI brandText;
        [SerializeField] private TextMeshProUGUI priceText;
        [SerializeField] private TextMeshProUGUI nutriScoreText;
        [SerializeField] private TextMeshProUGUI ecoScoreText;
        [SerializeField] private Image productImage;
        [SerializeField] private Button cardButton;

        private ProductData productData;

        private void Awake()
        {
            if (cardButton != null)
            {
                cardButton.onClick.AddListener(OnCardClicked);
            }
        }

        /// <summary>
        /// Set product data for this card
        /// </summary>
        public void SetProductData(ProductData product)
        {
            productData = product;
            UpdateDisplay();
        }

        private void UpdateDisplay()
        {
            if (productData == null) return;

            if (productNameText != null)
                productNameText.text = productData.name;

            if (brandText != null)
                brandText.text = productData.brand;

            if (priceText != null)
                priceText.text = $"{productData.price:F2}€";

            if (nutriScoreText != null && productData.nutritionalInfo != null)
                nutriScoreText.text = $"Nutri: {productData.nutritionalInfo.nutriScore}";

            if (ecoScoreText != null && productData.ecologicalInfo != null)
                ecoScoreText.text = $"Éco: {productData.ecologicalInfo.ecoScore}";
        }

        private void OnCardClicked()
        {
            if (productData != null)
            {
                PlayerPrefs.SetString("CurrentProductId", productData.id);
                Utils.NavigationManager.Instance.LoadScene("ProductInfoScene");
            }
        }

        private void OnDestroy()
        {
            if (cardButton != null)
                cardButton.onClick.RemoveListener(OnCardClicked);
        }
    }
}
