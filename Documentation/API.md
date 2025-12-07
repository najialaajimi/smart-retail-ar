# Documentation API - Smart Retail AR

## Table des Matières
- [Structures de Données](#structures-de-données)
- [API Publique](#api-publique)
- [Managers](#managers)
- [Events et Callbacks](#events-et-callbacks)
- [Exemples d'Utilisation](#exemples-dutilisation)

## Structures de Données

### ProductData

Représente un produit dans la base de données.

```csharp
namespace SmartRetailAR.Data
{
    [Serializable]
    public class ProductData
    {
        public string id;                          // Identifiant unique (ex: "PROD001")
        public string name;                        // Nom du produit
        public string category;                    // Catégorie (ex: "Produits Laitiers")
        public string brand;                       // Marque
        public float price;                        // Prix en euros
        public string description;                 // Description
        public NutritionalInfo nutritionalInfo;    // Infos nutritionnelles
        public EcologicalInfo ecologicalInfo;      // Infos écologiques
        public EthicalInfo ethicalInfo;           // Infos éthiques
        public List<string> allergens;            // Liste des allergènes
        public string imageUrl;                    // URL/path de l'image
        public string barcode;                     // Code-barres
        
        // Scores calculés (non sérialisés)
        [NonSerialized] public float relevanceScore;    // Score de pertinence
        [NonSerialized] public float similarityScore;   // Score de similarité
    }
}
```

### NutritionalInfo

Informations nutritionnelles d'un produit.

```csharp
[Serializable]
public class NutritionalInfo
{
    public int calories;        // Calories (kcal)
    public float protein;       // Protéines (g)
    public float carbs;         // Glucides (g)
    public float fat;           // Lipides (g)
    public float fiber;         // Fibres (g)
    public float sugar;         // Sucres (g)
    public int sodium;          // Sodium (mg)
    public string nutriScore;   // Nutri-Score (A-E)
}
```

### EcologicalInfo

Informations écologiques d'un produit.

```csharp
[Serializable]
public class EcologicalInfo
{
    public float carbonFootprint;   // Empreinte carbone (kg CO2)
    public string packaging;        // Type d'emballage
    public string origin;           // Pays d'origine
    public bool organic;            // Produit biologique
    public string ecoScore;         // Éco-Score (A-E)
}
```

### EthicalInfo

Informations éthiques d'un produit.

```csharp
[Serializable]
public class EthicalInfo
{
    public bool fairTrade;          // Commerce équitable
    public bool local;              // Production locale
    public string ethicalScore;     // Score éthique (A-E)
}
```

### UserPreferences

Préférences et profil utilisateur.

```csharp
[Serializable]
public class UserPreferences
{
    // Préférences alimentaires
    public bool vegetarian;
    public bool vegan;
    public bool glutenFree;
    public bool lactoseFree;
    public bool organic;
    
    // Filtres d'allergènes
    public List<string> allergenFilters;
    
    // Historique
    public List<string> scanHistoryIds;
    public List<long> scanHistoryTimestamps;
    
    // Compteurs (parallel lists pour sérialisation)
    public List<string> scanCountKeys;
    public List<int> scanCountValues;
    
    // Poids de scoring
    public float nutritionalWeight = 0.3f;
    public float ecologicalWeight = 0.3f;
    public float economicalWeight = 0.2f;
    public float ethicalWeight = 0.2f;
    public float maxBudget = 100f;
}
```

### ScoreBreakdown

Détail des scores d'un produit.

```csharp
[Serializable]
public class ScoreBreakdown
{
    public float nutritionalScore;   // Score nutritionnel (0-1)
    public float ecologicalScore;    // Score écologique (0-1)
    public float economicalScore;    // Score économique (0-1)
    public float ethicalScore;       // Score éthique (0-1)
    public float overallScore;       // Score global (0-1)
}
```

## API Publique

### ProductDatabase

Singleton pour accéder à la base de données produits.

#### Méthodes

```csharp
// Accès à l'instance
ProductDatabase.Instance

// Récupérer un produit par ID
ProductData GetProductById(string id)

// Récupérer tous les produits
List<ProductData> GetAllProducts()

// Récupérer produits par catégorie
List<ProductData> GetProductsByCategory(string category)

// Récupérer produits par marque
List<ProductData> GetProductsByBrand(string brand)

// Rechercher des produits
List<ProductData> SearchProducts(string searchTerm)

// Récupérer toutes les catégories
List<string> GetAllCategories()

// Nombre de produits
int GetProductCount()
```

#### Exemples

```csharp
// Récupérer un produit
ProductData product = ProductDatabase.Instance.GetProductById("PROD001");
if (product != null)
{
    Debug.Log($"Found: {product.name}");
}

// Rechercher des produits
List<ProductData> results = ProductDatabase.Instance.SearchProducts("bio");
foreach (var product in results)
{
    Debug.Log(product.name);
}

// Filtrer par catégorie
List<ProductData> dairy = ProductDatabase.Instance
    .GetProductsByCategory("Produits Laitiers");
```

### RecommendationEngine

Moteur de recommandations intelligent.

#### Méthodes

```csharp
// Accès à l'instance
RecommendationEngine.Instance

// Obtenir des recommandations personnalisées
List<ProductData> GetRecommendations(ProductData currentProduct, int count = 5)

// Obtenir des produits similaires
List<ProductData> GetSimilarProducts(ProductData product, int count = 3)

// Obtenir des alternatives meilleures
List<ProductData> GetAlternatives(ProductData product, int count = 3)

// Obtenir les produits tendance
List<ProductData> GetTrendingProducts(int count = 5)

// Recherche multi-critères
List<ProductData> GetProductsByCriteria(
    string category = null,
    float? maxPrice = null,
    string nutriScore = null,
    bool? organic = null,
    int count = 10)

// Mettre à jour les paramètres
void UpdateSettings(int maxRecs, float simThreshold, bool enableML)

// Rafraîchir les préférences utilisateur
void RefreshUserPreferences()
```

#### Exemples

```csharp
// Recommandations pour un produit
ProductData current = ProductDatabase.Instance.GetProductById("PROD001");
List<ProductData> recommendations = RecommendationEngine.Instance
    .GetRecommendations(current, 5);

// Produits similaires
List<ProductData> similar = RecommendationEngine.Instance
    .GetSimilarProducts(current, 3);

// Alternatives meilleures
List<ProductData> alternatives = RecommendationEngine.Instance
    .GetAlternatives(current, 3);

// Recherche avec critères
List<ProductData> filtered = RecommendationEngine.Instance
    .GetProductsByCriteria(
        category: "Produits Laitiers",
        maxPrice: 5.0f,
        organic: true,
        count: 10
    );
```

### ScoringSystem

Système de scoring multi-critères.

#### Méthodes

```csharp
// Calculer le score global
float CalculateOverallScore(ProductData product, UserPreferences preferences = null)

// Calculer le score nutritionnel
float CalculateNutritionalScore(ProductData product)

// Calculer le score écologique
float CalculateEcologicalScore(ProductData product)

// Calculer le score économique
float CalculateEconomicalScore(ProductData product, UserPreferences preferences = null)

// Calculer le score éthique
float CalculateEthicalScore(ProductData product)

// Obtenir le détail des scores
ScoreBreakdown GetScoreBreakdown(ProductData product, UserPreferences preferences = null)
```

#### Exemples

```csharp
ScoringSystem scoring = FindObjectOfType<ScoringSystem>();
ProductData product = ProductDatabase.Instance.GetProductById("PROD001");

// Score global
float overallScore = scoring.CalculateOverallScore(product);
Debug.Log($"Overall Score: {overallScore:F2}");

// Détail des scores
ScoreBreakdown breakdown = scoring.GetScoreBreakdown(product);
Debug.Log($"Nutritional: {breakdown.nutritionalScore:F2}");
Debug.Log($"Ecological: {breakdown.ecologicalScore:F2}");
Debug.Log($"Economical: {breakdown.economicalScore:F2}");
Debug.Log($"Ethical: {breakdown.ethicalScore:F2}");
```

### UserPreferencesManager

Gestionnaire des préférences utilisateur.

#### Méthodes

```csharp
// Accès à l'instance
UserPreferencesManager.Instance

// Charger les préférences
void LoadPreferences()

// Sauvegarder les préférences
void SavePreferences()

// Obtenir les préférences actuelles
UserPreferences GetPreferences()

// Mettre à jour les préférences
void UpdatePreferences(UserPreferences newPreferences)
```

#### Méthodes UserPreferences

```csharp
// Initialiser le dictionnaire de compteurs
void InitializeDictionary()

// Obtenir le nombre de scans d'un produit
int GetProductScanCount(string productId)

// Incrémenter le compteur de scan
void IncrementScanCount(string productId)

// Ajouter à l'historique
void AddToScanHistory(string productId)

// Vérifier compatibilité avec préférences
bool MatchesDietaryPreferences(ProductData product)

// Obtenir les produits favoris
List<string> GetFavoriteProducts(int count = 5)

// Effacer toutes les données
void ClearData()
```

#### Exemples

```csharp
// Obtenir les préférences
UserPreferences prefs = UserPreferencesManager.Instance.GetPreferences();

// Modifier les préférences
prefs.vegetarian = true;
prefs.allergenFilters.Add("Lait");
prefs.nutritionalWeight = 0.4f;

// Sauvegarder
UserPreferencesManager.Instance.SavePreferences();

// Ajouter un scan
prefs.AddToScanHistory("PROD001");

// Vérifier compatibilité
ProductData product = ProductDatabase.Instance.GetProductById("PROD002");
if (prefs.MatchesDietaryPreferences(product))
{
    Debug.Log("Product matches user preferences");
}

// Favoris
List<string> favorites = prefs.GetFavoriteProducts(5);
```

## Managers

### NavigationManager

Gestionnaire de navigation entre scènes.

#### Méthodes

```csharp
// Accès à l'instance
NavigationManager.Instance

// Charger une scène
void LoadScene(string sceneName)

// Recharger la scène actuelle
void ReloadCurrentScene()

// Retour à l'écran précédent
void GoBack()

// Obtenir le nom de la scène actuelle
string GetCurrentScene()

// Vérifier si en chargement
bool IsLoading()
```

#### Exemples

```csharp
// Navigation simple
NavigationManager.Instance.LoadScene("ProductInfoScene");

// Retour
NavigationManager.Instance.GoBack();

// Vérifier état
if (!NavigationManager.Instance.IsLoading())
{
    // Navigate
}
```

### AnalyticsManager

Gestionnaire d'analytics et métriques.

#### Méthodes

```csharp
// Accès à l'instance
AnalyticsManager.Instance

// Tracker un événement personnalisé
void TrackEvent(string eventName, Dictionary<string, object> parameters = null)

// Tracker un scan de produit
void TrackProductScan(string productId, float detectionTime)

// Tracker une interaction avec recommandation
void TrackRecommendation(string productId, string recommendedProductId, bool accepted)

// Tracker une vue d'écran
void TrackScreenView(string screenName)

// Tracker un timing
void TrackTiming(string category, string variable, float timeMs)

// Tracker une erreur
void TrackError(string errorType, string errorMessage)

// Tracker la performance AR
void TrackARPerformance(float fps, float latency)

// Obtenir la durée de session
float GetSessionDuration()

// Obtenir le compteur d'événements
int GetEventCount(string eventName)

// Obtenir le résumé analytics
AnalyticsSummary GetAnalyticsSummary()

// Sauvegarder les données
void SaveAnalyticsData()

// Effacer les données
void ClearAnalyticsData()
```

#### Exemples

```csharp
// Tracker un scan
AnalyticsManager.Instance.TrackProductScan("PROD001", 0.85f);

// Tracker une recommandation
AnalyticsManager.Instance.TrackRecommendation("PROD001", "PROD002", true);

// Événement personnalisé
AnalyticsManager.Instance.TrackEvent("user_action",
    new Dictionary<string, object>
    {
        { "action_type", "comparison" },
        { "product_count", 3 }
    });

// Résumé
AnalyticsSummary summary = AnalyticsManager.Instance.GetAnalyticsSummary();
Debug.Log(summary.ToString());
```

### PerformanceMonitor

Moniteur de performance temps réel.

#### Méthodes

```csharp
// Accès à l'instance
PerformanceMonitor.Instance

// Obtenir le FPS actuel
float GetCurrentFPS()

// Obtenir le FPS moyen
float GetAverageFPS()

// Obtenir l'utilisation mémoire actuelle
long GetCurrentMemoryMB()

// Obtenir le résumé de performance
PerformanceSummary GetPerformanceSummary()

// Réinitialiser les statistiques
void ResetStatistics()

// Activer/désactiver le monitoring
void SetMonitoringEnabled(bool enabled)

// Forcer le garbage collection
void ForceGarbageCollection()
```

#### Exemples

```csharp
// Vérifier les performances
float fps = PerformanceMonitor.Instance.GetCurrentFPS();
long memory = PerformanceMonitor.Instance.GetCurrentMemoryMB();

Debug.Log($"FPS: {fps:F1}, Memory: {memory}MB");

// Résumé complet
PerformanceSummary summary = PerformanceMonitor.Instance.GetPerformanceSummary();
Debug.Log(summary.ToString());
```

### AR Components

#### ARImageTracker

Gestion de la reconnaissance d'images AR.

```csharp
// Events
public event Action<string> OnProductDetected;
public event Action<string> OnProductLost;

// Méthodes
ARTrackedImage GetTrackedImage(string imageName)
List<ARTrackedImage> GetAllTrackedImages()
void SetTrackingEnabled(bool enabled)
bool IsTrackingEnabled()
void ClearTrackedImages()
```

#### ARDataOverlay

Gestion de l'affichage AR des informations.

```csharp
// Afficher un overlay
void DisplayProductOverlay(string productId, Vector3 position, Quaternion rotation)

// Masquer un overlay
void HideProductOverlay(string productId)

// Effacer tous les overlays
void ClearAllOverlays()

// Obtenir le nombre d'overlays actifs
int GetActiveOverlayCount()
```

#### ARSessionManager

Gestion de la session AR.

```csharp
// Events
public event Action OnSessionInitialized;
public event Action OnSessionStarted;
public event Action OnSessionStopped;
public event Action<ARSessionStateChangedEventArgs> OnSessionStateChanged;

// Méthodes
void StartSession()
void StopSession()
void ResetSession()
void PauseSession()
void ResumeSession()
ARSessionState GetSessionState()
bool IsSessionActive()
bool IsARSupported()
Camera GetARCamera()
```

## Events et Callbacks

### Subscription Pattern

```csharp
// S'abonner à un événement
ARImageTracker tracker = FindObjectOfType<ARImageTracker>();
tracker.OnProductDetected += HandleProductDetected;

// Handler
private void HandleProductDetected(string productId)
{
    Debug.Log($"Product detected: {productId}");
    // Traiter la détection
}

// Se désabonner
private void OnDestroy()
{
    if (tracker != null)
        tracker.OnProductDetected -= HandleProductDetected;
}
```

### Events Disponibles

```csharp
// AR Events
ARImageTracker.OnProductDetected(string productId)
ARImageTracker.OnProductLost(string productId)
ARSessionManager.OnSessionInitialized()
ARSessionManager.OnSessionStarted()
ARSessionManager.OnSessionStopped()
ARSessionManager.OnSessionStateChanged(ARSessionStateChangedEventArgs args)
```

## Exemples d'Utilisation

### Exemple Complet: Scan et Recommandations

```csharp
using UnityEngine;
using SmartRetailAR.Data;
using SmartRetailAR.AR;
using SmartRetailAR.Recommendations;
using System.Collections.Generic;

public class ProductScanExample : MonoBehaviour
{
    private ARImageTracker imageTracker;
    
    private void Start()
    {
        // Setup AR tracking
        imageTracker = FindObjectOfType<ARImageTracker>();
        if (imageTracker != null)
        {
            imageTracker.OnProductDetected += OnProductDetected;
        }
    }
    
    private void OnProductDetected(string productId)
    {
        // Track analytics
        AnalyticsManager.Instance.TrackProductScan(productId, Time.time);
        
        // Get product data
        ProductData product = ProductDatabase.Instance.GetProductById(productId);
        if (product == null)
        {
            Debug.LogError($"Product not found: {productId}");
            return;
        }
        
        // Add to user history
        UserPreferences prefs = UserPreferencesManager.Instance.GetPreferences();
        prefs.AddToScanHistory(productId);
        UserPreferencesManager.Instance.SavePreferences();
        
        // Calculate scores
        ScoringSystem scoring = FindObjectOfType<ScoringSystem>();
        ScoreBreakdown scores = scoring.GetScoreBreakdown(product, prefs);
        
        Debug.Log($"Product: {product.name}");
        Debug.Log($"Overall Score: {scores.overallScore:F2}");
        
        // Get recommendations
        List<ProductData> recommendations = RecommendationEngine.Instance
            .GetRecommendations(product, 3);
        
        Debug.Log($"Found {recommendations.Count} recommendations:");
        foreach (var rec in recommendations)
        {
            Debug.Log($"- {rec.name} (Score: {rec.relevanceScore:F2})");
        }
        
        // Get alternatives
        List<ProductData> alternatives = RecommendationEngine.Instance
            .GetAlternatives(product, 3);
        
        if (alternatives.Count > 0)
        {
            Debug.Log($"Better alternatives available:");
            foreach (var alt in alternatives)
            {
                Debug.Log($"- {alt.name}");
            }
        }
    }
    
    private void OnDestroy()
    {
        if (imageTracker != null)
        {
            imageTracker.OnProductDetected -= OnProductDetected;
        }
    }
}
```

### Exemple: Personnalisation Utilisateur

```csharp
using UnityEngine;
using SmartRetailAR.Data;
using System.Collections.Generic;

public class UserProfileSetup : MonoBehaviour
{
    public void SetupUserProfile()
    {
        // Get preferences
        UserPreferences prefs = UserPreferencesManager.Instance.GetPreferences();
        
        // Set dietary preferences
        prefs.vegan = true;
        prefs.glutenFree = true;
        prefs.organic = true;
        
        // Set allergens
        prefs.allergenFilters = new List<string> { "Lait", "Gluten" };
        
        // Set scoring weights
        prefs.nutritionalWeight = 0.4f;  // Prioritize nutrition
        prefs.ecologicalWeight = 0.4f;   // Prioritize ecology
        prefs.economicalWeight = 0.1f;
        prefs.ethicalWeight = 0.1f;
        
        // Set budget
        prefs.maxBudget = 50f;
        
        // Save
        UserPreferencesManager.Instance.SavePreferences();
        
        // Refresh recommendation engine
        RecommendationEngine.Instance.RefreshUserPreferences();
        
        Debug.Log("User profile configured");
    }
}
```

---

**Documentation complète de l'API Smart Retail AR**
