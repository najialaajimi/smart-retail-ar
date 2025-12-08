# Guide de Développement - Smart Retail AR

## Table des Matières
- [Configuration de l'Environnement](#configuration-de-lenvironnement)
- [Structure du Projet](#structure-du-projet)
- [Conventions de Code](#conventions-de-code)
- [Workflow de Développement](#workflow-de-développement)
- [Debugging](#debugging)
- [Tests](#tests)
- [Contribution](#contribution)

## Configuration de l'Environnement

### Prérequis

#### Logiciels Requis
- **Unity Hub** 3.x ou supérieur
- **Unity Editor** 2022.3.62f3 LTS (exactement)
- **Visual Studio 2022** ou **JetBrains Rider** (recommandé)
- **Git** 2.x ou supérieur

#### SDK et Outils Mobiles

**Pour Android:**
- Android Studio
- Android SDK Platform 24+ (Android 7.0)
- Android SDK Build-Tools 30.0.3+
- NDK (r21e recommandé)
- JDK 11

**Pour iOS:**
- macOS 12.0+
- Xcode 14.0+
- iOS SDK 11.0+
- Apple Developer Account

### Installation et Setup

#### 1. Cloner le Repository

```bash
git clone https://github.com/najialaajimi/smart-retail-ar.git
cd smart-retail-ar
```

#### 2. Ouvrir dans Unity

1. Lancer Unity Hub
2. Click "Add" > "Add project from disk"
3. Sélectionner le dossier `smart-retail-ar`
4. Ouvrir avec Unity 2022.3.62f3 LTS

#### 3. Configuration des Packages

Les packages nécessaires seront automatiquement importés depuis `Packages/manifest.json`:
- AR Foundation 5.1.5
- ARCore XR Plugin 5.1.5 (Android)
- ARKit XR Plugin 5.1.5 (iOS)
- TextMeshPro
- Unity UI

#### 4. Configuration du Projet

**Android:**
```
File > Build Settings > Android
Player Settings:
  - Company Name: [Votre entreprise]
  - Product Name: Smart Retail AR
  - Package Name: com.yourcompany.smartretailar
  - Minimum API Level: 24
  - Target API Level: 33
  
XR Plug-in Management:
  - ARCore: ✓
  
Player > Other Settings:
  - Graphics API: OpenGLES3, Vulkan
  - Scripting Backend: IL2CPP
  - Target Architectures: ARM64
```

**iOS:**
```
File > Build Settings > iOS
Player Settings:
  - Company Name: [Votre entreprise]
  - Product Name: Smart Retail AR
  - Bundle Identifier: com.yourcompany.smartretailar
  - Minimum iOS Version: 11.0
  
XR Plug-in Management:
  - ARKit: ✓
  
Player > Other Settings:
  - Target SDK: Device SDK
  - Architecture: ARM64
  - Camera Usage Description: "Required for AR product scanning"
```

## Structure du Projet

### Organisation des Dossiers

```
smart-retail-ar/
├── Assets/
│   ├── Scripts/
│   │   ├── UI/              # Controllers d'interface utilisateur
│   │   ├── AR/              # Composants AR Foundation
│   │   ├── Recommendations/ # Moteur de recommandations
│   │   ├── Testing/         # Analytics et tests
│   │   ├── Data/            # Structures de données
│   │   └── Utils/           # Utilitaires et helpers
│   ├── Scenes/
│   │   ├── Frontend/        # Scènes d'interface utilisateur
│   │   ├── AR/              # Scènes AR
│   │   └── Testing/         # Scènes de test
│   ├── Resources/
│   │   └── Data/            # Données JSON (produits)
│   ├── Prefabs/
│   │   ├── UI/              # Prefabs d'interface
│   │   └── AR/              # Prefabs AR
│   └── AR/
│       ├── ReferenceImages/ # Images de référence pour tracking
│       └── ARPrefabs/       # Prefabs spécifiques AR
├── ProjectSettings/         # Configuration Unity
├── Packages/                # Package Manager
└── Documentation/           # Documentation du projet
```

### Scripts Principaux

#### UI Layer (`Assets/Scripts/UI/`)
- **HomeController.cs** - Écran d'accueil
- **ScannerController.cs** - Interface de scan
- **ProductInfoController.cs** - Détails produit
- **ProductCard.cs** - Composant réutilisable

#### Data Layer (`Assets/Scripts/Data/`)
- **ProductData.cs** - Structures de données
- **ProductDatabase.cs** - Singleton pour BD
- **UserPreferences.cs** - Préférences utilisateur

#### AR Layer (`Assets/Scripts/AR/`)
- **ARImageTracker.cs** - Tracking d'images
- **ARDataOverlay.cs** - Overlay AR
- **ARSessionManager.cs** - Gestion session

#### Recommendation Layer (`Assets/Scripts/Recommendations/`)
- **RecommendationEngine.cs** - Moteur principal
- **ScoringSystem.cs** - Système de scoring

#### Testing Layer (`Assets/Scripts/Testing/`)
- **AnalyticsManager.cs** - Analytics
- **PerformanceMonitor.cs** - Performance

## Conventions de Code

### Style C#

**Naming Conventions:**
```csharp
// Classes, Structs, Enums: PascalCase
public class ProductDatabase { }
public struct ProductData { }
public enum ScoreType { }

// Public Methods: PascalCase
public void LoadProducts() { }

// Private Methods: PascalCase
private void InitializeDatabase() { }

// Public Fields: PascalCase
public string ProductName;

// Private Fields: camelCase with underscore prefix
private string _productId;
private bool _isInitialized;

// Local Variables: camelCase
int productCount = 0;
float totalScore = 0f;

// Constants: UPPER_CASE
private const int MAX_PRODUCTS = 100;
private const string DEFAULT_CATEGORY = "General";
```

**Formatting:**
```csharp
// Braces on new line (Allman style)
if (condition)
{
    DoSomething();
}
else
{
    DoSomethingElse();
}

// Properties
public string Name { get; private set; }

// Auto-properties
public int Count { get; set; }

// Comments
/// <summary>
/// Load product from database by ID
/// </summary>
/// <param name="productId">Unique product identifier</param>
/// <returns>ProductData or null if not found</returns>
public ProductData LoadProduct(string productId)
{
    // Implementation
}
```

### Architecture Patterns

**Singleton Pattern:**
```csharp
public class MyManager : MonoBehaviour
{
    private static MyManager _instance;
    
    public static MyManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = new GameObject("MyManager");
                _instance = go.AddComponent<MyManager>();
                DontDestroyOnLoad(go);
            }
            return _instance;
        }
    }
    
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }
}
```

**Observer Pattern (Events):**
```csharp
public class ARImageTracker : MonoBehaviour
{
    // Define event
    public event Action<string> OnProductDetected;
    
    // Trigger event
    private void DetectProduct(string productId)
    {
        OnProductDetected?.Invoke(productId);
    }
}

// Subscribe to event
tracker.OnProductDetected += HandleProductDetection;
```

### Commentaires et Documentation

**XML Documentation:**
```csharp
/// <summary>
/// Calculates the nutritional score for a product
/// </summary>
/// <param name="product">Product to evaluate</param>
/// <returns>Score between 0 and 1</returns>
public float CalculateNutritionalScore(ProductData product)
{
    // Implementation
}
```

**Inline Comments:**
```csharp
// Good: Explain WHY, not WHAT
// Apply cooldown to prevent multiple rapid detections
if (Time.time - lastDetectionTime < DETECTION_COOLDOWN)
{
    return;
}

// Bad: Obvious comment
// Set x to 5
int x = 5;
```

## Workflow de Développement

### Git Workflow

#### Branches

```
main                    # Production-ready code
├── develop            # Integration branch
│   ├── feature/xxx    # New features
│   ├── bugfix/xxx     # Bug fixes
│   └── hotfix/xxx     # Urgent fixes
```

#### Créer une Feature

```bash
# Créer et basculer sur nouvelle branche
git checkout -b feature/nouvelle-fonctionnalite

# Développer et commiter
git add .
git commit -m "Add: Description de la fonctionnalité"

# Pousser vers remote
git push origin feature/nouvelle-fonctionnalite

# Créer Pull Request sur GitHub
```

#### Commit Messages

Format: `Type: Description courte`

**Types:**
- `Add:` Nouvelle fonctionnalité
- `Fix:` Correction de bug
- `Update:` Modification de fonctionnalité existante
- `Refactor:` Refactoring de code
- `Docs:` Documentation seulement
- `Test:` Ajout/modification de tests
- `Style:` Formatting, pas de changement de code

**Exemples:**
```
Add: Implement product comparison feature
Fix: Resolve AR tracking stability issue
Update: Improve recommendation algorithm accuracy
Refactor: Simplify scoring system calculations
Docs: Update API documentation
Test: Add unit tests for RecommendationEngine
```

### Développement Itératif

#### 1. Planification
- Définir les objectifs
- Créer des user stories
- Estimer la complexité

#### 2. Implémentation
- Créer une branche feature
- Développer par petits incréments
- Commiter fréquemment

#### 3. Testing
- Tests unitaires pour logique
- Tests d'intégration pour interactions
- Tests manuels sur device

#### 4. Review
- Self-review du code
- Pull Request review
- Code quality check

#### 5. Merge
- Merger dans develop
- Supprimer la branche feature
- Déployer si nécessaire

## Debugging

### Unity Editor

**Console Logging:**
```csharp
Debug.Log("Information message");
Debug.LogWarning("Warning message");
Debug.LogError("Error message");

// Avec contexte
Debug.Log($"Product detected: {productId}", gameObject);
```

**Conditional Compilation:**
```csharp
#if UNITY_EDITOR
    Debug.Log("Development log");
#endif

#if DEVELOPMENT_BUILD || UNITY_EDITOR
    // Debug code
#endif
```

### Visual Studio Debugging

**Attacher au processus:**
1. Debug > Attach Unity Debugger
2. Sélectionner le processus Unity
3. Placer des breakpoints
4. F5 pour continuer, F10 pour step over

**Breakpoints:**
```csharp
public void MyMethod()
{
    int x = 5;              // Placer breakpoint ici
    DoSomething(x);
    Debug.Log("Done");
}
```

### AR Debugging

**Remote Debugging:**
```csharp
// Activer le debug overlay AR
[SerializeField] private bool showDebugOverlay = true;

private void OnGUI()
{
    if (showDebugOverlay)
    {
        GUI.Label(new Rect(10, 10, 300, 100), 
            $"FPS: {GetFPS()}\n" +
            $"Tracked Images: {GetTrackedCount()}");
    }
}
```

**ADB Logcat (Android):**
```bash
# Voir les logs Unity
adb logcat -s Unity

# Filtrer par tag
adb logcat -s Unity ActivityManager

# Sauvegarder dans un fichier
adb logcat > logcat.txt
```

### Performance Profiling

**Unity Profiler:**
1. Window > Analysis > Profiler
2. Build and Run sur device
3. Profiler > Target > AndroidPlayer/iOSPlayer
4. Analyser CPU, GPU, Memory

**Key Metrics:**
- CPU Usage
- Rendering
- Memory (GC Alloc)
- Physics
- Audio

## Tests

### Tests Unitaires

**Setup:**
```csharp
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

[TestFixture]
public class ProductDatabaseTests
{
    [SetUp]
    public void Setup()
    {
        // Initialize before each test
    }
    
    [TearDown]
    public void Teardown()
    {
        // Cleanup after each test
    }
    
    [Test]
    public void TestGetProductById()
    {
        // Arrange
        ProductDatabase db = ProductDatabase.Instance;
        
        // Act
        ProductData product = db.GetProductById("PROD001");
        
        // Assert
        Assert.IsNotNull(product);
        Assert.AreEqual("PROD001", product.id);
    }
}
```

**Exécution:**
- Window > General > Test Runner
- PlayMode / EditMode
- Run All / Run Selected

### Tests d'Intégration

**Exemple:**
```csharp
[UnityTest]
public IEnumerator TestScannerToProductInfo()
{
    // Load scanner scene
    SceneManager.LoadScene("ARScannerScene");
    yield return null;
    
    // Simulate product detection
    ScannerController scanner = FindObjectOfType<ScannerController>();
    scanner.OnProductDetected("PROD001");
    
    yield return new WaitForSeconds(1f);
    
    // Verify navigation to product info
    Assert.AreEqual("ProductInfoScene", SceneManager.GetActiveScene().name);
}
```

### Tests sur Device

**Android:**
```bash
# Build et installer
Unity: File > Build and Run

# Ou via command line
/path/to/Unity -projectPath . -buildTarget Android -executeMethod BuildScript.BuildAndroid
```

**iOS:**
```bash
# Générer Xcode project
Unity: File > Build Settings > Build

# Ouvrir dans Xcode
open builds/iOS/Unity-iPhone.xcodeproj

# Build et run depuis Xcode
```

## Contribution

### Pull Request Process

1. **Fork le repository**
2. **Créer une branche feature**
3. **Faire vos modifications**
4. **Ajouter des tests**
5. **Update documentation**
6. **Commit avec messages clairs**
7. **Push vers votre fork**
8. **Créer Pull Request**

### PR Checklist

- [ ] Code suit les conventions
- [ ] Tests ajoutés/mis à jour
- [ ] Documentation mise à jour
- [ ] Pas de warnings Unity
- [ ] Build passe (Android/iOS)
- [ ] Tests passent
- [ ] Performance acceptable
- [ ] Review effectuée

### Code Review Guidelines

**Pour les reviewers:**
- Vérifier la logique et algorithmes
- Checker les edge cases
- Valider les performances
- Tester manuellement si possible
- Donner feedback constructif

**Pour les auteurs:**
- Répondre aux commentaires
- Effectuer les modifications demandées
- Re-request review après changements
- Merger quand approuvé

## Ressources et Liens

### Documentation Unity
- [Unity Manual](https://docs.unity3d.com/Manual/index.html)
- [Scripting API](https://docs.unity3d.com/ScriptReference/)
- [AR Foundation](https://docs.unity3d.com/Packages/com.unity.xr.arfoundation@5.1/manual/index.html)

### Outils
- [Visual Studio](https://visualstudio.microsoft.com/)
- [JetBrains Rider](https://www.jetbrains.com/rider/)
- [GitKraken](https://www.gitkraken.com/)

### Communauté
- [Unity Forum](https://forum.unity.com/)
- [Stack Overflow](https://stackoverflow.com/questions/tagged/unity3d)
- [Discord Unity](https://discord.com/invite/unity)

---

**Bon développement! 🚀**
