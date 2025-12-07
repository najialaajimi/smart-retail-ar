# Guide de Résolution - Écran Bleu Sans Boutons

## Problème Identifié

L'écran bleu sans boutons est causé par le fait que les **scènes Unity n'ont pas encore d'interface utilisateur (UI) configurée**. 

Les scripts C# sont créés (HomeController.cs, ScannerController.cs, etc.) mais les GameObjects Unity avec Canvas, Buttons, et Text ne sont pas encore ajoutés aux scènes.

---

## Solution Rapide: Créer une Scène de Test Simple

### Étape 1: Ouvrir Unity et Créer l'Interface

1. **Ouvrir Unity Editor** (pas le build test.x86_64)
2. Dans Unity, ouvrir **HomeScene.unity** (Assets → Scenes → HomeScene.unity)

### Étape 2: Créer le Canvas et les Éléments UI

#### 2.1 Créer le Canvas Principal

1. Dans la Hierarchy, clic droit → **UI** → **Canvas**
2. Sélectionner le Canvas créé
3. Dans l'Inspector, Canvas Scaler:
   - UI Scale Mode: **Scale With Screen Size**
   - Reference Resolution: **1920 x 1080**

#### 2.2 Créer le Texte de Bienvenue

1. Clic droit sur Canvas → **UI** → **Text - TextMeshPro**
   - Si demandé, cliquer "Import TMP Essentials"
2. Renommer en "WelcomeText"
3. Dans l'Inspector:
   - Text: **"Bienvenue!"**
   - Font Size: **48**
   - Alignment: Center + Middle
   - Color: Blanc
4. Rect Transform:
   - Anchor: Top Center
   - Pos Y: **-100**
   - Width: **800**, Height: **100**

#### 2.3 Créer le Bouton Scanner

1. Clic droit sur Canvas → **UI** → **Button - TextMeshPro**
2. Renommer en "ScanButton"
3. Sélectionner le bouton, dans Rect Transform:
   - Anchor: Middle Center
   - Pos X: **0**, Pos Y: **0**
   - Width: **400**, Height: **80**
4. Ouvrir ScanButton dans Hierarchy
5. Sélectionner "Text (TMP)" sous ScanButton
6. Changer le texte en: **"Scanner Produit"**
7. Font Size: **36**

#### 2.4 Créer le Bouton Profil

1. Clic droit sur Canvas → **UI** → **Button - TextMeshPro**
2. Renommer en "ProfileButton"
3. Dans Rect Transform:
   - Anchor: Middle Center
   - Pos X: **0**, Pos Y: **-120**
   - Width: **400**, Height: **80**
4. Texte du bouton: **"Mon Profil"**
5. Font Size: **36**

### Étape 3: Connecter le Script HomeController

1. Dans Hierarchy, sélectionner **Canvas**
2. Dans l'Inspector, cliquer **Add Component**
3. Chercher et ajouter **HomeController**
4. Dans le script HomeController:
   - **Scan Button**: Glisser le bouton "ScanButton" depuis Hierarchy
   - **Profile Button**: Glisser le bouton "ProfileButton"
   - **Welcome Text**: Glisser "WelcomeText"

### Étape 4: Sauvegarder et Tester

1. **Ctrl+S** ou File → Save pour sauvegarder la scène
2. Cliquer sur le bouton **Play** (▶️) en haut
3. Vous devriez maintenant voir:
   - Texte "Bienvenue!"
   - Bouton "Scanner Produit"
   - Bouton "Mon Profil"

---

## Solution Alternative: Utiliser un Script de Configuration Automatique

Créer un script qui configure automatiquement l'UI:

### Créer SetupUI.cs

Créer ce fichier dans `Assets/Scripts/`:

```csharp
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SetupUI : MonoBehaviour
{
    [ContextMenu("Setup Home Scene UI")]
    public void SetupHomeSceneUI()
    {
        // Créer Canvas
        GameObject canvasGO = new GameObject("Canvas");
        Canvas canvas = canvasGO.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasGO.AddComponent<CanvasScaler>();
        canvasGO.AddComponent<GraphicRaycaster>();
        
        CanvasScaler scaler = canvasGO.GetComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        
        // Créer EventSystem si n'existe pas
        if (FindObjectOfType<UnityEngine.EventSystems.EventSystem>() == null)
        {
            GameObject eventSystem = new GameObject("EventSystem");
            eventSystem.AddComponent<UnityEngine.EventSystems.EventSystem>();
            eventSystem.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();
        }
        
        // Créer Welcome Text
        GameObject welcomeTextGO = new GameObject("WelcomeText");
        welcomeTextGO.transform.SetParent(canvasGO.transform, false);
        TextMeshProUGUI welcomeText = welcomeTextGO.AddComponent<TextMeshProUGUI>();
        welcomeText.text = "Bienvenue!";
        welcomeText.fontSize = 48;
        welcomeText.alignment = TextAlignmentOptions.Center;
        welcomeText.color = Color.white;
        
        RectTransform welcomeRect = welcomeTextGO.GetComponent<RectTransform>();
        welcomeRect.anchorMin = new Vector2(0.5f, 1f);
        welcomeRect.anchorMax = new Vector2(0.5f, 1f);
        welcomeRect.pivot = new Vector2(0.5f, 1f);
        welcomeRect.anchoredPosition = new Vector2(0, -100);
        welcomeRect.sizeDelta = new Vector2(800, 100);
        
        // Créer Scan Button
        GameObject scanButtonGO = CreateButton(canvasGO.transform, "ScanButton", "Scanner Produit", new Vector2(0, 0));
        
        // Créer Profile Button
        GameObject profileButtonGO = CreateButton(canvasGO.transform, "ProfileButton", "Mon Profil", new Vector2(0, -120));
        
        Debug.Log("UI Setup Complete!");
    }
    
    private GameObject CreateButton(Transform parent, string name, string text, Vector2 position)
    {
        GameObject buttonGO = new GameObject(name);
        buttonGO.transform.SetParent(parent, false);
        
        Image image = buttonGO.AddComponent<Image>();
        image.color = new Color(0.2f, 0.6f, 1f);
        
        Button button = buttonGO.AddComponent<Button>();
        
        RectTransform buttonRect = buttonGO.GetComponent<RectTransform>();
        buttonRect.anchorMin = new Vector2(0.5f, 0.5f);
        buttonRect.anchorMax = new Vector2(0.5f, 0.5f);
        buttonRect.pivot = new Vector2(0.5f, 0.5f);
        buttonRect.anchoredPosition = position;
        buttonRect.sizeDelta = new Vector2(400, 80);
        
        GameObject textGO = new GameObject("Text");
        textGO.transform.SetParent(buttonGO.transform, false);
        TextMeshProUGUI buttonText = textGO.AddComponent<TextMeshProUGUI>();
        buttonText.text = text;
        buttonText.fontSize = 36;
        buttonText.alignment = TextAlignmentOptions.Center;
        buttonText.color = Color.white;
        
        RectTransform textRect = textGO.GetComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.sizeDelta = Vector2.zero;
        
        return buttonGO;
    }
}
```

### Utiliser le Script de Setup

1. Créer un GameObject vide dans la scène: Hierarchy → Create Empty
2. Nommer "UISetup"
3. Ajouter le script SetupUI au GameObject
4. Dans l'Inspector, clic droit sur SetupUI → **Setup Home Scene UI**
5. L'UI sera créée automatiquement!

---

## Pourquoi l'Écran est Bleu?

### Explication Technique

1. **Caméra par Défaut**: Les scènes Unity contiennent une caméra avec fond bleu (couleur par défaut)
2. **Pas de Canvas**: Sans Canvas et éléments UI, seul le fond de la caméra est visible
3. **Scripts Seuls**: Les scripts C# existent mais nécessitent des GameObjects Unity pour fonctionner

### Vérification

Dans Unity Editor:
1. Ouvrir n'importe quelle scène
2. Regarder la **Hierarchy** (panneau gauche)
3. Si vous voyez seulement "Main Camera" → **Pas d'UI = Écran bleu**
4. Vous devez voir: Canvas, Buttons, Text → **UI visible**

---

## Différence Entre Unity Editor et Build

### Unity Editor (Recommandé pour Développement)
- Permet d'éditer les scènes
- Voir la Hierarchy et l'Inspector
- Ajouter des GameObjects et UI
- Mode Play pour tester

### Build (test.x86_64, test.apk)
- Version finale compilée
- Ne peut pas être modifiée
- Si les scènes n'ont pas d'UI avant le build → Écran bleu dans le build

**Important**: Vous devez configurer l'UI dans Unity Editor AVANT de faire un build!

---

## Prochaines Étapes Recommandées

### 1. Configuration Minimale pour Tester (5 minutes)

```bash
# Ouvrir Unity Editor (pas le build)
cd ~/Unity/Hub/Editor/2022.3.62f3/Editor
./Unity -projectPath ~/smart-retail-ar
```

Dans Unity:
1. Ouvrir HomeScene.unity
2. Suivre "Étape 2: Créer le Canvas et les Éléments UI" ci-dessus
3. Cliquer Play pour tester

### 2. Configuration Complète pour Toutes les Scènes

Pour chaque scène (Scanner, ProductInfo, etc.), répéter le processus de création UI selon les besoins de cette scène.

#### ScannerScene.unity devrait avoir:
- Canvas
- RawImage (pour la caméra)
- Button "Scan"
- Button "Torch"
- Button "Back"
- Text "Status"

#### ProductInfoScene.unity devrait avoir:
- Canvas
- Text (nom produit, marque, description)
- Text (informations nutritionnelles)
- Slider (scores)
- Button "Alternatives"
- Button "Back"

---

## Commandes de Vérification

### Vérifier que Unity Editor est Installé
```bash
ls ~/Unity/Hub/Editor/2022.3.62f3/Editor/Unity
```

### Ouvrir Unity avec le Projet
```bash
cd ~/Unity/Hub/Editor/2022.3.62f3/Editor
./Unity -projectPath ~/smart-retail-ar
```

### Vérifier les Logs Unity
```bash
tail -f ~/.config/unity3d/Editor.log
```

---

## Note Importante sur les Builds

Les fichiers suivants dans votre répertoire sont des BUILDS (pas pour éditer):
- `test.x86_64` - Build Linux standalone
- `test.apk` - Build Android
- `test_Data/` - Données du build

Pour développer et ajouter l'UI, vous **DEVEZ** utiliser Unity Editor, pas ces builds!

---

## Résumé

**Problème**: Écran bleu sans boutons  
**Cause**: Scènes Unity sans Canvas/UI GameObjects  
**Solution**: Ouvrir Unity Editor et créer l'UI manuellement ou avec le script SetupUI.cs

**Workflow Correct**:
1. ✅ Ouvrir Unity Editor
2. ✅ Créer UI dans les scènes
3. ✅ Tester en mode Play
4. ✅ Faire un build seulement après

**Workflow Incorrect**:
1. ❌ Faire un build directement
2. ❌ Lancer le build (test.x86_64)
3. ❌ Voir écran bleu

---

## Support Additionnel

Si le problème persiste après avoir créé l'UI dans Unity Editor, vérifier:

1. **Canvas est visible**:
   - Sélectionner Canvas dans Hierarchy
   - Inspector → Canvas → Render Mode = "Screen Space - Overlay"

2. **EventSystem existe**:
   - Hierarchy devrait avoir "EventSystem" GameObject
   - Si absent: Hierarchy → UI → Event System

3. **Caméra correctement configurée**:
   - Main Camera → Clear Flags = "Solid Color"
   - Background = Couleur voulue (pas forcément bleu)

---

**Fichier**: BLUE_SCREEN_FIX.md  
**Version**: 1.0  
**Pour**: Unity 2022.3.62f3 sur Ubuntu 24.04
