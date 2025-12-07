# Technical Documentation - Sprint 1

## Overview
This document provides technical details about the Sprint 1 implementation of Smart Retail AR.

## Code Architecture

### Design Patterns

#### 1. Singleton Pattern
Used for all manager classes to ensure single instance across scenes:

**Implementation:**
```csharp
private static ManagerType _instance;
public static ManagerType Instance
{
    get
    {
        if (_instance == null)
        {
            GameObject go = new GameObject("ManagerName");
            _instance = go.AddComponent<ManagerType>();
            DontDestroyOnLoad(go);
        }
        return _instance;
    }
}
```

**Applied to:**
- NavigationManager
- QRCodeManager  
- ProductDatabase

**Benefits:**
- Global access point
- Persistent across scene loads
- Lazy initialization
- Thread-safe in Unity's single-threaded environment

#### 2. Component Pattern
Each UI screen has a dedicated controller component:

- **HomeController** - Manages home screen UI and navigation
- **ScannerController** - Handles QR scanning logic
- **ProductInfoController** - Displays product details
- **RecommendationsController** - Manages alternative products
- **ProfileController** - User profile and preferences

#### 3. Data Transfer Object (DTO)
ProductData acts as a DTO for product information:
- Serializable with Unity's JsonUtility
- Contains all product properties
- Nested NutritionalInfo class

### Data Serialization

#### JSON Serialization Strategy
Unity's JsonUtility has limitations with dictionaries. We use parallel lists:

```csharp
// Instead of Dictionary<string, int>
public List<string> scanCountKeys;
public List<int> scanCountValues;

// Runtime dictionary for performance
[NonSerialized]
private Dictionary<string, int> _productScanCount;
```

**Rationale:**
- JsonUtility doesn't support Dictionary serialization
- Parallel lists maintain serialization compatibility
- Runtime dictionary provides O(1) lookups
- Sync method keeps data consistent

#### PlayerPrefs Usage
UserPreferences saved as JSON string in PlayerPrefs:
- Lightweight and persistent
- Cross-platform compatible
- Suitable for user settings
- Not for sensitive data

### Scene Management

#### Navigation Flow
```
HomeScene
├── ScannerScene
│   └── ProductInfoScene
│       └── RecommendationsScene
│           └── ProductInfoScene (alternative product)
└── ProfileScene
```

#### Async Loading
Scenes loaded asynchronously to prevent frame drops:
```csharp
private IEnumerator LoadSceneAsync(string sceneName)
{
    AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
    while (!asyncLoad.isDone)
    {
        float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);
        yield return null;
    }
}
```

### Event System

#### QR Code Events
QRCodeManager uses delegates for event handling:

```csharp
public delegate void QRCodeScannedDelegate(string productId);
public event QRCodeScannedDelegate OnQRCodeScanned;
```

**Benefits:**
- Decouples scanner from UI
- Multiple subscribers possible
- Type-safe callbacks

## Database Implementation

### ProductDatabase Class

#### Loading Strategy
1. Load JSON from Resources on first access
2. Parse into List<ProductData>
3. Build Dictionary for O(1) lookups by ID
4. Cache in memory for subsequent queries

#### Query Methods
- `GetProduct(string id)` - O(1) lookup by ID
- `GetAllProducts()` - Returns cached list
- `GetProductsByCategory(string)` - LINQ filtering
- `GetProductsByTag(string)` - LINQ filtering
- `FilterProducts(...)` - Multiple criteria filtering
- `GetAlternatives(string id)` - Retrieves alternative products

#### Performance Considerations
- Dictionary lookup: O(1)
- LINQ queries: O(n) but acceptable for 50-100 products
- In-memory caching: No disk I/O after initial load
- Lazy loading: Database loads on first access

### Data Model

#### Product Structure
```
ProductData
├── Basic Info (id, name, brand, description)
├── Origin (country, region)
├── Nutritional Info (nested object)
│   ├── Macros (calories, proteins, carbs, fats)
│   └── Details (fiber, sugar, salt)
├── Scores (health, eco)
├── Price
├── Categories (list)
├── Tags (list)
└── Alternative IDs (list)
```

#### Tag System
Tags enable flexible filtering:
- Bio
- Vegan
- Vegetarian
- GlutenFree
- LactoseFree
- HighProtein
- WholeGrain
- Local
- FairTrade

## UI Controllers

### Scanner Test Mode

#### Purpose
Enable development and testing without AR camera:
```csharp
public bool testMode = true;
public string[] testProductIds = { "prod001", ... };
```

#### Simulation Flow
1. User clicks scan button
2. Random product ID selected from testProductIds
3. Simulated 0.5s delay
4. Product validation against database
5. Navigation to ProductInfoScene

#### Production Mode
Set `testMode = false` to enable:
- Real AR camera
- Actual QR code detection
- AR Foundation integration (Sprint 2)

### Filter Implementation

#### Recommendations Filtering
Multiple boolean filters applied sequentially:

```csharp
var filtered = _alternatives.AsEnumerable();

if (bioToggle.isOn)
    filtered = filtered.Where(p => p.tags.Contains("Bio"));
    
if (ecoToggle.isOn)
    filtered = filtered.Where(p => p.ecoScore >= 7f);
```

**Performance:**
- LINQ deferred execution
- Filters applied lazily
- Final materialization with ToList()

### Score Visualization

#### Color Coding
```csharp
private Color GetScoreColor(float score)
{
    if (score >= 7f) return Green;      // Good
    else if (score >= 4f) return Orange; // Moderate  
    else return Red;                     // Poor
}
```

#### UI Elements
- Slider components (0-1 range normalized from 0-10)
- Text labels with formatted scores
- Color-coded backgrounds

## Memory Management

### DontDestroyOnLoad
Managers persist across scenes:
- Prevents recreation on scene load
- Maintains singleton instance
- Preserves cached data

### Cleanup
Controllers clean up in OnDestroy:
```csharp
private void OnDestroy()
{
    if (button != null)
        button.onClick.RemoveListener(OnButtonClicked);
}
```

**Prevents:**
- Memory leaks
- Null reference exceptions
- Duplicate event subscriptions

## Error Handling

### Defensive Programming
- Null checks before accessing references
- Validation of product IDs
- Fallback to empty collections
- Debug.Log for error tracking

### Example:
```csharp
if (string.IsNullOrEmpty(productId))
{
    Debug.LogError("No product ID available");
    return;
}

var product = ProductDatabase.Instance.GetProduct(productId);
if (product == null)
{
    Debug.LogError($"Product not found: {productId}");
    return;
}
```

## Performance Metrics

### Target Metrics (Sprint 1)
- UI Frame Rate: ≥60 FPS ✓
- Scene Load Time: <2 seconds ✓
- Database Load: <500ms ✓
- Memory Usage: <100MB ✓

### Optimization Techniques
1. **Object Pooling** - Ready for product cards (Sprint 2)
2. **Lazy Loading** - Database loads on first access
3. **Caching** - In-memory dictionaries
4. **Async Operations** - Scene loading

## Testing Strategy

### Manual Testing Checklist
- [ ] Home screen displays correctly
- [ ] Navigation to scanner works
- [ ] Test mode scan selects random product
- [ ] Product info displays all fields
- [ ] Scores show correct colors
- [ ] Filters work in recommendations
- [ ] Profile saves preferences
- [ ] History tracks scanned products
- [ ] Back navigation works correctly

### Edge Cases Handled
- Product not found in database
- Empty alternatives list
- First-time user (no preferences)
- Invalid QR data format
- Missing UI references

## Future Enhancements (Sprint 2+)

### AR Integration
1. Replace test mode with AR Foundation camera
2. Implement real QR detection with ZXing
3. Add AR overlay for product information
4. Enable object tracking

### Database
1. Remote database with REST API
2. Image loading from URLs
3. Real-time product updates
4. Offline caching strategy

### UI/UX
1. Animations and transitions
2. Loading indicators
3. Error screens
4. Onboarding tutorial
5. Search functionality

### Performance
1. Object pooling for product cards
2. Texture streaming for images
3. Level of detail (LOD) for 3D models
4. Background loading

## Dependencies

### Unity Packages
- **AR Foundation 5.1.5** - AR framework
- **ARCore XR Plugin 5.1.5** - Android AR
- **ARKit XR Plugin 5.1.5** - iOS AR
- **XR Interaction Toolkit 3.0.8** - AR interactions
- **UGUI 2.0.0** - UI system
- **TextMeshPro 3.0.9** - Text rendering
- **Addressables 2.3.1** - Asset management

### Version Compatibility
All packages verified compatible with Unity 2022.3.62f3 LTS.

## Code Quality

### Standards
- Clear naming conventions
- XML documentation comments
- Consistent formatting
- Namespace organization

### Best Practices
- Single Responsibility Principle
- Dependency Injection ready
- Event-driven architecture
- Separation of concerns

## Deployment

### Build Settings

#### Android
```
Package Name: com.smartretail.ar
Version: 1.0.0
Min API: 24 (Android 7.0)
Target API: Latest
Backend: IL2CPP
Architecture: ARM64
```

#### iOS
```
Bundle ID: com.smartretail.ar
Version: 1.0.0
Min iOS: 11.0
SDK: Device
Architecture: ARM64
```

### AR Requirements
- Android: ARCore supported device
- iOS: ARKit compatible device (A9 chip or later)

## Troubleshooting

### Common Issues

#### Database Not Loading
- Check file path: `Resources/Data/products_database.json`
- Verify JSON syntax with validator
- Check Unity console for errors

#### Navigation Not Working
- Verify scene names in NavigationManager constants
- Add scenes to Build Settings
- Check scene naming in project

#### Scores Not Displaying
- Verify Slider and Text references
- Check score values in database (0-10 range)
- Ensure UI components are active

## Contact

For technical questions or issues, consult the development team.

## Changelog

### v1.0.0 - Sprint 1
- Initial implementation
- All frontend interfaces
- Product database with 52 products
- Test mode scanner
- User preferences system
- Complete navigation system