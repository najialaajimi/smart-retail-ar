# Guide de Tests - Smart Retail AR

## Table des Matières

1. [Types de Tests](#types-de-tests)
2. [Configuration de l'Environnement](#configuration-de-lenvironnement)
3. [Tests Unitaires](#tests-unitaires)
4. [Tests d'Intégration](#tests-dintégration)
5. [Tests de Performance](#tests-de-performance)
6. [Tests AR](#tests-ar)
7. [Tests Utilisateurs](#tests-utilisateurs)
8. [Rapports de Tests](#rapports-de-tests)

---

## Types de Tests

### 1. Tests Automatisés (TestManager)
- Tests unitaires des composants
- Tests d'intégration
- Validation des KPIs
- Tests de performance

### 2. Tests Manuels
- Tests d'interface utilisateur
- Tests AR en conditions réelles
- Tests d'utilisabilité
- Tests de compatibilité

### 3. Tests de Performance
- FPS et frame time
- Consommation mémoire
- Utilisation batterie
- Latence de scan

---

## Configuration de l'Environnement

### Environnement de Test

**Matériel requis:**
- Device Android (API 24+) avec ARCore
- Device iOS (11.0+) avec ARKit
- QR codes imprimés (min 3x3cm)
- Éclairage contrôlé
- Surface plane pour AR

**Conditions d'éclairage:**
- Normal: 300-500 lux
- Faible: 100-200 lux
- Fort: 800-1000 lux

**Configuration Unity:**
```
Edit > Project Settings > Quality
- Quality Level: Medium (pour tests)
- V-Sync: Off (pour mesure FPS précise)
```

---

## Tests Unitaires

### Test 1: Chargement Base de Données

**Objectif:** Vérifier que la base de données se charge correctement

**Procédure:**
```csharp
[Test]
public void TestDatabaseLoading()
{
    // Arrange
    var db = ProductDatabaseManager.Instance;
    
    // Act
    System.Threading.Thread.Sleep(500); // Wait for loading
    
    // Assert
    Assert.IsTrue(db.IsLoaded, "Database should be loaded");
    Assert.IsNotNull(db.GetAllProducts(), "Products list should not be null");
    Assert.IsTrue(db.GetAllProducts().Count > 0, "Should have products");
}
```

**Critères de succès:**
- ✅ Base de données chargée en < 2 secondes
- ✅ Au moins 50 produits dans la base
- ✅ Aucune erreur de parsing JSON

### Test 2: Recherche Produit par ID

**Objectif:** Vérifier la recherche de produits

**Procédure:**
```csharp
[Test]
public void TestProductRetrieval()
{
    // Arrange
    var db = ProductDatabaseManager.Instance;
    string testId = "PROD001";
    
    // Act
    var product = db.GetProductById(testId);
    
    // Assert
    Assert.IsNotNull(product, "Product should be found");
    Assert.AreEqual(testId, product.id, "Product ID should match");
    Assert.IsNotEmpty(product.name, "Product should have a name");
}
```

**Critères de succès:**
- ✅ Produit trouvé en < 10ms
- ✅ Toutes les propriétés renseignées
- ✅ ID correspond exactement

### Test 3: Calcul de Recommandations

**Objectif:** Vérifier le moteur de recommandations

**Procédure:**
```csharp
[Test]
public void TestRecommendations()
{
    // Arrange
    var engine = RecommendationEngine.Instance;
    var product = ProductDatabaseManager.Instance.GetProductById("PROD001");
    
    // Act
    var recommendations = engine.GetRecommendations(product, 5);
    
    // Assert
    Assert.IsNotNull(recommendations, "Recommendations should not be null");
    Assert.IsTrue(recommendations.Count > 0, "Should have recommendations");
    Assert.IsTrue(recommendations.Count <= 5, "Should respect max count");
    
    // Verify different category
    foreach (var rec in recommendations)
    {
        Assert.AreEqual(product.category, rec.category, 
            "Recommendations should be same category");
    }
}
```

**Critères de succès:**
- ✅ Au moins 3 recommandations retournées
- ✅ Même catégorie que le produit original
- ✅ Score de pertinence > 0

---

## Tests d'Intégration

### Test 4: Flux Complet de Scan

**Objectif:** Tester le flux utilisateur complet

**Scénario:**
1. Lancer l'application
2. Naviguer vers Scanner
3. Scanner un QR code
4. Afficher détails produit
5. Voir recommandations
6. Retour au scanner

**Procédure manuelle:**

**Étape 1: Lancer l'app**
- Ouvrir l'application
- Vérifier écran d'accueil s'affiche
- ⏱️ Temps de chargement: < 3 secondes

**Étape 2: Scanner**
- Cliquer sur "Scanner"
- Activer caméra
- Pointer vers QR code PROD001
- ⏱️ Temps de détection: < 1 seconde

**Étape 3: Détails**
- Vérifier nom produit affiché
- Vérifier prix affiché
- Vérifier scores (éco, santé, qualité)
- Vérifier informations nutritionnelles

**Étape 4: Recommandations**
- Cliquer "Voir alternatives"
- Vérifier liste de recommandations
- Vérifier filtres fonctionnent
- Sélectionner une alternative

**Critères de succès:**
- ✅ Navigation fluide sans crash
- ✅ Données correctement affichées
- ✅ Transitions < 500ms
- ✅ Aucune erreur console

### Test 5: Persistance des Données

**Objectif:** Vérifier que les préférences sont sauvegardées

**Procédure:**
1. Modifier préférences (Bio: ON, Local: ON)
2. Quitter l'application
3. Relancer l'application
4. Vérifier préférences conservées

**Code de test:**
```csharp
[Test]
public void TestPreferencesPersistence()
{
    // Arrange
    var prefs = UserPreferences.Instance;
    
    // Act
    prefs.PreferBio = true;
    prefs.PreferLocal = true;
    prefs.MaxPrice = 25.0f;
    
    // Simulate restart
    PlayerPrefs.Save();
    
    // Create new instance
    var newPrefs = UserPreferences.Instance;
    
    // Assert
    Assert.IsTrue(newPrefs.PreferBio, "Bio preference should persist");
    Assert.IsTrue(newPrefs.PreferLocal, "Local preference should persist");
    Assert.AreEqual(25.0f, newPrefs.MaxPrice, "Max price should persist");
}
```

---

## Tests de Performance

### Test 6: KPIs

**Objectif:** Valider les KPIs cibles

**KPI 1: Taux de Reconnaissance ≥ 95%**

```csharp
[Test]
public void TestRecognitionRate()
{
    // Simulate 100 scans
    for (int i = 0; i < 100; i++)
    {
        AnalyticsManager.Instance.StartScanTimer();
        
        // Simulate scan
        var product = ProductDatabaseManager.Instance
            .GetProductById($"PROD{i % 50 + 1:D3}");
        
        bool success = product != null;
        AnalyticsManager.Instance.TrackProductScan(
            success ? product.id : "UNKNOWN", success);
    }
    
    float rate = AnalyticsManager.Instance.GetRecognitionRate();
    Assert.IsTrue(rate >= 95f, $"Recognition rate {rate}% should be ≥95%");
}
```

**KPI 2: Latence ≤ 1 seconde**

```csharp
[Test]
public void TestScanLatency()
{
    List<float> latencies = new List<float>();
    
    for (int i = 0; i < 20; i++)
    {
        float startTime = Time.realtimeSinceStartup;
        
        // Perform scan
        var product = ProductDatabaseManager.Instance
            .GetProductById($"PROD{i + 1:D3}");
        
        float latency = Time.realtimeSinceStartup - startTime;
        latencies.Add(latency);
    }
    
    float avgLatency = latencies.Average();
    Assert.IsTrue(avgLatency <= 1.0f, 
        $"Average latency {avgLatency:F2}s should be ≤1s");
}
```

**KPI 3: Satisfaction ≥ 80%**

Mesuré via feedback utilisateur (test manuel)

### Test 7: Performance FPS

**Objectif:** Maintenir 30 FPS minimum

**Procédure:**
```csharp
[Test]
public void TestFrameRate()
{
    // Run for 10 seconds
    float duration = 10f;
    float elapsed = 0f;
    List<float> fpsReadings = new List<float>();
    
    while (elapsed < duration)
    {
        float fps = PerformanceMonitor.Instance.GetFPS();
        if (fps > 0)
            fpsReadings.Add(fps);
        
        elapsed += Time.deltaTime;
        yield return null;
    }
    
    float avgFps = fpsReadings.Average();
    Assert.IsTrue(avgFps >= 30f, $"Average FPS {avgFps:F1} should be ≥30");
}
```

### Test 8: Mémoire

**Objectif:** Consommation mémoire < 500 MB

**Procédure:**
```csharp
[Test]
public void TestMemoryUsage()
{
    // Trigger heavy operations
    for (int i = 0; i < 50; i++)
    {
        var products = ProductDatabaseManager.Instance.GetAllProducts();
        var recommendations = RecommendationEngine.Instance
            .GetRecommendations(products[0], 10);
    }
    
    long memory = PerformanceMonitor.Instance.GetUsedMemory();
    long memoryMB = memory / (1024 * 1024);
    
    Assert.IsTrue(memoryMB < 500, 
        $"Memory usage {memoryMB}MB should be <500MB");
}
```

---

## Tests AR

### Test 9: Initialisation AR

**Objectif:** Session AR démarre correctement

**Procédure manuelle:**
1. Lancer ARScene sur device
2. Attendre initialisation AR
3. Vérifier caméra active
4. Vérifier détection plans

**Checklist:**
- [ ] Caméra s'active dans les 2 secondes
- [ ] Détection de plans fonctionne
- [ ] Overlay AR s'affiche
- [ ] Pas de crash ou freeze

### Test 10: Tracking AR

**Objectif:** Overlay suit correctement le produit

**Procédure:**
1. Scanner produit avec QR code
2. Passer en mode AR
3. Placer overlay sur surface
4. Déplacer device
5. Vérifier overlay reste ancré

**Critères:**
- ✅ Overlay stable (pas de tremblement)
- ✅ Billboard effect fonctionne
- ✅ Informations lisibles
- ✅ Mise à jour fluide (60 Hz)

### Test 11: Reconnaissance QR en Conditions Variées

**Objectif:** Scanner fonctionne dans différentes conditions

**Test Matrix:**

| Condition | Éclairage | Distance | Angle | Résultat Attendu |
|-----------|-----------|----------|-------|-------------------|
| Optimal | Normal | 20cm | Face | ✅ Détection < 0.5s |
| Sombre | Faible | 20cm | Face | ✅ Détection < 1.5s |
| Lumineux | Fort | 20cm | Face | ✅ Détection < 1s |
| Angle | Normal | 20cm | 45° | ✅ Détection < 2s |
| Distance | Normal | 50cm | Face | ✅ Détection < 2s |

**Procédure pour chaque test:**
1. Configurer conditions
2. Scanner QR code 10 fois
3. Noter temps de détection
4. Calculer moyenne et taux de succès

---

## Tests Utilisateurs

### Test 12: Test UX Complet

**Participants:** 5-10 utilisateurs cibles

**Scénario:**
1. Installer l'application
2. Premier lancement (tutorial?)
3. Scanner 3 produits différents
4. Consulter détails
5. Voir recommandations
6. Utiliser filtres
7. Expérience AR

**Questionnaire:**

**Facilité d'utilisation (1-5):**
- [ ] Installation claire
- [ ] Interface intuitive
- [ ] Navigation facile
- [ ] Scan simple
- [ ] Informations utiles

**Satisfaction (1-5):**
- [ ] Rapidité de l'app
- [ ] Qualité AR
- [ ] Pertinence recommandations
- [ ] Design global

**Questions ouvertes:**
- Que pensez-vous de l'app?
- Quelles améliorations suggérez-vous?
- Utiliseriez-vous cette app en magasin?

**Métriques:**
- Temps pour premier scan: < 1 minute
- Taux de réussite scan: > 90%
- Note moyenne satisfaction: ≥ 4/5 (80%)

---

## Rapports de Tests

### Format de Rapport

```markdown
# Rapport de Test

**Date:** 2024-12-08
**Testeur:** [Nom]
**Version:** 1.0.0
**Device:** [Model]
**OS:** [Android/iOS Version]

## Résumé

- Tests passés: X / Y
- Taux de réussite: Z%
- Bugs critiques: N
- Bugs mineurs: M

## Tests Détaillés

### Test 1: [Nom]
- **Status:** ✅ Pass / ❌ Fail
- **Durée:** X secondes
- **Notes:** [Observations]

### Test 2: [Nom]
...

## KPIs

| KPI | Cible | Résultat | Status |
|-----|-------|----------|--------|
| Recognition Rate | ≥95% | 96.5% | ✅ |
| Latency | ≤1s | 0.8s | ✅ |
| Satisfaction | ≥80% | 85% | ✅ |

## Bugs Identifiés

1. **[Critique]** Description
2. **[Moyen]** Description
3. **[Mineur]** Description

## Recommandations

- Amélioration 1
- Amélioration 2
- Amélioration 3
```

### Exécution des Tests Automatisés

Dans Unity Console ou code:

```csharp
// Exécuter tous les tests
TestManager.Instance.RunTest("all");

// Exporter rapport
AnalyticsManager.Instance.ExportAnalytics();

// Logs dans:
// Application.persistentDataPath/analytics.json
```

---

## Checklist Finale Avant Release

### Tests Fonctionnels
- [ ] Tous les tests unitaires passent
- [ ] Tous les tests d'intégration passent
- [ ] Flux utilisateur complet vérifié
- [ ] Pas de crash sur devices tests

### Tests Performance
- [ ] KPI reconnaissance ≥ 95%
- [ ] KPI latence ≤ 1s
- [ ] FPS ≥ 30
- [ ] Mémoire < 500MB

### Tests AR
- [ ] AR initialisation OK
- [ ] Tracking stable
- [ ] QR scan toutes conditions
- [ ] Overlay lisible

### Tests Utilisateur
- [ ] 5+ utilisateurs testés
- [ ] Satisfaction ≥ 80%
- [ ] Feedback positif
- [ ] Bugs critiques résolus

### Build & Déploiement
- [ ] Build Android réussie
- [ ] Build iOS réussie
- [ ] Tests sur devices réels
- [ ] Permissions configurées
- [ ] Store assets prêts

---

**Document maintenu par:** Équipe QA  
**Dernière mise à jour:** Décembre 2024
