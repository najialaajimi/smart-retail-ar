using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProductCard : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI productNameText;
    public TextMeshProUGUI brandText;
    public TextMeshProUGUI priceText;
    public TextMeshProUGUI priceComparisonText;
    public TextMeshProUGUI healthScoreText;
    public TextMeshProUGUI ecoScoreText;
    public Image productImage;
    public Image healthScoreFill;
    public Image ecoScoreFill;
    public Transform tagsContainer;
    public Button cardButton;

    private ProductData productData;
    private ProductData comparisonProduct;

    public void SetupCard(ProductData product, ProductData comparison = null)
    {
        productData = product;
        comparisonProduct = comparison;

        if (product == null)
        {
            Debug.LogError("Product data is null");
            return;
        }

        // Basic info
        if (productNameText != null)
        {
            productNameText.text = product.name;
        }

        if (brandText != null)
        {
            brandText.text = product.brand;
        }

        // Price
        if (priceText != null)
        {
            priceText.text = $"{product.price:F2}€";
        }

        // Price comparison
        if (priceComparisonText != null && comparison != null)
        {
            float difference = product.price - comparison.price;
            if (difference > 0)
            {
                priceComparisonText.text = $"+{difference:F2}€";
                priceComparisonText.color = Color.red;
            }
            else if (difference < 0)
            {
                priceComparisonText.text = $"{difference:F2}€";
                priceComparisonText.color = Color.green;
            }
            else
            {
                priceComparisonText.text = "Même prix";
                priceComparisonText.color = Color.gray;
            }
        }

        // Scores
        if (product.scores != null)
        {
            if (healthScoreText != null)
            {
                healthScoreText.text = $"{product.scores.healthScore:F0}";
            }

            if (healthScoreFill != null)
            {
                healthScoreFill.fillAmount = product.scores.healthScore / 100f;
            }

            if (ecoScoreText != null)
            {
                ecoScoreText.text = $"{product.scores.ecoScore:F0}";
            }

            if (ecoScoreFill != null)
            {
                ecoScoreFill.fillAmount = product.scores.ecoScore / 100f;
            }
        }

        // Tags
        DisplayTags();

        // TODO: Load product image
    }

    void DisplayTags()
    {
        if (tagsContainer == null || productData == null || productData.tags == null) return;

        // Clear existing tags
        foreach (Transform child in tagsContainer)
        {
            Destroy(child.gameObject);
        }

        // Display up to 3 tags
        int tagCount = Mathf.Min(3, productData.tags.Count);
        for (int i = 0; i < tagCount; i++)
        {
            // TODO: Create tag visual elements
        }
    }

    public void OnCardClicked()
    {
        if (productData != null)
        {
            QRCodeManager.Instance.SetCurrentProductId(productData.productId);
            NavigationManager.Instance.NavigateToProductInfo();
        }
    }
}
