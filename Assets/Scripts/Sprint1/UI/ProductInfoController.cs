using UnityEngine;
using UnityEngine.UI;
using SmartRetailAR.Utils;
using SmartRetailAR.Data;
using System.Collections.Generic;

namespace SmartRetailAR.UI
{
    public class ProductInfoController : MonoBehaviour
    {
        [Header("UI Elements")]
        public Text productNameText;
        public Text brandText;
        public Text priceText;
        public Text descriptionText;
        public Text categoryText;
        public Text originText;
        public Image productImage;
        
        [Header("Nutritional Info")]
        public Text caloriesText;
        public Text proteinsText;
        public Text carbsText;
        public Text fatsText;
        
        [Header("Scores")]
        public Text ecoScoreText;
        public Text healthScoreText;
        public Text nutriScoreText;
        
        [Header("Flags")]
        public GameObject bioTag;
        public GameObject veganTag;
        public GameObject vegetarianTag;
        
        [Header("Buttons")]
        public Button backButton;
        public Button viewAlternativesButton;
        public Button addToWishlistButton;
        public Button viewARButton;

        private ProductData _currentProduct;

        private void Start()
        {
            SetupButtons();
            LoadProductInfo();
        }

        private void SetupButtons()
        {
            if (backButton != null)
                backButton.onClick.AddListener(() => NavigationManager.Instance.GoBack());

            if (viewAlternativesButton != null)
                viewAlternativesButton.onClick.AddListener(ViewAlternatives);

            if (addToWishlistButton != null)
                addToWishlistButton.onClick.AddListener(AddToWishlist);

            if (viewARButton != null)
                viewARButton.onClick.AddListener(() => NavigationManager.Instance.LoadAROverlayScene());
        }

        private void LoadProductInfo()
        {
            var qrData = QRCodeManager.Instance.GetLastScannedCode();
            if (qrData != null)
            {
                _currentProduct = ProductDatabase.Instance.GetProductById(qrData.productId);
                if (_currentProduct != null)
                {
                    DisplayProductInfo(_currentProduct);
                }
            }
        }

        private void DisplayProductInfo(ProductData product)
        {
            // Basic Info
            if (productNameText != null)
                productNameText.text = product.name;

            if (brandText != null)
                brandText.text = product.brand;

            if (priceText != null)
                priceText.text = $"{product.price:F2} {product.currency}";

            if (descriptionText != null)
                descriptionText.text = product.description;

            if (categoryText != null)
                categoryText.text = $"{product.category} > {product.subcategory}";

            if (originText != null)
                originText.text = $"Origine: {product.origin}";

            // Nutritional Info
            if (product.nutritionalInfo != null)
            {
                if (caloriesText != null)
                    caloriesText.text = $"{product.nutritionalInfo.calories} kcal";

                if (proteinsText != null)
                    proteinsText.text = $"Protéines: {product.nutritionalInfo.proteins}g";

                if (carbsText != null)
                    carbsText.text = $"Glucides: {product.nutritionalInfo.carbohydrates}g";

                if (fatsText != null)
                    fatsText.text = $"Lipides: {product.nutritionalInfo.fats}g";
            }

            // Scores
            if (product.scores != null)
            {
                if (ecoScoreText != null)
                    ecoScoreText.text = $"Eco-Score: {product.scores.ecoScore}";

                if (healthScoreText != null)
                    healthScoreText.text = $"Santé: {product.scores.healthScore}";

                if (nutriScoreText != null)
                    nutriScoreText.text = $"Nutri-Score: {product.scores.nutriScore}";
            }

            // Flags
            if (product.flags != null)
            {
                if (bioTag != null)
                    bioTag.SetActive(product.flags.isBio);

                if (veganTag != null)
                    veganTag.SetActive(product.flags.isVegan);

                if (vegetarianTag != null)
                    vegetarianTag.SetActive(product.flags.isVegetarian);
            }
        }

        private void ViewAlternatives()
        {
            if (_currentProduct != null)
            {
                PlayerPrefs.SetString("ComparisonProductId", _currentProduct.id);
                NavigationManager.Instance.LoadRecommendationsScene();
            }
        }

        private void AddToWishlist()
        {
            if (_currentProduct != null)
            {
                Debug.Log($"Added to wishlist: {_currentProduct.name}");
                // TODO: Implement wishlist functionality in Sprint 3
            }
        }
    }
}
