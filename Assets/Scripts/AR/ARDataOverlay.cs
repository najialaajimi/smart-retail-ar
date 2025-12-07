using UnityEngine;
using UnityEngine.UI;
using TMPro;
using SmartRetailAR.Data;
using System.Collections.Generic;

namespace SmartRetailAR.AR
{
    /// <summary>
    /// AR Data Overlay - Displays product information as AR overlay
    /// Anchors UI elements to tracked products
    /// </summary>
    public class ARDataOverlay : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private Canvas overlayCanvas;
        [SerializeField] private GameObject overlayPrefab;
        [SerializeField] private Transform overlayContainer;

        [Header("Display Settings")]
        [SerializeField] private float displayDistance = 0.3f;
        [SerializeField] private Vector3 displayOffset = new Vector3(0, 0.2f, 0);
        [SerializeField] private float fadeInDuration = 0.5f;
        [SerializeField] private float fadeOutDuration = 0.3f;

        [Header("Content Settings")]
        [SerializeField] private bool showNutritionalInfo = true;
        [SerializeField] private bool showEcologicalInfo = true;
        [SerializeField] private bool showPrice = true;
        [SerializeField] private bool showRecommendations = true;

        private Dictionary<string, GameObject> activeOverlays = new Dictionary<string, GameObject>();
        private Dictionary<string, ProductData> displayedProducts = new Dictionary<string, ProductData>();

        private void Start()
        {
            if (overlayCanvas == null)
            {
                Debug.LogError("Overlay canvas not assigned");
            }
        }

        /// <summary>
        /// Display product data overlay at specified position
        /// </summary>
        public void DisplayProductOverlay(string productId, Vector3 position, Quaternion rotation)
        {
            // Get product data
            ProductData product = ProductDatabase.Instance.GetProductById(productId);
            if (product == null)
            {
                Debug.LogError($"Product not found: {productId}");
                return;
            }

            // Check if overlay already exists
            if (activeOverlays.ContainsKey(productId))
            {
                UpdateOverlayPosition(productId, position, rotation);
                return;
            }

            // Create overlay instance
            GameObject overlay = CreateOverlay(product, position, rotation);
            if (overlay != null)
            {
                activeOverlays[productId] = overlay;
                displayedProducts[productId] = product;
                AnimateOverlayIn(overlay);
            }
        }

        /// <summary>
        /// Create overlay UI for product
        /// </summary>
        private GameObject CreateOverlay(ProductData product, Vector3 position, Quaternion rotation)
        {
            GameObject overlay;

            if (overlayPrefab != null)
            {
                // Use prefab if available
                overlay = Instantiate(overlayPrefab, overlayContainer != null ? overlayContainer : transform);
            }
            else
            {
                // Create basic overlay programmatically
                overlay = CreateBasicOverlay();
            }

            // Position overlay
            overlay.transform.position = position + displayOffset;
            overlay.transform.rotation = rotation;

            // Populate overlay with product data
            PopulateOverlay(overlay, product);

            return overlay;
        }

        /// <summary>
        /// Create basic overlay UI programmatically
        /// </summary>
        private GameObject CreateBasicOverlay()
        {
            GameObject overlay = new GameObject("ProductOverlay");
            overlay.transform.SetParent(overlayContainer != null ? overlayContainer : transform);

            // Add Canvas if needed
            Canvas canvas = overlay.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.WorldSpace;
            canvas.worldCamera = Camera.main;

            CanvasScaler scaler = overlay.AddComponent<CanvasScaler>();
            scaler.dynamicPixelsPerUnit = 10;

            // Create panel background
            GameObject panel = new GameObject("Panel");
            panel.transform.SetParent(overlay.transform);
            Image panelImage = panel.AddComponent<Image>();
            panelImage.color = new Color(0, 0, 0, 0.8f);

            RectTransform panelRect = panel.GetComponent<RectTransform>();
            panelRect.sizeDelta = new Vector2(300, 400);

            // Create text container
            GameObject textContainer = new GameObject("TextContainer");
            textContainer.transform.SetParent(panel.transform);
            VerticalLayoutGroup layout = textContainer.AddComponent<VerticalLayoutGroup>();
            layout.padding = new RectOffset(10, 10, 10, 10);
            layout.spacing = 5;
            layout.childForceExpandWidth = true;
            layout.childForceExpandHeight = false;
            layout.childControlHeight = true;

            RectTransform textRect = textContainer.GetComponent<RectTransform>();
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.sizeDelta = Vector2.zero;

            return overlay;
        }

        /// <summary>
        /// Populate overlay with product information
        /// </summary>
        private void PopulateOverlay(GameObject overlay, ProductData product)
        {
            // Find or create text fields
            TextMeshProUGUI[] textFields = overlay.GetComponentsInChildren<TextMeshProUGUI>();
            
            string overlayText = BuildOverlayText(product);
            
            if (textFields.Length > 0)
            {
                textFields[0].text = overlayText;
            }
            else
            {
                // Create text field if none exists
                GameObject textObj = new GameObject("OverlayText");
                textObj.transform.SetParent(overlay.transform);
                
                TextMeshProUGUI text = textObj.AddComponent<TextMeshProUGUI>();
                text.text = overlayText;
                text.fontSize = 14;
                text.color = Color.white;
                text.alignment = TextAlignmentOptions.TopLeft;

                RectTransform textRect = textObj.GetComponent<RectTransform>();
                textRect.anchorMin = Vector2.zero;
                textRect.anchorMax = Vector2.one;
                textRect.sizeDelta = Vector2.zero;
                textRect.anchoredPosition = Vector2.zero;
            }
        }

        /// <summary>
        /// Build text content for overlay
        /// </summary>
        private string BuildOverlayText(ProductData product)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            
            // Product name and brand
            sb.AppendLine($"<b>{product.name}</b>");
            sb.AppendLine($"{product.brand}");
            
            if (showPrice)
            {
                sb.AppendLine($"\n<b>Prix:</b> {product.price:F2}€");
            }

            if (showNutritionalInfo && product.nutritionalInfo != null)
            {
                sb.AppendLine($"\n<b>Nutri-Score:</b> {product.nutritionalInfo.nutriScore}");
                sb.AppendLine($"Calories: {product.nutritionalInfo.calories} kcal");
            }

            if (showEcologicalInfo && product.ecologicalInfo != null)
            {
                sb.AppendLine($"\n<b>Éco-Score:</b> {product.ecologicalInfo.ecoScore}");
                sb.AppendLine($"Origine: {product.ecologicalInfo.origin}");
            }

            return sb.ToString();
        }

        /// <summary>
        /// Update overlay position and rotation
        /// </summary>
        private void UpdateOverlayPosition(string productId, Vector3 position, Quaternion rotation)
        {
            if (activeOverlays.ContainsKey(productId))
            {
                GameObject overlay = activeOverlays[productId];
                if (overlay != null)
                {
                    overlay.transform.position = position + displayOffset;
                    overlay.transform.rotation = rotation;
                    
                    // Make overlay face camera
                    if (Camera.main != null)
                    {
                        overlay.transform.LookAt(Camera.main.transform);
                        overlay.transform.Rotate(0, 180, 0);
                    }
                }
            }
        }

        /// <summary>
        /// Hide product overlay
        /// </summary>
        public void HideProductOverlay(string productId)
        {
            if (activeOverlays.ContainsKey(productId))
            {
                GameObject overlay = activeOverlays[productId];
                AnimateOverlayOut(overlay, () =>
                {
                    Destroy(overlay);
                    activeOverlays.Remove(productId);
                    displayedProducts.Remove(productId);
                });
            }
        }

        /// <summary>
        /// Animate overlay fade in
        /// </summary>
        private void AnimateOverlayIn(GameObject overlay)
        {
            CanvasGroup canvasGroup = overlay.GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                canvasGroup = overlay.AddComponent<CanvasGroup>();
            }

            StopCoroutine(nameof(FadeCanvasGroup));
            StartCoroutine(FadeCanvasGroup(canvasGroup, 0f, 1f, fadeInDuration, null));
        }

        /// <summary>
        /// Animate overlay fade out
        /// </summary>
        private void AnimateOverlayOut(GameObject overlay, System.Action onComplete)
        {
            CanvasGroup canvasGroup = overlay.GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                canvasGroup = overlay.AddComponent<CanvasGroup>();
            }

            StartCoroutine(FadeCanvasGroup(canvasGroup, canvasGroup.alpha, 0f, fadeOutDuration, onComplete));
        }

        /// <summary>
        /// Coroutine for fading canvas group
        /// </summary>
        private System.Collections.IEnumerator FadeCanvasGroup(CanvasGroup canvasGroup, float startAlpha, float endAlpha, float duration, System.Action onComplete)
        {
            float elapsed = 0f;
            canvasGroup.alpha = startAlpha;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / duration);
                
                // Ease out cubic for fade in, ease in cubic for fade out
                float easedT = endAlpha > startAlpha 
                    ? 1f - Mathf.Pow(1f - t, 3f) // Ease out cubic
                    : Mathf.Pow(t, 3f); // Ease in cubic
                
                canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, easedT);
                yield return null;
            }

            canvasGroup.alpha = endAlpha;
            onComplete?.Invoke();
        }

        /// <summary>
        /// Clear all overlays
        /// </summary>
        public void ClearAllOverlays()
        {
            foreach (var overlay in activeOverlays.Values)
            {
                if (overlay != null)
                {
                    Destroy(overlay);
                }
            }
            activeOverlays.Clear();
            displayedProducts.Clear();
        }

        /// <summary>
        /// Get active overlay count
        /// </summary>
        public int GetActiveOverlayCount()
        {
            return activeOverlays.Count;
        }
    }
}
