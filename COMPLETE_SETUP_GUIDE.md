# Guide Complet: Configuration de Toutes les Scènes

## Comprendre le Problème

L'erreur "No product ID available" apparaît parce que:
1. ✅ HomeScene a maintenant l'UI (boutons visibles)
2. ❌ Quand vous cliquez sur "Scanner Produit", Unity charge **ScannerScene**
3. ❌ ScannerScene n'a pas d'UI configurée
4. ❌ ProductInfoScene n'a pas d'UI non plus

**Solution**: Configurer l'UI pour TOUTES les scènes dans le bon ordre.

---

## Workflow Complet de l'Application

```
HomeScene (Accueil)
    ↓ Clic "Scanner Produit"
ScannerScene (Scanner QR)
    ↓ Scan réussi
ProductInfoScene (Infos Produit)
    ↓ Clic "Voir Alternatives"
RecommendationsScene (Alternatives)
    ↓ Sélection produit
ProductInfoScene (Nouveau produit)

HomeScene (Accueil)
    ↓ Clic "Mon Profil"
ProfileScene (Profil utilisateur)
```

---

## Ordre de Configuration Recommandé

Configurez les scènes dans cet ordre:
1. ✅ **HomeScene** (Déjà fait)
2. **ScannerScene** (Étape suivante)
3. **ProductInfoScene**
4. **RecommendationsScene**
5. **ProfileScene**

---

## Méthode 1: Configuration Automatique (RAPIDE)

### Pour Chaque Scène

1. **Ouvrir la scène** dans Unity (double-clic dans Assets/Scenes/)
2. **Créer GameObject vide**:
   - Hierarchy → Create Empty
   - Nommer "UISetup"
3. **Ajouter le script SetupUI**:
   - Inspector → Add Component → "SetupUI"
4. **Exécuter la commande appropriée**:
   - Clic droit sur SetupUI dans Inspector
   - Sélectionner la commande correspondante:

| Scène | Commande Menu Contextuel |
|-------|-------------------------|
| HomeScene | Setup Home Scene UI |
| ScannerScene | Setup Scanner Scene UI |
| ProductInfoScene | Setup Product Info Scene UI |
| RecommendationsScene | (Manuel - voir ci-dessous) |
| ProfileScene | (Manuel - voir ci-dessous) |

5. **Sauvegarder**: Ctrl+S ou File → Save

---

## Configuration Détaillée par Scène

### 1. HomeScene ✅ (Déjà configuré)

**Éléments nécessaires:**
- Canvas
- WelcomeText (TextMeshPro)
- ScanButton (Button)
- ProfileButton (Button)
- HomeController (script attaché au Canvas)

**Connexions HomeController:**
- `scanButton` → ScanButton
- `profileButton` → ProfileButton
- `welcomeText` → WelcomeText

---

### 2. ScannerScene 🔧

#### Configuration Automatique
1. Ouvrir ScannerScene.unity
2. Hierarchy → Create Empty → Nommer "UISetup"
3. Add Component → SetupUI
4. Clic droit sur SetupUI → **"Setup Scanner Scene UI"**

#### Connexion du ScannerController
1. Sélectionner le **Canvas** dans Hierarchy
2. Add Component → **ScannerController**
3. Connecter les références:
   - `scanButton` → Glisser "ScanButton"
   - `torchButton` → Glisser "TorchButton"
   - `backButton` → Glisser "BackButton"
   - `statusText` → Glisser "StatusText"
   - `cameraView` → Glisser "CameraView"
   
4. Configurer le test mode:
   - `testMode` → ✓ (coché)
   - `testProductCount` → 10

#### Vérification
- Cliquer Play
- Le statut "Pointez vers le QR code du produit" apparaît
- Bouton "Scanner" visible et cliquable

---

### 3. ProductInfoScene 🔧

#### Configuration Automatique
1. Ouvrir ProductInfoScene.unity
2. Hierarchy → Create Empty → "UISetup"
3. Add Component → SetupUI
4. Clic droit → **"Setup Product Info Scene UI"**

#### Connexion du ProductInfoController
1. Sélectionner **Canvas**
2. Add Component → **ProductInfoController**
3. Connecter TOUTES les références:

**Textes de base:**
- `productNameText` → "ProductNameText"
- `brandText` → "BrandText"
- `descriptionText` → "DescriptionText"
- `originText` → "OriginText"
- `priceText` → Créer nouveau Text (TMP): "PriceText"

**Informations nutritionnelles:**
- `caloriesText` → "CaloriesText"
- `proteinsText` → "ProteinsText"
- `carbohydratesText` → "CarbsText"
- `fatsText` → "FatsText"

**Scores:**
- `healthScoreText` → "HealthScoreText"
- `ecoScoreText` → "EcoScoreText"
- `healthScoreSlider` → Créer: Hierarchy → UI → Slider → Nommer "HealthScoreSlider"
- `ecoScoreSlider` → Créer: Hierarchy → UI → Slider → Nommer "EcoScoreSlider"

**Boutons:**
- `alternativesButton` → "AlternativesButton"
- `backButton` → "BackButton"

**Image produit (optionnel):**
- `productImage` → Créer: UI → Raw Image → "ProductImage"

---

### 4. RecommendationsScene 🔧

#### Configuration Manuelle (Pas de script auto)

**Créer les éléments:**

1. **Canvas** (si pas existant)
   - UI Scale Mode: Scale With Screen Size
   - Reference Resolution: 1920 x 1080

2. **Scroll View pour la liste**:
   - Clic droit Canvas → UI → Scroll View
   - Nommer "ProductListScrollView"
   - Rect Transform:
     - Anchor: Stretch (tous côtés)
     - Left: 50, Right: 50, Top: 150, Bottom: 150

3. **Container pour les produits**:
   - Sous ScrollView → Viewport → Content
   - Déjà existant, nommer "ProductListContainer"
   - Add Component → Vertical Layout Group
     - Spacing: 10
     - Child Force Expand: Width ✓

4. **Filtres (en haut)**:
   - Canvas → UI → Toggle → "BioToggle"
   - Canvas → UI → Toggle → "EcoToggle"
   - Canvas → UI → Toggle → "PriceToggle"
   - Canvas → UI → Toggle → "HealthToggle"
   - Position: Top de la scène

5. **Bouton Retour**:
   - Canvas → UI → Button → "BackButton"
   - Position: Bottom

#### Connexion RecommendationsController
1. Canvas → Add Component → **RecommendationsController**
2. Connecter:
   - `productListContainer` → ProductListContainer (Content de ScrollView)
   - `bioToggle` → BioToggle
   - `ecoToggle` → EcoToggle
   - `priceToggle` → PriceToggle
   - `healthToggle` → HealthToggle
   - `backButton` → BackButton
   - `productCardPrefab` → (Laisser vide pour l'instant)

---

### 5. ProfileScene 🔧

#### Configuration Manuelle

**Créer les éléments:**

1. **Canvas** (standard)

2. **Nom d'utilisateur**:
   - Text (TMP) → "UserNameText" (affichage)
   - Input Field (TMP) → "UserNameInput" (édition)

3. **Préférences alimentaires**:
   - Toggle → "VegetarianToggle" + Label "Végétarien"
   - Toggle → "VeganToggle" + Label "Vegan"
   - Toggle → "GlutenFreeToggle" + Label "Sans Gluten"
   - Toggle → "LactoseFreeToggle" + Label "Sans Lactose"

4. **Paramètres**:
   - Toggle → "NotificationsToggle" + Label "Notifications"
   - Toggle → "EcoModeToggle" + Label "Mode Éco"

5. **Historique**:
   - Scroll View → "HistoryScrollView"
   - Content → "HistoryContainer"

6. **Statistiques**:
   - Text (TMP) → "TotalScansText"

7. **Boutons**:
   - Button → "SaveButton" (texte: "Sauvegarder")
   - Button → "BackButton" (texte: "Retour")

#### Connexion ProfileController
1. Canvas → Add Component → **ProfileController**
2. Connecter toutes les références selon les noms ci-dessus

---

## Comment les Composants Fonctionnent Ensemble

### 1. Système de Navigation

```csharp
// NavigationManager.cs (Singleton)
NavigationManager.Instance.GoToScanner();  // Charge ScannerScene
NavigationManager.Instance.GoToProductInfo();  // Charge ProductInfoScene
NavigationManager.Instance.NavigateBack();  // Retour scène précédente
```

**Utilisé par:**
- HomeController: Boutons Scanner et Profil
- ScannerController: Bouton Back
- ProductInfoController: Boutons Alternatives et Back
- RecommendationsController: Bouton Back
- ProfileController: Bouton Back

### 2. Gestion des Produits

```csharp
// QRCodeManager.cs (Singleton)
QRCodeManager.Instance.SetCurrentProductId("prod001");  // Définir produit actuel
string id = QRCodeManager.Instance.GetCurrentProductId();  // Récupérer ID
```

**Workflow:**
1. **ScannerController**: Scan QR → Appelle `SetCurrentProductId()`
2. **ProductInfoController**: `Start()` → Appelle `GetCurrentProductId()` → Charge les données

### 3. Base de Données Produits

```csharp
// ProductDatabase.cs (Singleton)
ProductData product = ProductDatabase.Instance.GetProduct("prod001");
List<ProductData> alternatives = ProductDatabase.Instance.GetAlternatives("prod001");
```

### 4. Préférences Utilisateur

```csharp
// UserPreferences.cs
UserPreferences prefs = UserPreferences.Load();  // Charger depuis PlayerPrefs
prefs.AddToHistory("prod001");  // Ajouter à l'historique
prefs.Save();  // Sauvegarder
```

---

## Flux de Données Complet

### Exemple: Scanner un Produit

```
1. HomeScene
   └─ Utilisateur clique "Scanner Produit"
   └─ HomeController.OnScanButtonClicked()
   └─ NavigationManager.Instance.GoToScanner()

2. ScannerScene chargée
   └─ ScannerController.Start()
   └─ Utilisateur clique "Scanner"
   └─ ScannerController.SimulateScan()
   └─ Produit aléatoire sélectionné (ex: "prod005")
   └─ QRCodeManager.Instance.SetCurrentProductId("prod005")
   └─ NavigationManager.Instance.GoToProductInfo()

3. ProductInfoScene chargée
   └─ ProductInfoController.Start()
   └─ LoadProduct()
   └─ productId = QRCodeManager.Instance.GetCurrentProductId() // "prod005"
   └─ product = ProductDatabase.Instance.GetProduct("prod005")
   └─ DisplayProduct() // Affiche les infos
   └─ UserPreferences.Load().AddToHistory("prod005")

4. Utilisateur clique "Voir les alternatives"
   └─ ProductInfoController.OnAlternativesButtonClicked()
   └─ NavigationManager.Instance.GoToRecommendations()

5. RecommendationsScene chargée
   └─ RecommendationsController.Start()
   └─ LoadAlternatives()
   └─ productId = QRCodeManager.Instance.GetCurrentProductId()
   └─ alternatives = ProductDatabase.Instance.GetAlternatives("prod005")
   └─ DisplayAlternatives() // Affiche la liste
```

---

## Résolution de l'Erreur "No product ID available"

### Cause
Cette erreur apparaît dans **ProductInfoController.cs ligne 66** quand:
```csharp
string productId = QRCodeManager.Instance.GetCurrentProductId();
if (string.IsNullOrEmpty(productId)) {
    Debug.LogError("No product ID available");  // ← ICI
    return;
}
```

### Solution
**L'ID produit doit être défini AVANT de charger ProductInfoScene**.

Cela se produit normalement dans ScannerScene:
```csharp
QRCodeManager.Instance.SetCurrentProductId(productId);
```

**Pourquoi vous voyez l'erreur:**
- Vous cliquez sur "Scanner Produit" dans HomeScene
- ScannerScene se charge
- MAIS ScannerScene n'a pas d'UI configurée
- ScannerController ne peut pas fonctionner
- Donc aucun produit n'est scanné
- Donc l'ID n'est jamais défini
- Si vous passez quand même à ProductInfoScene → Erreur!

**Correction:**
Configurez ScannerScene complètement AVANT de tester la navigation!

---

## Liste de Vérification Complète

### HomeScene ✅
- [ ] Canvas existe
- [ ] WelcomeText, ScanButton, ProfileButton créés
- [ ] HomeController attaché au Canvas
- [ ] Toutes les références connectées
- [ ] Test Play: Boutons visibles et cliquables

### ScannerScene
- [ ] Canvas existe
- [ ] CameraView, StatusText, ScanButton, TorchButton, BackButton créés
- [ ] ScannerController attaché au Canvas
- [ ] Toutes les références connectées
- [ ] testMode = true
- [ ] Test Play: Interface scanner visible
- [ ] Test Scan: Clic sur Scanner charge un produit

### ProductInfoScene
- [ ] Canvas existe
- [ ] Tous les Text pour infos produit créés
- [ ] Sliders pour les scores créés
- [ ] Boutons Alternatives et Back créés
- [ ] ProductInfoController attaché au Canvas
- [ ] Toutes les références connectées (minimum 10+)
- [ ] Test: Navigation depuis Scanner affiche les infos

### RecommendationsScene
- [ ] Canvas avec ScrollView
- [ ] Toggles pour filtres créés
- [ ] BackButton créé
- [ ] RecommendationsController attaché
- [ ] Références connectées

### ProfileScene
- [ ] Canvas avec tous les éléments UI
- [ ] ProfileController attaché
- [ ] Références connectées

---

## Raccourci: Script de Test Simple

Si vous voulez tester rapidement sans configurer toutes les scènes, créez ce script de test:

```csharp
// TestNavigation.cs - Mettre dans Assets/Scripts/Utils/
using UnityEngine;
using SmartRetailAR.Utils;

public class TestNavigation : MonoBehaviour
{
    void Start()
    {
        // Définir un produit par défaut pour les tests
        QRCodeManager.Instance.SetCurrentProductId("prod001");
        Debug.Log("Test product ID set to prod001");
    }
}
```

Attachez-le à n'importe quel GameObject dans HomeScene. Cela permet de tester ProductInfoScene même si ScannerScene n'est pas configurée.

---

## Commandes Unity Utiles

### Navigation Rapide
- **Ctrl+1** : Scene view
- **Ctrl+2** : Game view
- **Ctrl+7** : Inspector
- **Play/Stop** : Ctrl+P

### Sauvegarder
- **Ctrl+S** : Sauvegarder la scène courante
- **Ctrl+Shift+S** : Sauvegarder toutes les scènes

### Trouver un GameObject
- Dans Hierarchy, taper le nom dans la barre de recherche en haut

---

## Ordre d'Exécution Recommandé

1. **Configurer ScannerScene** (30 min)
   - Utiliser SetupUI.cs auto-setup
   - Connecter ScannerController
   - Tester: Play dans ScannerScene

2. **Configurer ProductInfoScene** (45 min)
   - Utiliser SetupUI.cs auto-setup
   - Ajouter les éléments manquants (sliders, image)
   - Connecter ProductInfoController
   - Tester: Définir manuellement un ID produit avec TestNavigation.cs

3. **Test Navigation HomeScene → Scanner → ProductInfo** (15 min)
   - Démarrer depuis HomeScene
   - Cliquer Scanner Produit
   - Cliquer Scanner dans ScannerScene
   - Vérifier ProductInfoScene affiche les infos

4. **Configurer RecommendationsScene** (1h)
   - Configuration manuelle complète
   - Tester depuis ProductInfoScene

5. **Configurer ProfileScene** (1h)
   - Configuration manuelle complète
   - Tester depuis HomeScene

---

## Temps Total Estimé

- Configuration automatique (Home + Scanner + ProductInfo): **1-2 heures**
- Configuration manuelle (Recommendations + Profile): **2-3 heures**
- **Total**: 3-5 heures pour toutes les scènes complètes

---

## Support et Débogage

### Logs Unity
```bash
tail -f ~/.config/unity3d/Editor.log
```

### Vérifier les Références
Dans Unity Editor, sélectionner le Controller:
- Inspector affiche "Missing" en rouge si référence non connectée
- Toutes les références doivent avoir un GameObject assigné

### Console Unity
- Window → General → Console (Ctrl+Shift+C)
- Filtrer par Error/Warning/Log

---

**Version**: 1.1  
**Dernière mise à jour**: Décembre 2024  
**Pour**: Unity 2022.3.62f3 sur Ubuntu 24.04
