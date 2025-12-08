# Unity Scenes Structure

## Overview
This directory contains all Unity scene files (.unity) for the Smart Retail AR application.

## Scene Directory Structure

```
Assets/Scenes/
├── Frontend/              # UI and frontend scenes
│   ├── HomeScene.unity               - Main entry point
│   ├── ProductInfoScene.unity        - Product details display
│   ├── RecommendationsScene.unity    - Recommendations view
│   └── ProfileScene.unity            - User profile
└── AR/                   # AR-enabled scenes
    ├── ARScannerScene.unity          - AR product scanner
    └── ARProductViewScene.unity      - AR product view

```

## Frontend Scenes

### HomeScene.unity
- **Purpose**: Main entry point of the application
- **Components**: HomeController, navigation buttons
- **Navigation**: Can navigate to Scanner, Recommendations, Profile

### ProductInfoScene.unity
- **Purpose**: Display detailed product information
- **Components**: ProductInfoController, product details UI
- **Features**: Shows nutritional, ecological, and ethical data

### RecommendationsScene.unity
- **Purpose**: Display personalized product recommendations
- **Components**: RecommendationsController, product cards
- **Features**: Similar products, alternatives, trending items

### ProfileScene.unity
- **Purpose**: User profile and preferences management
- **Components**: ProfileController, user settings
- **Features**: Dietary preferences, allergen filters, scan history

## AR Scenes

### ARScannerScene.unity
- **Purpose**: AR product scanning with camera
- **Components**: 
  - AR Session Origin with AR Camera
  - AR Session
  - ScannerController
  - ARImageTracker
  - ARSessionManager
- **Features**: Real-time image recognition, product detection
- **Test Mode**: Set `testMode = true` in ScannerController for testing without AR

### ARProductViewScene.unity
- **Purpose**: View product in AR with 3D visualization
- **Components**: 
  - AR Session Origin with AR Camera
  - AR Session
  - ARDataOverlay
- **Features**: AR overlay of product information, 3D models

## Scene Configuration

### Build Settings
To use these scenes in builds, add them to Build Settings in this order:

1. Assets/Scenes/Frontend/HomeScene.unity
2. Assets/Scenes/AR/ARScannerScene.unity
3. Assets/Scenes/Frontend/ProductInfoScene.unity
4. Assets/Scenes/Frontend/RecommendationsScene.unity
5. Assets/Scenes/Frontend/ProfileScene.unity
6. Assets/Scenes/AR/ARProductViewScene.unity

### Scene Names for Navigation
When using NavigationManager, use these scene names:
- "HomeScene"
- "ARScannerScene"
- "ProductInfoScene"
- "RecommendationsScene"
- "ProfileScene"
- "ARProductViewScene"

## Testing Scenes

### Testing in Unity Editor

**Frontend Scenes:**
- Can be tested directly in the Unity Editor
- No special configuration required

**AR Scenes:**
- Require AR-capable device for full functionality
- Use Test Mode in ScannerController for editor testing:
  ```csharp
  testMode = true;
  testProductIds = new string[] { "PROD001", "PROD002", "PROD003" };
  ```

### Testing on Device

1. Build for Android (ARCore) or iOS (ARKit)
2. Install on AR-capable device
3. Point camera at product images
4. View AR overlays and information

## Scene Setup Notes

### AR Session Configuration
All AR scenes include:
- **AR Session Origin**: Contains AR Camera and manages AR space
- **AR Camera**: Replaces standard camera, provides AR view
- **AR Session**: Manages AR session lifecycle

### Script References
Scene GameObjects reference these scripts:
- HomeController → Assets/Scripts/UI/HomeController.cs
- ScannerController → Assets/Scripts/UI/ScannerController.cs
- ProductInfoController → Assets/Scripts/UI/ProductInfoController.cs
- ARImageTracker → Assets/Scripts/AR/ARImageTracker.cs
- ARDataOverlay → Assets/Scripts/AR/ARDataOverlay.cs
- ARSessionManager → Assets/Scripts/AR/ARSessionManager.cs

## Customization

### Adding New Scenes
1. Create scene in appropriate folder (Frontend/AR/Testing)
2. Add required GameObjects and components
3. Add scene to Build Settings
4. Update NavigationManager scene names

### Modifying Existing Scenes
1. Open scene in Unity Editor
2. Modify GameObjects, components, or layout
3. Save scene (Ctrl+S / Cmd+S)
4. Test changes in Editor or on device

## Important Notes

- AR scenes require AR Foundation 5.1.5+
- Android requires ARCore-compatible device (API 24+)
- iOS requires ARKit-compatible device (iOS 11.0+)
- Test Mode is available for development without AR hardware
- All scenes work with the Singleton managers (ProductDatabase, NavigationManager, etc.)

## Troubleshooting

**Scene won't open:**
- Ensure Unity version is 2022.3.62f3 LTS
- Check that all packages are installed (AR Foundation, etc.)

**AR not working:**
- Verify device is AR-compatible
- Check XR Plug-in Management settings
- Ensure camera permissions are granted

**Scripts not attached:**
- Script GUIDs in .unity files may need regeneration
- Reimport scripts in Unity Editor
- Manually attach scripts to GameObjects

## Additional Resources

- See Documentation/sprint2details.md for AR setup
- See Documentation/DEVELOPMENT.md for development guide
- See README.md for project overview
