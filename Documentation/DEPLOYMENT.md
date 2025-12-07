# Guide de Déploiement - Smart Retail AR

## Table des Matières
- [Préparation au Déploiement](#préparation-au-déploiement)
- [Build Android](#build-android)
- [Build iOS](#build-ios)
- [Publication Stores](#publication-stores)
- [Configuration Production](#configuration-production)
- [Monitoring et Maintenance](#monitoring-et-maintenance)

## Préparation au Déploiement

### Checklist Pré-Déploiement

#### Code et Assets
- [ ] Tous les tests passent
- [ ] Pas de warnings dans Console
- [ ] Performance validée (≥30 FPS)
- [ ] Tous les assets optimisés
- [ ] Base de données produits complète
- [ ] Images de référence AR validées

#### Configuration
- [ ] Version number incrémenté
- [ ] Bundle identifier configuré
- [ ] API keys configurés
- [ ] Analytics activé
- [ ] Crash reporting configuré

#### Documentation
- [ ] README.md à jour
- [ ] CHANGELOG.md mis à jour
- [ ] Store descriptions préparées
- [ ] Screenshots et vidéos prêtes

#### Légal
- [ ] CGU rédigées
- [ ] Politique de confidentialité
- [ ] Licences vérifiées
- [ ] Permissions justifiées

### Configuration du Projet

#### Player Settings

**Common Settings:**
```
Company Name: [Votre entreprise]
Product Name: Smart Retail AR
Version: 1.0.0
Bundle Version Code: 1

Icon: [Votre icône 1024x1024]
Default Orientation: Auto Rotation
  - Portrait: ✓
  - Portrait Upside Down: ✗
  - Landscape Left: ✓
  - Landscape Right: ✓
```

**Identification:**
```
Android: com.yourcompany.smartretailar
iOS: com.yourcompany.smartretailar
```

## Build Android

### Configuration Android

#### Player Settings

```
File > Build Settings > Android > Player Settings

Other Settings:
  - Package Name: com.yourcompany.smartretailar
  - Minimum API Level: Android 7.0 'Nougat' (API level 24)
  - Target API Level: API level 33
  - Scripting Backend: IL2CPP
  - Target Architectures: ARM64 ✓
  - Internet Access: Require
  - Write Permission: External (SDCard)

Publishing Settings:
  - Keystore Manager...
    - Create New Keystore
    - Alias: smartretailar_release
    - Password: [Secure password]
    - Validity: 25 years
```

#### XR Settings

```
XR Plug-in Management:
  - ARCore: ✓
  
ARCore Settings:
  - Depth: Optional (pour future features)
  - Light Estimation: Everything
```

#### Optimization Settings

```
Player > Other Settings:
  - Graphics API:
    - OpenGLES3 (primary)
    - Vulkan (fallback)
  - Multithreaded Rendering: ✓
  - Static Batching: ✓
  - Dynamic Batching: ✓
  - GPU Skinning: ✓

Player > Publishing Settings:
  - Split Application Binary: ✓ (si >100MB)
  - Use App Bundle (Google Play): ✓
```

### Création du Keystore

#### Via Unity

```
Edit > Project Settings > Player > Publishing Settings
  - Keystore Manager
  - Create New
  
Keystore:
  Path: [Projet]/Keystores/smartretailar.keystore
  Password: [SECURISER]
  
Key Alias:
  Alias: smartretailar_release
  Password: [SECURISER]
  Validity: 25 years
  
IMPORTANT: Sauvegarder le keystore et les mots de passe en lieu sûr!
```

#### Via Keytool (Alternative)

```bash
keytool -genkey -v \
  -keystore smartretailar.keystore \
  -alias smartretailar_release \
  -keyalg RSA \
  -keysize 2048 \
  -validity 10000
```

### Build APK/AAB

#### Via Unity Editor

**Development Build:**
```
File > Build Settings > Android
  - Development Build: ✓
  - Script Debugging: ✓
  - Build
```

**Release Build:**
```
File > Build Settings > Android
  - Development Build: ✗
  - Build App Bundle (Google Play): ✓
  - Build
```

#### Via Command Line

```bash
# Build APK
/Applications/Unity/Hub/Editor/2022.3.62f3/Unity.app/Contents/MacOS/Unity \
  -quit -batchmode -projectPath . \
  -buildTarget Android \
  -executeMethod BuildScript.BuildAndroid

# Build AAB (App Bundle)
/Applications/Unity/Hub/Editor/2022.3.62f3/Unity.app/Contents/MacOS/Unity \
  -quit -batchmode -projectPath . \
  -buildTarget Android \
  -executeMethod BuildScript.BuildAndroidBundle
```

#### Build Script (Assets/Editor/BuildScript.cs)

```csharp
using UnityEditor;
using UnityEngine;

public class BuildScript
{
    [MenuItem("Build/Android APK")]
    public static void BuildAndroid()
    {
        BuildPlayerOptions options = new BuildPlayerOptions();
        options.scenes = GetScenes();
        options.locationPathName = "Builds/Android/SmartRetailAR.apk";
        options.target = BuildTarget.Android;
        options.options = BuildOptions.None;
        
        BuildPipeline.BuildPlayer(options);
    }
    
    [MenuItem("Build/Android AAB")]
    public static void BuildAndroidBundle()
    {
        EditorUserBuildSettings.buildAppBundle = true;
        BuildAndroid();
    }
    
    private static string[] GetScenes()
    {
        return new string[]
        {
            "Assets/Scenes/Frontend/HomeScene.unity",
            "Assets/Scenes/AR/ARScannerScene.unity",
            "Assets/Scenes/Frontend/ProductInfoScene.unity",
            // Add all scenes
        };
    }
}
```

### Test sur Device

```bash
# Installer APK
adb install Builds/Android/SmartRetailAR.apk

# Ou via Unity
File > Build Settings > Build And Run

# Logs en temps réel
adb logcat -s Unity
```

## Build iOS

### Configuration iOS

#### Player Settings

```
File > Build Settings > iOS > Player Settings

Other Settings:
  - Bundle Identifier: com.yourcompany.smartretailar
  - Minimum iOS Version: 11.0
  - Target SDK: Device SDK
  - Architecture: ARM64
  - Camera Usage Description: "Requis pour scanner les produits en AR"

Publishing Settings:
  - Automatically Sign: ✗ (pour production)
  - Signing Team ID: [Votre Team ID]
  - Provisioning Profile: [Votre profile]
```

#### XR Settings

```
XR Plug-in Management:
  - ARKit: ✓
  
ARKit Settings:
  - Face Tracking: ✗
  - Require ARKit Support: ✓
```

#### Optimization Settings

```
Player > Other Settings:
  - Graphics API:
    - Metal (automatique)
  - Accelerometer Frequency: 60 Hz
  - Target minimum iOS Version: 11.0

Player > Publishing Settings:
  - Strip Engine Code: ✓ (release)
  - Script Call Optimization: Fast but no exceptions
```

### Configuration Xcode

#### Build iOS Project

```
File > Build Settings > iOS > Build

Output: [Projet]/Builds/iOS/
```

#### Configuration dans Xcode

1. **Ouvrir le projet:**
```bash
open Builds/iOS/Unity-iPhone.xcodeproj
```

2. **Signing & Capabilities:**
```
Signing:
  - Automatically manage signing: ✗
  - Team: [Votre équipe]
  - Provisioning Profile: [Profile de distribution]
  
Capabilities:
  - Camera: ✓
```

3. **Info.plist:**
```xml
<key>NSCameraUsageDescription</key>
<string>Requis pour scanner les produits en réalité augmentée</string>

<key>UIRequiredDeviceCapabilities</key>
<array>
    <string>arkit</string>
    <string>arm64</string>
</array>
```

4. **Build Settings:**
```
- iOS Deployment Target: 11.0
- Valid Architectures: arm64
- Build Configuration: Release
- Enable Bitcode: No
```

### Build IPA

#### Via Xcode

```
Product > Archive

Archives > Distribute App
  - App Store Connect (pour soumission)
  - Ad Hoc (pour test interne)
  - Development (pour debug)
```

#### Via Command Line

```bash
# Build archive
xcodebuild -project Unity-iPhone.xcodeproj \
  -scheme Unity-iPhone \
  -configuration Release \
  -archivePath build/SmartRetailAR.xcarchive \
  archive

# Export IPA
xcodebuild -exportArchive \
  -archivePath build/SmartRetailAR.xcarchive \
  -exportPath build/ \
  -exportOptionsPlist ExportOptions.plist
```

**ExportOptions.plist:**
```xml
<?xml version="1.0" encoding="UTF-8"?>
<!DOCTYPE plist PUBLIC "-//Apple//DTD PLIST 1.0//EN" "http://www.apple.com/DTDs/PropertyList-1.0.dtd">
<plist version="1.0">
<dict>
    <key>method</key>
    <string>app-store</string>
    <key>teamID</key>
    <string>YOUR_TEAM_ID</string>
    <key>uploadBitcode</key>
    <false/>
    <key>compileBitcode</key>
    <false/>
</dict>
</plist>
```

### Test sur Device

```bash
# Via Xcode
Product > Run (⌘R)

# Ou installer IPA
xcrun altool --upload-app -f SmartRetailAR.ipa \
  -u [AppleID] -p [App-specific password]
```

## Publication Stores

### Google Play Store

#### 1. Créer Application

```
Google Play Console > Toutes les applications > Créer une application
  - Nom: Smart Retail AR
  - Langue: Français
  - Type: Application
  - Gratuite/Payante: Gratuite
```

#### 2. Fiche Store

**Texte descriptif:**
```
Titre: Smart Retail AR - Scanner de Produits
Description courte: Scannez les produits et obtenez des informations nutritionnelles et écologiques en AR

Description complète:
Smart Retail AR révolutionne votre expérience d'achat en magasin!

🔍 FONCTIONNALITÉS:
• Reconnaissance instantanée des produits en réalité augmentée
• Informations nutritionnelles détaillées (Nutri-Score)
• Impact écologique et origine des produits
• Recommandations personnalisées d'alternatives meilleures
• Filtres allergènes et préférences alimentaires

📊 INFORMATIONS AFFICHÉES:
• Nutri-Score, calories, macronutriments
• Éco-Score, empreinte carbone, packaging
• Commerce équitable, origine locale
• Allergènes et ingrédients

🎯 RECOMMANDATIONS INTELLIGENTES:
Algorithme multi-critères évaluant:
- Qualité nutritionnelle
- Impact environnemental
- Rapport qualité/prix
- Éthique et traçabilité

✨ SIMPLE ET RAPIDE:
1. Pointez votre caméra vers un produit
2. Les informations s'affichent en AR
3. Consultez les alternatives recommandées
4. Faites des choix éclairés!

Catégories:
- Achats
- Alimentation et boissons
- Santé et forme
```

**Assets graphiques:**
```
Icône: 512x512px PNG (32-bit)
Feature Graphic: 1024x500px JPEG ou PNG
Screenshots:
  - Téléphone: Min 2, max 8 (16:9 ou 9:16)
  - Tablette 7": 2 screenshots
  - Tablette 10": 2 screenshots
Vidéo promo: URL YouTube (optionnel)
```

#### 3. Configuration

**Contenu de l'application:**
```
Classification du contenu:
  - Questionnaire sur le contenu
  - Rating: Tous publics

Confidentialité:
  - URL politique de confidentialité: [Votre URL]
  - Données collectées: Informations de l'appareil, Interactions
  - Partage de données: Non

Public cible:
  - Âge: 3+ ans
```

**Store presence:**
```
Catégorie: Achats
Balises: ar, shopping, nutrition, eco-score, santé

Prix et distribution:
  - Gratuite
  - Pays: France, Europe, Monde
  - Appareils: Téléphones et tablettes compatibles ARCore
```

#### 4. Upload AAB

```
Release > Production > Créer une version
  - Upload AAB
  - Notes de version:
    v1.0.0
    - Lancement initial
    - Reconnaissance AR des produits
    - Informations nutritionnelles et écologiques
    - Recommandations personnalisées
```

#### 5. Soumettre pour Review

```
Vérifier tout > Soumettre pour examen
Délai: 1-7 jours généralement
```

### Apple App Store

#### 1. App Store Connect

```
App Store Connect > Mes Apps > + (Nouvelle App)
  - Plateformes: iOS
  - Nom: Smart Retail AR
  - Langue principale: Français
  - Bundle ID: com.yourcompany.smartretailar
  - SKU: smartretailar-ios
```

#### 2. Informations App

**Métadonnées:**
```
Nom: Smart Retail AR
Sous-titre: Scanner de Produits en AR

Description:
[Même description que Google Play, adaptée]

Mots-clés: ar,shopping,nutrition,eco-score,santé,produits,magasin,courses

URL assistance: https://yourwebsite.com/support
URL marketing: https://yourwebsite.com
Politique de confidentialité: https://yourwebsite.com/privacy
```

**Screenshots:**
```
iPhone 6.7":
  - 1290 x 2796 pixels
  - 3-10 screenshots

iPhone 6.5":
  - 1284 x 2778 pixels  
  - 3-10 screenshots

iPhone 5.5":
  - 1242 x 2208 pixels
  - 3-10 screenshots

iPad Pro 12.9":
  - 2048 x 2732 pixels
  - 3-10 screenshots (optionnel)
```

#### 3. Prix et Disponibilité

```
Prix: Gratuit
Disponibilité: Tous les territoires
Date de sortie: Après approbation
```

#### 4. Informations de Version

```
Version 1.0.0
Copyright: © 2024 [Votre entreprise]

Notes de version:
Bienvenue sur Smart Retail AR!

Cette première version inclut:
• Reconnaissance AR des produits
• Informations nutritionnelles complètes
• Impact écologique et origine
• Recommandations intelligentes
• Filtres personnalisés

Rating: 4+
```

#### 5. Upload Build

```
Xcode > Window > Organizer > Archives
  - Sélectionner l'archive
  - Distribute App > App Store Connect
  - Upload

Ou via Application Loader
```

#### 6. Soumettre pour Review

```
App Store Connect:
  - Sélectionner le build
  - Remplir les informations de test
  - Soumettre pour examen

Informations de contact:
  - Contact review: email + téléphone
  - Notes: Instructions de test si nécessaire

Délai: 1-3 jours généralement
```

## Configuration Production

### Analytics

**Firebase Setup:**
```csharp
// Initialize Firebase
Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
    Firebase.FirebaseApp app = Firebase.FirebaseApp.DefaultInstance;
});

// Track events
Firebase.Analytics.FirebaseAnalytics.LogEvent("product_scan",
    new Firebase.Analytics.Parameter("product_id", productId));
```

**Unity Analytics:**
```csharp
// Enable in Services window
// Track custom events
Analytics.CustomEvent("recommendation_accepted",
    new Dictionary<string, object>
    {
        { "product_id", productId },
        { "recommendation_id", recommendationId }
    });
```

### Crash Reporting

**Firebase Crashlytics:**
```csharp
Crashlytics.Log("Product scan initiated");
Crashlytics.SetCustomKey("product_id", productId);

try
{
    // Code
}
catch (Exception e)
{
    Crashlytics.LogException(e);
    throw;
}
```

### Remote Config

```csharp
// Firebase Remote Config
FirebaseRemoteConfig.DefaultInstance.FetchAsync(TimeSpan.Zero)
    .ContinueWithOnMainThread(task => {
        FirebaseRemoteConfig.DefaultInstance.ActivateAsync();
        
        // Get values
        int maxRecommendations = (int)FirebaseRemoteConfig
            .DefaultInstance.GetValue("max_recommendations").LongValue;
    });
```

## Monitoring et Maintenance

### Monitoring Production

**Métriques à suivre:**
- Crashes (target: <1%)
- ANR (target: <0.5%)
- DAU/MAU
- Retention (D1, D7, D30)
- Session duration
- User ratings

**Alertes:**
- Crash rate spike
- API errors
- Performance degradation
- User rating drop

### Mises à Jour

**Version Numbering:**
```
Format: MAJOR.MINOR.PATCH

1.0.0 - Initial release
1.0.1 - Bug fixes
1.1.0 - New features
2.0.0 - Major update
```

**Release Process:**
1. Développer en branche feature
2. Merger dans develop
3. Tester en staging
4. Créer release branch
5. Build et test final
6. Merger dans main
7. Tag version
8. Build production
9. Soumettre aux stores

### Support Utilisateur

**Channels:**
- Email: support@smartretailar.com
- In-app feedback
- Store reviews response
- FAQ website

**Response Time:**
- Critical bugs: 24h
- General support: 48-72h
- Feature requests: 1-2 weeks

---

**Bon déploiement! 🚀**
