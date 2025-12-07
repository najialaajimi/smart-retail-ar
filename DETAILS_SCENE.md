# Guide Détaillé: Configuration de Toutes les Scènes Unity

Ce guide fournit les instructions **complètes et détaillées** pour configurer **toutes les 6 scènes** Unity du projet Smart Retail AR, y compris la création manuelle de chaque élément UI et la connexion de tous les composants.

---

## Table des Matières

1. [HomeScene - Écran d'Accueil](#1-homescene---écran-daccueil)
2. [ScannerScene - Scanner QR Code](#2-scannerscene---scanner-qr-code)
3. [ProductInfoScene - Informations Produit](#3-productinfoscene---informations-produit)
4. [RecommendationsScene - Recommandations](#4-recommendationsscene---recommandations)
5. [ProfileScene - Profil Utilisateur](#5-profilescene---profil-utilisateur)
6. [MainScene - Scène Principale](#6-mainscene---scène-principale)
7. [Ordre de Configuration Recommandé](#ordre-de-configuration-recommandé)
8. [Comment les Composants Fonctionnent Ensemble](#comment-les-composants-fonctionnent-ensemble)

---

## 1. HomeScene - Écran d'Accueil

### Objectif
Écran de démarrage avec un message de bienvenue et deux boutons principaux pour accéder au scanner et au profil.

### Étape 1: Ouvrir la Scène

1. Dans Unity, aller à **Project** → **Assets** → **Scenes**
2. Double-cliquer sur **HomeScene.unity**

### Étape 2: Créer le Canvas et les Éléments UI

#### 2.1 Créer le Canvas Principal

1. Dans la **Hierarchy**, clic droit → **UI** → **Canvas**
2. Sélectionner le **Canvas** créé
3. Dans l'**Inspector**, configurer **Canvas Scaler**:
   - **UI Scale Mode**: `Scale With Screen Size`
   - **Reference Resolution**: `1920 x 1080`
   - **Match**: `0.5` (balance entre width et height)

4. Ajouter un **EventSystem** si non présent:
   - Si pas de "EventSystem" dans Hierarchy
   - Clic droit → **UI** → **Event System**

#### 2.2 Créer le Texte de Bienvenue

1. Clic droit sur **Canvas** → **UI** → **Text - TextMeshPro**
   - Si demandé, cliquer **"Import TMP Essentials"**
   - Attendre la fin de l'import
2. Renommer en **"WelcomeText"**
3. Dans l'**Inspector**, configurer **TextMeshProUGUI**:
   - **Text**: `"Bienvenue à Smart Retail AR!"`
   - **Font Size**: `48`
   - **Font Style**: Bold
   - **Alignment**: Horizontal = Center, Vertical = Middle
   - **Color**: Blanc (255, 255, 255, 255)
4. Configurer **Rect Transform**:
   - **Anchor Preset**: Top Center (cliquer en haut + Alt pour position)
   - **Pos X**: `0`
   - **Pos Y**: `-100`
   - **Width**: `800`
   - **Height**: `100`

#### 2.3 Créer le Bouton Scanner

1. Clic droit sur **Canvas** → **UI** → **Button - TextMeshPro**
2. Renommer en **"ScanButton"**
3. Sélectionner **ScanButton**, configurer **Rect Transform**:
   - **Anchor Preset**: Middle Center
   - **Pos X**: `0`
   - **Pos Y**: `0`
   - **Width**: `400`
   - **Height**: `80`
4. Configurer le **Button Component**:
   - **Transition**: ColorTint
   - **Normal Color**: Bleu (0, 120, 215, 255)
   - **Highlighted Color**: Bleu clair (100, 180, 255, 255)
   - **Pressed Color**: Bleu foncé (0, 80, 150, 255)
5. Ouvrir **ScanButton** dans Hierarchy (cliquer sur la flèche)
6. Sélectionner **"Text (TMP)"** sous ScanButton
7. Dans l'Inspector:
   - **Text**: `"Scanner Produit"`
   - **Font Size**: `36`
   - **Alignment**: Center + Middle
   - **Color**: Blanc

#### 2.4 Créer le Bouton Profil

1. Clic droit sur **Canvas** → **UI** → **Button - TextMeshPro**
2. Renommer en **"ProfileButton"**
3. Configurer **Rect Transform**:
   - **Anchor Preset**: Middle Center
   - **Pos X**: `0`
   - **Pos Y**: `-120`
   - **Width**: `400`
   - **Height**: `80`
4. Configurer le **Button Component**:
   - **Transition**: ColorTint
   - **Normal Color**: Vert (0, 150, 100, 255)
   - **Highlighted Color**: Vert clair (100, 200, 150, 255)
   - **Pressed Color**: Vert foncé (0, 100, 60, 255)
5. Ouvrir **ProfileButton** et sélectionner **"Text (TMP)"**
6. Dans l'Inspector:
   - **Text**: `"Mon Profil"`
   - **Font Size**: `36`
   - **Alignment**: Center + Middle
   - **Color**: Blanc

### Étape 3: Connecter le Script HomeController

1. Dans **Hierarchy**, sélectionner **Canvas**
2. Dans l'**Inspector**, cliquer **Add Component**
3. Chercher et ajouter **"HomeController"**
4. Dans le composant **HomeController**:
   - **Scan Button**: Glisser **ScanButton** depuis Hierarchy
   - **Profile Button**: Glisser **ProfileButton** depuis Hierarchy
   - **Welcome Text**: Glisser **WelcomeText** depuis Hierarchy

### Étape 4: Sauvegarder et Tester

1. **Ctrl+S** (ou File → Save) pour sauvegarder la scène
2. Cliquer sur le bouton **Play** (▶️) en haut de l'éditeur
3. Vérifier:
   - ✅ Texte "Bienvenue à Smart Retail AR!" visible
   - ✅ Bouton "Scanner Produit" cliquable
   - ✅ Bouton "Mon Profil" cliquable
   - ✅ Animation fade-in s'exécute

---

## 2. ScannerScene - Scanner QR Code

### Objectif
Interface de scan QR code avec vue caméra, bouton de scan, torche, et indicateur de statut.

### Étape 1: Ouvrir la Scène

1. Dans Unity, aller à **Project** → **Assets** → **Scenes**
2. Double-cliquer sur **ScannerScene.unity**

### Étape 2: Créer le Canvas et les Éléments UI

#### 2.1 Créer le Canvas Principal

1. **Hierarchy** → clic droit → **UI** → **Canvas**
2. Configurer **Canvas Scaler**:
   - **UI Scale Mode**: `Scale With Screen Size`
   - **Reference Resolution**: `1920 x 1080`
   - **Match**: `0.5`
3. Vérifier présence **EventSystem**

#### 2.2 Créer la Vue Caméra (Camera View)

1. Clic droit sur **Canvas** → **UI** → **Raw Image**
2. Renommer en **"CameraView"**
3. Configurer **Rect Transform**:
   - **Anchor Preset**: Stretch Both (double-cliquer sur Stretch en bas à droite + Alt)
   - **Left**: `0`, **Top**: `0`, **Right**: `0`, **Bottom**: `0`
4. Dans **Raw Image Component**:
   - **Color**: Gris foncé (50, 50, 50, 255) - placeholder pour caméra
   - **UV Rect**: X=0, Y=0, W=1, H=1

#### 2.3 Créer le Cadre de Visée (Scan Frame)

1. Clic droit sur **Canvas** → **UI** → **Image**
2. Renommer en **"ScanFrame"**
3. Configurer **Rect Transform**:
   - **Anchor Preset**: Middle Center
   - **Pos X**: `0`, **Pos Y**: `0`
   - **Width**: `400`, **Height**: `400`
4. Configurer **Image Component**:
   - **Color**: Cyan (0, 255, 255, 150) - semi-transparent
   - **Image Type**: Sliced (si vous avez un sprite de cadre)
   - Pour l'instant, laisser **Source Image** vide

#### 2.4 Créer le Texte d'Instruction

1. Clic droit sur **Canvas** → **UI** → **Text - TextMeshPro**
2. Renommer en **"InstructionText"**
3. Dans l'**Inspector**:
   - **Text**: `"Pointez vers le QR code du produit"`
   - **Font Size**: `28`
   - **Alignment**: Center + Middle
   - **Color**: Blanc
4. Configurer **Rect Transform**:
   - **Anchor Preset**: Top Center
   - **Pos Y**: `-80`
   - **Width**: `700`, **Height**: `60`

#### 2.5 Créer le Texte de Statut

1. Clic droit sur **Canvas** → **UI** → **Text - TextMeshPro**
2. Renommer en **"StatusText"**
3. Dans l'**Inspector**:
   - **Text**: `"Prêt à scanner"`
   - **Font Size**: `24`
   - **Alignment**: Center + Middle
   - **Color**: Jaune (255, 255, 0, 255)
4. Configurer **Rect Transform**:
   - **Anchor Preset**: Bottom Center
   - **Pos Y**: `200`
   - **Width**: `700`, **Height**: `50`

#### 2.6 Créer le Bouton Scan

1. Clic droit sur **Canvas** → **UI** → **Button - TextMeshPro**
2. Renommer en **"ScanButton"**
3. Configurer **Rect Transform**:
   - **Anchor Preset**: Bottom Center
   - **Pos Y**: `120`
   - **Width**: `200`, **Height**: `70`
4. Configurer couleur du bouton (bleu)
5. Texte du bouton: `"SCANNER"`, Font Size: `32`

#### 2.7 Créer le Bouton Torche

1. Clic droit sur **Canvas** → **UI** → **Button - TextMeshPro**
2. Renommer en **"TorchButton"**
3. Configurer **Rect Transform**:
   - **Anchor Preset**: Bottom Right
   - **Pos X**: `-100`, **Pos Y**: `120`
   - **Width**: `80`, **Height**: `70`
4. Configurer couleur du bouton (orange)
5. Texte du bouton: `"💡"` ou `"TORCHE"`, Font Size: `28`

#### 2.8 Créer le Bouton Retour

1. Clic droit sur **Canvas** → **UI** → **Button - TextMeshPro**
2. Renommer en **"BackButton"**
3. Configurer **Rect Transform**:
   - **Anchor Preset**: Top Left
   - **Pos X**: `100`, **Pos Y**: `-50`
   - **Width**: `120`, **Height**: `60`
4. Configurer couleur du bouton (gris)
5. Texte du bouton: `"← Retour"`, Font Size: `24`

### Étape 3: Connecter le Script ScannerController

1. Sélectionner **Canvas** dans Hierarchy
2. **Add Component** → **"ScannerController"**
3. Connecter les références (glisser-déposer depuis Hierarchy):
   - **Scan Button**: → ScanButton
   - **Torch Button**: → TorchButton
   - **Back Button**: → BackButton
   - **Status Text**: → StatusText
   - **Camera View**: → CameraView
   - **Instruction Text**: → InstructionText (optionnel)
4. **Important**: Cocher **Test Mode** = ✓ (pour tester sans caméra AR)
5. Le tableau **Test Product IDs** sera rempli automatiquement

### Étape 4: Sauvegarder et Tester

1. **Ctrl+S** pour sauvegarder
2. Cliquer **Play** (▶️)
3. Vérifier:
   - ✅ Vue caméra (gris) visible
   - ✅ Cadre de scan cyan visible
   - ✅ Instructions visibles en haut
   - ✅ Statut "Prêt à scanner" visible
   - ✅ Bouton SCANNER fonctionnel
   - ✅ En mode test, le scan simule automatiquement

---

## 3. ProductInfoScene - Informations Produit

### Objectif
Afficher les détails complets d'un produit scanné: nom, image, nutrition, scores, origine.

### Étape 1: Ouvrir la Scène

1. **Project** → **Assets** → **Scenes** → **ProductInfoScene.unity**

### Étape 2: Créer le Canvas et les Éléments UI

#### 2.1 Créer le Canvas Principal

1. **Hierarchy** → clic droit → **UI** → **Canvas**
2. Configurer **Canvas Scaler**: Scale With Screen Size (1920 x 1080)
3. Vérifier **EventSystem**

#### 2.2 Créer le Panneau Principal (ScrollView)

1. Clic droit sur **Canvas** → **UI** → **Scroll View**
2. Renommer en **"ProductScrollView"**
3. Configurer **Rect Transform**:
   - **Anchor Preset**: Stretch Both
   - Marges: **Left**: `50`, **Top**: `100`, **Right**: `50`, **Bottom**: `150`

#### 2.3 Créer le Contenu (Content Panel)

Le ScrollView crée automatiquement un panel "Content". Configurer:
1. Sélectionner **Content** sous ProductScrollView
2. Ajouter **Vertical Layout Group**:
   - **Spacing**: `20`
   - **Child Force Expand**: Width = ✓, Height = ✗
   - **Padding**: Left=20, Right=20, Top=20, Bottom=20
3. Ajouter **Content Size Fitter**:
   - **Vertical Fit**: Preferred Size

#### 2.4 Créer le Nom du Produit

1. Clic droit sur **Content** → **UI** → **Text - TextMeshPro**
2. Renommer en **"ProductNameText"**
3. Configurer:
   - **Text**: `"Nom du Produit"`
   - **Font Size**: `42`
   - **Font Style**: Bold
   - **Alignment**: Center
   - **Color**: Blanc
4. Ajouter **Layout Element**:
   - **Preferred Height**: `80`

#### 2.5 Créer l'Image du Produit

1. Clic droit sur **Content** → **UI** → **Image**
2. Renommer en **"ProductImage"**
3. Configurer:
   - **Color**: Blanc
   - **Preserve Aspect**: ✓
4. Configurer **Rect Transform**:
   - **Height**: `300`
5. Ajouter **Layout Element**:
   - **Preferred Height**: `300`

#### 2.6 Créer la Marque

1. Clic droit sur **Content** → **UI** → **Text - TextMeshPro**
2. Renommer en **"BrandText"**
3. Configurer:
   - **Text**: `"Marque: -"`
   - **Font Size**: `28`
   - **Alignment**: Center
   - **Color**: Gris clair (200, 200, 200)
4. **Layout Element** → Preferred Height: `50`

#### 2.7 Créer la Catégorie

1. Clic droit sur **Content** → **UI** → **Text - TextMeshPro**
2. Renommer en **"CategoryText"**
3. Configurer:
   - **Text**: `"Catégorie: -"`
   - **Font Size**: `24`
   - **Alignment**: Center
   - **Color**: Gris (180, 180, 180)
4. **Layout Element** → Preferred Height: `40`

#### 2.8 Créer le Score Santé

1. Clic droit sur **Content** → **UI** → **Text - TextMeshPro**
2. Renommer en **"HealthScoreText"**
3. Configurer:
   - **Text**: `"Score Santé: -"`
   - **Font Size**: `32`
   - **Font Style**: Bold
   - **Alignment**: Center
   - **Color**: Vert (100, 255, 100)
4. **Layout Element** → Preferred Height: `60`

#### 2.9 Créer le Score Écologique

1. Clic droit sur **Content** → **UI** → **Text - TextMeshPro**
2. Renommer en **"EcoScoreText"**
3. Configurer:
   - **Text**: `"Score Écologique: -"`
   - **Font Size**: `32`
   - **Font Style**: Bold
   - **Alignment**: Center
   - **Color**: Vert (100, 255, 100)
4. **Layout Element** → Preferred Height: `60`

#### 2.10 Créer le Titre Nutrition

1. Clic droit sur **Content** → **UI** → **Text - TextMeshPro**
2. Renommer en **"NutritionTitle"**
3. Configurer:
   - **Text**: `"Informations Nutritionnelles (pour 100g)"`
   - **Font Size**: `28`
   - **Font Style**: Bold
   - **Alignment**: Center
4. **Layout Element** → Preferred Height: `50`

#### 2.11 Créer les Informations Nutritionnelles

1. Clic droit sur **Content** → **UI** → **Text - TextMeshPro**
2. Renommer en **"NutritionText"**
3. Configurer:
   - **Text**: `"Calories: -\nProtéines: -\nGlucides: -\nLipides: -"`
   - **Font Size**: `24`
   - **Alignment**: Left
   - **Color**: Blanc
4. **Layout Element** → Preferred Height: `150`

#### 2.12 Créer l'Origine

1. Clic droit sur **Content** → **UI** → **Text - TextMeshPro**
2. Renommer en **"OriginText"**
3. Configurer:
   - **Text**: `"Origine: -"`
   - **Font Size**: `24`
   - **Alignment**: Center
4. **Layout Element** → Preferred Height: `50`

#### 2.13 Créer les Tags

1. Clic droit sur **Content** → **UI** → **Text - TextMeshPro**
2. Renommer en **"TagsText"**
3. Configurer:
   - **Text**: `"Tags: -"`
   - **Font Size**: `22`
   - **Alignment**: Center
   - **Color**: Cyan (100, 255, 255)
4. **Layout Element** → Preferred Height: `60`

#### 2.14 Créer le Bouton Alternatives

1. Clic droit sur **Canvas** → **UI** → **Button - TextMeshPro**
2. Renommer en **"AlternativesButton"**
3. Configurer **Rect Transform**:
   - **Anchor Preset**: Bottom Center
   - **Pos Y**: `50`
   - **Width**: `400`, **Height**: `70`
4. Configurer couleur (vert)
5. Texte: `"Voir les Alternatives"`, Font Size: `32`

#### 2.15 Créer le Bouton Retour

1. Clic droit sur **Canvas** → **UI** → **Button - TextMeshPro**
2. Renommer en **"BackButton"**
3. Configurer **Rect Transform**:
   - **Anchor Preset**: Top Left
   - **Pos X**: `100`, **Pos Y**: `-50`
   - **Width**: `120`, **Height**: `60`
4. Texte: `"← Retour"`, Font Size: `24`

### Étape 3: Connecter le Script ProductInfoController

1. Sélectionner **Canvas**
2. **Add Component** → **"ProductInfoController"**
3. Connecter toutes les références (glisser-déposer):
   - **Product Name Text**: → ProductNameText
   - **Product Image**: → ProductImage
   - **Brand Text**: → BrandText
   - **Category Text**: → CategoryText
   - **Health Score Text**: → HealthScoreText
   - **Eco Score Text**: → EcoScoreText
   - **Nutrition Text**: → NutritionText
   - **Origin Text**: → OriginText
   - **Tags Text**: → TagsText
   - **Alternatives Button**: → AlternativesButton
   - **Back Button**: → BackButton

### Étape 4: Sauvegarder et Tester

1. **Ctrl+S** pour sauvegarder
2. Pour tester cette scène:
   - Vous devez d'abord scanner un produit depuis ScannerScene
   - Ou simuler en ajoutant un ID dans QRCodeManager

---

## 4. RecommendationsScene - Recommandations

### Objectif
Liste de produits alternatifs avec filtres (Bio, Éco, Prix, Santé) et comparaison.

### Étape 1: Ouvrir la Scène

1. **Project** → **Assets** → **Scenes** → **RecommendationsScene.unity**

### Étape 2: Créer le Canvas

1. **Hierarchy** → **UI** → **Canvas**
2. Configurer **Canvas Scaler**: Scale With Screen Size (1920 x 1080)
3. Vérifier **EventSystem**

#### 2.1 Créer le Titre

1. Clic droit sur **Canvas** → **UI** → **Text - TextMeshPro**
2. Renommer en **"TitleText"**
3. Configurer:
   - **Text**: `"Produits Alternatifs"`
   - **Font Size**: `48`
   - **Font Style**: Bold
   - **Alignment**: Center
4. **Rect Transform**:
   - **Anchor**: Top Center
   - **Pos Y**: `-50`
   - **Width**: `800`, **Height**: `80`

#### 2.2 Créer le Panneau de Filtres

1. Clic droit sur **Canvas** → **UI** → **Panel**
2. Renommer en **"FiltersPanel"**
3. Configurer **Rect Transform**:
   - **Anchor**: Top Stretch
   - **Top**: `120`, **Height**: `80`
   - **Left**: `50`, **Right**: `50`
4. Configurer **Image** (background):
   - **Color**: Gris semi-transparent (100, 100, 100, 150)

#### 2.3 Créer le Layout pour les Filtres

1. Sélectionner **FiltersPanel**
2. **Add Component** → **Horizontal Layout Group**:
   - **Spacing**: `20`
   - **Padding**: All = `10`
   - **Child Alignment**: Middle Center
   - **Child Force Expand**: Both = ✓

#### 2.4 Créer les Toggle de Filtres

Pour chaque filtre (Bio, Éco, Prix, Santé), répéter:

**Filtre BIO:**
1. Clic droit sur **FiltersPanel** → **UI** → **Toggle**
2. Renommer en **"BioToggle"**
3. Ajouter **Layout Element**:
   - **Preferred Width**: `200`
4. Sélectionner "Label" sous BioToggle:
   - **Text**: `"🌱 Bio"`
   - **Font Size**: `28`
   - **Color**: Blanc

**Filtre ÉCO:**
1. Clic droit sur **FiltersPanel** → **UI** → **Toggle**
2. Renommer en **"EcoToggle"**
3. **Layout Element** → Preferred Width: `200`
4. Label → Text: `"♻️ Éco"`, Font Size: `28`

**Filtre PRIX:**
1. Clic droit sur **FiltersPanel** → **UI** → **Toggle**
2. Renommer en **"PriceToggle"**
3. **Layout Element** → Preferred Width: `200`
4. Label → Text: `"💰 Prix"`, Font Size: `28`

**Filtre SANTÉ:**
1. Clic droit sur **FiltersPanel** → **UI** → **Toggle**
2. Renommer en **"HealthToggle"**
3. **Layout Element** → Preferred Width: `200`
4. Label → Text: `"❤️ Santé"`, Font Size: `28`

#### 2.5 Créer le ScrollView pour les Produits

1. Clic droit sur **Canvas** → **UI** → **Scroll View**
2. Renommer en **"ProductsScrollView"**
3. Configurer **Rect Transform**:
   - **Anchor**: Stretch Both
   - **Left**: `50`, **Right**: `50`
   - **Top**: `220`, **Bottom**: `150`

#### 2.6 Configurer le Content Panel

1. Sélectionner **Content** sous ProductsScrollView
2. **Add Component** → **Vertical Layout Group**:
   - **Spacing**: `15`
   - **Padding**: All = `20`
   - **Child Force Expand**: Width = ✓, Height = ✗
3. **Add Component** → **Content Size Fitter**:
   - **Vertical Fit**: Preferred Size

#### 2.7 Créer le Prefab ProductCard (Template)

1. Clic droit sur **Content** → **UI** → **Panel**
2. Renommer en **"ProductCardTemplate"**
3. Configurer **Rect Transform**:
   - **Height**: `150`
4. Ajouter **Layout Element**:
   - **Preferred Height**: `150`
5. Ajouter **Horizontal Layout Group**:
   - **Spacing**: `20`
   - **Padding**: All = `10`
   - **Child Force Expand**: Height = ✓

**Créer l'image du produit dans le template:**
1. Clic droit sur **ProductCardTemplate** → **UI** → **Image**
2. Renommer en **"CardProductImage"**
3. **Layout Element** → Preferred Width: `130`
4. **Preserve Aspect**: ✓

**Créer le container d'infos:**
1. Clic droit sur **ProductCardTemplate** → **Create Empty**
2. Renommer en **"InfoContainer"**
3. **Add Component** → **Vertical Layout Group**:
   - **Spacing**: `5`
   - **Child Alignment**: Upper Left
   - **Child Force Expand**: Width = ✓, Height = ✗

**Créer le nom dans InfoContainer:**
1. Clic droit sur **InfoContainer** → **UI** → **Text - TextMeshPro**
2. Renommer en **"CardNameText"**
3. Configurer:
   - **Text**: `"Nom Produit"`
   - **Font Size**: `24`
   - **Font Style**: Bold
4. **Layout Element** → Preferred Height: `40`

**Créer la marque:**
1. Clic droit sur **InfoContainer** → **UI** → **Text - TextMeshPro**
2. Renommer en **"CardBrandText"**
3. Text: `"Marque"`, Font Size: `20`, Color: Gris
4. **Layout Element** → Preferred Height: `30`

**Créer le prix:**
1. Clic droit sur **InfoContainer** → **UI** → **Text - TextMeshPro**
2. Renommer en **"CardPriceText"**
3. Text: `"Prix: -"`, Font Size: `22`, Color: Cyan
4. **Layout Element** → Preferred Height: `30`

**Créer les scores:**
1. Clic droit sur **InfoContainer** → **UI** → **Text - TextMeshPro**
2. Renommer en **"CardScoresText"**
3. Text: `"❤️ - | ♻️ -"`, Font Size: `20`
4. **Layout Element** → Preferred Height: `30`

**IMPORTANT**: Désactiver le template:
1. Sélectionner **ProductCardTemplate**
2. Décocher la case en haut de l'Inspector (désactive le GameObject)

#### 2.8 Créer le Bouton Retour

1. Clic droit sur **Canvas** → **UI** → **Button - TextMeshPro**
2. Renommer en **"BackButton"**
3. **Rect Transform**:
   - **Anchor**: Bottom Center
   - **Pos Y**: `50`
   - **Width**: `300`, **Height**: `70`
4. Texte: `"← Retour"`, Font Size: `32`

### Étape 3: Connecter le Script RecommendationsController

1. Sélectionner **Canvas**
2. **Add Component** → **"RecommendationsController"**
3. Connecter les références:
   - **Product List Container**: → Content (sous ProductsScrollView)
   - **Product Card Prefab**: → ProductCardTemplate
   - **Bio Toggle**: → BioToggle
   - **Eco Toggle**: → EcoToggle
   - **Price Toggle**: → PriceToggle
   - **Health Toggle**: → HealthToggle
   - **Back Button**: → BackButton
   - **Title Text**: → TitleText (optionnel)

### Étape 4: Sauvegarder

1. **Ctrl+S** pour sauvegarder
2. Cette scène sera accessible depuis ProductInfoScene

---

## 5. ProfileScene - Profil Utilisateur

### Objectif
Gestion du profil utilisateur: préférences alimentaires, historique des scans, paramètres.

### Étape 1: Ouvrir la Scène

1. **Project** → **Assets** → **Scenes** → **ProfileScene.unity**

### Étape 2: Créer le Canvas

1. **Hierarchy** → **UI** → **Canvas**
2. Configurer **Canvas Scaler**: Scale With Screen Size (1920 x 1080)
3. Vérifier **EventSystem**

#### 2.1 Créer le Titre

1. Clic droit sur **Canvas** → **UI** → **Text - TextMeshPro**
2. Renommer en **"TitleText"**
3. Configurer:
   - **Text**: `"Mon Profil"`
   - **Font Size**: `48`
   - **Font Style**: Bold
   - **Alignment**: Center
4. **Rect Transform**:
   - **Anchor**: Top Center
   - **Pos Y**: `-50`
   - **Width**: `600`, **Height**: `80`

#### 2.2 Créer le ScrollView Principal

1. Clic droit sur **Canvas** → **UI** → **Scroll View**
2. Renommer en **"ProfileScrollView"**
3. **Rect Transform**:
   - **Anchor**: Stretch Both
   - **Left**: `50`, **Top**: `150`, **Right**: `50`, **Bottom**: `150`

#### 2.3 Configurer le Content

1. Sélectionner **Content** sous ProfileScrollView
2. **Add Component** → **Vertical Layout Group**:
   - **Spacing**: `20`
   - **Padding**: All = `30`
   - **Child Force Expand**: Width = ✓, Height = ✗
3. **Add Component** → **Content Size Fitter**:
   - **Vertical Fit**: Preferred Size

#### 2.4 Créer Section Avatar

1. Clic droit sur **Content** → **UI** → **Panel**
2. Renommer en **"AvatarSection"**
3. **Layout Element** → Preferred Height: `200`
4. Configurer background (couleur gris foncé)

**Créer l'image avatar:**
1. Clic droit sur **AvatarSection** → **UI** → **Image**
2. Renommer en **"AvatarImage"**
3. **Rect Transform**:
   - **Anchor**: Middle Center
   - **Width**: `150`, **Height**: `150`
4. Configurer pour être circulaire (optionnel avec sprite)

#### 2.5 Créer Section Préférences

1. Clic droit sur **Content** → **UI** → **Text - TextMeshPro**
2. Renommer en **"PreferencesTitle"**
3. Text: `"Préférences Alimentaires"`, Font Size: `32`, Bold
4. **Layout Element** → Preferred Height: `50`

**Créer le container de préférences:**
1. Clic droit sur **Content** → **Create Empty**
2. Renommer en **"PreferencesContainer"**
3. **Add Component** → **Vertical Layout Group**:
   - **Spacing**: `10`
   - **Padding**: Left=20, Right=20
4. **Layout Element** → Preferred Height: `250`

**Créer des toggles de préférences:**

Pour chaque préférence (Végétarien, Vegan, Sans Gluten, Sans Lactose, Halal):

1. Clic droit sur **PreferencesContainer** → **UI** → **Toggle**
2. Renommer (ex: **"VegetarianToggle"**)
3. Configurer Label:
   - Text: `"🥗 Végétarien"`, Font Size: `26`
4. **Layout Element** → Preferred Height: `50`

Répéter pour:
- **VeganToggle**: `"🌱 Vegan"`
- **GlutenFreeToggle**: `"🌾 Sans Gluten"`
- **LactoseFreeToggle**: `"🥛 Sans Lactose"`
- **HalalToggle**: `"☪️ Halal"`

#### 2.6 Créer Section Historique

1. Clic droit sur **Content** → **UI** → **Text - TextMeshPro**
2. Renommer en **"HistoryTitle"**
3. Text: `"Historique des Scans"`, Font Size: `32`, Bold
4. **Layout Element** → Preferred Height: `50`

**Créer le texte d'historique:**
1. Clic droit sur **Content** → **UI** → **Text - TextMeshPro**
2. Renommer en **"HistoryText"**
3. Configurer:
   - **Text**: `"Aucun scan pour le moment"`
   - **Font Size**: `22`
   - **Alignment**: Left
   - **Color**: Gris clair
4. **Layout Element** → Preferred Height: `200`

#### 2.7 Créer Section Statistiques

1. Clic droit sur **Content** → **UI** → **Text - TextMeshPro**
2. Renommer en **"StatsTitle"**
3. Text: `"Statistiques"`, Font Size: `32`, Bold
4. **Layout Element** → Preferred Height: `50`

**Créer le texte de stats:**
1. Clic droit sur **Content** → **UI** → **Text - TextMeshPro**
2. Renommer en **"StatsText"**
3. Configurer:
   - **Text**: `"Produits scannés: 0\nScore santé moyen: -\nScore éco moyen: -"`
   - **Font Size**: `24`
   - **Alignment**: Left
4. **Layout Element** → Preferred Height: `120`

#### 2.8 Créer Bouton Effacer Historique

1. Clic droit sur **Content** → **UI** → **Button - TextMeshPro**
2. Renommer en **"ClearHistoryButton"**
3. **Layout Element** → Preferred Height: `70`
4. Configurer couleur (rouge)
5. Texte: `"Effacer l'Historique"`, Font Size: `28`

#### 2.9 Créer Bouton Retour

1. Clic droit sur **Canvas** → **UI** → **Button - TextMeshPro**
2. Renommer en **"BackButton"**
3. **Rect Transform**:
   - **Anchor**: Bottom Center
   - **Pos Y**: `50`
   - **Width**: `300`, **Height**: `70`
4. Texte: `"← Retour"`, Font Size: `32`

### Étape 3: Connecter le Script ProfileController

1. Sélectionner **Canvas**
2. **Add Component** → **"ProfileController"**
3. Connecter les références:
   - **Avatar Image**: → AvatarImage
   - **Vegetarian Toggle**: → VegetarianToggle
   - **Vegan Toggle**: → VeganToggle
   - **Gluten Free Toggle**: → GlutenFreeToggle
   - **Lactose Free Toggle**: → LactoseFreeToggle
   - **Halal Toggle**: → HalalToggle
   - **History Text**: → HistoryText
   - **Stats Text**: → StatsText
   - **Clear History Button**: → ClearHistoryButton
   - **Back Button**: → BackButton

### Étape 4: Sauvegarder

1. **Ctrl+S** pour sauvegarder
2. Cette scène est accessible depuis HomeScene

---

## 6. MainScene - Scène Principale

### Objectif
Scène de démarrage qui initialise les managers et charge HomeScene.

### Configuration Simple

Cette scène est déjà configurée et ne nécessite généralement pas de modification. Elle contient:

1. **Main Camera**: Caméra par défaut
2. **Managers** (optionnels, créés automatiquement):
   - NavigationManager (singleton)
   - QRCodeManager (singleton)
   - ProductDatabase (singleton)

### Si vous devez la recréer:

1. **Hierarchy** → Create Empty → Renommer **"GameManager"**
2. **Add Component** → Nouveau script **"GameInitializer"**
3. Dans le script, ajouter au Start():
```csharp
void Start() {
    // Initialize singletons
    NavigationManager.Instance.LoadScene("HomeScene");
}
```

**Note**: MainScene peut rester vide car les Singletons avec DontDestroyOnLoad se créent automatiquement au premier accès.

---

## Ordre de Configuration Recommandé

Pour une configuration efficace, suivez cet ordre:

### Phase 1: Scènes de Base (2-3 heures)
1. ✅ **HomeScene** (30 min) - Suivre Section 1
2. ✅ **ScannerScene** (45 min) - Suivre Section 2
3. ✅ **ProductInfoScene** (1h) - Suivre Section 3

**Tester après Phase 1**: HomeScene → ScannerScene → ProductInfoScene

### Phase 2: Scènes Avancées (2-3 heures)
4. **RecommendationsScene** (1-1.5h) - Suivre Section 4
5. **ProfileScene** (1h) - Suivre Section 5

**Tester après Phase 2**: Navigation complète entre toutes les scènes

### Phase 3: Finalisation (30 min)
6. Vérifier MainScene
7. Tester le flux complet plusieurs fois
8. Ajuster les couleurs et espacements

---

## Comment les Composants Fonctionnent Ensemble

### 1. Navigation entre Scènes

**NavigationManager** (Singleton):
- Gère le chargement asynchrone des scènes
- Persiste entre les scènes (DontDestroyOnLoad)
- Méthode principale: `LoadScene(string sceneName)`

**Exemple de flux**:
```
HomeScene → Clic "Scanner Produit"
  → HomeController.OnScanButtonClick()
    → NavigationManager.Instance.LoadScene("ScannerScene")
      → Unity charge ScannerScene
        → ScannerController.Start()
```

### 2. Passage de Données (Product ID)

**QRCodeManager** (Singleton):
- Stocke l'ID du produit scanné
- Persiste entre les scènes
- Propriété: `string CurrentProductId`

**Exemple de flux**:
```
ScannerScene → Scan réussi
  → ScannerController.SimulateScan()
    → QRCodeManager.Instance.SetScannedProduct(productId)
      → NavigationManager.Instance.LoadScene("ProductInfoScene")
        → ProductInfoScene.Start()
          → string id = QRCodeManager.Instance.CurrentProductId
            → ProductDatabase.Instance.GetProductById(id)
```

### 3. Récupération des Données Produit

**ProductDatabase** (Singleton):
- Charge products_database.json au démarrage
- Fournit méthodes de recherche et filtrage
- Méthodes principales:
  - `GetProductById(string id)`
  - `GetAlternatives(string id, filters)`
  - `SearchProducts(query)`

**Exemple d'utilisation**:
```csharp
// Dans ProductInfoController
void LoadProduct() {
    string productId = QRCodeManager.Instance.CurrentProductId;
    if (string.IsNullOrEmpty(productId)) {
        Debug.LogError("No product ID available");
        return;
    }
    
    ProductData product = ProductDatabase.Instance.GetProductById(productId);
    if (product != null) {
        // Afficher les données
        productNameText.text = product.name;
        // etc...
    }
}
```

### 4. Gestion des Préférences Utilisateur

**UserPreferences** (Singleton):
- Sauvegarde les préférences avec PlayerPrefs
- Méthodes principales:
  - `AddScannedProduct(string id)`
  - `GetScanCount(string id)`
  - `IsVegetarian, IsVegan, etc.`

**Exemple**:
```csharp
// Dans ProfileController
void OnPreferenceChanged(bool value) {
    UserPreferences.Instance.IsVegetarian = value;
    UserPreferences.Instance.SavePreferences();
}
```

### 5. Connexions entre Contrôleurs

**HomeController**:
- Boutons → LoadScene("ScannerScene") ou LoadScene("ProfileScene")

**ScannerController**:
- Test Mode: Simule le scan automatiquement
- Real Mode: Utilise AR camera pour détecter QR codes
- Après scan → SetScannedProduct() → LoadScene("ProductInfoScene")

**ProductInfoController**:
- Start() → Récupère ID depuis QRCodeManager
- Charge données depuis ProductDatabase
- Bouton Alternatives → LoadScene("RecommendationsScene")

**RecommendationsController**:
- Start() → Récupère ID produit courant
- Charge alternatives depuis ProductDatabase
- Filtres → Re-filtre la liste en temps réel
- Instantie ProductCard pour chaque alternative

**ProfileController**:
- Start() → Charge préférences depuis UserPreferences
- Toggles → Sauvegarde les changements
- Affiche l'historique des scans

---

## Résumé des Fichiers Créés

Après avoir suivi ce guide, vous aurez configuré:

- **6 scènes Unity** complètement fonctionnelles
- **50+ GameObjects UI** créés manuellement
- **12 scripts C#** connectés aux UI
- **Navigation complète** entre toutes les scènes
- **Système de données** fonctionnel avec 52 produits

---

## Dépannage Commun

### Erreur: "No product ID available"
**Cause**: ScannerScene n'a pas d'UI ou le scan n'a pas été effectué
**Solution**: Configurer ScannerScene complètement (Section 2)

### Erreur: "NullReferenceException" dans Controller
**Cause**: Une référence UI n'est pas connectée dans l'Inspector
**Solution**: Vérifier que TOUTES les références sont glissées-déposées

### Les boutons ne répondent pas
**Cause**: EventSystem manquant
**Solution**: Hierarchy → UI → Event System

### Le texte n'apparaît pas
**Cause**: TextMeshPro pas importé
**Solution**: Window → TextMeshPro → Import TMP Essentials

### La navigation ne fonctionne pas
**Cause**: Les scènes ne sont pas dans Build Settings
**Solution**: File → Build Settings → Add Open Scenes (pour chaque scène)

---

## Temps Estimés

- **HomeScene**: 30 minutes
- **ScannerScene**: 45 minutes
- **ProductInfoScene**: 1 heure
- **RecommendationsScene**: 1-1.5 heures
- **ProfileScene**: 1 heure
- **Tests et ajustements**: 30 minutes

**Total**: 4.5 - 5.5 heures pour une configuration manuelle complète

---

## Prochaines Étapes

Après avoir configuré toutes les scènes:

1. **Tester le flux complet**:
   - HomeScene → Scanner → ProductInfo → Alternatives → Retour
   - HomeScene → Profil → Modifier préférences → Retour

2. **Personnaliser l'apparence**:
   - Ajuster les couleurs
   - Ajouter des images/sprites
   - Améliorer les animations

3. **Préparer pour Sprint 2** (AR Integration):
   - Tester sur appareil mobile
   - Configurer les permissions caméra
   - Intégrer AR Foundation pour scan réel

---

**Ce guide est complet et détaillé. En suivant chaque section, vous aurez une application Smart Retail AR totalement fonctionnelle!** 🚀
