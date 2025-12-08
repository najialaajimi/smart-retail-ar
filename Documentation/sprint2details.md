# Sprint 2 : Intégration AR avec Superposition des Données

## Vue d'ensemble
Sprint 2 implémente la reconnaissance d'image en temps réel et la superposition AR des informations produits en utilisant AR Foundation. Ce sprint transforme l'application en une véritable expérience de réalité augmentée.

## Objectifs du Sprint
- ✅ Implémenter la reconnaissance d'image en temps réel
- ✅ Créer le système de superposition AR des informations
- ✅ Intégrer AR Foundation avec les interfaces existantes
- ✅ Optimiser les performances AR pour mobile
- ✅ Gérer la session AR et le cycle de vie

## Architecture AR

### Structure des Scripts AR
```
Assets/Scripts/AR/
├── ARImageTracker.cs          - Reconnaissance d'image temps réel
├── ARDataOverlay.cs           - Superposition données AR
├── ARSessionManager.cs        - Gestion session AR
├── ARPlacementController.cs   - Placement objets 3D (future)
├── ARUIManager.cs             - Interface AR (future)
└── ARPerformanceOptimizer.cs  - Optimisation (future)
```

### Assets AR
```
Assets/AR/
├── ReferenceImages/           - Images de référence produits
├── ARMaterials/              - Matériaux AR
├── ARPrefabs/                - Prefabs UI AR
└── TrackedImages/            - Base d'images trackées
```

## Composants AR Principaux

### 1. ARImageTracker

**Responsabilités:**
- Détection d'images de référence en temps réel
- Suivi des produits détectés
- Gestion des événements de tracking
- Application du cooldown de détection

**Configuration AR Foundation:**
```csharp
[RequireComponent(typeof(ARTrackedImageManager))]
public class ARImageTracker : MonoBehaviour
{
    [SerializeField] private ARTrackedImageManager trackedImageManager;
    [SerializeField] private XRReferenceImageLibrary referenceImageLibrary;
    [SerializeField] private float detectionConfidenceThreshold = 0.7f;
}
```

**Événements:**
- `OnProductDetected(string productId)` - Produit détecté
- `OnProductLost(string productId)` - Tracking perdu

**Fonctionnalités clés:**
- Tracking state validation
- Cooldown de détection (1 seconde)
- Extraction automatique du Product ID depuis le nom d'image
- Dictionnaire de suivi des images actives

**Format de nommage des images:**
- `ProductImage_PROD001` - Format standard
- `PROD001` - Format simplifié
- Extraction automatique du ID après underscore

### 2. ARDataOverlay

**Responsabilités:**
- Affichage des informations en overlay AR
- Ancrage des UI elements aux produits
- Animations d'apparition/disparition
- Mise à jour dynamique des positions

**Configuration:**
```csharp
[Header("Display Settings")]
[SerializeField] private float displayDistance = 0.3f;
[SerializeField] private Vector3 displayOffset = new Vector3(0, 0.2f, 0);
[SerializeField] private float fadeInDuration = 0.5f;
[SerializeField] private float fadeOutDuration = 0.3f;
```

**Contenu affiché:**
- Nom et marque du produit
- Prix
- Nutri-Score et informations nutritionnelles
- Éco-Score et origine
- Recommandations contextuelles (optionnel)

**Fonctionnalités:**
- `DisplayProductOverlay(productId, position, rotation)` - Afficher overlay
- `HideProductOverlay(productId)` - Masquer overlay
- `UpdateOverlayPosition(productId, position, rotation)` - MAJ position
- `ClearAllOverlays()` - Nettoyer tous les overlays

**Système d'overlay:**
- Canvas World Space pour affichage 3D
- CanvasScaler pour scaling adaptatif
- VerticalLayoutGroup pour organisation automatique
- LeanTween pour animations fluides (si disponible)

### 3. ARSessionManager

**Responsabilités:**
- Gestion du cycle de vie de la session AR
- Initialisation et configuration AR Foundation
- Gestion des états AR
- Contrôle de la caméra AR

**États AR gérés:**
- `None` - Aucune session
- `Unsupported` - AR non supporté
- `CheckingAvailability` - Vérification disponibilité
- `NeedsInstall` - Installation requise
- `Ready` - Session prête
- `SessionTracking` - Tracking actif

**Événements:**
- `OnSessionInitialized` - Session initialisée
- `OnSessionStarted` - Session démarrée
- `OnSessionStopped` - Session arrêtée
- `OnSessionStateChanged` - Changement d'état

**Méthodes principales:**
- `StartSession()` - Démarrer la session
- `StopSession()` - Arrêter la session
- `PauseSession()` - Mettre en pause
- `ResumeSession()` - Reprendre
- `ResetSession()` - Réinitialiser

**Gestion du cycle de vie:**
```csharp
private void OnApplicationPause(bool pauseStatus)
{
    if (pauseStatus)
        PauseSession();
    else
        ResumeSession();
}
```

## Configuration AR Foundation

### Packages Requis
```json
{
  "com.unity.ar.foundation": "5.1.5",
  "com.unity.xr.arcore": "5.1.5",     // Android
  "com.unity.xr.arkit": "5.1.5"       // iOS
}
```

### Composants AR Foundation Nécessaires
- **ARSession** - Gère la session AR globale
- **ARSessionOrigin** - Point d'origine du monde AR
- **ARCameraManager** - Gestion de la caméra AR
- **ARTrackedImageManager** - Reconnaissance d'images
- **XRReferenceImageLibrary** - Bibliothèque d'images de référence

### Configuration de la Scène AR

#### Hiérarchie recommandée:
```
ARScannerScene
├── AR Session Origin
│   ├── AR Camera
│   └── AR Session
├── ARImageTracker (script)
├── ARSessionManager (script)
├── ARDataOverlay (script)
└── UI
    ├── Canvas
    ├── Instructions Text
    └── Back Button
```

## Bibliothèque d'Images de Référence

### Création XRReferenceImageLibrary

1. **Créer la bibliothèque:**
   - Assets > Create > XR > Reference Image Library

2. **Ajouter des images:**
   - Résolution recommandée: 1024x1024 ou plus
   - Format: PNG, JPG
   - Images claires et contrastées
   - Éviter les surfaces uniformes

3. **Configuration par image:**
   - Name: `ProductImage_PROD001`
   - Size: Taille physique réelle en mètres
   - Keep Texture: Activé pour visualisation

### Meilleures Pratiques Images

**Caractéristiques des bonnes images:**
- Haute résolution (≥ 1024x1024)
- Contraste élevé
- Détails distincts et textures
- Pas de surfaces uniformes
- Bon éclairage sans reflets

**À éviter:**
- Images floues ou pixelisées
- Surfaces brillantes ou réfléchissantes
- Patterns répétitifs simples
- Couleurs très uniformes

## Intégration avec Sprint 1

### Modifications ScannerController

Le `ScannerController` s'intègre avec les composants AR:

```csharp
public class ScannerController : MonoBehaviour
{
    private ARImageTracker imageTracker;
    
    private void Start()
    {
        imageTracker = FindObjectOfType<ARImageTracker>();
        if (imageTracker != null)
        {
            imageTracker.OnProductDetected += OnProductDetected;
        }
    }
    
    public void OnProductDetected(string productId)
    {
        // Navigation vers ProductInfo
        ProductData product = ProductDatabase.Instance.GetProductById(productId);
        if (product != null)
        {
            PlayerPrefs.SetString("CurrentProductId", productId);
            Utils.NavigationManager.Instance.LoadScene("ProductInfoScene");
        }
    }
}
```

### Flux de Détection Complet

1. **Utilisateur pointe caméra vers produit**
2. **ARImageTracker détecte l'image de référence**
3. **Événement OnProductDetected déclenché**
4. **ARDataOverlay affiche les informations**
5. **ScannerController gère la navigation**
6. **ProductInfoController affiche détails complets**

## Optimisation des Performances

### Cibles de Performance
- **FPS:** ≥ 30 FPS constant
- **Latence détection:** ≤ 1 seconde
- **Utilisation mémoire:** ≤ 512 MB
- **Batterie:** Optimisée pour utilisation prolongée

### Stratégies d'Optimisation

#### 1. Limite du nombre d'images trackées
```csharp
trackedImageManager.maxNumberOfMovingImages = 3;
```

#### 2. Désactivation du tracking quand inutile
```csharp
public void SetTrackingEnabled(bool enabled)
{
    if (trackedImageManager != null)
        trackedImageManager.enabled = enabled;
}
```

#### 3. Cooldown de détection
```csharp
private const float DETECTION_COOLDOWN = 1.0f;
private Dictionary<string, float> lastDetectionTime;
```

#### 4. Réutilisation des overlays
```csharp
// Réutiliser overlay existant au lieu de créer un nouveau
if (activeOverlays.ContainsKey(productId))
{
    UpdateOverlayPosition(productId, position, rotation);
    return;
}
```

#### 5. Object Pooling pour overlays
- Pré-instancier des overlays
- Réutiliser au lieu de détruire/recréer
- Réduire la charge du garbage collector

### Monitoring Performance

Intégration avec PerformanceMonitor:
```csharp
PerformanceMonitor.Instance.GetCurrentFPS();
AnalyticsManager.Instance.TrackARPerformance(fps, latency);
```

## Tests AR

### Tests en Environnement Contrôlé

1. **Test des images de référence:**
   - Vérifier reconnaissance à différentes distances
   - Tester différents angles (0°, 30°, 45°)
   - Tester différents éclairages

2. **Test de stabilité:**
   - Tracking stable pendant mouvement caméra
   - Récupération après occlusion
   - Performance avec multiple images

3. **Test de performance:**
   - FPS pendant tracking actif
   - Latence de détection
   - Utilisation mémoire

### Cas de Test

| Test Case | Description | Résultat Attendu |
|-----------|-------------|------------------|
| TC-AR-001 | Détection image frontale | Détection ≤ 1s |
| TC-AR-002 | Détection angle 30° | Détection ≤ 1.5s |
| TC-AR-003 | Tracking pendant mouvement | Stable, pas de jitter |
| TC-AR-004 | Récupération après occlusion | Reprend tracking ≤ 2s |
| TC-AR-005 | Performance multiple images | FPS ≥ 30 |

### Mode Test Sans AR

Pour développement sans appareil AR:
```csharp
[SerializeField] private bool testMode = false;

if (testMode)
{
    // Simuler détection après délai
    Invoke(nameof(SimulateScan), 2f);
}
```

## Plateforme Cible

### Android (ARCore)
- Minimum: Android 7.0 (API level 24)
- Appareils supportant ARCore
- Configuration: Player Settings > XR Plug-in Management > ARCore

### iOS (ARKit)
- Minimum: iOS 11.0
- iPhone 6S et plus récents
- Configuration: Player Settings > XR Plug-in Management > ARKit

### Build Settings

**Android:**
```
Minimum API Level: 24
Target API Level: 33
Graphics API: OpenGLES3 / Vulkan
Architecture: ARM64
```

**iOS:**
```
Minimum iOS Version: 11.0
Target SDK: Device SDK
Architecture: ARM64
Camera Usage Description: Required for AR
```

## Dépannage

### Problèmes Courants

**Problème:** Images non détectées
- **Solution:** Vérifier qualité images, éclairage, distance

**Problème:** Tracking instable
- **Solution:** Améliorer features images, réduire mouvement caméra

**Problème:** Performance faible
- **Solution:** Réduire nombre images trackées, optimiser overlays

**Problème:** AR non disponible
- **Solution:** Vérifier compatibilité appareil, installer ARCore/ARKit

## Prochaines Étapes (Sprint 3)

Le Sprint 3 ajoutera:
- Moteur de recommandations avancé
- Scoring multi-critères
- Personnalisation basée sur ML
- Comparaison de produits

## Références

### Documentation AR Foundation
- [AR Foundation Manual](https://docs.unity3d.com/Packages/com.unity.xr.arfoundation@5.1/manual/index.html)
- [ARCore Documentation](https://developers.google.com/ar)
- [ARKit Documentation](https://developer.apple.com/documentation/arkit)

### Best Practices AR
- Optimiser images de référence pour tracking rapide
- Limiter nombre d'images trackées simultanément
- Gérer correctement le cycle de vie AR
- Tester sur vrais appareils dès que possible
