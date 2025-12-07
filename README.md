# Smart Retail AR - Sprint 1

## Description
Application AR pour améliorer l'expérience client en magasin, en superposant des informations (nutrition, origine, alternatives) sur les produits scannés.

## Sprint 1 - Interface Graphique Frontend

Ce sprint implémente toutes les interfaces graphiques frontend pour scanner des QR codes et afficher des informations produits.

## Configuration Technique

### Version Unity Requise
- **Unity 2022.3.62f3 (LTS)** - VERSION OBLIGATOIRE

### Packages Installés
- AR Foundation 5.1.5
- ARCore XR Plugin 5.1.5
- ARKit XR Plugin 5.1.5
- XR Interaction Toolkit 3.0.8
- UI Toolkit (UGUI) 2.0.0
- TextMeshPro 3.0.9
- Addressables 2.3.1

## Architecture du Projet

```
Assets/
├── Scripts/
│   ├── UI/
│   │   ├── HomeController.cs          # Écran d'accueil
│   │   ├── ScannerController.cs       # Interface scanner QR
│   │   ├── ProductInfoController.cs   # Informations produit
│   │   ├── RecommendationsController.cs # Alternatives produits
│   │   ├── ProfileController.cs       # Profil utilisateur
│   │   └── ProductCard.cs             # Composant carte produit
│   ├── Data/
│   │   ├── ProductData.cs             # Structure données produit
│   │   ├── ProductDatabase.cs         # Gestionnaire base de données
│   │   └── UserPreferences.cs         # Préférences utilisateur
│   └── Utils/
│       ├── NavigationManager.cs       # Gestion navigation
│       └── QRCodeManager.cs           # Gestion scan QR
├── Scenes/
│   ├── MainScene.unity
│   ├── HomeScene.unity
│   ├── ScannerScene.unity
│   ├── ProductInfoScene.unity
│   ├── RecommendationsScene.unity
│   └── ProfileScene.unity
├── Resources/
│   └── Data/
│       └── products_database.json     # Base de données 52 produits
└── Prefabs/
    └── UI/
```

## Fonctionnalités Implémentées

### 1. Écran d'Accueil (HomeController)
- Bouton "Scanner Produit"
- Bouton "Mon Profil"
- Message de bienvenue personnalisé
- Animation d'entrée avec fade-in

### 2. Scanner QR Code (ScannerController)
- **Mode Test** activé par défaut pour simulation
- Scan de QR codes simulé avec 10+ produits de test
- Bouton scan avec cooldown
- Bouton torche
- Indicateur de statut
- Navigation automatique vers informations produit

### 3. Informations Produit (ProductInfoController)
- Nom, marque, description
- Origine et région
- Prix
- Informations nutritionnelles (calories, protéines, glucides, lipides)
- Scores santé et écologique (0-10) avec indicateurs colorés
- Bouton vers alternatives

### 4. Recommandations/Alternatives (RecommendationsController)
- Liste des produits alternatifs
- Filtres fonctionnels:
  - Bio
  - Éco-responsable (score ≥ 7)
  - Prix (≤ limite)
  - Score santé (≥ limite)
- Cartes produits avec ProductCard component
- Navigation vers détails produit

### 5. Profil Utilisateur (ProfileController)
- Nom d'utilisateur modifiable
- Préférences alimentaires:
  - Végétarien
  - Vegan
  - Sans gluten
  - Sans lactose
- Historique des scans (10 derniers)
- Statistiques (nombre total de scans)
- Paramètres (notifications, mode éco)
- Sauvegarde dans PlayerPrefs

### 6. Navigation (NavigationManager)
- Singleton pattern avec DontDestroyOnLoad
- Navigation fluide entre scènes
- Gestion du retour (bouton back)
- Méthodes dédiées pour chaque écran

## Base de Données Produits

52 produits d'exemple avec:
- ID unique
- Nom, marque, description
- Informations nutritionnelles complètes
- Scores santé et écologique
- Origine et région
- Tags (Bio, Vegan, Sans gluten, etc.)
- Alternatives recommandées (3-4 par produit)

### Catégories de Produits
- Produits laitiers et alternatives végétales
- Pain et boulangerie
- Pâtes et céréales
- Protéines végétales
- Huiles
- Chocolats
- Compotes et desserts
- Petit-déjeuner
- Jus et boissons
- Tartinades
- Thés et infusions

## Utilisation

### Mode Test (Scanner)
Le scanner est configuré en mode test par défaut avec `testMode = true`. Cela permet de tester sans caméra AR:

1. Ouvrir HomeScene
2. Cliquer "Scanner Produit"
3. Cliquer le bouton "Scan"
4. Un produit aléatoire sera sélectionné parmi les produits de test
5. Navigation automatique vers ProductInfoScene

### Produits de Test
Produits IDs disponibles en test mode:
- prod001 à prod010 (configurés dans ScannerController)

### Ajouter des Produits
Modifier `Assets/Resources/Data/products_database.json`:
```json
{
  "id": "prod053",
  "name": "Nouveau Produit",
  "brand": "Marque",
  ...
}
```

## Patterns de Conception

### Singleton Pattern
Utilisé pour les managers (NavigationManager, QRCodeManager, ProductDatabase):
- Instance unique accessible globalement
- DontDestroyOnLoad pour persistance
- Lazy initialization

### Data Classes
- ProductData: Serializable avec JsonUtility
- UserPreferences: Sauvegarde/chargement PlayerPrefs
- Utilisation de listes parallèles pour dictionnaires (compatibilité JsonUtility)

### UI Controllers
- Un controller par scène
- Gestion des références UI
- Cleanup dans OnDestroy

## Build Settings

### Android
- Minimum API Level: Android 7.0 (API 24)
- Target API Level: Latest
- Scripting Backend: IL2CPP
- Target Architectures: ARM64

### iOS
- Minimum iOS Version: 11.0
- Target SDK: Device SDK
- Architecture: ARM64

## Performance

- Interface fluide 60 FPS
- Chargement optimisé avec Resources.Load
- Cache des données en mémoire
- Async scene loading

## Prochaines Étapes (Sprint 2)

1. Intégration AR Foundation pour scan QR réel
2. Activation caméra AR
3. Overlay AR sur produits
4. Détection et tracking produits
5. Animations AR
6. Tests sur devices physiques

## Notes Techniques

### Compatibilité Packages
Tous les packages sont vérifiés compatibles avec Unity 2022.3.62f3:
- AR Foundation 5.1.5 ✓
- ARCore XR Plugin 5.1.5 ✓
- ARKit XR Plugin 5.1.5 ✓

### Limitations Actuelles
- Scanner en mode test uniquement (pas de caméra AR)
- Images produits en placeholder
- Pas de chargement distant d'images

## Développement

### Ouvrir le Projet
1. Installer Unity 2022.3.62f3
2. Ouvrir le dossier du projet
3. Unity installera automatiquement les packages
4. Ouvrir HomeScene pour commencer

### Structure du Code
- Namespace `SmartRetailAR.Data` pour les données
- Namespace `SmartRetailAR.UI` pour les controllers UI
- Namespace `SmartRetailAR.Utils` pour les utilitaires

### Conventions
- Singletons avec pattern standard Unity
- Serialization avec JsonUtility
- UI avec Unity UGUI
- Gestion mémoire avec DontDestroyOnLoad

## Licence

[À définir]

## Auteurs

Smart Retail AR Team