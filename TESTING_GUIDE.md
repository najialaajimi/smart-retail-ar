# Smart Retail AR - Testing Guide

## Overview

This guide explains how to test the Smart Retail AR application in Sprint 1.

## Testing Modes

### 1. Unity Editor Play Mode

**Primary testing environment for development**

#### How to Test:
1. Open Unity Editor
2. Load `HomeScene.unity`
3. Click Play button
4. Navigate through the application

#### What to Test:
- ✅ Navigation between all screens
- ✅ Scanner simulation
- ✅ Product information display
- ✅ Recommendations filtering and sorting
- ✅ Profile preferences and history
- ✅ Data persistence (preferences save/load)

### 2. Standalone Build Testing

#### Create a Build:
1. File > Build Settings
2. Select platform (Android/iOS/PC)
3. Click "Build and Run"
4. Test on actual device/computer

#### Performance Testing:
- Check FPS (should maintain 60 FPS)
- Monitor load times (should be < 2 seconds)
- Test on different screen sizes

## Test Cases

### Test Case 1: Home Screen Navigation

**Objective**: Verify home screen loads and navigation works

**Steps**:
1. Launch application
2. Verify "Smart Retail AR" title displays
3. Click "Scanner Produit" button
4. Verify navigation to Scanner scene
5. Use back button to return to home
6. Click "Mon Profil" button
7. Verify navigation to Profile scene

**Expected Result**: All navigation works smoothly with no errors

**Status**: ⬜ Not Tested / ✅ Passed / ❌ Failed

---

### Test Case 2: QR Code Scanner (Test Mode)

**Objective**: Verify scanner simulation works correctly

**Steps**:
1. Navigate to Scanner screen
2. Click capture/scan button
3. Wait for automatic scan (1.5s)
4. Verify success message appears
5. Verify navigation to Product Info screen
6. Check that product data is displayed

**Expected Result**: Scanner simulates scan and navigates to product info

**Test Products**:
- PROD001: Yaourt Nature Bio
- PROD010: Pâtes Complètes Bio
- PROD016: Pommes Bio

**Status**: ⬜ Not Tested / ✅ Passed / ❌ Failed

---

### Test Case 3: Product Information Display

**Objective**: Verify all product information displays correctly

**Steps**:
1. Scan a product (PROD001)
2. Verify the following are displayed:
   - Product name: "Yaourt Nature Bio"
   - Brand: "Biocoop"
   - Nutritional info (calories, proteins, etc.)
   - Origin (country, region, producer)
   - Health score (85/100)
   - Eco score (90/100)
   - Nutrition grade (A)
   - Tags: ["Bio", "Local", "Éco-responsable"]
3. Click "Voir les alternatives" button
4. Verify navigation to Recommendations

**Expected Result**: All product data displays correctly

**Status**: ⬜ Not Tested / ✅ Passed / ❌ Failed

---

### Test Case 4: Recommendations Filtering

**Objective**: Verify filtering system works correctly

**Steps**:
1. Navigate to Recommendations screen (scan PROD001 first)
2. Verify alternative products are listed
3. Enable "Bio" filter
4. Verify only Bio products remain
5. Disable "Bio" filter
6. Enable "Éco-responsable" filter
7. Verify filtering updates
8. Test multiple filters simultaneously

**Expected Result**: Filters work correctly and update the list

**Filters to Test**:
- ✅ Bio
- ✅ Éco-responsable
- ✅ Prix (maximum price)
- ✅ Score santé (minimum health score)

**Status**: ⬜ Not Tested / ✅ Passed / ❌ Failed

---

### Test Case 5: Recommendations Sorting

**Objective**: Verify sorting system works correctly

**Steps**:
1. Navigate to Recommendations screen
2. Select "Prix croissant" in dropdown
3. Verify products are sorted by ascending price
4. Select "Prix décroissant"
5. Verify products are sorted by descending price
6. Select "Score santé"
7. Verify products are sorted by health score
8. Select "Score écologique"
9. Verify products are sorted by eco score

**Expected Result**: All sort options work correctly

**Status**: ⬜ Not Tested / ✅ Passed / ❌ Failed

---

### Test Case 6: User Profile - Preferences

**Objective**: Verify preferences can be set and saved

**Steps**:
1. Navigate to Profile screen
2. Select dietary preference: "Végétarien"
3. Select dietary preference: "Sans gluten"
4. Verify preferences are visually marked
5. Exit application
6. Restart application
7. Navigate to Profile screen
8. Verify preferences are still selected

**Expected Result**: Preferences persist across sessions

**Preferences to Test**:
- Végétarien
- Végétalien
- Sans gluten
- Sans lactose
- Bio uniquement
- Éco-responsable
- Local
- Sans allergènes

**Status**: ⬜ Not Tested / ✅ Passed / ❌ Failed

---

### Test Case 7: User Profile - History

**Objective**: Verify scan history is saved and displayed

**Steps**:
1. Scan product PROD001
2. Return to home
3. Scan product PROD010
4. Return to home
5. Scan product PROD016
6. Navigate to Profile screen
7. Verify history shows last 3 scans
8. Verify scan count is displayed
9. Click on a history item
10. Verify navigation to that product's info

**Expected Result**: History displays correctly and is clickable

**Status**: ⬜ Not Tested / ✅ Passed / ❌ Failed

---

### Test Case 8: User Profile - Settings

**Objective**: Verify settings can be changed and saved

**Steps**:
1. Navigate to Profile screen
2. Toggle "Son" setting off
3. Toggle "Feedback haptique" off
4. Toggle "Mode sombre" on
5. Change language dropdown
6. Exit and restart application
7. Navigate to Profile screen
8. Verify all settings are persisted

**Expected Result**: Settings persist across sessions

**Settings to Test**:
- ✅ Sound enabled/disabled
- ✅ Haptic feedback enabled/disabled
- ✅ Tutorial show/hide
- ✅ Dark mode enabled/disabled
- ✅ Language selection

**Status**: ⬜ Not Tested / ✅ Passed / ❌ Failed

---

### Test Case 9: Database Loading

**Objective**: Verify product database loads correctly

**Steps**:
1. Check Unity Console on startup
2. Verify "Loaded X products from database" message
3. Verify X = 50 (number of products)
4. Test scanning various product IDs
5. Verify all products load correctly

**Expected Result**: Database loads 50 products successfully

**Status**: ⬜ Not Tested / ✅ Passed / ❌ Failed

---

### Test Case 10: Error Handling

**Objective**: Verify error messages display correctly

**Steps**:
1. Modify scanner to simulate error
2. In `ScannerController.cs`, trigger `OnScanError`
3. Verify error message displays
4. Verify error message color is red
5. Verify error message clears after 3 seconds

**Expected Result**: Errors are handled gracefully

**Status**: ⬜ Not Tested / ✅ Passed / ❌ Failed

---

## Performance Testing

### Frame Rate Test

**Target**: 60 FPS

**Steps**:
1. Enable Stats in Game view
2. Navigate through all screens
3. Monitor FPS counter
4. Note any drops below 60 FPS

**Acceptance Criteria**: FPS ≥ 60 in all screens

---

### Load Time Test

**Target**: < 2 seconds per screen

**Steps**:
1. Time navigation between screens
2. Record load times for:
   - Home → Scanner
   - Scanner → Product Info
   - Product Info → Recommendations
   - Any → Profile

**Acceptance Criteria**: All loads < 2 seconds

---

### Memory Usage Test

**Target**: Optimized for mobile devices

**Steps**:
1. Use Unity Profiler
2. Monitor memory allocation
3. Check for memory leaks
4. Test with multiple scans

**Acceptance Criteria**: No memory leaks, stable memory usage

---

## Integration Testing

### Navigation Flow Test

**Complete User Journey**:
1. Start at Home
2. Navigate to Scanner
3. Scan product
4. View product info
5. View alternatives
6. Select alternative
7. View alternative info
8. Return to home
9. View profile
10. Check history
11. Modify preferences

**Expected Result**: Complete flow works without errors

---

## Regression Testing

Run all test cases after any code changes to ensure no functionality is broken.

### Regression Checklist:
- ⬜ All navigation works
- ⬜ Scanner simulation works
- ⬜ Product info displays correctly
- ⬜ Recommendations filtering works
- ⬜ Recommendations sorting works
- ⬜ Profile preferences save/load
- ⬜ Profile history works
- ⬜ Settings persist
- ⬜ Database loads correctly
- ⬜ Performance targets met

---

## Bug Reporting Template

When a bug is found, report using this template:

```
**Bug Title**: [Short description]

**Severity**: Critical / High / Medium / Low

**Steps to Reproduce**:
1. 
2. 
3. 

**Expected Behavior**:
[What should happen]

**Actual Behavior**:
[What actually happens]

**Screenshots/Logs**:
[If applicable]

**Environment**:
- Unity Version: 
- Platform: 
- Device: 

**Additional Notes**:
[Any other relevant information]
```

---

## Test Results Summary

### Sprint 1 Acceptance Criteria

| Criteria | Status |
|----------|--------|
| All interfaces created and functional | ⬜ |
| Smooth navigation between screens | ⬜ |
| QR scanner operational (simulation) | ⬜ |
| Product information displays correctly | ⬜ |
| Recommendations system functional | ⬜ |
| Responsive and mobile-optimized interface | ⬜ |
| Modular and maintainable code | ⬜ |
| Database with 50+ products | ⬜ |

### Overall Status

- **Total Test Cases**: 10
- **Passed**: 0
- **Failed**: 0
- **Not Tested**: 10

---

## Automated Testing (Future)

For Sprint 2, consider implementing:
- Unit tests for data classes
- Integration tests for managers
- UI automation tests
- Performance benchmarking

---

**Happy Testing! 🧪**
