# Guide d'Intégration - Smart Retail AR

## Table des Matières

1. [Configuration Initiale](#configuration-initiale)
2. [Intégration AR Foundation](#intégration-ar-foundation)
3. [Configuration de la Base de Données](#configuration-de-la-base-de-données)
4. [Génération des QR Codes](#génération-des-qr-codes)
5. [Création des Scènes](#création-des-scènes)
6. [Configuration des Scripts](#configuration-des-scripts)
7. [Tests et Validation](#tests-et-validation)

---

## Configuration Initiale

### 1. Prérequis Système

Assurez-vous d'avoir:
- Unity 2022.3.62f3 LTS ou version ultérieure
- 10 GB d'espace disque libre
- Git installé
- Python 3.7+ (pour génération QR codes)

### 2. Clone du Projet

```bash
git clone https://github.com/najialaajimi/smart-retail-ar.git
cd smart-retail-ar
git checkout smart-retail-ar-complet-final
```

### 3. Ouverture dans Unity

1. Ouvrir Unity Hub
2. Ajouter le projet (Add > Select folder)
3. Vérifier la version Unity (2022.3.62f3)
4. Ouvrir le projet

**⏱️ Temps estimé: 10-15 minutes**

---

## Intégration AR Foundation

### 1. Vérification des Packages

Ouvrir `Window > Package Manager` et vérifier:

- ✅ AR Foundation 5.1.5
- ✅ AR Core XR Plugin 5.1.5
- ✅ AR Kit XR Plugin 5.1.5
- ✅ XR Plugin Management 4.4.1

Si manquants, installer depuis Package Manager.

### 2. Configuration XR Plugin Management

1. `Edit > Project Settings > XR Plugin Management`
2. Onglet **Android**: Cocher `ARCore`
3. Onglet **iOS**: Cocher `ARKit`
4. Appliquer les changements

### 3. Configuration Player Settings

#### Pour Android:
```
Edit > Project Settings > Player > Android

Other Settings:
  - Package Name: com.smartretail.ar
  - Minimum API Level: Android 7.0 (API 24)
  - Target API Level: Automatic (highest installed)
  - Scripting Backend: IL2CPP
  - Target Architectures: ARM64 ✓

Publishing Settings:
  - Build: Custom Keystore (optionnel pour release)
```

#### Pour iOS:
```
Edit > Project Settings > Player > iOS

Other Settings:
  - Camera Usage Description: "Nécessaire pour scanner les produits en AR"
  - Target minimum iOS Version: 11.0
  - Architecture: ARM64
  - Requires ARKit Support: ✓
```

**⏱️ Temps estimé: 15 minutes**

---

## Configuration de la Base de Données

### 1. Structure de la Base de Données

Le fichier `Assets/Resources/Data/products_database.json` contient 50 produits.

**Structure d'un produit:**

```json
{
  "id": "PROD001",
  "name": "Nom du produit",
  "brand": "Marque",
  "category": "Catégorie",
  "origin": "Origine",
  "price": 12.99,
  "barcode": "7000000000001",
  "qrCode": "PROD001",
  "nutritionalInfo": {
    "servingSize": 100,
    "calories": 150,
    "protein": 8.5,
    "carbohydrates": 25.0,
    "sugars": 5.0,
    "fats": 3.5,
    "saturatedFats": 1.0,
    "fiber": 4.0,
    "sodium": 200,
    "cholesterol": 0,
    "vitaminA": 15,
    "vitaminC": 20,
    "calcium": 10,
    "iron": 8
  },
  "ecoScore": 85.5,
  "healthScore": 78.3,
  "qualityScore": 92.1,
  "certifications": ["EU Organic", "Fair Trade"],
  "isBio": true,
  "isLocal": true,
  "isVegan": false,
  "isGlutenFree": false,
  "isFairTrade": true,
  "alternativeIds": ["PROD002", "PROD003"],
  "description": "Description du produit",
  "imageUrl": "products/prod001.jpg",
  "manufacturer": "Fabricant",
  "expirationDate": "2025-12-31T00:00:00Z"
}
```

### 2. Ajout de Nouveaux Produits

Pour ajouter un produit:

1. Ouvrir `Assets/Resources/Data/products_database.json`
2. Copier un produit existant comme template
3. Modifier toutes les valeurs
4. Générer un ID unique (ex: PROD051)
5. Sauvegarder le fichier

### 3. Catégories Disponibles

- Dairy (Produits laitiers)
- Fruits
- Vegetables (Légumes)
- Beverages (Boissons)
- Snacks (Collations)
- Bakery (Boulangerie)
- Meat (Viande)
- Seafood (Fruits de mer)
- Frozen (Surgelés)
- Pantry (Épicerie)

### 4. Validation de la Base de Données

Exécuter ce script Python pour valider:

```python
import json

with open('Assets/Resources/Data/products_database.json', 'r') as f:
    db = json.load(f)
    products = db['products']
    
    print(f"Nombre de produits: {len(products)}")
    
    # Vérifier IDs uniques
    ids = [p['id'] for p in products]
    if len(ids) == len(set(ids)):
        print("✓ Tous les IDs sont uniques")
    else:
        print("✗ Des IDs sont dupliqués!")
    
    # Vérifier structure
    required_fields = ['id', 'name', 'brand', 'category', 'price']
    for product in products:
        missing = [f for f in required_fields if f not in product]
        if missing:
            print(f"✗ Produit {product.get('id', '?')} manque: {missing}")
```

**⏱️ Temps estimé: 20 minutes**

---

## Génération des QR Codes

### 1. Installation des Dépendances Python

```bash
pip install qrcode[pil]
```

### 2. Génération Automatique

Exécuter le script:

```bash
python3 generate_qrcodes.py
```

Ce script va:
- Lire la base de données
- Générer un QR code PNG pour chaque produit
- Créer un fichier HTML imprimable
- Sauvegarder dans `Assets/Resources/QRCodes/`

### 3. Génération Manuelle

Si vous préférez générer manuellement:

```python
import qrcode

product_id = "PROD001"

qr = qrcode.QRCode(
    version=1,
    error_correction=qrcode.constants.ERROR_CORRECT_L,
    box_size=10,
    border=4,
)
qr.add_data(product_id)
qr.make(fit=True)

img = qr.make_image(fill_color="black", back_color="white")
img.save(f"Assets/Resources/QRCodes/{product_id}.png")
```

### 4. Impression des QR Codes

1. Ouvrir `Assets/Resources/QRCodes/printable_qr_codes.html`
2. Imprimer (Ctrl+P / Cmd+P)
3. Paramètres d'impression:
   - Format: A4
   - Orientation: Portrait
   - Marges: Normales
   - Échelle: 100%

**Spécifications d'impression:**
- Taille recommandée: 3x3 cm minimum
- Résolution: 300 DPI
- Format: Noir et blanc
- Support: Papier autocollant ou étiquettes

### 5. Application des QR Codes

Options:
- Coller sur les produits physiques
- Placer sur les étagères
- Afficher sur un écran pour tests
- Créer des cartes produits

**⏱️ Temps estimé: 15 minutes**

---

## Création des Scènes

### 1. Scène HomeScene

**Création:**
1. `File > New Scene`
2. Sauvegarder sous `Assets/Scenes/HomeScene.unity`

**Hiérarchie:**
```
HomeScene
├── Main Camera
├── Canvas
│   ├── Title Text
│   ├── Start Button
│   ├── Scanner Button
│   └── Settings Button
└── EventSystem
```

**Scripts à attacher:**
- `NavigationManager` (nouveau GameObject vide)
- `ProductDatabaseManager` (nouveau GameObject vide)

### 2. Scène ScannerScene

**Création:**
1. `GameObject > XR > AR Session Origin`
2. `GameObject > XR > AR Session`
3. Sauvegarder sous `Assets/Scenes/ScannerScene.unity`

**Hiérarchie:**
```
ScannerScene
├── AR Session
├── AR Session Origin
│   ├── AR Camera
│   │   └── AR Camera Manager (component)
│   └── AR Tracked Image Manager (component)
├── QRScanner (Empty GameObject)
│   └── QRScanner.cs
├── Canvas
│   ├── Scanner Overlay
│   ├── Status Text
│   ├── Loading Indicator
│   └── Result Panel
│       ├── Product Name
│       ├── Product Price
│       ├── View Details Button
│       └── Scan Again Button
└── Managers
    ├── ProductDatabaseManager
    ├── NavigationManager
    └── AnalyticsManager
```

**Configuration QRScanner:**
- Test Mode: ✓ (pour développement sans AR)
- Test Product IDs: ["PROD001", "PROD002", "PROD003"]
- Scan Interval: 0.5
- Continuous Scanning: ☐

### 3. Scène ProductInfoScene

**Hiérarchie:**
```
ProductInfoScene
├── Main Camera
├── Canvas
│   ├── Header
│   │   ├── Product Name
│   │   ├── Brand Text
│   │   └── Back Button
│   ├── Product Image
│   ├── Info Panel
│   │   ├── Price
│   │   ├── Origin
│   │   ├── Category
│   │   └── Description
│   ├── Scores Section
│   │   ├── Eco Score Slider
│   │   ├── Health Score Slider
│   │   └── Quality Score Slider
│   ├── Nutritional Info Panel
│   │   ├── Calories
│   │   ├── Protein
│   │   ├── Carbs
│   │   └── ... (autres valeurs)
│   ├── Tags Container
│   │   ├── Bio Tag
│   │   ├── Local Tag
│   │   ├── Vegan Tag
│   │   └── ... (autres tags)
│   ├── Certifications Container
│   └── Action Buttons
│       ├── View AR Button
│       └── View Alternatives Button
└── ProductInfoUI (Controller)
```

**Script à attacher:**
- `ProductInfoUI.cs` sur le GameObject racine ou Controller

### 4. Scène ARScene

**Hiérarchie:**
```
ARScene
├── AR Session
├── AR Session Origin
│   ├── AR Camera
│   ├── AR Plane Manager
│   ├── AR Raycast Manager
│   └── AR Tracked Image Manager
├── AR Product Overlay (Prefab)
│   └── ARProductOverlay.cs
├── Canvas
│   ├── Instructions Text
│   ├── Toggle Nutritional Button
│   ├── Toggle Scores Button
│   └── Back Button
└── Managers
    ├── ARSessionManager
    ├── NavigationManager
    └── AnalyticsManager
```

### 5. Scène RecommendationsScene

**Hiérarchie:**
```
RecommendationsScene
├── Main Camera
├── Canvas
│   ├── Header
│   │   ├── Title
│   │   ├── Current Product Info
│   │   └── Back Button
│   ├── Filter Panel
│   │   ├── Bio Toggle
│   │   ├── Local Toggle
│   │   ├── Vegan Toggle
│   │   ├── Max Price Slider
│   │   ├── Min Eco Score Slider
│   │   ├── Apply Filters Button
│   │   └── Reset Filters Button
│   ├── Tab Buttons
│   │   ├── All Button
│   │   ├── Eco Button
│   │   ├── Healthy Button
│   │   └── Budget Button
│   └── Recommendations Scroll View
│       └── Content (Grid Layout)
│           └── (ProductCard instances créés dynamiquement)
└── RecommendationsUI (Controller)
```

**⏱️ Temps estimé: 45 minutes**

---

## Configuration des Scripts

### 1. ProductDatabaseManager

Aucune configuration requise. Le script charge automatiquement la base de données depuis `Resources/Data/products_database.json`.

**Vérification:**
```csharp
void Start() {
    if (ProductDatabaseManager.Instance.IsLoaded) {
        Debug.Log("Database loaded successfully!");
        var products = ProductDatabaseManager.Instance.GetAllProducts();
        Debug.Log($"Total products: {products.Count}");
    }
}
```

### 2. QRScanner

**Configuration dans l'Inspector:**
- AR Camera Manager: Référence vers ARCameraManager
- Test Mode: true (désactiver pour production)
- Test Product IDs: Liste de produits de test
- Scan Interval: 0.5 secondes
- Continuous Scanning: false

**Test:**
```csharp
QRScanner scanner = GetComponent<QRScanner>();
scanner.OnProductFound += (product) => {
    Debug.Log($"Product scanned: {product.name}");
};
scanner.StartScanning();
```

### 3. RecommendationEngine

**Configuration des critères par défaut:**
```csharp
void Start() {
    var criteria = new RecommendationEngine.RecommendationCriteria {
        preferBio = false,
        preferLocal = false,
        maxPrice = float.MaxValue,
        minEcoScore = 0f,
        minHealthScore = 0f
    };
    RecommendationEngine.Instance.SetCriteria(criteria);
}
```

### 4. AnalyticsManager

Aucune configuration requise. L'analytics démarre automatiquement.

**Export des données:**
```csharp
// Exporter les analytics
AnalyticsManager.Instance.ExportAnalytics();
// Fichier sauvegardé dans Application.persistentDataPath
```

### 5. PerformanceMonitor

**Configuration:**
- Show Debug Info: true (afficher FPS à l'écran)
- Update Interval: 1.0 seconde

**⏱️ Temps estimé: 20 minutes**

---

## Tests et Validation

### 1. Test de Chargement de la Base de Données

```csharp
[Test]
public void TestDatabaseLoading() {
    var db = ProductDatabaseManager.Instance;
    Assert.IsTrue(db.IsLoaded, "Database should be loaded");
    Assert.IsTrue(db.GetAllProducts().Count > 0, "Should have products");
}
```

### 2. Test de Scan QR

**Mode Test (sans AR):**
1. Lancer ScannerScene
2. Activer Test Mode dans QRScanner
3. Cliquer sur Scan
4. Vérifier affichage du produit

**Mode Réel (avec AR):**
1. Build sur device Android/iOS
2. Désactiver Test Mode
3. Pointer vers un QR code imprimé
4. Vérifier reconnaissance

### 3. Test de Recommandations

```csharp
[Test]
public void TestRecommendations() {
    var product = ProductDatabaseManager.Instance.GetProductById("PROD001");
    var recommendations = RecommendationEngine.Instance.GetRecommendations(product, 5);
    Assert.IsTrue(recommendations.Count > 0, "Should have recommendations");
}
```

### 4. Test de Performance

Exécuter:
```csharp
TestManager.Instance.RunTest("all");
```

Vérifier dans la console:
- ✓ Database loading test PASSED
- ✓ Product retrieval test PASSED
- ✓ QR Scanning test PASSED
- ✓ Recommendations test PASSED
- ✓ Performance test PASSED

### 5. Validation des KPIs

```
Recognition Rate: ≥ 95% ✓
Average Latency: ≤ 1.0s ✓
User Satisfaction: ≥ 80% ✓
```

**⏱️ Temps estimé: 30 minutes**

---

## Dépannage

### Problème: Database non chargée

**Solution:**
- Vérifier que `products_database.json` est dans `Assets/Resources/Data/`
- Vérifier la syntaxe JSON (pas d'erreurs)
- Regarder les logs Unity pour erreurs

### Problème: QR codes non reconnus

**Solution:**
- Vérifier la qualité d'impression (contraste élevé)
- Vérifier l'éclairage (éviter ombres/reflets)
- Augmenter la taille du QR code (min 3x3cm)
- Activer Test Mode pour développement

### Problème: AR ne démarre pas

**Solution:**
- Vérifier AR Foundation packages installés
- Vérifier XR Plugin Management activé
- Sur Android: ARCore services installé
- Sur iOS: Autorisation caméra accordée

### Problème: Performance faible

**Solution:**
- Réduire la qualité graphique (Edit > Project Settings > Quality)
- Désactiver Show Debug Info
- Optimiser les textures (compression)
- Limiter le nombre d'objets AR affichés

---

## Checklist Finale

Avant de considérer l'intégration complète:

- [ ] Tous les packages AR Foundation installés
- [ ] Base de données chargée avec succès
- [ ] QR codes générés et testés
- [ ] Toutes les scènes créées et configurées
- [ ] Scripts attachés correctement
- [ ] Tests automatisés passent
- [ ] Build Android/iOS réussie
- [ ] KPIs dans les cibles
- [ ] Documentation lue et comprise

---

**Temps total estimé: 2-3 heures**

**Félicitations! L'intégration est complète! 🎉**
