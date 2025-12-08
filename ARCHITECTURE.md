# Architecture Technique - Smart Retail AR

## Vue d'Ensemble

Smart Retail AR est une application mobile de réalité augmentée construite sur Unity avec AR Foundation. L'architecture suit un pattern MVC (Model-View-Controller) avec des managers singleton pour la gestion des services globaux.

## Diagramme d'Architecture

```
┌─────────────────────────────────────────────────────────────┐
│                     PRESENTATION LAYER                       │
├─────────────────────────────────────────────────────────────┤
│  ┌──────────┐  ┌──────────┐  ┌──────────┐  ┌──────────┐  │
│  │ Scanner  │  │ Product  │  │   AR     │  │  Recom.  │  │
│  │   UI     │  │ Info UI  │  │ Overlay  │  │   UI     │  │
│  └──────────┘  └──────────┘  └──────────┘  └──────────┘  │
└─────────────────────────────────────────────────────────────┘
                            ↓
┌─────────────────────────────────────────────────────────────┐
│                     CONTROLLER LAYER                         │
├─────────────────────────────────────────────────────────────┤
│  ┌────────────────┐  ┌────────────────┐  ┌──────────────┐ │
│  │  Navigation    │  │   Analytics    │  │  Performance │ │
│  │   Manager      │  │    Manager     │  │   Monitor    │ │
│  └────────────────┘  └────────────────┘  └──────────────┘ │
└─────────────────────────────────────────────────────────────┘
                            ↓
┌─────────────────────────────────────────────────────────────┐
│                     BUSINESS LOGIC LAYER                     │
├─────────────────────────────────────────────────────────────┤
│  ┌──────────┐  ┌──────────────┐  ┌────────────────────┐   │
│  │   QR     │  │   Product    │  │  Recommendation    │   │
│  │ Scanner  │  │   Database   │  │     Engine         │   │
│  └──────────┘  └──────────────┘  └────────────────────┘   │
│                                                              │
│  ┌──────────────────────────────────────────────────────┐  │
│  │          AR Session Manager (AR Foundation)          │  │
│  └──────────────────────────────────────────────────────┘  │
└─────────────────────────────────────────────────────────────┘
                            ↓
┌─────────────────────────────────────────────────────────────┐
│                       DATA LAYER                             │
├─────────────────────────────────────────────────────────────┤
│  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐     │
│  │   Product    │  │   Analytics  │  │    User      │     │
│  │   Database   │  │     Data     │  │ Preferences  │     │
│  │    (JSON)    │  │              │  │              │     │
│  └──────────────┘  └──────────────┘  └──────────────┘     │
└─────────────────────────────────────────────────────────────┘
                            ↓
┌─────────────────────────────────────────────────────────────┐
│                     PLATFORM LAYER                           │
├─────────────────────────────────────────────────────────────┤
│  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐     │
│  │   ARCore     │  │    ARKit     │  │   Unity      │     │
│  │  (Android)   │  │    (iOS)     │  │   Engine     │     │
│  └──────────────┘  └──────────────┘  └──────────────┘     │
└─────────────────────────────────────────────────────────────┘
```

## Composants Principaux

### 1. Sprint 1: Scanner & Base de Données

#### QRScanner.cs
**Responsabilité:** Scanner les QR codes et codes-barres

**Fonctionnalités:**
- Capture d'images depuis AR Camera
- Décodage QR/codes-barres avec ZXing
- Mode test pour développement
- Détection continue ou ponctuelle

**Dépendances:**
- UnityEngine.XR.ARFoundation
- ZXing Library
- ProductDatabaseManager

**Flux:**
```
User Action (Scan) → Camera Capture → Image Processing →
QR Decode → Database Lookup → Product Found Event
```

#### ProductDatabaseManager.cs
**Responsabilité:** Gestion de la base de données produits

**Fonctionnalités:**
- Chargement JSON depuis Resources
- Cache en mémoire avec Dictionary lookup
- Recherche par ID, QR code, barcode
- Filtrage et recherche

**Pattern:** Singleton avec DontDestroyOnLoad

**Optimisations:**
- Dictionary lookup O(1)
- Chargement lazy
- Cache persistent entre scènes

#### ProductData.cs
**Responsabilité:** Modèle de données produit

**Structure:**
```csharp
class ProductData {
    // Identification
    string id, name, brand, category, origin
    
    // Commerce
    float price, barcode, qrCode
    
    // Nutrition
    NutritionalInfo nutritionalInfo
    
    // Scores
    float ecoScore, healthScore, qualityScore
    
    // Tags
    bool isBio, isLocal, isVegan, isGlutenFree, isFairTrade
    
    // Relations
    List<string> alternativeIds, certifications
}
```

### 2. Sprint 2: AR Foundation

#### ARSessionManager.cs
**Responsabilité:** Gestion de la session AR

**Fonctionnalités:**
- Initialisation AR Foundation
- Gestion des plans détectés
- Tracking d'images
- Raycasting pour placement

**Lifecycle:**
```
CheckAvailability → Initialize → StartSession →
Tracking Loop → SessionPaused/Resumed → StopSession
```

**Events:**
- OnARSessionStarted
- OnARSessionFailed
- OnImageTracked

#### ARProductOverlay.cs
**Responsabilité:** Overlay AR d'informations produit

**Fonctionnalités:**
- Billboard effect (face caméra)
- Follow target transform
- Panels nutritionnels/scores
- Animation smooth

**Composants UI:**
- Canvas World Space
- TextMeshPro pour texte
- Image pour icônes/scores
- LayoutGroup pour organisation

### 3. Sprint 3: Recommandations

#### RecommendationEngine.cs
**Responsabilité:** Moteur de recommandations intelligent

**Algorithme:**
```
Score = 
    + PriceSimilarity * 10
    + EcoScoreImprovement * 0.5
    + HealthScoreImprovement * 0.5
    + PreferenceBonus (Bio: 20, Local: 15, Vegan: 15)
    - PenaltyOutOfBudget (-50)
    - PenaltyLowScores (-30)
```

**Types de recommandations:**
1. **All Recommendations:** Score global
2. **Eco-Friendly:** Trié par ecoScore
3. **Healthy:** Trié par healthScore
4. **Budget:** Trié par price (ascendant)

**Critères de filtrage:**
```csharp
class RecommendationCriteria {
    bool preferBio, preferLocal, preferVegan, preferGlutenFree, preferFairTrade
    float maxPrice, minEcoScore, minHealthScore
    string preferredCategory
}
```

### 4. Sprint 4: Analytics & Performance

#### AnalyticsManager.cs
**Responsabilité:** Tracking et analytics

**Métriques collectées:**
- Nombre de scans (succès/échecs)
- Temps de scan (latence)
- Produits vus
- Alternatives consultées
- Vues AR

**KPIs:**
- Taux de reconnaissance = (Scans réussis / Total scans) × 100
- Latence moyenne = Σ(temps scan) / Nombre scans
- Satisfaction = Score utilisateur

**Export:**
JSON vers `Application.persistentDataPath/analytics.json`

#### PerformanceMonitor.cs
**Responsabilité:** Monitoring performance temps réel

**Métriques:**
- FPS (frames per second)
- Frame time (ms)
- Mémoire utilisée (MB)
- Niveau batterie (%)

**Optimisations automatiques:**
- Réduction qualité si FPS < 30
- Garbage collection si mémoire > 500MB
- Désactivation features selon device

#### TestManager.cs
**Responsabilité:** Tests automatisés

**Tests:**
1. Database loading
2. Product retrieval
3. QR scanning simulation
4. Recommendations
5. Performance validation

**Rapport de test:**
```
Total Tests: 5
Passed: 5
Failed: 0
Success Rate: 100%
```

## Patterns de Conception

### 1. Singleton Pattern
**Utilisé par:** Tous les managers

```csharp
public class Manager : MonoBehaviour {
    private static Manager _instance;
    public static Manager Instance {
        get {
            if (_instance == null) {
                GameObject go = new GameObject("Manager");
                _instance = go.AddComponent<Manager>();
                DontDestroyOnLoad(go);
            }
            return _instance;
        }
    }
}
```

**Avantages:**
- Accès global facile
- Une seule instance garantie
- Persistent entre scènes

### 2. Observer Pattern (Events)
**Utilisé par:** QRScanner, ARSessionManager

```csharp
public System.Action<ProductData> OnProductFound;
public System.Action OnScanFailed;

// Usage
OnProductFound?.Invoke(product);
```

**Avantages:**
- Couplage faible
- Extensibilité
- Réactivité

### 3. Repository Pattern
**Utilisé par:** ProductDatabaseManager

```csharp
public interface IProductRepository {
    ProductData GetById(string id);
    List<ProductData> GetAll();
    List<ProductData> Search(string query);
}
```

**Avantages:**
- Abstraction de la source de données
- Testabilité
- Changement de source facilité

### 4. Strategy Pattern
**Utilisé par:** RecommendationEngine

```csharp
interface IRecommendationStrategy {
    List<ProductData> GetRecommendations(ProductData current);
}

class EcoStrategy : IRecommendationStrategy { ... }
class HealthStrategy : IRecommendationStrategy { ... }
class BudgetStrategy : IRecommendationStrategy { ... }
```

## Flux de Données

### Flux 1: Scanner un Produit

```
1. User clicks "Scan"
   ↓
2. QRScanner.StartScanning()
   ↓
3. ARCameraManager captures frame
   ↓
4. ZXing decodes QR code → "PROD001"
   ↓
5. ProductDatabaseManager.GetProductByQRCode("PROD001")
   ↓
6. Dictionary lookup → ProductData
   ↓
7. OnProductFound?.Invoke(product)
   ↓
8. UI displays product info
   ↓
9. AnalyticsManager.TrackProductScan()
```

### Flux 2: Afficher en AR

```
1. User clicks "View in AR"
   ↓
2. NavigationManager.ShowARView(product)
   ↓
3. Load ARScene
   ↓
4. ARSessionManager.StartARSession()
   ↓
5. Wait for plane detection
   ↓
6. User taps screen → Raycast
   ↓
7. Instantiate AR overlay at hit point
   ↓
8. ARProductOverlay.DisplayProduct(product)
   ↓
9. Overlay follows tracked surface
   ↓
10. AnalyticsManager.TrackARView()
```

### Flux 3: Obtenir Recommandations

```
1. User clicks "Alternatives"
   ↓
2. NavigationManager.ShowRecommendations(product)
   ↓
3. Load RecommendationsScene
   ↓
4. RecommendationEngine.GetRecommendations(product)
   ↓
5. Calculate scores for all similar products
   ↓
6. Sort by score descending
   ↓
7. Return top 10 products
   ↓
8. UI creates ProductCard for each
   ↓
9. Display in scroll view
   ↓
10. User applies filters → repeat from step 4
```

## Sécurité et Performance

### Sécurité

**Données:**
- Aucune donnée personnelle collectée
- Base de données locale (pas de serveur)
- Analytics stockées localement uniquement
- Pas d'accès réseau requis

**Permissions:**
- Caméra (obligatoire pour AR)
- Stockage (optionnel pour export analytics)

### Performance

**Optimisations:**

1. **Base de données:**
   - Cache Dictionary O(1)
   - Chargement unique au démarrage
   - Pas de réflexion ou parsing en runtime

2. **AR:**
   - Réduction résolution caméra (division par 2)
   - Scan interval configurable (500ms)
   - Désactivation features non utilisées

3. **UI:**
   - Object pooling pour ProductCards
   - Lazy loading des images
   - Canvas optimisé (batch)

4. **Mémoire:**
   - Garbage collection périodique
   - UnloadUnusedAssets sur changement scène
   - Texture compression

**Targets:**
- FPS: ≥ 30 (mobile)
- Memory: ≤ 500 MB
- Load time: ≤ 2 seconds
- Scan latency: ≤ 1 second

## Technologies

### Unity & Packages

| Package | Version | Usage |
|---------|---------|-------|
| Unity | 2022.3.62f3 | Moteur principal |
| AR Foundation | 5.1.5 | Framework AR |
| AR Core | 5.1.5 | Android AR |
| AR Kit | 5.1.5 | iOS AR |
| TextMeshPro | 3.0.6 | Rendu texte |
| Newtonsoft.Json | 3.2.1 | Parsing JSON |

### Librairies Externes

- **ZXing.Net:** Décodage QR/codes-barres
- **qrcode (Python):** Génération QR codes

### Plateformes

- **Android:** API 24+ (Android 7.0+)
- **iOS:** 11.0+
- **Unity Editor:** Mode simulation

## Déploiement

### Build Pipeline

```
1. Configure platform (Android/iOS)
   ↓
2. Set player settings
   ↓
3. Set build number/version
   ↓
4. Build APK/IPA
   ↓
5. Sign (release only)
   ↓
6. Test on device
   ↓
7. Deploy to store
```

### Build Settings

**Android:**
```
- Scripting Backend: IL2CPP
- Target Architecture: ARM64
- API Level: 24 (minimum), 34 (target)
- Compression: LZ4
- Split APK: No
```

**iOS:**
```
- Architecture: ARM64
- iOS Version: 11.0 minimum
- Bitcode: Disabled
- Strip Engine Code: Yes (release)
```

## Évolution Future

### Phase 2 (Prévue)

- [ ] API externe (OpenFoodFacts)
- [ ] Cloud database (Firebase)
- [ ] Authentification utilisateur
- [ ] Synchronisation multi-device
- [ ] Mode hors ligne amélioré

### Phase 3 (Potentielle)

- [ ] AI/ML pour reconnaissance image
- [ ] Tracking sans marqueur
- [ ] Partage social
- [ ] Mode multi-joueur
- [ ] Intégration e-commerce

## Maintenance

### Logs

**Levels:**
- Debug: Développement uniquement
- Info: Informations générales
- Warning: Problèmes non bloquants
- Error: Erreurs bloquantes

**Stockage:**
- Unity Console (Editor)
- Logcat (Android)
- Xcode Console (iOS)
- Application.persistentDataPath/logs.txt

### Mise à jour

**Version Numbering:**
- Format: MAJOR.MINOR.PATCH
- Exemple: 1.0.0 → 1.1.0 (nouvelle feature)
- Exemple: 1.1.0 → 1.1.1 (bugfix)

**Processus:**
1. Branch feature/bugfix
2. Développement + tests
3. Code review
4. Merge to develop
5. Test intégration
6. Merge to main
7. Tag version
8. Build + deploy

## Documentation

### Pour Développeurs

- README.md: Vue d'ensemble et setup
- INTEGRATION_GUIDE.md: Guide détaillé
- ARCHITECTURE.md: Ce document
- Code comments: Inline documentation

### Pour Utilisateurs

- In-app tutorial
- FAQ (à créer)
- Video tutorials (à créer)
- Support email

---

**Document maintenu par:** Équipe de développement  
**Dernière mise à jour:** Décembre 2024  
**Version:** 1.0.0
