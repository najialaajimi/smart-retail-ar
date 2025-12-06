# Smart Retail AR - Sprint 1 Documentation

## Vue d'ensemble du projet

Smart Retail AR est une application de réalité augmentée conçue pour améliorer l'expérience d'achat en magasin. L'application permet aux utilisateurs de scanner des codes QR sur les produits et d'accéder instantanément à des informations détaillées sur la nutrition, l'origine, l'impact environnemental et des alternatives recommandées.

## Sprint 1 - Interfaces Graphiques Frontend

Ce sprint se concentre sur la création de toutes les interfaces utilisateur nécessaires pour la phase 1 du projet, qui inclut le scan de codes QR et l'affichage d'informations produit de base.

## Architecture du Projet

### Structure des Dossiers

```
Assets/
├── Scripts/
│   ├── UI/                      # Contrôleurs d'interface utilisateur
│   │   ├── HomeController.cs
│   │   ├── ScannerController.cs
│   │   ├── ProductInfoController.cs
│   │   ├── RecommendationsController.cs
│   │   ├── ProfileController.cs
│   │   └── ProductCard.cs
│   ├── Data/                    # Modèles de données
│   │   ├── ProductData.cs
│   │   ├── ProductDatabase.cs
│   │   └── UserPreferences.cs
│   └── Utils/                   # Utilitaires
│       ├── NavigationManager.cs
│       └── QRCodeManager.cs
├── Scenes/                      # Scènes Unity
│   ├── HomeScene.unity
│   ├── ScannerScene.unity
│   ├── ProductInfoScene.unity
│   ├── RecommendationsScene.unity
│   └── ProfileScene.unity
├── Prefabs/
│   └── UI/                      # Prefabs d'interface
├── Resources/
│   └── Data/
│       └── products_database.json
└── Materials/
    └── UI/
```

## Composants Principaux

### 1. Écran d'Accueil (HomeController.cs)

**Fonctionnalités :**
- Affichage du logo et branding "Smart Retail AR"
- Bouton principal "Scanner Produit" pour accéder au scanner
- Bouton "Mon Profil" pour accéder au profil utilisateur
- Animations d'introduction
- Gestion du tutoriel pour les nouveaux utilisateurs

**Navigation :**
- Vers Scanner (ScannerScene)
- Vers Profil (ProfileScene)
- Vers Aide (à implémenter)

### 2. Scanner QR Code (ScannerController.cs)

**Fonctionnalités :**
- Interface de scan avec aperçu caméra en temps réel
- Overlay de visée avec cadre de scan
- Bouton de capture pour déclencher le scan
- Bouton torche pour activer/désactiver la lampe
- Indicateur de statut du scan
- Instructions claires pour l'utilisateur
- Mode test avec simulation de scan

**Navigation :**
- Retour vers écran précédent
- Vers ProductInfo après scan réussi

**Intégration AR :**
- Préparé pour intégration avec AR Foundation
- Méthodes de contrôle caméra définies
- Gestion d'événements pour scan réussi/échoué

### 3. Informations Produit (ProductInfoController.cs)

**Fonctionnalités :**
- Affichage du nom et de la marque du produit
- Image du produit
- Informations nutritionnelles détaillées :
  - Calories
  - Protéines
  - Glucides
  - Lipides
  - Fibres
  - Sucres
  - Sel
- Informations d'origine :
  - Pays
  - Région
  - Producteur
- Scores de santé et écologiques :
  - Score santé (0-100)
  - Score écologique (0-100)
  - Grade nutritionnel (A-E)
- Tags (Bio, Éco-responsable, etc.)
- Bouton "Voir les alternatives"

**Navigation :**
- Retour vers écran précédent
- Vers Recommendations

### 4. Recommandations (RecommendationsController.cs)

**Fonctionnalités :**
- Liste des produits alternatifs
- Système de filtrage avancé :
  - Bio
  - Éco-responsable
  - Prix maximum
  - Score santé minimum
- Système de tri :
  - Par prix (croissant/décroissant)
  - Par score santé
  - Par score écologique
- Cards produits avec :
  - Image miniature
  - Nom et marque
  - Prix comparatif
  - Scores affichés

**Navigation :**
- Retour vers ProductInfo
- Vers ProductInfo (produit alternatif sélectionné)

### 5. Profil Utilisateur (ProfileController.cs)

**Fonctionnalités :**
- Informations utilisateur :
  - Avatar
  - Nom d'utilisateur
- Préférences alimentaires :
  - Végétarien
  - Végétalien
  - Sans gluten
  - Sans lactose
  - Bio uniquement
  - Éco-responsable
  - Local
  - Sans allergènes
- Historique des scans :
  - Liste des 10 derniers produits scannés
  - Nombre total de scans
  - Compteur par produit
- Statistiques :
  - Nombre de choix sains
  - Nombre de choix écologiques
  - Scans cette semaine
- Paramètres :
  - Son activé/désactivé
  - Feedback haptique
  - Affichage du tutoriel
  - Mode sombre
  - Langue

**Navigation :**
- Retour vers écran précédent
- Vers ProductInfo (depuis historique)

## Gestion des Données

### ProductData.cs

Structure de données pour un produit :
```csharp
{
  "productId": "PROD001",
  "name": "Nom du produit",
  "brand": "Marque",
  "price": 2.50,
  "nutritionalInfo": { ... },
  "origin": { ... },
  "scores": { ... },
  "tags": ["Bio", "Local"],
  "alternativeIds": ["PROD002", "PROD003"]
}
```

### ProductDatabase.cs

Gestion centralisée de la base de données produits :
- Chargement depuis JSON
- Recherche par ID
- Récupération des alternatives
- Filtrage et recherche
- Singleton pattern pour accès global

### UserPreferences.cs

Sauvegarde locale des préférences utilisateur :
- Identité utilisateur
- Préférences alimentaires
- Historique des scans
- Paramètres application
- Sauvegarde via PlayerPrefs

## Utilitaires

### NavigationManager.cs

Gestion centralisée de la navigation :
- Navigation entre scènes
- Historique de navigation
- Navigation arrière
- Singleton pattern

Méthodes principales :
- `NavigateToHome()`
- `NavigateToScanner()`
- `NavigateToProductInfo()`
- `NavigateToRecommendations()`
- `NavigateToProfile()`
- `NavigateBack()`

### QRCodeManager.cs

Gestion du scan de codes QR :
- Contrôle du processus de scan
- Décodage des codes QR
- Validation avec la base de données
- Événements pour succès/échec
- Singleton pattern

Format des QR codes supportés :
- `PRODUCT:productId`
- `productId` (format simple)

## Base de Données Produits

La base de données contient **50 produits d'exemple** dans `products_database.json`.

### Catégories de produits :
- Produits laitiers (yaourts, lait, fromages)
- Produits céréaliers (pain, pâtes, riz, céréales)
- Viandes et poissons
- Fruits et légumes
- Huiles et condiments
- Boissons (jus, café, thé)
- Produits sucrés (chocolat, miel)

### Données par produit :
- Informations nutritionnelles complètes
- Origine détaillée
- Scores santé et écologiques
- Tags pour filtrage
- Liens vers alternatives

## Design System

### Couleurs Recommandées
- Primaire : Bleu/Vert (écologique)
- Succès : Vert (#4CAF50)
- Attention : Orange (#FF9800)
- Erreur : Rouge (#F44336)
- Scores :
  - Excellent (80+) : Vert
  - Bon (60-79) : Jaune
  - Moyen (40-59) : Orange
  - Faible (<40) : Rouge

### Typographie
- Titres : 24-32pt
- Sous-titres : 18-20pt
- Corps de texte : 14-16pt
- Petits textes : 12pt

### Layout
- Marges : 16-24px
- Espacement entre éléments : 8-16px
- Coins arrondis : 8-12px
- Taille minimale des boutons : 44x44pt

## Optimisation Performance

### Objectifs :
- ✅ Interface fluide à 60 FPS
- ✅ Temps de chargement < 2 secondes
- ✅ Utilisation mémoire optimisée

### Bonnes pratiques :
- Chargement paresseux des données
- Mise en cache des produits consultés
- Pooling des objets UI réutilisables
- Compression des textures
- Optimisation des requêtes base de données

## Tests et Validation

### Mode Test
Le `ScannerController` inclut un mode test activable :
```csharp
public bool testMode = true;
public string[] testProductIds = { "PROD001", "PROD002", "PROD003" };
```

### IDs de produits de test recommandés :
- PROD001 : Yaourt Nature Bio
- PROD010 : Pâtes Complètes Bio
- PROD016 : Pommes Bio
- PROD026 : Poulet Bio
- PROD033 : Chocolat Noir Bio 70%

## Configuration Unity Requise

### Version Unity
- Unity 2022.3 LTS ou plus récent

### Packages Requis
- TextMeshPro
- AR Foundation (pour Sprint 2)
- XR Plugin Management (pour Sprint 2)
- ARCore XR Plugin (Android)
- ARKit XR Plugin (iOS)

### Paramètres Build
- Plateforme cible : Android / iOS
- Minimum API Level (Android) : 24
- Target iOS : 12.0+
- Graphics API : OpenGL ES 3.0+ / Metal

## Prochaines Étapes (Sprint 2)

1. **Intégration AR Foundation**
   - Configuration ARSession
   - Détection de plans
   - Placement d'objets 3D

2. **Scan QR Code Réel**
   - Intégration caméra AR
   - Bibliothèque de décodage QR (ZXing)
   - Optimisation performance scan

3. **Overlay AR**
   - Affichage informations en AR
   - Ancrage spatial
   - UI contextuelle 3D

4. **Assets Visuels**
   - Modèles 3D produits
   - Icônes et illustrations
   - Animations UI avancées

## Support et Contact

Pour toute question ou problème, référez-vous à la documentation technique ou contactez l'équipe de développement.

## Licence

© 2024 Smart Retail AR - Tous droits réservés
