# Smart Retail AR - Setup Guide

## Prerequisites

### Required Software
1. **Unity Hub** (Latest version)
   - Download: https://unity.com/download

2. **Unity 2022.3.62f3 LTS** (EXACT VERSION REQUIRED)
   - Install via Unity Hub
   - Select Android Build Support module
   - Select iOS Build Support module (Mac only)

3. **Git** (for version control)
   - Windows: https://git-scm.com/download/win
   - Mac: Pre-installed or via Homebrew
   - Linux: `sudo apt-get install git`

### Optional Tools
- **Visual Studio Code** with C# extension
- **Visual Studio 2022** (Windows)
- **Rider** (JetBrains IDE)

## Installation Steps

### 1. Clone Repository
```bash
git clone https://github.com/najialaajimi/smart-retail-ar.git
cd smart-retail-ar
```

### 2. Open in Unity

#### Via Unity Hub
1. Open Unity Hub
2. Click "Add" → "Add project from disk"
3. Navigate to cloned repository folder
4. Select the folder
5. Verify Unity version shows "2022.3.62f3"
6. Click on project to open

#### Via Command Line
```bash
# Open Unity with specific project
"/Applications/Unity/Hub/Editor/2022.3.62f3/Unity.app/Contents/MacOS/Unity" -projectPath /path/to/smart-retail-ar
```

### 3. Package Installation

Unity will automatically:
1. Detect `Packages/manifest.json`
2. Download required packages
3. Import AR Foundation and dependencies
4. Compile scripts

**Wait for completion** - This may take 5-10 minutes on first load.

### 4. Verify Installation

Check Unity Console for:
- ✅ No errors
- ✅ "Loaded X products from database" message
- ✅ All scripts compiled successfully

### 5. Initial Scene Setup

1. Open Scene: `Assets/Scenes/HomeScene.unity`
2. Press Play to test in Editor
3. Verify UI appears correctly

## Project Structure Verification

Ensure these folders exist:
```
smart-retail-ar/
├── Assets/
│   ├── Scenes/           (6 .unity files)
│   ├── Scripts/          
│   │   ├── Data/         (3 .cs files)
│   │   ├── UI/           (6 .cs files)
│   │   └── Utils/        (2 .cs files)
│   ├── Resources/
│   │   └── Data/         (products_database.json)
│   ├── Prefabs/
│   └── Materials/
├── Packages/
│   └── manifest.json
├── ProjectSettings/
│   └── ProjectVersion.txt
└── README.md
```

## Configuration

### Build Settings

#### For Android Development
1. File → Build Settings
2. Select "Android" platform
3. Click "Switch Platform"
4. Add scenes:
   - HomeScene
   - ScannerScene
   - ProductInfoScene
   - RecommendationsScene
   - ProfileScene

5. Player Settings:
   - Company Name: "Smart Retail AR"
   - Product Name: "Smart Retail AR"
   - Package Name: "com.smartretail.ar"
   - Minimum API Level: Android 7.0 (API 24)
   - Target API Level: Automatic (highest)
   - Scripting Backend: IL2CPP
   - Target Architectures: ARM64

#### For iOS Development
1. File → Build Settings
2. Select "iOS" platform
3. Click "Switch Platform"
4. Add all scenes (same as Android)

5. Player Settings:
   - Bundle Identifier: "com.smartretail.ar"
   - Minimum iOS Version: 11.0
   - Target SDK: Device SDK
   - Architecture: ARM64

### Editor Settings

#### Recommended Settings
1. Edit → Preferences → External Tools
   - External Script Editor: Your preferred IDE
   - Generate .csproj files: All

2. Edit → Project Settings → Editor
   - Asset Serialization Mode: Force Text
   - Version Control Mode: Visible Meta Files

## Testing

### In Unity Editor

#### Test Mode Scanner
1. Open `HomeScene.unity`
2. Press Play (▶️)
3. Click "Scanner Produit" button
4. In ScannerScene, click "Scan" button
5. Random product should load
6. Verify navigation to ProductInfoScene

#### Test Product Database
1. Open `Assets/Scripts/Data/ProductDatabase.cs`
2. View Console for "Loaded 52 products from database"
3. Open Scene, play, and verify products load

#### Test User Preferences
1. Go to ProfileScene
2. Enter user name
3. Toggle dietary preferences
4. Save
5. Stop Play mode
6. Play again - preferences should persist

### Debug Features

#### Enable Detailed Logging
In each Controller script, logs are sent to Console:
- Scene transitions
- Product loading
- User actions
- Errors

View: Window → General → Console (Ctrl/Cmd + Shift + C)

#### Test Product IDs
Scanner test mode uses these IDs:
```
prod001 - Yaourt Nature Bio
prod002 - Yaourt Grec 0%
prod003 - Yaourt au Soja Vanille
prod004 - Skyr Nature
prod005 - Pain Complet Bio
...
```

## Troubleshooting

### Issue: Unity Can't Find Scripts
**Solution:**
1. Reimport all: Assets → Reimport All
2. Regenerate project files: Assets → Open C# Project

### Issue: Database Not Loading
**Symptoms:** Error "Could not load products_database.json"

**Solutions:**
1. Verify file exists at `Assets/Resources/Data/products_database.json`
2. Check JSON syntax (use JSONLint validator)
3. Reimport: Right-click file → Reimport

### Issue: Navigation Not Working
**Symptoms:** Scenes don't load when clicking buttons

**Solutions:**
1. Add scenes to Build Settings (File → Build Settings)
2. Verify scene names match in NavigationManager.cs
3. Check scenes exist in Assets/Scenes/

### Issue: Package Import Fails
**Symptoms:** Packages stuck downloading or errors

**Solutions:**
1. Delete Library folder
2. Reopen project (Unity will regenerate)
3. Check internet connection
4. Clear package cache: Window → Package Manager → Gear → Clear Cache

### Issue: AR Foundation Errors
**Note:** AR features not yet active in Sprint 1

**If you see AR errors:**
1. These are expected (Sprint 2 feature)
2. Scanner uses test mode currently
3. AR camera will be enabled in next sprint

## Development Workflow

### Making Changes

1. **Always work in a branch:**
```bash
git checkout -b feature/your-feature-name
```

2. **Test changes:**
   - Play in Editor
   - Check Console for errors
   - Test all affected scenes

3. **Commit regularly:**
```bash
git add .
git commit -m "Description of changes"
git push origin feature/your-feature-name
```

### Adding New Products

1. Open `Assets/Resources/Data/products_database.json`
2. Add new product object:
```json
{
  "id": "prod053",
  "name": "Product Name",
  "brand": "Brand",
  "description": "Description",
  "origin": "Country",
  "originRegion": "Region",
  "nutritionalInfo": {
    "calories": 100,
    "proteins": 5.0,
    "carbohydrates": 10.0,
    "fats": 3.0,
    "fiber": 2.0,
    "sugar": 5.0,
    "salt": 0.5,
    "servingSize": "100g"
  },
  "healthScore": 7.5,
  "ecoScore": 8.0,
  "price": 2.99,
  "categories": ["Category1", "Category2"],
  "tags": ["Tag1", "Tag2"],
  "alternativeIds": ["prod001", "prod002", "prod003"]
}
```

3. Save file
4. Test: Product should appear in database

### Creating New Scenes

1. File → New Scene
2. Save in `Assets/Scenes/`
3. Add scene to Build Settings
4. Create controller script in `Assets/Scripts/UI/`
5. Link controller to scene

### Code Style

Follow existing patterns:
- Use namespaces (SmartRetailAR.*)
- Add XML comments for public methods
- Null-check UI references
- Clean up in OnDestroy()
- Use meaningful variable names

## Performance Tips

### Editor Performance
- Close unnecessary Editor windows
- Disable auto-refresh: Preferences → Asset Pipeline → Auto Refresh: disabled
- Use Editor Coroutines for long operations

### Runtime Performance
- Profile with: Window → Analysis → Profiler
- Monitor memory: Check "Simple" view
- Target: 60 FPS in Editor, 30+ on device

## Next Steps

After successful setup:

1. ✅ Read README.md for project overview
2. ✅ Review TECHNICAL_DOC.md for architecture
3. ✅ Test all scenes in Editor
4. ✅ Explore code in preferred IDE
5. ⏭️ Wait for Sprint 2 (AR Integration)

## Support

### Resources
- Unity Documentation: https://docs.unity3d.com/2022.3/Documentation/Manual/
- AR Foundation Guide: https://docs.unity3d.com/Packages/com.unity.xr.arfoundation@5.1/
- C# Reference: https://docs.microsoft.com/dotnet/csharp/

### Getting Help
1. Check TECHNICAL_DOC.md
2. Review Unity Console errors
3. Search Unity Forums
4. Contact development team

## License

[To be defined]

---

**Version:** 1.0.0 - Sprint 1  
**Last Updated:** December 2024  
**Unity Version:** 2022.3.62f3