# Contributing to Smart Retail AR

Thank you for your interest in contributing to Smart Retail AR! This document provides guidelines for contributing to the project.

## Table of Contents

1. [Code of Conduct](#code-of-conduct)
2. [Getting Started](#getting-started)
3. [Development Workflow](#development-workflow)
4. [Coding Standards](#coding-standards)
5. [Commit Guidelines](#commit-guidelines)
6. [Pull Request Process](#pull-request-process)
7. [Testing Guidelines](#testing-guidelines)

## Code of Conduct

### Our Pledge

We are committed to providing a welcoming and inspiring community for all. Please be respectful and constructive in your interactions.

### Our Standards

- Use welcoming and inclusive language
- Be respectful of differing viewpoints
- Accept constructive criticism gracefully
- Focus on what is best for the community
- Show empathy towards other community members

## Getting Started

### Prerequisites

- Unity 2022.3 LTS or newer
- Git
- Visual Studio or VS Code
- Basic knowledge of C# and Unity

### Setup

1. Fork the repository
2. Clone your fork: `git clone https://github.com/YOUR_USERNAME/smart-retail-ar.git`
3. Add upstream remote: `git remote add upstream https://github.com/najialaajimi/smart-retail-ar.git`
4. Create a branch: `git checkout -b feature/your-feature-name`

## Development Workflow

### Branch Naming

- `feature/` - New features
- `bugfix/` - Bug fixes
- `hotfix/` - Urgent fixes
- `refactor/` - Code refactoring
- `docs/` - Documentation updates
- `test/` - Test additions/modifications

Example: `feature/add-barcode-scanner`

### Making Changes

1. **Create a branch** from `main`
2. **Make your changes** following coding standards
3. **Test thoroughly** using the testing guide
4. **Document** your changes in code comments
5. **Update** relevant documentation files
6. **Commit** with clear messages

## Coding Standards

### C# Style Guide

#### Naming Conventions

```csharp
// Classes: PascalCase
public class ProductDatabase { }

// Methods: PascalCase
public void LoadDatabase() { }

// Variables: camelCase
private string productId;

// Constants: UPPER_SNAKE_CASE or PascalCase
private const int MAX_PRODUCTS = 100;

// Properties: PascalCase
public string ProductName { get; set; }

// Events: PascalCase with "On" prefix
public event Action OnQRCodeScanned;

// Private fields: camelCase with underscore prefix (optional)
private ProductData _currentProduct;
```

#### Code Organization

```csharp
// Order of elements in a class:
// 1. Constants
// 2. Static fields
// 3. Fields
// 4. Properties
// 5. Events
// 6. Unity messages (Awake, Start, Update, etc.)
// 7. Public methods
// 8. Private methods
```

#### Comments

```csharp
// Use XML documentation for public members
/// <summary>
/// Gets a product by its unique identifier.
/// </summary>
/// <param name="productId">The product ID to search for</param>
/// <returns>The product data or null if not found</returns>
public ProductData GetProductById(string productId)
{
    // Use inline comments sparingly, only when necessary
    // Code should be self-documenting when possible
}
```

#### Best Practices

- **Single Responsibility**: Each class should have one clear purpose
- **DRY**: Don't Repeat Yourself - extract common code
- **KISS**: Keep It Simple, Stupid - avoid over-engineering
- **Early Returns**: Use early returns to reduce nesting
- **Null Checks**: Always check for null references
- **Error Handling**: Use try-catch for operations that might fail

### Unity-Specific Guidelines

#### Inspector Fields

```csharp
[Header("UI References")]
public Button scanButton;
public TextMeshProUGUI titleText;

[Header("Settings")]
[Range(0, 100)]
public float maxSpeed = 50f;

[Tooltip("Time to wait before scan")]
public float scanDelay = 1.5f;
```

#### Singleton Pattern

```csharp
public class MyManager : MonoBehaviour
{
    private static MyManager instance;
    public static MyManager Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject go = new GameObject("MyManager");
                instance = go.AddComponent<MyManager>();
                DontDestroyOnLoad(go);
            }
            return instance;
        }
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }
}
```

#### Event Cleanup

```csharp
void Start()
{
    QRCodeManager.Instance.OnQRCodeScanned += HandleScan;
}

void OnDestroy()
{
    // Always unsubscribe from events
    if (QRCodeManager.Instance != null)
    {
        QRCodeManager.Instance.OnQRCodeScanned -= HandleScan;
    }
}
```

### File Organization

```
Assets/Scripts/
├── UI/              # UI controllers
├── Data/            # Data models
├── Utils/           # Utility scripts
├── Managers/        # Game managers
└── Extensions/      # Extension methods
```

## Commit Guidelines

### Commit Message Format

```
<type>(<scope>): <subject>

<body>

<footer>
```

#### Types

- `feat`: New feature
- `fix`: Bug fix
- `docs`: Documentation only
- `style`: Code style changes (formatting, etc.)
- `refactor`: Code refactoring
- `test`: Adding or updating tests
- `chore`: Maintenance tasks

#### Examples

```bash
feat(scanner): add torch control functionality

Added ability to toggle flashlight on/off during scanning.
Includes UI button and QRCodeManager integration.

Closes #123
```

```bash
fix(database): handle missing products gracefully

Added null checks when loading products from database.
Prevents crash when product ID doesn't exist.
```

```bash
docs(readme): update installation instructions

Clarified Unity version requirements and added
troubleshooting section for common setup issues.
```

### Commit Best Practices

- Write clear, concise commit messages
- Keep commits focused and atomic
- Test before committing
- Don't commit broken code
- Don't commit large binary files

## Pull Request Process

### Before Submitting

1. ✅ Test your changes thoroughly
2. ✅ Update documentation if needed
3. ✅ Run any existing tests
4. ✅ Ensure code follows style guidelines
5. ✅ Rebase on latest `main` if needed

### PR Description Template

```markdown
## Description
Brief description of what this PR does.

## Type of Change
- [ ] Bug fix
- [ ] New feature
- [ ] Breaking change
- [ ] Documentation update

## Testing
Describe how you tested these changes.

## Screenshots (if applicable)
Add screenshots for UI changes.

## Checklist
- [ ] Code follows style guidelines
- [ ] Documentation updated
- [ ] Tests pass
- [ ] No warnings in console
- [ ] Tested on target platform
```

### Review Process

1. Submit PR with clear description
2. Wait for maintainer review
3. Address feedback promptly
4. Make requested changes
5. PR will be merged when approved

## Testing Guidelines

### Before Committing

1. Test in Unity Editor Play mode
2. Check for console errors/warnings
3. Test all affected features
4. Test on target platform if possible
5. Verify documentation accuracy

### Test Checklist

- [ ] Feature works as expected
- [ ] No console errors
- [ ] No performance degradation
- [ ] UI is responsive
- [ ] Edge cases handled
- [ ] Error messages are clear

### Performance Testing

- Monitor FPS in Profiler
- Check memory allocation
- Test on mobile device
- Verify load times

## Areas for Contribution

### High Priority

- [ ] Unity UI implementation for all scenes
- [ ] AR Foundation integration
- [ ] Real QR code scanning
- [ ] 3D product models
- [ ] Image loading system

### Medium Priority

- [ ] Sound effects system
- [ ] Haptic feedback
- [ ] Dark mode theme
- [ ] Language localization
- [ ] Analytics integration

### Nice to Have

- [ ] Tutorial overlays
- [ ] Animated transitions
- [ ] Product comparison tool
- [ ] Social sharing
- [ ] Favorites/bookmarks

## Getting Help

- Check existing documentation
- Review closed issues
- Search pull requests
- Ask in discussions
- Contact maintainers

## Recognition

Contributors will be recognized in:
- CHANGELOG.md
- Project README.md
- Release notes

---

Thank you for contributing to Smart Retail AR! 🚀
