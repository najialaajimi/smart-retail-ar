# Smart Retail AR - Version Complète 🛒📱

Application de réalité augmentée complète pour améliorer l'expérience client en magasin. Scannez des produits avec votre smartphone et accédez instantanément à des informations détaillées en AR.

## 🎯 Aperçu du Projet

Smart Retail AR est une application mobile complète qui intègre 4 sprints de développement :

1. **Sprint 1** : Scanner QR code et informations basiques
2. **Sprint 2** : Intégration AR avec superposition des données
3. **Sprint 3** : Moteur de recommandations et filtres
4. **Sprint 4** : Tests utilisateurs et optimisations

## 🚀 Caractéristiques Principales

### Scanner & Reconnaissance
- ✅ Scanner QR code en temps réel
- ✅ Reconnaissance d'images avec ML
- ✅ Scanner de codes-barres multi-formats
- ✅ Mode test sans matériel AR

### Réalité Augmentée
- ✅ Tracking de produits en temps réel
- ✅ Superposition d'informations AR
- ✅ Détection de plans AR
- ✅ Calibration AR automatique

### Recommandations IA
- ✅ Moteur de recommandations personnalisées
- ✅ Filtres multi-critères (bio, végan, origine, etc.)
- ✅ Comparaison de produits côte à côte
- ✅ Scoring écologique et santé

### Analytics & Optimisation
- ✅ Analytics en temps réel
- ✅ Tests A/B intégrés
- ✅ Optimisation automatique des performances
- ✅ Gestion intelligente de la batterie
- ✅ Optimisation mémoire avec cache

## 📊 Base de Données

**5000+ produits** disponibles avec :
- Informations nutritionnelles détaillées
- Scores écologiques, santé, sociaux
- Origine et traçabilité
- Certifications (Bio, Écocert, Commerce Équitable, etc.)
- Alternatives et recommandations
- Images et modèles 3D

## 🛠️ Configuration Technique

### Prérequis
- **Unity** : 2022.3.62f3 LTS (OBLIGATOIRE)
- **Scripting Backend** : IL2CPP
- **API Level** : .NET Standard 2.1
- **Plateformes** : Android API 24+ / iOS 11.0+
- **Architecture** : ARM64

### Packages Unity Inclus
```
- AR Foundation 5.1.5
- AR Core 5.1.5
- AR Kit 5.1.5
- XR Interaction Toolkit 3.0.8
- ML Agents 2.0.1
- Barracuda 3.0.0
- Addressables 2.3.1
- Analytics 5.0.0
- TextMeshPro 3.0.9
- Cinemachine 2.9.7
```

## 📁 Structure du Projet

```
Assets/
├── Scenes/
│   ├── Sprint1/ (6 scènes : Splash, Onboarding, Home, QRScanner, ProductInfo, Settings)
│   ├── Sprint2/ (4 scènes : ARCamera, ARProductTracking, AROverlay, ARCalibration)
│   ├── Sprint3/ (5 scènes : Recommendations, Filter, Comparison, Wishlist, Profile)
│   └── Sprint4/ (5 scènes : TestMode, Analytics, Feedback, Tutorial, Debug)
├── Scripts/
│   ├── Sprint1/ (UI, Data, Utils - 9 scripts)
│   ├── Sprint2/ (AR, Recognition, UI - 12 scripts)
│   ├── Sprint3/ (Recommendations, ML, UI, Data - 13 scripts)
│   └── Sprint4/ (Testing, Analytics, Optimization, UI - 16 scripts)
├── Resources/
│   └── Data/
│       └── products_database.json (5000+ produits)
├── Prefabs/ (UI, AR, Products)
├── Materials/ (UI, AR, Products)
└── StreamingAssets/ML/ (Modèles ML)
```

## 🎮 Démarrage Rapide

### Installation
1. Cloner le repository
```bash
git clone https://github.com/najialaajimi/smart-retail-ar.git
```

2. Ouvrir avec Unity 2022.3.62f3
```
File > Open Project > Sélectionner le dossier
```

3. Attendre l'import des packages

### Configuration Build

#### Android
1. File > Build Settings > Android
2. Player Settings :
   - Minimum API Level : 24
   - Target API Level : 33
   - Scripting Backend : IL2CPP
   - Target Architectures : ARM64
3. Build

#### iOS
1. File > Build Settings > iOS
2. Player Settings :
   - Minimum iOS Version : 11.0
   - Target iOS Version : 16.0
   - Scripting Backend : IL2CPP
   - Architecture : ARM64
3. Build & Run

## 🧪 Mode Test

Le mode test est activé par défaut pour permettre le développement sans matériel AR :
- Simulation de scans QR automatique
- Produits aléatoires de la base de données
- Désactiver via `QRScannerController.testMode = false`

## 📈 KPIs Cibles

### Performance Technique
- ✅ Reconnaissance produit ≥ 95%
- ✅ Latence affichage AR ≤ 1 seconde
- ✅ Framerate AR stable ≥ 30 FPS
- ✅ Temps de démarrage ≤ 3 secondes

### Expérience Utilisateur
- 🎯 Satisfaction utilisateur ≥ 80%
- 🎯 Taux d'adoption ≥ 70%
- 🎯 Temps d'apprentissage ≤ 5 minutes
- 🎯 Précision recommandations ≥ 85%

## 🔧 Scripts Principaux

### Managers (Singletons)
- `NavigationManager` - Navigation entre scènes
- `ProductDatabase` - Gestion de 5000+ produits
- `QRCodeManager` - Gestion des scans QR
- `ARManager` - Gestion AR Foundation
- `RecommendationEngine` - Moteur IA de recommandations
- `AnalyticsManager` - Tracking événements
- `PerformanceOptimizer` - Optimisation automatique
- `MemoryManager` - Gestion mémoire & cache
- `BatteryOptimizer` - Optimisation batterie

## 🎨 Fonctionnalités par Sprint

### Sprint 1 - Scanner & Info Basiques
- Écran splash avec logo
- Tutoriel onboarding
- Scanner QR code
- Affichage informations produit
- Paramètres application

### Sprint 2 - Intégration AR
- Caméra AR en temps réel
- Tracking produits stable
- Superposition informations AR
- Calibration AR
- Reconnaissance d'images ML

### Sprint 3 - Recommandations
- Algorithmes collaborative filtering
- Filtres multi-critères
- Comparateur produits
- Scoring écologique/santé
- Liste de souhaits

### Sprint 4 - Tests & Analytics
- Mode test utilisateur
- Dashboard analytics
- Collecte feedback
- Tests A/B
- Optimisation batterie/mémoire

## 📱 Utilisation

1. **Premier Lancement**
   - Splash screen (3 secondes)
   - Onboarding tutoriel
   - Accueil application

2. **Scanner un Produit**
   - Appuyer sur "Scanner"
   - Pointer vers un code QR ou produit
   - Visualiser les informations

3. **Mode AR**
   - Activer la caméra AR
   - Scanner un produit en temps réel
   - Voir les informations superposées

4. **Recommandations**
   - Consulter les alternatives
   - Comparer les produits
   - Filtrer par critères

## 📄 Licence

Tous droits réservés - Smart Retail AR Project

## 🔄 Version

**v1.0.0** - Implémentation complète (Sprints 1-4)

---

**Note** : Ce projet nécessite Unity 2022.3.62f3 LTS. D'autres versions de Unity peuvent ne pas être compatibles avec les packages AR Foundation utilisés.

🌟 **Bon développement avec Smart Retail AR !** 🌟