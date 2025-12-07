# Guide Complet d'Intégration des Scènes - Smart Retail AR

**Document officiel pour intégrer toutes les scènes avec toutes les fonctionnalités**

## 📋 Table des Matières

1. [Préparation Initiale](#préparation-initiale)
2. [Scène 1: HomeScene](#scène-1-homescene)
3. [Scène 2: ScannerScene](#scène-2-scannerscene)
4. [Scène 3: ProductInfoScene](#scène-3-productinfoscene)
5. [Scène 4: RecommendationsScene](#scène-4-recommendationsscene)
6. [Scène 5: ProfileScene](#scène-5-profilescene)
7. [Scène 6: MainScene](#scène-6-mainscene)
8. [Tests et Validation](#tests-et-validation)
9. [Dépannage](#dépannage)

---

## ⚙️ Préparation Initiale

**Durée: 10 minutes**

### Étape 1: Importer TextMeshPro

```
1. Ouvrir Unity 2022.3.62f3
2. Ouvrir le projet ~/smart-retail-ar
3. Window → TextMeshPro → Import TMP Essential Resources
4. Cliquer "Import" dans la fenêtre
5. Attendre la fin de l'import (10 secondes)
6. Fermer Unity complètement
7. Rouvrir Unity avec le projet
```

### Étape 2: Vérifier la Structure du Projet

```
Assets/
├── Scenes/
│   ├── HomeScene.unity
│   ├── ScannerScene.unity
│   ├── ProductInfoScene.unity
│   ├── RecommendationsScene.unity
│   ├── ProfileScene.unity
│   └── MainScene.unity
├── Scripts/
│   ├── Data/
│   ├── UI/
│   └── Utils/
└── Resources/
    └── ProductDatabase.json
```

### Étape 3: Configurer Build Settings

```
1. File → Build Settings
2. Platform: Android ou iOS
3. Add Open Scenes → Ajouter toutes les 6 scènes dans l'ordre:
   - MainScene (index 0)
   - HomeScene (index 1)
   - ScannerScene (index 2)
   - ProductInfoScene (index 3)
   - RecommendationsScene (index 4)
   - ProfileScene (index 5)
4. Cliquer "Switch Platform" si nécessaire
```

---

## 🏠 Scène 1: HomeScene

**Durée: 15 minutes**  
**Méthode: Automatique avec SetupUI**

### Étapes d'Intégration

#### 1. Ouvrir la Scène

```
File → Open Scene → Assets/Scenes/HomeScene.unity
```

#### 2. Créer l'UI Automatiquement

```
1. Hierarchy → Clic droit → Create Empty
2. Renommer: "UISetup" (touche F2)
3. Inspector → Add Component → taper "SetupUI"
4. Clic droit sur "Setup UI (Script)" dans Inspector
5. Sélectionner: "Setup Home Scene UI"
```

**✅ Résultat:** Canvas créé avec WelcomeText, ScanButton, ProfileButton, EventSystem

#### 3. Ajouter le Controller

```
1. Dans Hierarchy, sélectionner Canvas
2. Inspector → Add Component → taper "HomeController"
```

#### 4. Connecter les Composants

```
Dans Inspector, section HomeController:
- Welcome Text: Glisser "WelcomeText" depuis Hierarchy
- Scan Button: Glisser "ScanButton" depuis Hierarchy
- Profile Button: Glisser "ProfileButton" depuis Hierarchy
```

#### 5. Personnaliser (Optionnel)

**Modifier le texte de bienvenue:**
```
1. Sélectionner WelcomeText dans Hierarchy
2. Inspector → Text Input (TMP) → changer le texte
3. Exemples: "Bienvenue!", "Smart Retail AR", "Scanner pour commencer"
```

**Modifier les couleurs des boutons:**
```
1. Sélectionner ScanButton
2. Inspector → Image → Color → choisir une couleur
3. Répéter pour ProfileButton
```

#### 6. Sauvegarder et Tester

```
1. Ctrl+S pour sauvegarder
2. Cliquer Play (▶️)
3. Vérifier que les boutons apparaissent
4. Cliquer "Scanner Produit" → devrait charger ScannerScene (pas encore configurée)
5. Stop (⏹️)
```

---

## 📸 Scène 2: ScannerScene

**Durée: 20 minutes**  
**Méthode: Automatique avec SetupUI + Configuration AR**

### Étapes d'Intégration

#### 1. Ouvrir la Scène

```
File → Open Scene → Assets/Scenes/ScannerScene.unity
```

#### 2. Créer l'UI Automatiquement

```
1. Hierarchy → Clic droit → Create Empty
2. Renommer: "UISetup"
3. Add Component → "SetupUI"
4. Clic droit sur SetupUI → "Setup Scanner Scene UI"
```

**✅ Résultat:** Canvas créé avec CameraView, ScanFrame, ScanButton, TorchButton, BackButton, StatusText

#### 3. Ajouter le Controller

```
1. Sélectionner Canvas dans Hierarchy
2. Add Component → "ScannerController"
```

#### 4. Connecter les Composants

```
Dans Inspector, section ScannerController:
- Camera View: Glisser "CameraView" depuis Hierarchy
- Scan Button: Glisser "ScanButton"
- Torch Button: Glisser "TorchButton"
- Back Button: Glisser "BackButton"
- Status Text: Glisser "StatusText"
- Scan Frame: Glisser "ScanFrame"
```

#### 5. Activer le Mode Test

```
Dans Inspector, section ScannerController:
☑ Test Mode (cocher cette case)
```

**💡 Important:** Le mode test permet de tester sans caméra AR réelle - il génère des scans aléatoires

#### 6. Configurer AR (Pour Production)

**Note:** Pour Sprint 2 - AR Camera Integration

```
1. Hierarchy → Clic droit → XR → AR Session
2. Hierarchy → Clic droit → XR → AR Session Origin
3. Sous AR Session Origin → Sélectionner AR Camera
4. Inspector → AR Camera Manager → vérifier configuration
```

#### 7. Sauvegarder et Tester

```
1. Ctrl+S
2. Play (▶️)
3. Cliquer "Scanner" → génère un scan test
4. Devrait charger ProductInfoScene avec un produit aléatoire
5. Stop (⏹️)
```

---

## 📦 Scène 3: ProductInfoScene

**Durée: 25 minutes**  
**Méthode: Automatique avec SetupUI**

### Étapes d'Intégration

#### 1. Ouvrir la Scène

```
File → Open Scene → Assets/Scenes/ProductInfoScene.unity
```

#### 2. Créer l'UI Automatiquement

```
1. Create Empty → "UISetup"
2. Add Component → "SetupUI"
3. Clic droit → "Setup Product Info Scene UI"
```

**✅ Résultat:** ScrollView créé avec 13 éléments UI

#### 3. Ajouter le Controller

```
1. Sélectionner Canvas
2. Add Component → "ProductInfoController"
```

#### 4. Connecter TOUS les Composants (13 champs)

```
Dans Inspector, section ProductInfoController:

Champ                     GameObject à glisser
─────────────────────────────────────────────────
Product Name Text      ← ProductNameText
Product Image          ← ProductImage
Brand Text             ← BrandText
Category Text          ← CategoryText
Health Score Text      ← HealthScoreText
Eco Score Text         ← EcoScoreText
Nutrition Title Text   ← NutritionTitleText
Nutrition Text         ← NutritionText
Origin Text            ← OriginText
Tags Text              ← TagsText
Alternatives Button    ← AlternativesButton
Back Button            ← BackButton
Scroll View            ← ScrollView (composant ScrollRect)
```

**💡 Astuce:** Pour ScrollView, glisser le GameObject "ScrollView", pas "Content"

#### 5. Configurer les Scores (Optionnel)

**Personnaliser les couleurs des scores:**

```
Sélectionner HealthScoreText:
- Inspector → Color → #00FF00 (vert pour santé)

Sélectionner EcoScoreText:
- Inspector → Color → #00AA00 (vert foncé pour écologie)
```

#### 6. Sauvegarder et Tester

```
1. Ctrl+S
2. Pour tester avec un produit:
   - Ouvrir ScannerScene
   - Play → Scanner → devrait afficher ProductInfoScene
   - Vérifier toutes les informations du produit
3. Stop
```

---

## 🎯 Scène 4: RecommendationsScene

**Durée: 90 minutes**  
**Méthode: Manuelle (SetupUI non supporté)**

### Étapes d'Intégration

#### 1. Ouvrir la Scène

```
File → Open Scene → Assets/Scenes/RecommendationsScene.unity
```

#### 2. Créer le Canvas Principal

```
1. Hierarchy → Clic droit → UI → Canvas
2. Sélectionner Canvas
3. Inspector → Canvas Scaler:
   - UI Scale Mode: Scale With Screen Size
   - Reference Resolution: 1920 x 1080
```

#### 3. Créer le Titre

```
1. Clic droit sur Canvas → UI → Text - TextMeshPro
2. Renommer: "TitleText"
3. Inspector → TextMeshPro - Text (UI):
   - Text: "Recommandations"
   - Font Size: 48
   - Alignment: Center + Middle
   - Color: Blanc
4. Rect Transform:
   - Anchor: Top Center
   - Pos Y: -50
   - Width: 800, Height: 80
```

#### 4. Créer le FiltersPanel

```
1. Clic droit sur Canvas → UI → Panel
2. Renommer: "FiltersPanel"
3. Rect Transform:
   - Anchor: Top Stretch
   - Left: 0, Right: 0
   - Top: -120, Height: 100
4. Add Component → Horizontal Layout Group:
   - Padding: Left/Right/Top/Bottom: 20
   - Spacing: 20
   - Child Alignment: Middle Center
   - Child Force Expand: Width ☑, Height ☑
```

#### 5. Créer les 4 Filtres (Toggles)

**Pour chaque filtre, répéter:**

```
1. Clic droit sur FiltersPanel → UI → Toggle
2. Renommer selon le filtre:
   - "BioToggle"
   - "EcoToggle"
   - "PriceToggle"
   - "HealthToggle"
3. Sélectionner le Toggle
4. Dans Hierarchy, dérouler le Toggle → sélectionner "Label"
5. Inspector → Text: changer selon le filtre:
   - "Bio"
   - "Écologique"
   - "Prix"
   - "Santé"
6. Rect Transform du Toggle:
   - Width: 200, Height: 60
```

#### 6. Créer le ScrollView pour la Liste

```
1. Clic droit sur Canvas → UI → Scroll View
2. Renommer: "ProductListScrollView"
3. Rect Transform:
   - Anchor: Stretch Stretch
   - Left: 20, Right: 20
   - Top: -230, Bottom: 20
4. Inspector → Scroll Rect:
   - Vertical: ☑
   - Horizontal: ☐
   - Movement Type: Elastic
```

#### 7. Configurer le Content

```
1. Dérouler ProductListScrollView dans Hierarchy
2. Sélectionner "Content"
3. Add Component → Vertical Layout Group:
   - Padding: 10 partout
   - Spacing: 10
   - Child Force Expand: Width ☑, Height ☐
4. Add Component → Content Size Fitter:
   - Vertical Fit: Preferred Size
```

#### 8. Créer le ProductCardTemplate

```
1. Clic droit sur Content → UI → Panel
2. Renommer: "ProductCardTemplate"
3. Rect Transform:
   - Width: 350, Height: 120
4. Inspector → Layout Element (Add Component):
   - Preferred Height: 120 ☑
   - Min Height: 120
```

#### 9. Créer les Éléments du Card

**Image du produit:**
```
1. Clic droit sur ProductCardTemplate → UI → Image
2. Renommer: "ProductImage"
3. Rect Transform:
   - Anchor: Left Center
   - Pos X: 60, Pos Y: 0
   - Width: 100, Height: 100
```

**Nom du produit:**
```
1. Clic droit sur ProductCardTemplate → UI → Text - TextMeshPro
2. Renommer: "NameText"
3. Text: "Nom du Produit"
4. Font Size: 20
5. Rect Transform:
   - Anchor: Top Stretch
   - Left: 120, Right: 10
   - Top: -10, Height: 30
```

**Marque:**
```
1. Clic droit sur ProductCardTemplate → UI → Text - TextMeshPro
2. Renommer: "BrandText"
3. Text: "Marque"
4. Font Size: 16
5. Color: Gris (0.5, 0.5, 0.5, 1)
6. Rect Transform:
   - Anchor: Top Stretch
   - Left: 120, Right: 10
   - Top: -45, Height: 25
```

**Score santé:**
```
1. Clic droit sur ProductCardTemplate → UI → Text - TextMeshPro
2. Renommer: "HealthScoreText"
3. Text: "Santé: 8/10"
4. Font Size: 14
5. Color: Vert (0, 1, 0, 1)
6. Rect Transform:
   - Anchor: Bottom Left
   - Pos X: 120, Pos Y: 15
   - Width: 100, Height: 25
```

**Score éco:**
```
1. Clic droit sur ProductCardTemplate → UI → Text - TextMeshPro
2. Renommer: "EcoScoreText"
3. Text: "Éco: 7/10"
4. Font Size: 14
5. Color: Vert foncé (0, 0.7, 0, 1)
6. Rect Transform:
   - Anchor: Bottom Left
   - Pos X: 230, Pos Y: 15
   - Width: 100, Height: 25
```

#### 10. Désactiver le Template

```
1. Sélectionner ProductCardTemplate dans Hierarchy
2. Inspector → En haut à gauche, décocher la case
   (Le GameObject devient grisé - c'est normal, c'est un template)
```

#### 11. Ajouter le Controller

```
1. Sélectionner Canvas
2. Add Component → "RecommendationsController"
```

#### 12. Connecter TOUS les Composants

```
Dans Inspector, section RecommendationsController:

Champ                    GameObject à glisser
────────────────────────────────────────────────
Title Text            ← TitleText
Bio Toggle            ← BioToggle
Eco Toggle            ← EcoToggle
Price Toggle          ← PriceToggle
Health Toggle         ← HealthToggle
Product List          ← Content (sous ProductListScrollView)
Product Card Template ← ProductCardTemplate
Scroll View           ← ProductListScrollView
```

#### 13. Sauvegarder et Tester

```
1. Ctrl+S
2. Play (▶️)
3. Activer/désactiver les filtres → la liste doit se mettre à jour
4. Vérifier que les produits s'affichent avec toutes les infos
5. Stop
```

---

## 👤 Scène 5: ProfileScene

**Durée: 75 minutes**  
**Méthode: Manuelle (SetupUI non supporté)**

### Étapes d'Intégration

#### 1. Ouvrir la Scène

```
File → Open Scene → Assets/Scenes/ProfileScene.unity
```

#### 2. Créer le Canvas Principal

```
1. Hierarchy → Clic droit → UI → Canvas
2. Canvas Scaler: Scale With Screen Size (1920x1080)
```

#### 3. Créer la Section Nom d'Utilisateur

**Texte d'affichage:**
```
1. Clic droit sur Canvas → UI → Text - TextMeshPro
2. Renommer: "UserNameText"
3. Text: "Utilisateur"
4. Font Size: 32
5. Rect Transform:
   - Anchor: Top Center
   - Pos Y: -50, Width: 400, Height: 60
```

**Champ de saisie:**
```
1. Clic droit sur Canvas → UI → Input Field - TextMeshPro
2. Renommer: "UserNameInput"
3. Placeholder: "Entrez votre nom"
4. Rect Transform:
   - Anchor: Top Center
   - Pos Y: -120, Width: 400, Height: 50
```

#### 4. Créer la Section Préférences Alimentaires

**Panel des préférences:**
```
1. Clic droit sur Canvas → UI → Panel
2. Renommer: "PreferencesSection"
3. Rect Transform:
   - Anchor: Top Stretch
   - Left: 50, Right: 50
   - Top: -200, Height: 300
4. Add Component → Vertical Layout Group:
   - Padding: 20 partout
   - Spacing: 15
```

**Titre de la section:**
```
1. Clic droit sur PreferencesSection → UI → Text - TextMeshPro
2. Renommer: "PreferencesTitle"
3. Text: "Préférences Alimentaires"
4. Font Size: 28
5. Add Component → Layout Element:
   - Preferred Height: 40
```

**Créer les 4 Toggles:**

```
Pour chaque toggle:
1. Clic droit sur PreferencesSection → UI → Toggle
2. Renommer (dans l'ordre):
   - VegetarianToggle
   - VeganToggle
   - GlutenFreeToggle
   - LactoseFreeToggle
3. Modifier le Label (enfant du Toggle):
   - "Végétarien"
   - "Végan"
   - "Sans Gluten"
   - "Sans Lactose"
4. Add Component → Layout Element:
   - Preferred Height: 50
```

#### 5. Créer la Section Paramètres

**Panel des paramètres:**
```
1. Clic droit sur Canvas → UI → Panel
2. Renommer: "SettingsSection"
3. Rect Transform:
   - Anchor: Top Stretch
   - Left: 50, Right: 50
   - Top: -520, Height: 150
4. Add Component → Vertical Layout Group:
   - Padding: 20, Spacing: 15
```

**Titre:**
```
1. Clic droit sur SettingsSection → UI → Text - TextMeshPro
2. Renommer: "SettingsTitle"
3. Text: "Paramètres"
4. Font Size: 28
```

**Créer 2 Toggles de paramètres:**

```
1. Clic droit sur SettingsSection → UI → Toggle
2. Renommer: "NotificationsToggle"
3. Label: "Notifications activées"

1. Clic droit sur SettingsSection → UI → Toggle
2. Renommer: "EcoModeToggle"
3. Label: "Mode écologique"
```

#### 6. Créer la Section Historique

**Panel historique:**
```
1. Clic droit sur Canvas → UI → Panel
2. Renommer: "HistorySection"
3. Rect Transform:
   - Anchor: Top Stretch
   - Left: 50, Right: 50
   - Top: -690, Height: 250
```

**Titre:**
```
1. Clic droit sur HistorySection → UI → Text - TextMeshPro
2. Renommer: "HistoryTitle"
3. Text: "Historique des Scans"
4. Font Size: 28
5. Rect Transform:
   - Anchor: Top Stretch
   - Top: -10, Height: 40
```

**ScrollView pour l'historique:**
```
1. Clic droit sur HistorySection → UI → Scroll View
2. Renommer: "HistoryScrollView"
3. Rect Transform:
   - Anchor: Stretch Stretch
   - Left: 10, Right: 10
   - Top: -60, Bottom: 10
4. Scroll Rect: Vertical ☑, Horizontal ☐
```

**Configurer le Content:**
```
1. Sélectionner Content (sous HistoryScrollView)
2. Renommer: "HistoryContainer"
3. Add Component → Vertical Layout Group:
   - Spacing: 5
4. Add Component → Content Size Fitter:
   - Vertical Fit: Preferred Size
```

#### 7. Créer la Section Statistiques

**Panel statistiques:**
```
1. Clic droit sur Canvas → UI → Panel
2. Renommer: "StatisticsSection"
3. Rect Transform:
   - Anchor: Bottom Stretch
   - Left: 50, Right: 50
   - Bottom: 20, Height: 120
4. Add Component → Horizontal Layout Group:
   - Padding: 20, Spacing: 20
```

**Texte des statistiques:**
```
1. Clic droit sur StatisticsSection → UI → Text - TextMeshPro
2. Renommer: "TotalScansText"
3. Text: "Total Scans: 0"
4. Font Size: 20
5. Add Component → Layout Element:
   - Flexible Width: 1
```

#### 8. Créer les Boutons d'Action

**Bouton Sauvegarder:**
```
1. Clic droit sur Canvas → UI → Button - TextMeshPro
2. Renommer: "SaveButton"
3. Rect Transform:
   - Anchor: Bottom Center
   - Pos X: -110, Pos Y: 180
   - Width: 200, Height: 60
4. Modifier le texte enfant: "Sauvegarder"
```

**Bouton Retour:**
```
1. Clic droit sur Canvas → UI → Button - TextMeshPro
2. Renommer: "BackButton"
3. Rect Transform:
   - Anchor: Bottom Center
   - Pos X: 110, Pos Y: 180
   - Width: 200, Height: 60
4. Modifier le texte enfant: "Retour"
```

#### 9. Ajouter le Controller

```
1. Sélectionner Canvas
2. Add Component → "ProfileController"
```

#### 10. Connecter TOUS les Composants

```
Dans Inspector, section ProfileController:

Champ                     GameObject à glisser
─────────────────────────────────────────────────
User Name Text         ← UserNameText
User Name Input        ← UserNameInput
Save Button            ← SaveButton
Back Button            ← BackButton
Vegetarian Toggle      ← VegetarianToggle
Vegan Toggle           ← VeganToggle
Gluten Free Toggle     ← GlutenFreeToggle
Lactose Free Toggle    ← LactoseFreeToggle
Notifications Toggle   ← NotificationsToggle
Eco Mode Toggle        ← EcoModeToggle
History Container      ← HistoryContainer
Total Scans Text       ← TotalScansText
```

**⚠️ Note:** Les champs `avatarImage`, `bioToggle` et `historyItemPrefab` dans le code ne sont pas obligatoires.

#### 11. Sauvegarder et Tester

```
1. Ctrl+S
2. Play (▶️)
3. Modifier le nom d'utilisateur
4. Activer/désactiver les toggles
5. Cliquer "Sauvegarder" → les préférences sont enregistrées
6. Stop → Relancer → les préférences doivent être conservées
```

---

## 🚀 Scène 6: MainScene

**Durée: 5 minutes**  
**Méthode: Déjà configurée**

### Vérification

#### 1. Ouvrir la Scène

```
File → Open Scene → Assets/Scenes/MainScene.unity
```

#### 2. Vérifier le NavigationManager

```
1. Hierarchy → devrait contenir "NavigationManager"
2. Sélectionner NavigationManager
3. Inspector → Script "Navigation Manager" attaché
4. Component DontDestroyOnLoad présent
```

#### 3. Vérifier les Scènes dans Build Settings

```
File → Build Settings
Vérifier que toutes les scènes sont listées:
☑ MainScene (index 0)
☑ HomeScene (index 1)
☑ ScannerScene (index 2)
☑ ProductInfoScene (index 3)
☑ RecommendationsScene (index 4)
☑ ProfileScene (index 5)
```

**✅ Cette scène est prête - aucune modification nécessaire!**

---

## ✅ Tests et Validation

### Test Complet du Flux de Navigation

#### Test 1: Démarrage de l'Application

```
1. File → Open Scene → MainScene.unity
2. Play (▶️)
3. ✓ HomeScene doit se charger automatiquement
4. ✓ Boutons "Scanner Produit" et "Mon Profil" visibles
```

#### Test 2: Navigation vers Scanner

```
1. Cliquer "Scanner Produit"
2. ✓ ScannerScene doit se charger
3. ✓ Boutons Scanner, Torche, Retour visibles
4. ✓ StatusText affiche "Prêt à scanner"
```

#### Test 3: Scan d'un Produit (Mode Test)

```
1. Dans ScannerScene, cliquer "Scanner"
2. ✓ StatusText change: "Scan en cours..." puis "Produit trouvé!"
3. ✓ ProductInfoScene se charge
4. ✓ Toutes les informations du produit s'affichent:
   - Nom, marque, catégorie
   - Scores santé et éco
   - Informations nutritionnelles
   - Origine, tags
```

#### Test 4: Alternatives de Produits

```
1. Dans ProductInfoScene, cliquer "Voir Alternatives"
2. ✓ RecommendationsScene se charge
3. ✓ Liste de produits similaires affichée
4. ✓ Filtres fonctionnels (Bio, Éco, Prix, Santé)
```

#### Test 5: Profil Utilisateur

```
1. Depuis HomeScene, cliquer "Mon Profil"
2. ✓ ProfileScene se charge
3. Modifier le nom d'utilisateur
4. Activer des préférences alimentaires
5. Cliquer "Sauvegarder"
6. ✓ Message de confirmation dans Console
7. Retour → Profil → ✓ Préférences conservées
```

#### Test 6: Boutons Retour

```
1. Depuis n'importe quelle scène
2. Cliquer bouton "Retour"
3. ✓ Retourne à la scène précédente
4. ✓ Historique de navigation fonctionne
```

### Tests de Persistance des Données

#### Test 7: Historique des Scans

```
1. Scanner 5 produits différents
2. Aller dans Profil
3. ✓ Section "Historique" montre les 5 derniers scans
4. ✓ "Total Scans: 5" affiché
5. Fermer Unity complètement
6. Rouvrir → Profil
7. ✓ Historique conservé
```

#### Test 8: Préférences Utilisateur

```
1. Profil → Activer "Végétarien" + "Sans Gluten"
2. Sauvegarder
3. Fermer Unity
4. Rouvrir → Profil
5. ✓ Toggles toujours activés
6. Recommandations
7. ✓ Filtrage basé sur les préférences
```

---

## 🔧 Dépannage

### Problème 1: Menu Contextuel SetupUI n'apparaît pas

**Symptôme:** Clic droit sur SetupUI ne montre pas les options

**Solutions:**
```
1. Vérifier TextMeshPro importé:
   Window → TextMeshPro → Import TMP Essential Resources

2. Redémarrer Unity complètement

3. Vérifier le script:
   Assets/Scripts/Utils/SetupUI.cs
   Ligne 18-20 doivent contenir [ContextMenu(...)]

4. Si toujours pas visible → Créer l'UI manuellement (voir DETAILS_SCENE.md)
```

### Problème 2: Impossible de Glisser-Déposer les Composants

**Symptôme:** Les GameObjects ne peuvent pas être glissés vers les champs du Controller

**Solutions:**
```
1. Vérifier que le GameObject est du bon type:
   - TextMeshProUGUI pour les champs texte
   - Button pour les boutons
   - Toggle pour les toggles
   - Image pour les images

2. Scripts à jour:
   - Tous les Controllers utilisent TextMeshProUGUI
   - Commit 0f07180 a corrigé cette incompatibilité

3. Redémarrer Unity pour recharger les scripts

4. Vérifier dans Console (Ctrl+Shift+C) s'il y a des erreurs
```

### Problème 3: "No product ID available" dans ProductInfoScene

**Symptôme:** Erreur dans la console lors de l'ouverture de ProductInfoScene

**Cause:** ScannerScene n'a pas envoyé d'ID produit

**Solutions:**
```
1. Vérifier ScannerController:
   - testMode doit être ☑ (coché)
   - Tous les champs connectés

2. Vérifier QRCodeManager:
   Window → Console → filtrer par "QRCodeManager"
   Doit voir: "Product scanned: [product-id]"

3. Tester le flux complet:
   HomeScene → Scanner → Cliquer "Scanner" → ProductInfo
```

### Problème 4: Liste de Recommandations Vide

**Symptôme:** RecommendationsScene affiche une liste vide

**Solutions:**
```
1. Vérifier ProductDatabase:
   Assets/Resources/ProductDatabase.json doit exister
   Doit contenir 52 produits

2. Vérifier ProductCardTemplate:
   - Doit être DÉSACTIVÉ (grisé dans Hierarchy)
   - Doit être enfant de Content

3. Console → Filtrer "Recommendations"
   Vérifier les messages de debug

4. Tester les filtres:
   - Activer/désactiver chaque filtre
   - Vérifier que la liste se met à jour
```

### Problème 5: Préférences Non Sauvegardées

**Symptôme:** Les préférences ne persistent pas après redémarrage

**Solutions:**
```
1. Vérifier ProfileController:
   - Bouton "Sauvegarder" connecté
   - onClick listener configuré

2. Console → Chercher "Preferences saved"
   Doit apparaître après clic sur Sauvegarder

3. Vérifier PlayerPrefs (Windows):
   Registre: HKEY_CURRENT_USER\Software\[CompanyName]\[ProductName]

4. Vérifier PlayerPrefs (Mac):
   ~/Library/Preferences/[bundle identifier].plist

5. Forcer la sauvegarde:
   ProfileController.cs ligne 285: _preferences.Save();
```

### Problème 6: Écran Bleu dans les Builds

**Symptôme:** Build Android/iOS affiche écran bleu

**Cause:** Scènes non configurées dans Build Settings

**Solutions:**
```
1. File → Build Settings
2. Add Open Scenes → Ajouter TOUTES les scènes
3. Vérifier l'ordre:
   0. MainScene
   1. HomeScene
   2. ScannerScene
   3. ProductInfoScene
   4. RecommendationsScene
   5. ProfileScene

4. Player Settings:
   - Default Orientation: Portrait ou Sensor
   - Minimum API Level: 24+ (Android)
   - Target iOS: 12.0+ (iOS)
```

### Problème 7: Texte TextMeshPro Invisible

**Symptôme:** Texte créé mais n'apparaît pas

**Solutions:**
```
1. Sélectionner le Text GameObject
2. Inspector → TextMeshPro - Text (UI):
   - Font Asset: "LiberationSans SDF" (ou autre)
   - Font Size: > 0 (recommandé 24-48)
   - Color: Alpha = 255 (ou 1.0)
   - Vertex Color: Blanc

3. Rect Transform:
   - Width et Height suffisants (min 100x50)

4. Canvas:
   - Vérifier qu'un Canvas existe
   - EventSystem présent dans la scène
```

### Problème 8: Erreurs de Compilation

**Symptôme:** Erreurs CS... dans la console

**Solutions courantes:**
```
1. Missing using directive:
   Ajouter en haut du script:
   using UnityEngine.UI;
   using TMPro;
   using SmartRetailAR.Data;
   using SmartRetailAR.Utils;

2. Type mismatch:
   Vérifier que Text → TextMeshProUGUI
   Commit 0f07180 a corrigé cela

3. Namespace issues:
   Tous les scripts doivent être dans:
   namespace SmartRetailAR.[Data|UI|Utils]

4. Nettoyer et recompiler:
   Assets → Reimport All
   Redémarrer Unity
```

---

## 📊 Checklist Finale

### ✅ Toutes les Scènes Configurées

- [ ] MainScene: NavigationManager présent
- [ ] HomeScene: UI créée, HomeController connecté
- [ ] ScannerScene: UI créée, ScannerController connecté, testMode activé
- [ ] ProductInfoScene: UI créée, ProductInfoController avec 13 champs connectés
- [ ] RecommendationsScene: UI manuelle créée, RecommendationsController connecté
- [ ] ProfileScene: UI manuelle créée, ProfileController connecté

### ✅ Build Settings Configurés

- [ ] Toutes les 6 scènes ajoutées dans l'ordre
- [ ] Platform sélectionnée (Android/iOS)
- [ ] Player Settings configurés

### ✅ Assets Présents

- [ ] ProductDatabase.json dans Resources/
- [ ] Scripts compilent sans erreurs
- [ ] TextMeshPro importé

### ✅ Tests Passés

- [ ] Navigation entre toutes les scènes fonctionne
- [ ] Scan produit en mode test fonctionne
- [ ] Affichage des informations produit correct
- [ ] Filtres de recommandations fonctionnels
- [ ] Sauvegarde des préférences fonctionne
- [ ] Historique des scans enregistré

---

## 🎉 Félicitations!

**Vous avez complété l'intégration de toutes les scènes de Smart Retail AR!**

### Prochaines Étapes

1. **Sprint 2: AR Camera Integration**
   - Intégrer la caméra AR réelle
   - Implémenter la détection QR
   - Tester sur appareils physiques

2. **Sprint 3: Backend Integration** (optionnel)
   - API pour base de données produits
   - Synchronisation cloud
   - Authentification utilisateurs

3. **Sprint 4: Polish & Deployment**
   - Optimisations performances
   - Tests utilisateurs
   - Publication stores

### Ressources Additionnelles

- **DETAILS_SCENE.md**: Guide détaillé de création UI manuelle
- **COMPLETE_SETUP_GUIDE.md**: Explications flux de données
- **TECHNICAL_DOC.md**: Architecture complète
- **UBUNTU_SETUP.md**: Configuration Ubuntu 24.04

---

**Version:** 1.0  
**Date:** 2025-12-07  
**Auteur:** Sprint 1 Implementation Team  
**Contact:** Voir README.md
