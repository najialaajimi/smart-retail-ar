README for Smart Retail AR Unity Project
========================================

## Project Overview
Smart Retail AR - Complete implementation with all 4 sprints integrated.

## Unity Version
**Required**: Unity 2022.3.62f3 LTS

## Project Structure

### Sprints
1. **Sprint 1**: QR Code Scanner & Basic Info (6 scenes, 9 scripts)
2. **Sprint 2**: AR Integration (4 scenes, 12 scripts)
3. **Sprint 3**: Recommendations Engine (5 scenes, 13 scripts)
4. **Sprint 4**: Testing & Analytics (5 scenes, 16 scripts)

### Key Features
- 5000+ product database
- AR Foundation integration
- ML-based recommendations
- Performance optimization
- Analytics tracking
- Battery optimization

## Getting Started

### Requirements
- Unity 2022.3.62f3 LTS
- AR-capable device (Android API 24+ or iOS 11.0+)
- Minimum 2GB RAM
- Camera permission

### Setup
1. Open project in Unity 2022.3.62f3
2. Wait for package imports to complete
3. Configure build settings for target platform
4. Build and deploy to device

### First Run
1. App starts with SplashScene
2. Onboarding tutorial (first launch only)
3. Main home screen with all features

## Scenes

### Sprint 1 Scenes
- SplashScene.unity - App startup
- OnboardingScene.unity - Tutorial
- HomeScene.unity - Main menu
- QRScannerScene.unity - QR code scanning
- ProductInfoScene.unity - Product details
- SettingsScene.unity - App settings

### Sprint 2 Scenes
- ARCameraScene.unity - AR camera
- ARProductTrackingScene.unity - AR tracking
- AROverlayScene.unity - AR overlays
- ARCalibrationScene.unity - AR setup

### Sprint 3 Scenes
- RecommendationsScene.unity - Product recommendations
- FilterScene.unity - Advanced filters
- ComparisonScene.unity - Product comparison
- WishlistScene.unity - Saved products
- ProfileScene.unity - User profile

### Sprint 4 Scenes
- TestModeScene.unity - User testing
- AnalyticsScene.unity - Analytics dashboard
- FeedbackScene.unity - Feedback collection
- TutorialScene.unity - Interactive tutorials
- DebugScene.unity - Debug tools

## Key Scripts

### Managers (Singletons)
- NavigationManager - Scene navigation
- ProductDatabase - 5000+ products
- QRCodeManager - QR scanning
- DataManager - Data persistence
- ARManager - AR functionality
- RecommendationEngine - AI recommendations
- TestManager - Testing framework
- AnalyticsManager - Event tracking
- PerformanceOptimizer - Performance management
- MemoryManager - Memory optimization
- BatteryOptimizer - Battery management

## Database
- **Location**: Assets/Resources/Data/products_database.json
- **Products**: 5000+ items
- **Categories**: Alimentation, Hygiène, Entretien, Bébé, Animalerie
- **Data**: Nutrition, scores, certifications, alternatives

## Testing
- Test mode enabled by default in QRScannerController
- Simulates scans without AR hardware
- Performance profiling available
- Analytics tracking for all events

## Performance
- Target: 60 FPS on high-end devices
- Auto-optimization based on performance
- Battery-aware quality settings
- Memory management with caching

## Build Configuration

### Android
- Minimum API: 24
- Target API: 33
- IL2CPP backend
- ARM64 architecture

### iOS
- Minimum: iOS 11.0
- Target: iOS 16.0
- IL2CPP backend
- ARM64 architecture

## License
All rights reserved - Smart Retail AR Project

## Version
1.0.0 - Complete implementation (Sprints 1-4)
