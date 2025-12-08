# Smart Retail AR - Magasin Augmenté

![Unity Version](https://img.shields.io/badge/Unity-2022.3.62f3-blue)
![AR Foundation](https://img.shields.io/badge/AR%20Foundation-5.1.5-green)
![License](https://img.shields.io/badge/License-MIT-yellow)

## 📱 Vue d'ensemble

Smart Retail AR est une application de réalité augmentée innovante conçue pour améliorer l'expérience d'achat en magasin. L'application permet aux utilisateurs de scanner des produits avec leur smartphone et d'afficher instantanément des informations augmentées incluant les données nutritionnelles, l'origine, les scores écologiques et des recommandations de produits alternatifs.

### 🎯 Objectifs Pédagogiques

- Scanner un produit avec la caméra du smartphone
- Afficher des informations augmentées en réalité augmentée (AR)
- Proposer des recommandations alternatives (produits bio, écoresponsables)
- Suivre et analyser les performances de l'application

## 🏗️ Architecture

### Composants Principaux

```
Smart Retail AR
├── Capteurs
│   ├── Caméra AR (ARCore/ARKit)
│   ├── Scanner QR Code
│   └── Détection d'images
├── Services
│   ├── AR Foundation (session, tracking, overlay)
│   ├── Base de données produits (JSON local)
│   └── Moteur de recommandations
├── Application
│   ├── Gestion des produits
│   ├── Interface utilisateur AR
│   ├── Système de navigation
│   └── Analytics et performance
└── UI/UX
    ├── Scanner QR
    ├── Affichage produit
    ├── Overlay AR
    └── Recommandations
```

## 📋 Table des Matières

- [Prérequis](#prérequis)
- [Installation](#installation)
- [Structure du Projet](#structure-du-projet)
- [Sprints et Fonctionnalités](#sprints-et-fonctionnalités)
- [Guide d'Intégration](#guide-dintégration)
- [Base de Données Produits](#base-de-données-produits)
- [Génération de QR Codes](#génération-de-qr-codes)
- [Tests](#tests)
- [KPIs et Performance](#kpis-et-performance)
- [Déploiement](#déploiement)

## 🔧 Prérequis

### Logiciels Requis

- **Unity 2022.3.62f3 LTS** ou version ultérieure
- **Visual Studio 2019/2022** ou **VS Code** avec extension C#
- **Android Studio** (pour déploiement Android)
- **Xcode** (pour déploiement iOS, macOS uniquement)
- **Git** pour le contrôle de version

### Packages Unity Requis

Les packages suivants sont inclus dans le projet:

- AR Foundation 5.1.5
- AR Core XR Plugin 5.1.5
- AR Kit XR Plugin 5.1.5
- TextMeshPro 3.0.6
- Newtonsoft Json 3.2.1

### Plateformes Supportées

- **Android**: API Level 24 (Android 7.0) ou supérieur
- **iOS**: iOS 11.0 ou supérieur
- Dispositifs avec support ARCore/ARKit

## 📥 Installation

### Étape 1: Cloner le Répertoire

```bash
git clone https://github.com/najialaajimi/smart-retail-ar.git
cd smart-retail-ar
git checkout smart-retail-ar-complet-final
```

### Étape 2: Ouvrir dans Unity

1. Lancez **Unity Hub**
2. Cliquez sur **Add** et sélectionnez le dossier du projet
3. Assurez-vous que Unity **2022.3.62f3 LTS** est installé
4. Ouvrez le projet

### Étape 3: Importer les Packages

Unity devrait automatiquement importer tous les packages listés dans `Packages/manifest.json`. Si ce n'est pas le cas:

1. Ouvrez **Window > Package Manager**
2. Vérifiez que tous les packages AR Foundation sont installés
3. Si manquants, installez-les depuis le Package Manager

### Étape 4: Configuration de la Build

#### Pour Android:

1. **File > Build Settings**
2. Sélectionnez **Android**
3. Cliquez sur **Switch Platform**
4. **Player Settings**:
   - Minimum API Level: Android 7.0 (API 24)
   - Scripting Backend: IL2CPP
   - Target Architectures: ARM64
   - Package Name: com.smartretail.ar

#### Pour iOS:

1. **File > Build Settings**
2. Sélectionnez **iOS**
3. Cliquez sur **Switch Platform**
4. **Player Settings**:
   - Minimum iOS Version: 11.0
   - Camera Usage Description: "Nécessaire pour scanner les produits en AR"
   - Architecture: ARM64

## 📁 Structure du Projet

```
Assets/
├── Scenes/                          # Scènes Unity
│   ├── HomeScene.unity             # Écran d'accueil
│   ├── ScannerScene.unity          # Scanner QR
│   ├── ProductInfoScene.unity      # Détails produit
│   ├── ARScene.unity               # Vue AR
│   └── RecommendationsScene.unity  # Recommandations
│
├── Scripts/                         # Scripts C#
│   ├── Sprint1/                    # QR Scanner & Base
│   │   ├── Core/                   # Logique métier
│   │   │   └── QRScanner.cs
│   │   ├── Data/                   # Modèles de données
│   │   │   ├── ProductData.cs
│   │   │   └── ProductDatabaseManager.cs
│   │   ├── UI/                     # Interfaces utilisateur
│   │   │   ├── QRScannerUI.cs
│   │   │   └── ProductInfoUI.cs
│   │   └── Utils/                  # Utilitaires
│   │       └── NavigationManager.cs
│   │
│   ├── Sprint2/                    # Intégration AR
│   │   ├── AR/
│   │   │   └── ARSessionManager.cs
│   │   └── UI/
│   │       └── ARProductOverlay.cs
│   │
│   ├── Sprint3/                    # Recommandations
│   │   ├── Recommendations/
│   │   │   └── RecommendationEngine.cs
│   │   └── UI/
│   │       ├── RecommendationsUI.cs
│   │       └── ProductCard.cs
│   │
│   └── Sprint4/                    # Tests & Performance
│       ├── Analytics/
│       │   └── AnalyticsManager.cs
│       ├── Performance/
│       │   └── PerformanceMonitor.cs
│       └── Testing/
│           └── TestManager.cs
│
├── Resources/                       # Ressources chargées dynamiquement
│   ├── Data/
│   │   └── products_database.json  # Base de données produits
│   ├── Images/
│   │   └── Products/               # Images produits
│   └── QRCodes/                    # QR codes générés
│
├── Prefabs/                        # Prefabs Unity
│   ├── UI/
│   │   ├── ProductCard.prefab
│   │   ├── AROverlay.prefab
│   │   └── CertificationBadge.prefab
│   └── AR/
│       └── ARProductInfo.prefab
│
└── StreamingAssets/                # Assets accessibles en runtime
```

## 🚀 Sprints et Fonctionnalités

### Sprint 1: Scanner QR Code & Informations Basiques

**Objectif**: Créer un scanner QR fonctionnel et afficher les informations produit de base.

#### Fonctionnalités Implémentées:

✅ **QRScanner.cs**
- Scanner QR code avec la caméra
- Mode test pour le développement sans matériel AR
- Support des codes-barres (EAN-13, EAN-8, CODE-128)
- Détection automatique avec intervalle configurable

✅ **ProductDatabaseManager.cs**
- Chargement de la base de données JSON
- Recherche par ID, QR code, code-barre
- Filtrage par catégorie et tags
- Cache en mémoire pour performances optimales

✅ **QRScannerUI.cs & ProductInfoUI.cs**
- Interface de scan avec overlay visuel
- Affichage des informations produit
- Scores visuels (éco, santé, qualité)
- Tags et certifications

#### Utilisation:

```csharp
// Démarrer le scan
QRScanner scanner = GetComponent<QRScanner>();
scanner.OnProductFound += (product) => {
    Debug.Log($"Produit trouvé: {product.name}");
};
scanner.StartScanning();

// Récupérer un produit
var product = ProductDatabaseManager.Instance.GetProductById("PROD001");
```

### Sprint 2: Intégration AR & Overlay de Données

**Objectif**: Intégrer AR Foundation et superposer les informations sur les produits en temps réel.

#### Fonctionnalités Implémentées:

✅ **ARSessionManager.cs**
- Gestion de la session AR (ARCore/ARKit)
- Détection de plans
- Tracking d'images
- Raycasting pour placement d'objets

✅ **ARProductOverlay.cs**
- Overlay AR suivant le produit scanné
- Affichage nutritionnel en AR
- Scores visuels (barres de progression)
- Panneaux informatifs interactifs
- Billboard effect (toujours face à la caméra)

#### Utilisation:

```csharp
// Démarrer session AR
ARSessionManager.Instance.OnARSessionStarted += () => {
    Debug.Log("AR session prête");
};

// Afficher overlay sur produit
ARProductOverlay overlay = GetComponent<ARProductOverlay>();
overlay.DisplayProduct(product, trackedImageTransform);
```

### Sprint 3: Moteur de Recommandations & Filtres

**Objectif**: Proposer des alternatives intelligentes basées sur les préférences utilisateur.

#### Fonctionnalités Implémentées:

✅ **RecommendationEngine.cs**
- Algorithme de scoring multi-critères
- Recommandations basées sur:
  - Catégorie similaire
  - Amélioration scores (éco, santé)
  - Préférences utilisateur (bio, local, vegan)
  - Fourchette de prix
- Filtres avancés configurables

✅ **RecommendationsUI.cs**
- Interface de recommandations avec filtres
- Onglets thématiques (Tous, Éco, Santé, Budget)
- Sliders pour critères numériques
- Toggles pour préférences

✅ **ProductCard.cs**
- Carte produit réutilisable
- Affichage compact des informations clés
- Indicateurs visuels de scores
- Navigation vers détails

#### Utilisation:

```csharp
// Obtenir recommandations
var recommendations = RecommendationEngine.Instance
    .GetRecommendations(currentProduct, maxResults: 10);

// Appliquer des critères
var criteria = new RecommendationEngine.RecommendationCriteria {
    preferBio = true,
    preferLocal = true,
    maxPrice = 15.99f,
    minEcoScore = 70f
};
RecommendationEngine.Instance.SetCriteria(criteria);
```

### Sprint 4: Tests, Analytics & Performance

**Objectif**: Valider les fonctionnalités, suivre les KPIs et optimiser les performances.

#### Fonctionnalités Implémentées:

✅ **AnalyticsManager.cs**
- Suivi des scans (succès/échecs)
- Mesure de la latence
- Comptage des vues (produits, alternatives, AR)
- Calcul du taux de reconnaissance
- Export des données analytics

✅ **PerformanceMonitor.cs**
- Monitoring FPS en temps réel
- Suivi de l'utilisation mémoire
- État de la batterie
- Optimisation automatique de la qualité
- Interface de débogage

✅ **TestManager.cs**
- Suite de tests automatisés
- Tests unitaires pour chaque module
- Validation des KPIs
- Rapport de tests détaillé

#### KPIs Cibles:

| KPI | Cible | Status |
|-----|-------|--------|
| Reconnaissance produit | ≥ 95% | ✅ Implémenté |
| Latence d'affichage | ≤ 1 seconde | ✅ Implémenté |
| Satisfaction utilisateur | ≥ 80% | ✅ Implémenté |

## 📚 Guide d'Intégration Détaillé

### Étape 1: Configuration de la Base de Données

#### 1.1 Structure de la Base de Données

La base de données produits est au format JSON et contient 50 produits avec toutes leurs informations:

```json
{
  "products": [
    {
      "id": "PROD001",
      "name": "Organic Dairy Product 1",
      "brand": "BioMarket",
      "category": "Dairy",
      "origin": "France",
      "price": 12.99,
      "barcode": "7000000000001",
      "qrCode": "PROD001",
      "nutritionalInfo": {
        "servingSize": 100,
        "calories": 150,
        "protein": 8.5,
        ...
      },
      "ecoScore": 85.5,
      "healthScore": 78.3,
      "qualityScore": 92.1,
      "certifications": ["EU Organic", "Fair Trade"],
      "isBio": true,
      "isLocal": true,
      "isVegan": false,
      "alternativeIds": ["PROD002", "PROD005"],
      ...
    }
  ]
}
```

#### 1.2 Ajouter de Nouveaux Produits

Pour ajouter des produits:

1. Ouvrez `Assets/Resources/Data/products_database.json`
2. Ajoutez un nouvel objet dans le tableau `products`
3. Respectez la structure existante
4. Générez un QR code unique pour le produit

### Étape 2: Génération de QR Codes

#### 2.1 Génération Automatique (Recommandé)

Utilisez le script Python fourni:

```python
# Dans le répertoire du projet
python3 generate_qrcodes.py
```

Ce script:
- Lit la base de données produits
- Génère un QR code pour chaque produit
- Sauvegarde dans `Assets/Resources/QRCodes/`
- Crée un fichier PDF imprimable

#### 2.2 Génération Manuelle

Si vous préférez générer manuellement:

```python
import qrcode

# Pour chaque produit
product_id = "PROD001"
qr = qrcode.QRCode(version=1, box_size=10, border=5)
qr.add_data(product_id)
qr.make(fit=True)

img = qr.make_image(fill_color="black", back_color="white")
img.save(f"Assets/Resources/QRCodes/{product_id}.png")
```

#### 2.3 Impression des QR Codes

Les QR codes générés peuvent être:
- Imprimés et apposés sur les produits physiques
- Affichés sur écran pour tests
- Intégrés dans des étiquettes de rayons

**Recommandations d'impression:**
- Taille minimale: 3x3 cm
- Résolution: 300 DPI
- Contraste élevé (noir sur blanc)
- Espace blanc autour du code (quiet zone)

### Étape 3: Ajout d'Images Produits

#### 3.1 Format des Images

Placez les images produits dans `Assets/Resources/Images/Products/`:

- Format: JPG ou PNG
- Résolution recommandée: 512x512 px
- Ratio: 1:1 (carré)
- Taille max: 500KB par image
- Nommage: `prod001.jpg`, `prod002.jpg`, etc.

#### 3.2 Optimisation

Pour optimiser les images:

1. Dans Unity, sélectionnez l'image
2. Inspector > Texture Type: Sprite (2D and UI)
3. Max Size: 512 ou 1024
4. Compression: High Quality
5. Appliquer

### Étape 4: Configuration des Scènes

#### 4.1 Scène Scanner (ScannerScene)

Hiérarchie:
```
ScannerScene
├── AR Session Origin
│   ├── AR Camera
│   └── AR Camera Manager
├── QR Scanner (avec QRScanner.cs)
├── Canvas
│   ├── Scanner Overlay
│   ├── Status Text
│   └── Result Panel
└── Managers
    ├── ProductDatabaseManager
    └── NavigationManager
```

#### 4.2 Scène AR (ARScene)

Hiérarchie:
```
ARScene
├── AR Session
├── AR Session Origin
│   ├── AR Camera
│   ├── AR Plane Manager
│   └── AR Raycast Manager
├── AR Product Overlay (Prefab)
└── UI Canvas
    └── Control Buttons
```

#### 4.3 Configuration AR Foundation

1. **AR Session Origin**:
   - Tracking Origin Mode: Device
   - Camera Offset: (0, 0, 0)

2. **AR Plane Manager**:
   - Detection Mode: Horizontal
   - Plane Prefab: (Optionnel)

3. **AR Tracked Image Manager**:
   - Reference Image Library: (À créer)
   - Max Number Of Moving Images: 3

### Étape 5: Build et Déploiement

#### 5.1 Build Android

```bash
# Prérequis
# - Android SDK installé
# - Java JDK configuré

# Dans Unity:
# 1. File > Build Settings
# 2. Android > Switch Platform
# 3. Player Settings:
#    - API Level: 24+
#    - Scripting Backend: IL2CPP
#    - ARM64: ✓
# 4. Build

# Installation sur device
adb install SmartRetailAR.apk
```

#### 5.2 Build iOS

```bash
# Prérequis (macOS uniquement)
# - Xcode installé
# - Apple Developer Account

# Dans Unity:
# 1. File > Build Settings
# 2. iOS > Switch Platform
# 3. Player Settings:
#    - iOS Version: 11.0+
#    - Camera Usage Description rempli
# 4. Build

# Ouvrir le projet Xcode généré et build
```

## 🧪 Tests

### Tests Automatisés

Le système de tests est intégré dans `TestManager.cs`:

```csharp
// Lancer tous les tests
TestManager.Instance.RunTest("all");

// Test spécifique
TestManager.Instance.RunTest("scanning");
TestManager.Instance.RunTest("recommendations");
TestManager.Instance.RunTest("performance");
```

### Tests Manuels Recommandés

#### Test 1: Scan QR Code
1. Lancer l'application
2. Naviguer vers Scanner
3. Scanner un QR code produit
4. Vérifier affichage des informations
5. **Résultat attendu**: Latence < 1s

#### Test 2: AR Overlay
1. Scanner un produit
2. Passer en mode AR
3. Pointer vers une surface
4. Vérifier overlay informations
5. **Résultat attendu**: Overlay stable et lisible

#### Test 3: Recommandations
1. Afficher détails d'un produit
2. Cliquer sur "Voir alternatives"
3. Appliquer des filtres (Bio, Local)
4. Vérifier pertinence des résultats
5. **Résultat attendu**: ≥5 alternatives pertinentes

#### Test 4: Performance
1. Scanner 10 produits consécutifs
2. Vérifier Analytics
3. Contrôler FPS (≥30)
4. Vérifier mémoire (stable)
5. **Résultat attendu**: KPIs dans les cibles

### Tests en Conditions Réelles

**Environnement de test:**
- Éclairage: Normal, faible, fort
- Angles: Face, 30°, 45°, 60°
- Distance: 10cm à 50cm
- Surface: Produits réels, écran

**Critères de validation:**
- Taux de reconnaissance ≥ 95%
- Temps de scan ≤ 1s
- Stabilité AR (pas de jitter)
- Lisibilité UI (tous éclairages)

## 📊 KPIs et Performance

### Métriques Suivies

#### 1. Reconnaissance Produit
```
Taux = (Scans Réussis / Total Scans) × 100
Cible: ≥ 95%
```

#### 2. Latence
```
Latence Moyenne = Σ(Temps de Scan) / Nombre de Scans
Cible: ≤ 1 seconde
```

#### 3. Satisfaction Utilisateur
```
Score = (Avis Positifs / Total Avis) × 100
Cible: ≥ 80%
```

### Visualisation des KPIs

Les KPIs sont affichés dans:
- Console de débogage (temps réel)
- Fichier analytics.json (exporté)
- UI Performance Monitor (optionnel)

### Optimisation des Performances

#### Recommandations:

1. **Réduction de la qualité graphique** si FPS < 30
2. **Garbage Collection** si mémoire > 500MB
3. **Désactivation features** selon device
4. **Mise en cache** des textures fréquentes
5. **Limitation distance** de détection AR

## 🔒 Sécurité et Confidentialité

- Aucune donnée personnelle collectée
- Analytics stockées localement
- Pas de connexion serveur requise
- Code open-source et auditable

## 🤝 Contribution

Les contributions sont bienvenues! Pour contribuer:

1. Fork le projet
2. Créer une branche (`git checkout -b feature/AmazingFeature`)
3. Commit les changements (`git commit -m 'Add AmazingFeature'`)
4. Push vers la branche (`git push origin feature/AmazingFeature`)
5. Ouvrir une Pull Request

## 📄 Licence

Distribué sous licence MIT. Voir `LICENSE` pour plus d'informations.

## 👥 Équipe

Développé dans le cadre d'un projet pédagogique sur la réalité augmentée appliquée au retail.

## 📞 Support

Pour toute question ou problème:
- Ouvrir une issue sur GitHub
- Consulter la documentation Unity AR Foundation
- Vérifier les logs Unity pour erreurs

## 🗺️ Roadmap Future

- [ ] Intégration API produits externes (OpenFoodFacts)
- [ ] Support multi-langues
- [ ] Mode hors ligne amélioré
- [ ] Historique des scans
- [ ] Partage social
- [ ] Mode liste de courses
- [ ] Comparaison prix magasins
- [ ] Recettes basées sur produits scannés

---

**Version:** 1.0.0  
**Date:** Décembre 2024  
**Unity:** 2022.3.62f3 LTS  
**AR Foundation:** 5.1.5

---

## 🎓 Ressources d'Apprentissage

### Documentation Officielle
- [Unity AR Foundation](https://docs.unity3d.com/Packages/com.unity.xr.arfoundation@5.1/manual/index.html)
- [ARCore SDK](https://developers.google.com/ar)
- [ARKit SDK](https://developer.apple.com/arkit/)

### Tutoriels Recommandés
- Unity Learn: AR Development
- Coursera: Introduction to AR
- YouTube: AR Foundation Tutorials

---

**Bon développement! 🚀**