# Guide de Configuration - Smart Retail AR

Ce guide vous aidera à configurer et démarrer rapidement avec le projet Smart Retail AR.

## Table des Matières

1. [Prérequis](#prérequis)
2. [Installation](#installation)
3. [Configuration Unity](#configuration-unity)
4. [Structure du Projet](#structure-du-projet)
5. [Premiers Pas](#premiers-pas)
6. [Mode Test](#mode-test)
7. [Dépannage](#dépannage)

## Prérequis

### Logiciels Requis

- **Unity Hub** : [Télécharger](https://unity.com/download)
- **Unity 2022.3 LTS** : Installer via Unity Hub
- **Git** : [Télécharger](https://git-scm.com/)
- **Éditeur de code** : Visual Studio ou VS Code recommandé

### Connaissances Requises

- Bases de Unity et C#
- Compréhension des concepts de programmation orientée objet
- Familiarité avec les interfaces utilisateur Unity

## Installation

### 1. Cloner le Repository

```bash
# Via HTTPS
git clone https://github.com/najialaajimi/smart-retail-ar.git

# Via SSH
git clone git@github.com:najialaajimi/smart-retail-ar.git

# Naviguer dans le dossier
cd smart-retail-ar
```

### 2. Ouvrir dans Unity

1. Lancez **Unity Hub**
2. Cliquez sur **"Add"** (ou **"Ouvrir"**)
3. Sélectionnez le dossier `smart-retail-ar`
4. Assurez-vous que **Unity 2022.3 LTS** est sélectionné
5. Cliquez sur le projet pour l'ouvrir

### 3. Première Ouverture

Unity va :
- Importer tous les assets
- Compiler les scripts C#
- Configurer le projet

**⏱️ Temps estimé** : 2-5 minutes selon votre machine

## Configuration Unity

### Import TextMeshPro

Si demandé lors de la première ouverture :

1. Menu : **Window > TextMeshPro > Import TMP Essential Resources**
2. Cliquez sur **Import**
3. Attendez la fin de l'import

### Configuration Build

#### Pour Android

1. Menu : **File > Build Settings**
2. Sélectionnez **Android**
3. Cliquez sur **Switch Platform**
4. Menu : **Edit > Project Settings > Player**
5. Configurez :
   - **Company Name** : Votre nom
   - **Product Name** : Smart Retail AR
   - **Minimum API Level** : Android 7.0 (API Level 24)
   - **Target API Level** : Automatic (highest installed)

#### Pour iOS

1. Menu : **File > Build Settings**
2. Sélectionnez **iOS**
3. Cliquez sur **Switch Platform**
4. Menu : **Edit > Project Settings > Player**
5. Configurez :
   - **Target minimum iOS Version** : 12.0
   - **Camera Usage Description** : "Pour scanner les codes QR des produits"

## Structure du Projet

### Assets/Scripts/

```
Scripts/
├── UI/                 # Contrôleurs d'interface
├── Data/              # Modèles de données
└── Utils/             # Scripts utilitaires
```

### Assets/Scenes/

```
Scenes/
├── HomeScene.unity              # Écran d'accueil
├── ScannerScene.unity           # Scanner QR
├── ProductInfoScene.unity       # Info produit
├── RecommendationsScene.unity   # Alternatives
└── ProfileScene.unity           # Profil utilisateur
```

### Assets/Resources/

```
Resources/
└── Data/
    └── products_database.json   # Base de données produits
```

## Premiers Pas

### 1. Ouvrir la Scène Principale

1. Dans l'onglet **Project**, naviguez vers `Assets/Scenes/`
2. Double-cliquez sur **HomeScene.unity**
3. La scène s'ouvre dans l'éditeur

### 2. Exécuter en Mode Play

1. Cliquez sur le bouton **Play** ▶️ (ou appuyez sur Ctrl+P / Cmd+P)
2. L'application démarre en mode simulation
3. Testez la navigation entre les écrans

### 3. Navigation de Base

Dans l'application :
- **Écran d'Accueil** → Bouton "Scanner Produit" → Scanner
- **Scanner** → Scan automatique après 1.5s → Info Produit
- **Info Produit** → Bouton "Alternatives" → Recommandations
- **Retour** disponible sur tous les écrans

## Mode Test

Le projet inclut un mode test pour faciliter le développement.

### Activer le Mode Test

Dans `ScannerController.cs` :

```csharp
[Header("Test Mode")]
public bool testMode = true;  // ← Déjà activé par défaut
public string[] testProductIds = { "PROD001", "PROD002", "PROD003" };
```

### Produits de Test Disponibles

| ID | Produit | Type |
|---|---|---|
| PROD001 | Yaourt Nature Bio | Laitier |
| PROD010 | Pâtes Complètes Bio | Céréales |
| PROD016 | Pommes Bio | Fruits |
| PROD026 | Poulet Bio | Viande |
| PROD033 | Chocolat Noir Bio 70% | Sucré |
| PROD043 | Café Bio Équitable | Boisson |

### Modifier les Produits de Test

1. Ouvrez `Assets/Scenes/ScannerScene.unity`
2. Sélectionnez l'objet avec le composant `ScannerController`
3. Dans l'Inspector, modifiez le tableau `Test Product Ids`
4. Ajoutez des IDs de produits de `products_database.json`

## Tester les Fonctionnalités

### Test du Scanner

1. Lancez le jeu
2. Cliquez sur **"Scanner Produit"**
3. Cliquez sur le bouton de capture
4. Observez le scan simulé
5. Vérifiez la navigation vers l'écran d'info produit

### Test des Filtres

1. Scannez un produit
2. Cliquez sur **"Voir les alternatives"**
3. Activez les filtres (Bio, Éco-responsable, etc.)
4. Changez le tri dans le dropdown
5. Vérifiez que la liste se met à jour

### Test du Profil

1. Depuis l'accueil, cliquez sur **"Mon Profil"**
2. Modifiez les préférences alimentaires
3. Consultez l'historique des scans
4. Changez les paramètres (son, langue, etc.)

## Modification de la Base de Données

### Ajouter un Produit

1. Ouvrez `Assets/Resources/Data/products_database.json`
2. Ajoutez un nouveau produit dans le tableau `products` :

```json
{
  "productId": "PROD051",
  "name": "Nouveau Produit",
  "brand": "Marque",
  "price": 5.00,
  "nutritionalInfo": {
    "calories": 100,
    "proteins": 5.0,
    "carbohydrates": 15.0,
    "fats": 3.0,
    "fiber": 2.0,
    "sugar": 8.0,
    "salt": 0.5
  },
  "origin": {
    "country": "France",
    "region": "Bretagne",
    "producer": "Producteur Local"
  },
  "scores": {
    "healthScore": 75,
    "ecoScore": 80,
    "nutritionGrade": "B"
  },
  "tags": ["Bio", "Local"],
  "alternativeIds": ["PROD001", "PROD002"]
}
```

3. Sauvegardez le fichier
4. Redémarrez Unity Play mode

### Format des Données

Tous les champs sont obligatoires pour éviter les erreurs.

## Dépannage

### Problème : Scripts ne compilent pas

**Solution :**
1. Menu : **Assets > Reimport All**
2. Attendez la recompilation
3. Si l'erreur persiste, vérifiez les erreurs dans la Console

### Problème : Base de données non chargée

**Solution :**
1. Vérifiez que `products_database.json` est dans `Assets/Resources/Data/`
2. Assurez-vous que le JSON est valide (utilisez JSONLint)
3. Vérifiez la Console pour les messages d'erreur

### Problème : Scènes ne se chargent pas

**Solution :**
1. Menu : **File > Build Settings**
2. Cliquez sur **Add Open Scenes** pour chaque scène
3. Vérifiez que toutes les scènes sont listées

### Problème : TextMeshPro manquant

**Solution :**
1. Menu : **Window > Package Manager**
2. Trouvez **TextMeshPro**
3. Cliquez sur **Install** ou **Import**

### Problème : Performance faible

**Solution :**
1. Réduisez la qualité graphique dans **Edit > Project Settings > Quality**
2. Fermez les applications gourmandes en ressources
3. Désactivez le profiler si activé

## Scripts Utiles

### Afficher les Logs

```csharp
Debug.Log("Message d'information");
Debug.LogWarning("Avertissement");
Debug.LogError("Erreur");
```

Les logs apparaissent dans la **Console** Unity.

### Accéder aux Managers

```csharp
// Navigation
NavigationManager.Instance.NavigateToHome();

// QR Code
QRCodeManager.Instance.StartScanning();

// Base de données
ProductData product = ProductDatabase.Instance.GetProductById("PROD001");
```

### Sauvegarder les Préférences

```csharp
UserPreferences prefs = UserPreferences.Load();
prefs.dietaryPreferences.Add("Végétarien");
prefs.Save();
```

## Commandes Git Utiles

```bash
# Voir les modifications
git status

# Ajouter des fichiers
git add .

# Commit
git commit -m "Description des changements"

# Push
git push origin main

# Pull
git pull origin main

# Créer une branche
git checkout -b feature/ma-fonctionnalite
```

## Ressources Supplémentaires

- [Documentation Unity](https://docs.unity3d.com/)
- [Tutoriels Unity Learn](https://learn.unity.com/)
- [Documentation C#](https://docs.microsoft.com/fr-fr/dotnet/csharp/)
- [DOCUMENTATION.md](DOCUMENTATION.md) - Documentation technique complète

## Support

Pour toute question ou problème :
1. Consultez la [Documentation technique](DOCUMENTATION.md)
2. Vérifiez les issues GitHub
3. Créez une nouvelle issue si nécessaire

## Prochaines Étapes

Une fois familiarisé avec le projet :
1. ✅ Explorez les différentes scènes
2. ✅ Testez toutes les fonctionnalités
3. ✅ Modifiez la base de données
4. ✅ Personnalisez l'interface
5. ✅ Préparez-vous pour l'intégration AR (Sprint 2)

---

**Bon développement ! 🚀**
