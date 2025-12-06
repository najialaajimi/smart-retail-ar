# API Reference - Smart Retail AR

## Table des Matières

- [Data Classes](#data-classes)
- [UI Controllers](#ui-controllers)
- [Utility Managers](#utility-managers)
- [Events and Delegates](#events-and-delegates)

---

## Data Classes

### ProductData

**Namespace**: Global  
**Description**: Représente les données d'un produit

#### Properties

| Property | Type | Description |
|----------|------|-------------|
| productId | string | Identifiant unique du produit |
| name | string | Nom du produit |
| brand | string | Marque du produit |
| imageUrl | string | URL de l'image du produit |
| price | float | Prix du produit en euros |
| nutritionalInfo | NutritionalInfo | Informations nutritionnelles |
| origin | OriginInfo | Informations d'origine |
| scores | Scores | Scores santé et écologique |
| tags | List\<string\> | Tags (Bio, Local, etc.) |
| alternativeIds | List\<string\> | IDs des produits alternatifs |

#### Inner Classes

**NutritionalInfo**
```csharp
{
    float calories;      // Calories pour 100g
    float proteins;      // Protéines en grammes
    float carbohydrates; // Glucides en grammes
    float fats;          // Lipides en grammes
    float fiber;         // Fibres en grammes
    float sugar;         // Sucres en grammes
    float salt;          // Sel en grammes
}
```

**OriginInfo**
```csharp
{
    string country;   // Pays d'origine
    string region;    // Région
    string producer;  // Producteur
}
```

**Scores**
```csharp
{
    float healthScore;      // Score santé (0-100)
    float ecoScore;         // Score écologique (0-100)
    string nutritionGrade;  // Grade nutritionnel (A-E)
}
```

---

### ProductDatabase

**Namespace**: Global  
**Description**: Singleton gérant la base de données des produits

#### Static Properties

| Property | Type | Description |
|----------|------|-------------|
| Instance | ProductDatabase | Instance singleton |

#### Methods

##### LoadDatabase()
```csharp
public void LoadDatabase()
```
Charge la base de données depuis `Resources/Data/products_database.json`

##### GetProductById(string productId)
```csharp
public ProductData GetProductById(string productId)
```
Récupère un produit par son ID
- **Parameters**: 
  - `productId`: ID du produit
- **Returns**: ProductData ou null si non trouvé

##### GetAllProducts()
```csharp
public List<ProductData> GetAllProducts()
```
Récupère tous les produits
- **Returns**: Liste de tous les produits

##### GetAlternatives(string productId)
```csharp
public List<ProductData> GetAlternatives(string productId)
```
Récupère les alternatives d'un produit
- **Parameters**:
  - `productId`: ID du produit
- **Returns**: Liste des produits alternatifs

##### FilterProducts(List\<ProductData\> products, List\<string\> tags, float? minHealthScore, float? maxPrice)
```csharp
public List<ProductData> FilterProducts(
    List<ProductData> products, 
    List<string> tags = null,
    float? minHealthScore = null, 
    float? maxPrice = null
)
```
Filtre une liste de produits
- **Parameters**:
  - `products`: Liste de produits à filtrer
  - `tags`: Tags requis (optionnel)
  - `minHealthScore`: Score santé minimum (optionnel)
  - `maxPrice`: Prix maximum (optionnel)
- **Returns**: Liste filtrée

##### SearchProducts(string query)
```csharp
public List<ProductData> SearchProducts(string query)
```
Recherche des produits par nom ou marque
- **Parameters**:
  - `query`: Terme de recherche
- **Returns**: Liste des produits correspondants

---

### UserPreferences

**Namespace**: Global  
**Description**: Gère les préférences et données utilisateur

#### Properties

| Property | Type | Description |
|----------|------|-------------|
| userId | string | ID unique de l'utilisateur |
| userName | string | Nom de l'utilisateur |
| avatarUrl | string | URL de l'avatar |
| dietaryPreferences | List\<string\> | Préférences alimentaires |
| scannedProductIds | List\<string\> | IDs des produits scannés |
| productScanCount | Dictionary\<string, int\> | Compteur de scans par produit |
| settings | AppSettings | Paramètres application |

#### Methods

##### AddScannedProduct(string productId)
```csharp
public void AddScannedProduct(string productId)
```
Ajoute un produit à l'historique
- **Parameters**:
  - `productId`: ID du produit scanné

##### GetScanCount(string productId)
```csharp
public int GetScanCount(string productId)
```
Obtient le nombre de scans pour un produit
- **Parameters**:
  - `productId`: ID du produit
- **Returns**: Nombre de scans

##### Save()
```csharp
public void Save()
```
Sauvegarde les préférences localement (PlayerPrefs)

##### Load() (Static)
```csharp
public static UserPreferences Load()
```
Charge les préférences depuis PlayerPrefs
- **Returns**: Instance UserPreferences

---

## UI Controllers

### HomeController

**Namespace**: Global  
**Description**: Contrôleur pour l'écran d'accueil

#### Inspector Fields

| Field | Type | Description |
|-------|------|-------------|
| titleText | TextMeshProUGUI | Texte du titre |
| subtitleText | TextMeshProUGUI | Sous-titre |
| scanButton | Button | Bouton scanner |
| profileButton | Button | Bouton profil |
| helpButton | Button | Bouton aide |

#### Methods

##### Start()
Initialise l'UI et charge les données utilisateur

##### OnScanButtonClicked()
Navigation vers le scanner

##### OnProfileButtonClicked()
Navigation vers le profil

##### OnHelpButtonClicked()
Affiche l'aide/tutoriel

---

### ScannerController

**Namespace**: Global  
**Description**: Contrôleur pour l'écran de scan QR

#### Inspector Fields

| Field | Type | Description |
|-------|------|-------------|
| cameraPreview | RawImage | Aperçu caméra |
| scanFrame | Image | Cadre de visée |
| captureButton | Button | Bouton de capture |
| torchButton | Button | Bouton torche |
| backButton | Button | Bouton retour |
| instructionText | TextMeshProUGUI | Instructions |
| statusText | TextMeshProUGUI | Statut du scan |
| scanningOverlay | GameObject | Overlay de scan |
| testMode | bool | Mode test activé |
| testProductIds | string[] | IDs pour test |

#### Methods

##### StartScan()
```csharp
private void StartScan()
```
Démarre le processus de scan

##### OnQRCodeScanned(string productId)
```csharp
private void OnQRCodeScanned(string productId)
```
Callback appelé lors d'un scan réussi

##### OnScanError(string errorMessage)
```csharp
private void OnScanError(string errorMessage)
```
Callback appelé en cas d'erreur

---

### ProductInfoController

**Namespace**: Global  
**Description**: Contrôleur pour l'écran d'informations produit

#### Inspector Fields

| Field | Type | Description |
|-------|------|-------------|
| productNameText | TextMeshProUGUI | Nom du produit |
| brandText | TextMeshProUGUI | Marque |
| productImage | Image | Image produit |
| nutritionalInfo | TextMeshProUGUI[] | Infos nutritionnelles |
| originInfo | TextMeshProUGUI[] | Infos d'origine |
| healthScoreText | TextMeshProUGUI | Score santé |
| ecoScoreText | TextMeshProUGUI | Score écologique |
| healthScoreFill | Image | Barre score santé |
| ecoScoreFill | Image | Barre score éco |

#### Methods

##### DisplayProductInfo()
```csharp
private void DisplayProductInfo()
```
Affiche toutes les informations du produit

##### GetScoreColor(float score)
```csharp
private Color GetScoreColor(float score)
```
Retourne la couleur selon le score
- **Parameters**:
  - `score`: Score (0-100)
- **Returns**: Couleur correspondante

---

### RecommendationsController

**Namespace**: Global  
**Description**: Contrôleur pour l'écran de recommandations

#### Inspector Fields

| Field | Type | Description |
|-------|------|-------------|
| recommendationsContainer | Transform | Container des cards |
| productCardPrefab | GameObject | Prefab de card produit |
| bioFilter | Toggle | Filtre Bio |
| ecoFilter | Toggle | Filtre Éco |
| priceFilter | Toggle | Filtre Prix |
| healthFilter | Toggle | Filtre Santé |
| sortDropdown | TMP_Dropdown | Menu de tri |

#### Methods

##### ApplyFilters()
```csharp
private void ApplyFilters()
```
Applique les filtres sélectionnés

##### OnSortChanged()
```csharp
private void OnSortChanged()
```
Applique le tri sélectionné

##### DisplayRecommendations()
```csharp
private void DisplayRecommendations()
```
Affiche la liste des recommandations

---

### ProfileController

**Namespace**: Global  
**Description**: Contrôleur pour l'écran de profil

#### Inspector Fields

| Field | Type | Description |
|-------|------|-------------|
| userNameText | TextMeshProUGUI | Nom utilisateur |
| avatarImage | Image | Avatar |
| preferencesContainer | Transform | Container préférences |
| historyContainer | Transform | Container historique |
| soundToggle | Toggle | Toggle son |
| hapticToggle | Toggle | Toggle haptique |
| darkModeToggle | Toggle | Toggle mode sombre |
| languageDropdown | TMP_Dropdown | Sélection langue |

#### Methods

##### DisplayPreferences()
```csharp
private void DisplayPreferences()
```
Affiche les préférences alimentaires

##### DisplayHistory()
```csharp
private void DisplayHistory()
```
Affiche l'historique des scans

##### DisplayStats()
```csharp
private void DisplayStats()
```
Affiche les statistiques utilisateur

---

## Utility Managers

### NavigationManager

**Namespace**: Global  
**Description**: Singleton gérant la navigation entre scènes

#### Static Properties

| Property | Type | Description |
|----------|------|-------------|
| Instance | NavigationManager | Instance singleton |

#### Methods

##### NavigateToScene(string sceneName, float delay = 0f)
```csharp
public void NavigateToScene(string sceneName, float delay = 0f)
```
Navigation vers une scène
- **Parameters**:
  - `sceneName`: Nom de la scène
  - `delay`: Délai optionnel en secondes

##### NavigateToHome()
```csharp
public void NavigateToHome()
```
Navigation vers l'accueil

##### NavigateToScanner()
```csharp
public void NavigateToScanner()
```
Navigation vers le scanner

##### NavigateToProductInfo()
```csharp
public void NavigateToProductInfo()
```
Navigation vers info produit

##### NavigateToRecommendations()
```csharp
public void NavigateToRecommendations()
```
Navigation vers recommandations

##### NavigateToProfile()
```csharp
public void NavigateToProfile()
```
Navigation vers profil

##### NavigateBack()
```csharp
public void NavigateBack()
```
Retour à la scène précédente

---

### QRCodeManager

**Namespace**: Global  
**Description**: Singleton gérant le scan de codes QR

#### Static Properties

| Property | Type | Description |
|----------|------|-------------|
| Instance | QRCodeManager | Instance singleton |

#### Events

```csharp
public event Action<string> OnQRCodeScanned;
public event Action<string> OnScanError;
```

#### Methods

##### StartScanning()
```csharp
public void StartScanning()
```
Démarre le scanner

##### StopScanning()
```csharp
public void StopScanning()
```
Arrête le scanner

##### IsScanning()
```csharp
public bool IsScanning()
```
Vérifie si le scan est actif
- **Returns**: true si actif

##### SimulateScan(string qrCodeData)
```csharp
public void SimulateScan(string qrCodeData)
```
Simule un scan (mode test)
- **Parameters**:
  - `qrCodeData`: Données du QR code

##### GetCurrentProductId()
```csharp
public string GetCurrentProductId()
```
Obtient l'ID du produit actuel
- **Returns**: Product ID

##### SetCurrentProductId(string productId)
```csharp
public void SetCurrentProductId(string productId)
```
Définit l'ID du produit actuel
- **Parameters**:
  - `productId`: ID du produit

##### EnableTorch(bool enable)
```csharp
public void EnableTorch(bool enable)
```
Active/désactive la torche
- **Parameters**:
  - `enable`: true pour activer

---

## Events and Delegates

### QRCodeManager Events

**OnQRCodeScanned**
```csharp
public event Action<string> OnQRCodeScanned
```
Déclenché lors d'un scan réussi
- **Parameter**: productId (string)

**OnScanError**
```csharp
public event Action<string> OnScanError
```
Déclenché en cas d'erreur de scan
- **Parameter**: errorMessage (string)

### Usage Example

```csharp
void Start()
{
    QRCodeManager.Instance.OnQRCodeScanned += HandleQRCodeScanned;
    QRCodeManager.Instance.OnScanError += HandleScanError;
}

void HandleQRCodeScanned(string productId)
{
    Debug.Log($"Product scanned: {productId}");
    // Navigate to product info
}

void HandleScanError(string error)
{
    Debug.LogError($"Scan error: {error}");
    // Show error message
}

void OnDestroy()
{
    QRCodeManager.Instance.OnQRCodeScanned -= HandleQRCodeScanned;
    QRCodeManager.Instance.OnScanError -= HandleScanError;
}
```

---

## Usage Examples

### Example 1: Load and Display a Product

```csharp
// Get product from database
ProductData product = ProductDatabase.Instance.GetProductById("PROD001");

if (product != null)
{
    // Display product info
    productNameText.text = product.name;
    brandText.text = product.brand;
    priceText.text = $"{product.price:F2}€";
    
    // Display scores
    healthScoreText.text = $"{product.scores.healthScore:F0}/100";
    ecoScoreText.text = $"{product.scores.ecoScore:F0}/100";
}
```

### Example 2: Filter Products

```csharp
// Get all products
List<ProductData> allProducts = ProductDatabase.Instance.GetAllProducts();

// Filter for Bio and high health score
List<string> tags = new List<string> { "Bio" };
List<ProductData> filtered = ProductDatabase.Instance.FilterProducts(
    allProducts, 
    tags, 
    minHealthScore: 80f
);

// Display filtered products
foreach (var product in filtered)
{
    Debug.Log($"{product.name} - Health: {product.scores.healthScore}");
}
```

### Example 3: Save User Preferences

```csharp
// Load preferences
UserPreferences prefs = UserPreferences.Load();

// Add dietary preference
prefs.dietaryPreferences.Add("Végétarien");

// Record a scan
prefs.AddScannedProduct("PROD001");

// Change settings
prefs.settings.soundEnabled = false;
prefs.settings.language = "en";

// Save to disk
prefs.Save();
```

### Example 4: Navigate Between Scenes

```csharp
// Navigate to scanner
NavigationManager.Instance.NavigateToScanner();

// Navigate with delay
NavigationManager.Instance.NavigateToScene("ProductInfoScene", 1.5f);

// Go back
NavigationManager.Instance.NavigateBack();
```

---

## Constants and Enums

### Score Ranges

```csharp
// Health/Eco Score Interpretation
// 80-100: Excellent (Green)
// 60-79:  Good (Yellow)
// 40-59:  Average (Orange)
// 0-39:   Poor (Red)
```

### Nutrition Grades

```csharp
// A: Excellent
// B: Good
// C: Average
// D: Poor
// E: Very Poor
```

### Scene Names

```csharp
const string HOME_SCENE = "HomeScene";
const string SCANNER_SCENE = "ScannerScene";
const string PRODUCT_INFO_SCENE = "ProductInfoScene";
const string RECOMMENDATIONS_SCENE = "RecommendationsScene";
const string PROFILE_SCENE = "ProfileScene";
```

---

**Version**: Sprint 1  
**Last Updated**: 2024

For more information, see [DOCUMENTATION.md](DOCUMENTATION.md)
