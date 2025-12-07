# Sprint 1 : Développement des Interfaces Frontend

## Vue d'ensemble
Sprint 1 se concentre sur la création des interfaces utilisateur de base pour l'application Smart Retail AR. Ce sprint établit les fondations de l'expérience utilisateur avec des interfaces intuitives et réactives.

## Objectifs du Sprint
- ✅ Créer les écrans principaux de l'application
- ✅ Implémenter la navigation entre les écrans
- ✅ Développer le système de gestion des données produits
- ✅ Créer le système de préférences utilisateur
- ✅ Mettre en place l'architecture de base

## Architecture Frontend

### Structure des Scripts
```
Assets/Scripts/
├── UI/
│   ├── HomeController.cs          - Écran d'accueil principal
│   ├── ScannerController.cs       - Interface de scan AR
│   ├── ProductInfoController.cs   - Affichage détails produit
│   └── ProductCard.cs             - Composant carte produit
├── Data/
│   ├── ProductData.cs             - Structures de données produit
│   ├── ProductDatabase.cs         - Base de données produits
│   └── UserPreferences.cs         - Préférences utilisateur
└── Utils/
    └── NavigationManager.cs       - Gestion navigation
```

## Composants Principaux

### 1. HomeController
**Responsabilités:**
- Point d'entrée de l'application
- Navigation vers les différentes sections
- Affichage des statistiques utilisateur
- Message de bienvenue personnalisé

**Fonctionnalités:**
- Bouton Scanner (navigation vers ARScannerScene)
- Bouton Recommandations
- Bouton Profil
- Bouton Historique
- Statistiques de scan

### 2. ScannerController
**Responsabilités:**
- Gestion de l'interface de scan AR
- Détection des produits
- Affichage des instructions
- Mode test pour développement sans AR

**Fonctionnalités:**
- Support AR Foundation pour reconnaissance d'image
- Mode test avec simulation de scan
- Indicateur de scan en cours
- Navigation automatique vers ProductInfo après détection

**Mode Test:**
```csharp
testMode = true; // Active le mode test
testProductIds = ["PROD001", "PROD002", "PROD003"]; // Produits de test
```

### 3. ProductInfoController
**Responsabilités:**
- Affichage complet des informations produit
- Informations nutritionnelles et écologiques
- Recommandations de produits alternatifs
- Actions sur le produit (voir en AR, comparer)

**Sections d'information:**
- **Informations générales:** Nom, marque, prix, description
- **Nutritionnelles:** Calories, protéines, glucides, lipides, Nutri-Score
- **Écologiques:** Empreinte carbone, emballage, origine, Éco-Score
- **Recommandations:** Produits similaires et alternatifs

### 4. ProductCard
**Responsabilités:**
- Composant réutilisable pour affichage produit
- Affichage compact des informations essentielles
- Navigation vers détails produit au clic

**Informations affichées:**
- Nom du produit
- Marque
- Prix
- Nutri-Score
- Éco-Score

## Système de Données

### ProductData
Structure de données complète pour un produit:
```csharp
public class ProductData
{
    public string id;
    public string name;
    public string category;
    public string brand;
    public float price;
    public string description;
    public NutritionalInfo nutritionalInfo;
    public EcologicalInfo ecologicalInfo;
    public EthicalInfo ethicalInfo;
    public List<string> allergens;
    public string imageUrl;
    public string barcode;
}
```

### ProductDatabase
**Responsabilités:**
- Singleton pour gestion centralisée des produits
- Chargement depuis JSON (Resources/Data/products_database.json)
- Recherche et filtrage de produits
- Cache en mémoire pour performances

**Méthodes principales:**
- `GetProductById(string id)` - Récupération par ID
- `GetAllProducts()` - Liste complète
- `GetProductsByCategory(string category)` - Filtrage par catégorie
- `SearchProducts(string searchTerm)` - Recherche textuelle

### UserPreferences
**Responsabilités:**
- Gestion des préférences alimentaires
- Historique des scans
- Filtres d'allergènes
- Sauvegarde/chargement depuis PlayerPrefs

**Préférences supportées:**
- Végétarien / Vegan
- Sans gluten / Sans lactose
- Biologique uniquement
- Filtres d'allergènes personnalisés
- Historique des produits scannés

**Note importante:** Utilise des listes parallèles au lieu de Dictionary pour la sérialisation JSON avec Unity.

## Système de Navigation

### NavigationManager
**Responsabilités:**
- Singleton pour gestion de la navigation
- Chargement asynchrone des scènes
- Temps de chargement minimum pour transitions fluides
- Gestion de l'état de chargement

**Scènes principales:**
- `HomeScene` - Écran d'accueil
- `ARScannerScene` - Scanner AR
- `ProductInfoScene` - Détails produit
- `RecommendationsScene` - Recommandations
- `ProfileScene` - Profil utilisateur

## Base de Données Produits

### Structure JSON
```json
{
  "products": [
    {
      "id": "PROD001",
      "name": "Nom du produit",
      "category": "Catégorie",
      "brand": "Marque",
      "price": 2.99,
      "description": "Description",
      "nutritionalInfo": {
        "calories": 65,
        "protein": 3.5,
        "carbs": 4.5,
        "fat": 3.2,
        "fiber": 0,
        "sugar": 4.5,
        "sodium": 50,
        "nutriScore": "A"
      },
      "ecologicalInfo": {
        "carbonFootprint": 1.2,
        "packaging": "Recyclable",
        "origin": "France",
        "organic": true,
        "ecoScore": "A"
      },
      "ethicalInfo": {
        "fairTrade": true,
        "local": true,
        "ethicalScore": "A"
      },
      "allergens": ["Lait"],
      "imageUrl": "produit.png",
      "barcode": "3250391234567"
    }
  ]
}
```

### Catégories de produits
- Produits Laitiers
- Boulangerie
- Boissons
- Épicerie
- Fruits et Légumes
- Confiserie

## Patterns de Conception

### Singleton Pattern
Utilisé pour les managers globaux:
- `ProductDatabase`
- `UserPreferencesManager`
- `NavigationManager`

```csharp
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
```

### Observer Pattern
Utilisé pour les événements UI et navigation.

## Tests et Validation

### Mode Test
Le `ScannerController` inclut un mode test pour développement:
- Active/désactive avec `testMode` boolean
- Simule des scans avec `testProductIds`
- Permet le développement sans matériel AR

### Points de test
- ✅ Navigation entre toutes les scènes
- ✅ Chargement de la base de données produits
- ✅ Affichage des informations produit
- ✅ Sauvegarde/chargement des préférences utilisateur
- ✅ Recherche et filtrage de produits

## Installation et Configuration

### Prérequis
- Unity 2022.3.62f3 LTS
- TextMeshPro package
- AR Foundation 5.1.5 (pour Sprint 2)

### Setup
1. Ouvrir le projet Unity
2. Les packages nécessaires sont dans `Packages/manifest.json`
3. La base de données produits est dans `Assets/Resources/Data/`
4. Les scripts sont organisés par responsabilité dans `Assets/Scripts/`

### Configuration PlayerPrefs
Clés utilisées:
- `UserName` - Nom de l'utilisateur
- `TotalScannedProducts` - Compteur de scans
- `CurrentProductId` - Produit actuellement visualisé
- `UserPreferences` - Données de préférences sérialisées

## Optimisations

### Performance
- Cache en mémoire pour la base de données
- Dictionnaire pour recherche rapide par ID
- DontDestroyOnLoad pour managers persistants

### Mémoire
- Chargement lazy des données
- Utilisation de Resources.Load pour assets
- Nettoyage des références dans OnDestroy

## Prochaines Étapes (Sprint 2)

Le Sprint 2 se concentrera sur:
- Intégration complète d'AR Foundation
- Reconnaissance d'image en temps réel
- Superposition AR des données
- Optimisation des performances AR

## Références

### Documentation Unity
- [TextMeshPro Documentation](https://docs.unity3d.com/Manual/com.unity.textmeshpro.html)
- [UI System](https://docs.unity3d.com/Manual/UISystem.html)
- [PlayerPrefs](https://docs.unity3d.com/ScriptReference/PlayerPrefs.html)

### Best Practices
- Utiliser TextMeshPro pour tout le texte
- Singleton pattern pour managers globaux
- Sérialisation JSON pour données complexes
- PlayerPrefs pour préférences simples
