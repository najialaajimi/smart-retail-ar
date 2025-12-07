using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace SmartRetailAR.Utils
{
    /// <summary>
    /// Utility script to automatically setup UI elements in scenes
    /// Use this to quickly create basic UI structure for testing
    /// Right-click on this script in Inspector and use Context Menu options
    /// </summary>
    public class SetupUI : MonoBehaviour
    {
        /// <summary>
        /// Setup complete UI for Home Scene
        /// Right-click on this script in Inspector and select "Setup Home Scene UI"
        /// </summary>
        [ContextMenu("Setup Home Scene UI")]
        public void SetupHomeSceneUI()
        {
            Debug.Log("Setting up Home Scene UI...");
            
            // Create or find Canvas
            Canvas canvas = FindObjectOfType<Canvas>();
            GameObject canvasGO;
            
            if (canvas == null)
            {
                canvasGO = CreateCanvas();
                canvas = canvasGO.GetComponent<Canvas>();
            }
            else
            {
                canvasGO = canvas.gameObject;
            }
            
            // Ensure EventSystem exists
            EnsureEventSystem();
            
            // Create Welcome Text
            CreateWelcomeText(canvasGO.transform);
            
            // Create Scan Button
            CreateButton(canvasGO.transform, "ScanButton", "Scanner Produit", new Vector2(0, 0), new Color(0.2f, 0.6f, 1f));
            
            // Create Profile Button
            CreateButton(canvasGO.transform, "ProfileButton", "Mon Profil", new Vector2(0, -120), new Color(0.3f, 0.7f, 0.3f));
            
            Debug.Log("✓ Home Scene UI setup complete! Press Play to test.");
        }
        
        /// <summary>
        /// Setup complete UI for Scanner Scene
        /// </summary>
        [ContextMenu("Setup Scanner Scene UI")]
        public void SetupScannerSceneUI()
        {
            Debug.Log("Setting up Scanner Scene UI...");
            
            Canvas canvas = FindObjectOfType<Canvas>();
            GameObject canvasGO = canvas != null ? canvas.gameObject : CreateCanvas();
            EnsureEventSystem();
            
            // Create Camera View placeholder
            CreateCameraView(canvasGO.transform);
            
            // Create Status Text
            CreateStatusText(canvasGO.transform);
            
            // Create Scan Button
            CreateButton(canvasGO.transform, "ScanButton", "Scanner", new Vector2(0, -300), new Color(1f, 0.6f, 0.2f));
            
            // Create Torch Button
            CreateButton(canvasGO.transform, "TorchButton", "Torche", new Vector2(-150, -300), Color.yellow);
            
            // Create Back Button
            CreateButton(canvasGO.transform, "BackButton", "Retour", new Vector2(150, -300), Color.gray);
            
            Debug.Log("✓ Scanner Scene UI setup complete!");
        }
        
        /// <summary>
        /// Setup complete UI for Product Info Scene
        /// </summary>
        [ContextMenu("Setup Product Info Scene UI")]
        public void SetupProductInfoSceneUI()
        {
            Debug.Log("Setting up Product Info Scene UI...");
            
            Canvas canvas = FindObjectOfType<Canvas>();
            GameObject canvasGO = canvas != null ? canvas.gameObject : CreateCanvas();
            EnsureEventSystem();
            
            // Create product info texts
            CreateProductInfoTexts(canvasGO.transform);
            
            // Create Alternatives Button
            CreateButton(canvasGO.transform, "AlternativesButton", "Voir les Alternatives", new Vector2(0, -350), new Color(0.2f, 0.8f, 0.5f));
            
            // Create Back Button
            CreateButton(canvasGO.transform, "BackButton", "Retour", new Vector2(0, -440), Color.gray);
            
            Debug.Log("✓ Product Info Scene UI setup complete!");
        }
        
        #region Helper Methods
        
        private GameObject CreateCanvas()
        {
            GameObject canvasGO = new GameObject("Canvas");
            Canvas canvas = canvasGO.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            
            CanvasScaler scaler = canvasGO.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920, 1080);
            scaler.matchWidthOrHeight = 0.5f;
            
            canvasGO.AddComponent<GraphicRaycaster>();
            
            Debug.Log("✓ Canvas created");
            return canvasGO;
        }
        
        private void EnsureEventSystem()
        {
            if (FindObjectOfType<UnityEngine.EventSystems.EventSystem>() == null)
            {
                GameObject eventSystem = new GameObject("EventSystem");
                eventSystem.AddComponent<UnityEngine.EventSystems.EventSystem>();
                eventSystem.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();
                Debug.Log("✓ EventSystem created");
            }
        }
        
        private void CreateWelcomeText(Transform parent)
        {
            GameObject textGO = new GameObject("WelcomeText");
            textGO.transform.SetParent(parent, false);
            
            TextMeshProUGUI text = textGO.AddComponent<TextMeshProUGUI>();
            text.text = "Bienvenue, User!";
            text.fontSize = 48;
            text.alignment = TextAlignmentOptions.Center;
            text.color = Color.white;
            
            RectTransform rect = textGO.GetComponent<RectTransform>();
            rect.anchorMin = new Vector2(0.5f, 1f);
            rect.anchorMax = new Vector2(0.5f, 1f);
            rect.pivot = new Vector2(0.5f, 1f);
            rect.anchoredPosition = new Vector2(0, -100);
            rect.sizeDelta = new Vector2(800, 100);
            
            Debug.Log("✓ Welcome text created");
        }
        
        private void CreateStatusText(Transform parent)
        {
            GameObject textGO = new GameObject("StatusText");
            textGO.transform.SetParent(parent, false);
            
            TextMeshProUGUI text = textGO.AddComponent<TextMeshProUGUI>();
            text.text = "Pointez vers le QR code du produit";
            text.fontSize = 32;
            text.alignment = TextAlignmentOptions.Center;
            text.color = Color.white;
            
            RectTransform rect = textGO.GetComponent<RectTransform>();
            rect.anchorMin = new Vector2(0.5f, 1f);
            rect.anchorMax = new Vector2(0.5f, 1f);
            rect.pivot = new Vector2(0.5f, 1f);
            rect.anchoredPosition = new Vector2(0, -100);
            rect.sizeDelta = new Vector2(900, 80);
            
            Debug.Log("✓ Status text created");
        }
        
        private void CreateCameraView(Transform parent)
        {
            GameObject viewGO = new GameObject("CameraView");
            viewGO.transform.SetParent(parent, false);
            
            RawImage rawImage = viewGO.AddComponent<RawImage>();
            rawImage.color = new Color(0.2f, 0.2f, 0.2f, 1f); // Dark gray placeholder
            
            RectTransform rect = viewGO.GetComponent<RectTransform>();
            rect.anchorMin = new Vector2(0.5f, 0.5f);
            rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.pivot = new Vector2(0.5f, 0.5f);
            rect.anchoredPosition = new Vector2(0, 50);
            rect.sizeDelta = new Vector2(800, 600);
            
            Debug.Log("✓ Camera view placeholder created");
        }
        
        private void CreateProductInfoTexts(Transform parent)
        {
            // Product Name
            CreateSimpleText(parent, "ProductNameText", "Nom du Produit", new Vector2(0, 350), 42);
            
            // Brand
            CreateSimpleText(parent, "BrandText", "Marque", new Vector2(0, 290), 32);
            
            // Description
            CreateSimpleText(parent, "DescriptionText", "Description du produit...", new Vector2(0, 230), 24);
            
            // Origin
            CreateSimpleText(parent, "OriginText", "Origine: France", new Vector2(0, 180), 24);
            
            // Nutritional Info
            CreateSimpleText(parent, "CaloriesText", "Calories: 100 kcal", new Vector2(-200, 100), 20);
            CreateSimpleText(parent, "ProteinsText", "Protéines: 5g", new Vector2(-200, 70), 20);
            CreateSimpleText(parent, "CarbsText", "Glucides: 10g", new Vector2(200, 100), 20);
            CreateSimpleText(parent, "FatsText", "Lipides: 3g", new Vector2(200, 70), 20);
            
            // Scores
            CreateSimpleText(parent, "HealthScoreText", "Score Santé: 8.0/10", new Vector2(-150, 0), 28);
            CreateSimpleText(parent, "EcoScoreText", "Score Écologique: 7.5/10", new Vector2(150, 0), 28);
            
            Debug.Log("✓ Product info texts created");
        }
        
        private void CreateSimpleText(Transform parent, string name, string text, Vector2 position, float fontSize)
        {
            GameObject textGO = new GameObject(name);
            textGO.transform.SetParent(parent, false);
            
            TextMeshProUGUI tmpText = textGO.AddComponent<TextMeshProUGUI>();
            tmpText.text = text;
            tmpText.fontSize = fontSize;
            tmpText.alignment = TextAlignmentOptions.Center;
            tmpText.color = Color.white;
            
            RectTransform rect = textGO.GetComponent<RectTransform>();
            rect.anchorMin = new Vector2(0.5f, 0.5f);
            rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.pivot = new Vector2(0.5f, 0.5f);
            rect.anchoredPosition = position;
            rect.sizeDelta = new Vector2(400, fontSize * 2);
        }
        
        private GameObject CreateButton(Transform parent, string name, string text, Vector2 position, Color color)
        {
            GameObject buttonGO = new GameObject(name);
            buttonGO.transform.SetParent(parent, false);
            
            Image image = buttonGO.AddComponent<Image>();
            image.color = color;
            
            Button button = buttonGO.AddComponent<Button>();
            
            RectTransform buttonRect = buttonGO.GetComponent<RectTransform>();
            buttonRect.anchorMin = new Vector2(0.5f, 0.5f);
            buttonRect.anchorMax = new Vector2(0.5f, 0.5f);
            buttonRect.pivot = new Vector2(0.5f, 0.5f);
            buttonRect.anchoredPosition = position;
            buttonRect.sizeDelta = new Vector2(400, 80);
            
            GameObject textGO = new GameObject("Text (TMP)");
            textGO.transform.SetParent(buttonGO.transform, false);
            
            TextMeshProUGUI buttonText = textGO.AddComponent<TextMeshProUGUI>();
            buttonText.text = text;
            buttonText.fontSize = 32;
            buttonText.alignment = TextAlignmentOptions.Center;
            buttonText.color = Color.white;
            
            RectTransform textRect = textGO.GetComponent<RectTransform>();
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.sizeDelta = Vector2.zero;
            
            Debug.Log($"✓ Button '{name}' created");
            return buttonGO;
        }
        
        #endregion
    }
}
