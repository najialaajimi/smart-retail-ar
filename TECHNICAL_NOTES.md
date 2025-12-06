# Technical Notes & Future Improvements

## Sprint 1 - Code Review Feedback

This document tracks technical debt and improvement opportunities identified during Sprint 1 development.

## Resolved Issues

### ✅ Dictionary Serialization (UserPreferences.cs)
**Issue**: Dictionary serialization fails with Unity's JsonUtility  
**Status**: FIXED  
**Solution**: Implemented parallel lists (scanCountKeys, scanCountValues) with runtime dictionary for performance  
**Files**: Assets/Scripts/Data/UserPreferences.cs

## Future Improvements for Sprint 2+

### 1. Performance Optimizations

#### RecommendationsController.cs - Line 115-116
**Issue**: Multiple LINQ operations create new lists each iteration  
**Priority**: Medium  
**Recommendation**: Build filter predicate once and apply it, or use more efficient filtering  
**Estimated Effort**: 2 hours

```csharp
// Current (creates multiple lists):
filteredAlternatives = filteredAlternatives.Where(...).ToList();
filteredAlternatives = filteredAlternatives.Where(...).ToList();

// Suggested:
var predicate = BuildFilterPredicate(filters);
filteredAlternatives = alternatives.Where(predicate).ToList();
```

#### ProfileController.cs - Line 155-158
**Issue**: TakeLast and Reverse create unnecessary intermediate collections  
**Priority**: Low  
**Recommendation**: Use direct list access with indices for better performance  
**Estimated Effort**: 1 hour

```csharp
// Current:
var recentScans = userPreferences.scannedProductIds
    .TakeLast(10)
    .Reverse()
    .ToList();

// Suggested:
var count = userPreferences.scannedProductIds.Count;
var startIndex = Math.Max(0, count - 10);
var recentScans = new List<string>();
for (int i = count - 1; i >= startIndex; i--)
{
    recentScans.Add(userPreferences.scannedProductIds[i]);
}
```

### 2. Navigation System Enhancement

#### NavigationManager.cs - Line 88-101
**Issue**: Back navigation logic is confusing with simple scene swap  
**Priority**: High (for better UX)  
**Recommendation**: Implement proper navigation stack  
**Estimated Effort**: 4 hours

```csharp
// Suggested approach:
private Stack<string> navigationStack = new Stack<string>();

public void NavigateToScene(string sceneName)
{
    navigationStack.Push(currentScene);
    currentScene = sceneName;
    SceneManager.LoadScene(sceneName);
}

public void NavigateBack()
{
    if (navigationStack.Count > 0)
    {
        currentScene = navigationStack.Pop();
        SceneManager.LoadScene(currentScene);
    }
}
```

### 3. Internationalization (i18n)

#### QRCodeManager.cs - Line 75
**Issue**: Hardcoded French error messages  
**Priority**: Medium  
**Recommendation**: Implement localization system  
**Estimated Effort**: 8 hours (full system)

```csharp
// Current:
OnScanError?.Invoke("QR code vide ou invalide");

// Suggested:
OnScanError?.Invoke(LocalizationManager.GetString("ERROR_QR_EMPTY"));
```

#### ScannerController.cs - Line 106
**Issue**: Hardcoded French UI text  
**Priority**: Medium  
**Recommendation**: Same as above - use localization system

**Suggested Implementation**:
- Create LocalizationManager singleton
- JSON files for each language (fr.json, en.json, es.json, de.json)
- String key lookup system
- Language switching in settings

### 4. UI Text Management

**Recommendation**: Create centralized strings resource file

```csharp
public static class UIStrings
{
    // Scanner
    public const string SCANNER_INSTRUCTION = "Pointez vers le QR code du produit";
    public const string SCANNER_SCANNING = "Scan en cours...";
    public const string SCANNER_SUCCESS = "Scan réussi !";
    
    // Errors
    public const string ERROR_QR_EMPTY = "QR code vide ou invalide";
    public const string ERROR_PRODUCT_NOT_FOUND = "Produit non trouvé dans la base de données";
    
    // etc...
}
```

## Architecture Improvements

### 1. Dependency Injection

**Current**: Direct singleton access everywhere  
**Recommended**: Consider DI container for better testability

**Benefits**:
- Easier unit testing
- Better modularity
- Clearer dependencies

**Priority**: Low (for production code)  
**Estimated Effort**: 16 hours

### 2. Event System Enhancement

**Current**: Direct delegate events  
**Recommended**: Implement event bus for decoupled architecture

**Benefits**:
- Better decoupling
- Easier to add observers
- Central event management

**Priority**: Low  
**Estimated Effort**: 8 hours

### 3. Data Validation

**Recommendation**: Add data validation layer

```csharp
public class ProductDataValidator
{
    public static bool Validate(ProductData product)
    {
        if (string.IsNullOrEmpty(product.productId)) return false;
        if (string.IsNullOrEmpty(product.name)) return false;
        if (product.price < 0) return false;
        if (product.scores.healthScore < 0 || product.scores.healthScore > 100) return false;
        // etc...
        return true;
    }
}
```

**Priority**: Medium  
**Estimated Effort**: 4 hours

## Testing Improvements

### 1. Unit Tests

**Priority**: High (for production)  
**Estimated Effort**: 20 hours

**Recommended Tests**:
- ProductDatabase filtering logic
- UserPreferences save/load
- NavigationManager history
- QRCodeManager validation
- Data model serialization

### 2. Integration Tests

**Priority**: Medium  
**Estimated Effort**: 12 hours

**Recommended Tests**:
- Full navigation flow
- Product scan to display flow
- Filter and sort combinations
- Settings persistence

### 3. Performance Tests

**Priority**: Medium  
**Estimated Effort**: 8 hours

**Metrics to Test**:
- Scene load times
- Database query performance
- UI responsiveness
- Memory usage
- Frame rate stability

## Sprint 2 Specific

### 1. AR Foundation Integration

**Requirements**:
- AR Session Manager
- ARCamera configuration
- Plane detection
- Image tracking for QR codes
- 3D product models placement

**Estimated Effort**: 40 hours

### 2. Real QR Scanning

**Requirements**:
- ZXing.Net library integration
- Camera feed processing
- Real-time QR decoding
- Performance optimization

**Estimated Effort**: 16 hours

### 3. Visual Assets

**Requirements**:
- Unity UI implementation for all screens
- Product 3D models (50 models)
- UI icons and illustrations
- Animations and transitions
- Sound effects

**Estimated Effort**: 60 hours

### 4. Image Loading

**Requirements**:
- Async image loading from URLs
- Caching system
- Placeholder images
- Error handling

**Estimated Effort**: 8 hours

## Code Quality Metrics

### Current State (Sprint 1)

**Strengths**:
✅ Well-organized architecture  
✅ Consistent naming conventions  
✅ Good separation of concerns  
✅ Comprehensive documentation  
✅ Error handling present  

**Areas for Improvement**:
⚠️ No unit tests yet  
⚠️ Hardcoded strings (i18n)  
⚠️ Some LINQ performance issues  
⚠️ Navigation stack not implemented  
⚠️ No data validation layer  

### Goals for Sprint 2

**Code Coverage**: Target 70%+ test coverage  
**Performance**: Maintain 60 FPS on mid-range devices  
**Localization**: Support 4 languages (FR, EN, ES, DE)  
**Code Quality**: Zero compiler warnings  

## Best Practices to Adopt

### 1. Async/Await Pattern

For long-running operations:

```csharp
public async Task<ProductData> LoadProductAsync(string id)
{
    await Task.Delay(100); // Simulate delay
    return ProductDatabase.Instance.GetProductById(id);
}
```

### 2. Object Pooling

For frequently created/destroyed objects:

```csharp
public class ProductCardPool : MonoBehaviour
{
    private Queue<ProductCard> pool = new Queue<ProductCard>();
    
    public ProductCard GetCard()
    {
        if (pool.Count > 0)
            return pool.Dequeue();
        return Instantiate(cardPrefab);
    }
    
    public void ReturnCard(ProductCard card)
    {
        card.gameObject.SetActive(false);
        pool.Enqueue(card);
    }
}
```

### 3. ScriptableObjects

For configuration data:

```csharp
[CreateAssetMenu(fileName = "AppConfig", menuName = "Config/App Config")]
public class AppConfig : ScriptableObject
{
    public int maxProductsToLoad = 50;
    public float scanTimeout = 5f;
    public bool debugMode = false;
}
```

## Documentation Updates Needed

### For Sprint 2

1. Update README with AR features
2. Add AR integration guide
3. Update API reference with new classes
4. Add troubleshooting for AR issues
5. Update testing guide with device testing
6. Add deployment guide for iOS/Android

## Monitoring and Analytics

### Recommended Metrics

**Performance**:
- Average FPS
- Scene load times
- Memory usage
- Network latency (if applicable)

**Usage**:
- Products scanned per session
- Most viewed products
- Filter usage statistics
- Navigation patterns

**Errors**:
- Scan failures
- Database errors
- Navigation errors
- Crash reports

## Security Considerations

### Data Privacy

- ✅ Local data storage (no server)
- ✅ No sensitive data collected
- ⚠️ Consider GDPR compliance for scan history
- ⚠️ Add option to clear all data

### Best Practices

- Validate all user inputs
- Sanitize product data from database
- Secure local storage (encryption if needed)
- Handle permissions properly (camera, storage)

## Maintenance Plan

### Regular Tasks

**Weekly**:
- Review console logs
- Monitor performance metrics
- Check for Unity updates

**Monthly**:
- Update dependencies
- Review and address technical debt
- Performance optimization review

**Quarterly**:
- Architecture review
- Security audit
- Documentation update

## Conclusion

Sprint 1 has delivered a solid foundation with minimal technical debt. The identified improvements are not critical for functionality but will enhance performance, maintainability, and user experience in future sprints.

**Priority Order for Sprint 2**:
1. Critical serialization issues → ✅ FIXED
2. Navigation stack implementation
3. Localization system
4. Unit test coverage
5. Performance optimizations
6. Data validation

---

**Last Updated**: December 6, 2024  
**Next Review**: Before Sprint 2 kickoff
