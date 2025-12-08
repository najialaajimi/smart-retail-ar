# Third-Party Dependencies

This project uses the following third-party libraries and packages:

## Unity Packages (via Package Manager)

### AR Foundation 5.1.5
- **Publisher:** Unity Technologies
- **License:** Unity Companion License
- **Purpose:** Cross-platform AR framework
- **Documentation:** https://docs.unity3d.com/Packages/com.unity.xr.arfoundation@5.1

### AR Core XR Plugin 5.1.5
- **Publisher:** Unity Technologies
- **License:** Unity Companion License
- **Purpose:** Android AR support
- **Documentation:** https://docs.unity3d.com/Packages/com.unity.xr.arcore@5.1

### AR Kit XR Plugin 5.1.5
- **Publisher:** Unity Technologies
- **License:** Unity Companion License
- **Purpose:** iOS AR support
- **Documentation:** https://docs.unity3d.com/Packages/com.unity.xr.arkit@5.1

### TextMeshPro 3.0.6
- **Publisher:** Unity Technologies
- **License:** Unity Companion License
- **Purpose:** Advanced text rendering
- **Documentation:** https://docs.unity3d.com/Manual/com.unity.textmeshpro.html

### Newtonsoft Json 3.2.1
- **Publisher:** Unity Technologies / Newtonsoft
- **License:** MIT License
- **Purpose:** JSON serialization/deserialization
- **Documentation:** https://www.newtonsoft.com/json

## External Libraries

### ZXing.Net (Required for QR Scanning)

**Important:** ZXing.Net is required but not included in this repository due to licensing.

#### Installation Options:

**Option 1: NuGet Package (Recommended)**
```bash
# In Unity, use NuGetForUnity package
# Install via Package Manager:
# Add package from git URL: https://github.com/GlitchEnzo/NuGetForUnity.git?path=/src/NuGetForUnity

# Then install ZXing.Net from NuGet:
# Window > NuGet > Search "ZXing.Net" > Install
```

**Option 2: Manual DLL Import**
```bash
# Download ZXing.Net DLL from:
# https://www.nuget.org/packages/ZXing.Net/

# Place DLLs in:
Assets/Plugins/ZXing/
├── zxing.unity.dll
└── zxing.dll
```

**Option 3: Unity Asset Store**
- Search for "ZXing" or "QR Code Scanner" in Unity Asset Store
- Some free/paid assets include ZXing pre-configured

#### ZXing.Net Details:
- **Version:** 0.16.6 or higher
- **License:** Apache License 2.0
- **Repository:** https://github.com/micjahn/ZXing.Net
- **Purpose:** QR code and barcode decoding

#### Alternative: Native QR Scanning (No ZXing)

If you don't want to use ZXing, you can use native device APIs:

**Android (Java/Kotlin):**
```java
// Use Google ML Kit or ZXing Android
implementation 'com.google.mlkit:barcode-scanning:17.0.2'
```

**iOS (Swift/Objective-C):**
```swift
// Use AVFoundation's built-in QR detection
import AVFoundation
```

Then create Unity plugins to call these native methods.

## Python Dependencies (for QR Code Generation)

### qrcode
- **Version:** 7.3 or higher
- **License:** BSD License
- **Install:** `pip install qrcode[pil]`
- **Purpose:** Generate QR code images

### Pillow (PIL)
- **Version:** 9.0.0 or higher
- **License:** HPND License
- **Install:** Included with `qrcode[pil]`
- **Purpose:** Image processing for QR codes

## Installing All Dependencies

### For Unity Project:

```bash
# 1. Open Unity project
# 2. Unity will auto-install packages from manifest.json
# 3. Manually add ZXing (see above)
```

### For QR Code Generation:

```bash
# Install Python dependencies
pip install qrcode[pil]

# Or using requirements.txt
pip install -r requirements.txt
```

## License Compliance

### This Project
- **License:** MIT License
- **Commercial Use:** Allowed
- **Attribution:** Required

### Unity Packages
- All Unity packages are under Unity Companion License
- Free to use with Unity Editor
- No additional licensing required for deployment

### ZXing.Net
- Apache License 2.0
- Open source and free for commercial use
- Attribution required (included in NOTICES file)

### Python Libraries
- All Python libraries are open source
- No licensing issues for code generation tools
- Not distributed with the Unity app

## Verification Checklist

Before building, verify all dependencies:

- [ ] AR Foundation packages installed
- [ ] ZXing.Net library available
- [ ] TextMeshPro package imported
- [ ] No compilation errors in Unity
- [ ] Python environment set up (for QR generation)

## Support and Issues

If you encounter dependency issues:

1. **Unity Packages:** Use Package Manager to reinstall
2. **ZXing:** Check ZXing.Net GitHub issues page
3. **Python:** Use virtual environment for clean install
4. **Build Errors:** Check Unity Console for specific errors

## Updates

Keep dependencies up to date:

```bash
# Unity packages
# Window > Package Manager > Check for updates

# Python packages
pip install --upgrade qrcode pillow
```

## Security Notes

- All packages are from trusted sources
- Keep packages updated for security patches
- Review package changelogs before major updates
- Test thoroughly after dependency updates

---

**Last Updated:** December 2024  
**Maintainer:** Development Team
