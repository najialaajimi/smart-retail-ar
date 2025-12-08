# Sprint 3 : Moteur de Recommandations Avancé

## Vue d'ensemble
Sprint 3 implémente un système de recommandation intelligent avec scoring multi-critères, personnalisation utilisateur, et algorithmes de similarité produits. Ce sprint transforme l'application en un assistant d'achat personnalisé.

## Objectifs du Sprint
- ✅ Développer un algorithme de recommandation intelligent
- ✅ Créer un système de scoring multicritères
- ✅ Implémenter la personnalisation utilisateur
- ✅ Ajouter l'analyse comparative des produits
- ✅ Intégrer machine learning de base

## Architecture Recommandations

### Structure des Scripts
```
Assets/Scripts/Recommendations/
├── RecommendationEngine.cs     - Moteur principal
├── ScoringSystem.cs           - Système de scoring
├── UserProfileManager.cs      - Gestion profil (intégré dans UserPreferences)
├── ProductComparator.cs       - Comparaison produits (future)
├── MLRecommendations.cs       - Machine Learning (future)
└── PersonalizationManager.cs  - Personnalisation (future)
```

## Système de Recommandation

### 1. RecommendationEngine

**Responsabilités:**
- Génération de recommandations personnalisées
- Calcul de similarité entre produits
- Identification d'alternatives meilleures
- Gestion des produits tendance

**Algorithmes implémentés:**

#### A. Recommandations Personnalisées
```csharp
public List<ProductData> GetRecommendations(ProductData currentProduct, int count = 5)
```

**Critères de scoring:**
- **Similarité de catégorie (30%):** Même catégorie = score élevé
- **Préférences utilisateur (30%):** Score global du produit
- **Similarité produit (20%):** Similarité multi-critères
- **Historique utilisateur (20%):** Fréquence de scan

#### B. Produits Similaires
```csharp
public List<ProductData> GetSimilarProducts(ProductData product, int count = 3)
```

**Critères de similarité:**
- Catégorie identique
- Marque identique (bonus 0.5)
- Prix similaire (±20%)
- Nutri-Score identique
- Éco-Score identique
- Statut biologique identique

**Seuil de similarité:** 0.3 (configurable)

#### C. Alternatives Meilleures
```csharp
public List<ProductData> GetAlternatives(ProductData product, int count = 3)
```

**Logique:**
1. Filtrer par même catégorie
2. Appliquer préférences utilisateur
3. Calculer score global pour chaque produit
4. Retourner seulement les produits avec score > produit actuel

### 2. ScoringSystem

**Responsabilités:**
- Évaluation multi-critères des produits
- Calcul de scores normalisés (0-1)
- Pondération personnalisable
- Analyse détaillée par dimension

#### Dimensions de Scoring

##### A. Score Nutritionnel (0-1)

**Composants:**
- **Nutri-Score (60%):** Conversion A=1.0, B=0.8, C=0.6, D=0.4, E=0.2
- **Protéines (10%):** Normalisé sur 20g
- **Fibres (10%):** Normalisé sur 10g
- **Sucre (10%):** 1 - (sucre/30g) - moins = mieux
- **Sodium (10%):** 1 - (sodium/1000mg) - moins = mieux

```csharp
public float CalculateNutritionalScore(ProductData product)
{
    float score = 0f;
    score += GetNutriScoreValue(nutriScore) * 0.6f;
    score += Clamp01(protein / 20f) * 0.1f;
    score += Clamp01(fiber / 10f) * 0.1f;
    score += Clamp01(1f - sugar / 30f) * 0.1f;
    score += Clamp01(1f - sodium / 1000f) * 0.1f;
    return score;
}
```

##### B. Score Écologique (0-1)

**Composants:**
- **Éco-Score (40%):** Conversion A-E similaire à Nutri-Score
- **Empreinte carbone (30%):** 1 - (carbonFootprint/10) - moins = mieux
- **Biologique (20%):** 0.2 si bio, 0 sinon
- **Emballage (10%):** 0.1 si recyclable

```csharp
public float CalculateEcologicalScore(ProductData product)
{
    float score = 0f;
    score += GetEcoScoreValue(ecoScore) * 0.4f;
    score += Clamp01(1f - carbonFootprint / 10f) * 0.3f;
    score += (organic ? 0.2f : 0f);
    score += (packaging.Contains("recyclable") ? 0.1f : 0f);
    return score;
}
```

##### C. Score Économique (0-1)

**Composants:**
- **Accessibilité prix (60%):** 1 - (price/maxBudget)
- **Rapport qualité/prix (40%):** (qualité * affordability)
  - Qualité = moyenne(nutritionScore, ecoScore)

```csharp
public float CalculateEconomicalScore(ProductData product, UserPreferences prefs)
{
    float priceScore = Clamp01(1f - price / maxBudget) * 0.6f;
    float qualityScore = (NutriScore + EcoScore) / 2f;
    float valueScore = qualityScore * (1f - Clamp01(price / 50f)) * 0.4f;
    return priceScore + valueScore;
}
```

##### D. Score Éthique (0-1)

**Composants:**
- **Ethical-Score (50%):** Conversion A-E
- **Commerce équitable (30%):** 0.3 si fair trade
- **Local (20%):** 0.2 si local

```csharp
public float CalculateEthicalScore(ProductData product)
{
    float score = GetEthicalScoreValue(ethicalScore) * 0.5f;
    score += (fairTrade ? 0.3f : 0f);
    score += (local ? 0.2f : 0f);
    return score;
}
```

#### Score Global

**Formule:**
```
OverallScore = (NutriScore × WN) + (EcoScore × WE) + (EconoScore × WEc) + (EthicalScore × WEt)
```

**Poids par défaut:**
- Nutritionnel: 30%
- Écologique: 30%
- Économique: 20%
- Éthique: 20%

**Poids personnalisables via UserPreferences.**

### 3. Personnalisation Utilisateur

#### Profil Utilisateur (UserPreferences)

**Préférences alimentaires:**
```csharp
public bool vegetarian;
public bool vegan;
public bool glutenFree;
public bool lactoseFree;
public bool organic;
```

**Filtres d'allergènes:**
```csharp
public List<string> allergenFilters;
```

**Historique:**
```csharp
public List<string> scanHistoryIds;
public List<long> scanHistoryTimestamps;
```

**Compteurs de scan:**
```csharp
// Utilise parallel lists pour sérialisation
public List<string> scanCountKeys;
public List<int> scanCountValues;
private Dictionary<string, int> _productScanCount; // Runtime
```

**Poids de scoring:**
```csharp
public float nutritionalWeight = 0.3f;
public float ecologicalWeight = 0.3f;
public float economicalWeight = 0.2f;
public float ethicalWeight = 0.2f;
public float maxBudget = 100f;
```

#### Apprentissage des Préférences

**Basé sur l'historique:**
```csharp
public List<string> GetFavoriteProducts(int count = 5)
{
    // Retourne produits les plus scannés
    // Ordonnés par fréquence décroissante
}
```

**Matching préférences:**
```csharp
public bool MatchesDietaryPreferences(ProductData product)
{
    // Vérifie allergènes
    // Vérifie préférence bio
    // Vérifie autres filtres alimentaires
}
```

## Algorithmes de Recommandation

### Algorithme 1: Similarité de Contenu

**Principe:** Recommander produits similaires au produit actuel

**Méthode:**
1. Extraire features du produit actuel
2. Calculer similarité avec tous les autres produits
3. Filtrer par seuil de similarité
4. Ordonner par score de similarité

**Features utilisées:**
- Catégorie (poids fort)
- Marque
- Fourchette de prix
- Nutri-Score
- Éco-Score
- Statut biologique

### Algorithme 2: Filtrage Collaboratif (Simplifié)

**Principe:** Recommander basé sur historique utilisateur

**Méthode:**
1. Identifier produits favoris de l'utilisateur
2. Trouver produits similaires aux favoris
3. Exclure produits déjà scannés
4. Scorer par pertinence

**Implémentation:**
```csharp
List<string> favorites = userPreferences.GetFavoriteProducts(5);
foreach (string favoriteId in favorites)
{
    ProductData favorite = ProductDatabase.Instance.GetProductById(favoriteId);
    List<ProductData> similar = GetSimilarProducts(favorite, 3);
    // Agréger et scorer
}
```

### Algorithme 3: Recommandation Contextuelle

**Principe:** Considérer contexte et préférences utilisateur

**Facteurs contextuels:**
- Catégorie du produit actuel
- Préférences alimentaires
- Budget utilisateur
- Historique de scan
- Tendances (produits populaires)

**Scoring contextuel:**
```csharp
float score = 
    (categorySimilarity * 0.3f) +
    (userPreferenceMatch * 0.3f) +
    (productSimilarity * 0.2f) +
    (userHistoryRelevance * 0.2f);
```

## Recherche et Filtrage Avancés

### Recherche Multi-Critères

```csharp
public List<ProductData> GetProductsByCriteria(
    string category = null,
    float? maxPrice = null,
    string nutriScore = null,
    bool? organic = null,
    int count = 10)
```

**Filtres supportés:**
- Catégorie
- Prix maximum
- Nutri-Score spécifique
- Statut biologique
- Préférences utilisateur (automatique)

**Pipeline:**
1. Charger tous les produits
2. Appliquer chaque filtre séquentiellement
3. Scorer les produits filtrés
4. Ordonner par score
5. Retourner top N

### Produits Tendance

```csharp
public List<ProductData> GetTrendingProducts(int count = 5)
```

**Logique:**
- Basé sur fréquence de scan de l'utilisateur
- Retourne les N produits les plus scannés
- Utile pour recommandations rapides

## Base de Données Étendue

### Nouvelles Informations Produits

**EthicalInfo ajouté:**
```json
{
  "ethicalInfo": {
    "fairTrade": true,
    "local": true,
    "ethicalScore": "A"
  }
}
```

**Informations complètes:**
- Données nutritionnelles détaillées (calories, macros, fibres, sucre, sodium)
- Informations écologiques (carbone, emballage, origine, bio)
- Informations éthiques (commerce équitable, local)
- Scores normalisés (Nutri-Score, Éco-Score, Ethical-Score)

## Intégration avec Sprints Précédents

### ProductInfoController

**Ajout recommandations:**
```csharp
private void LoadRecommendations()
{
    List<ProductData> alternatives = RecommendationEngine.Instance
        .GetSimilarProducts(currentProduct, 3);
    
    foreach (var alternative in alternatives)
    {
        // Créer ProductCard
        // Afficher dans alternativesContainer
    }
}
```

### Tracking Analytics

```csharp
AnalyticsManager.Instance.TrackRecommendation(
    productId,
    recommendedProductId,
    accepted: true/false
);
```

## Machine Learning (Base)

### Préparation pour ML

**Données d'entraînement:**
- Historique de scans
- Produits acceptés/rejetés
- Temps passé sur produits
- Comparaisons effectuées

**Features pour ML:**
- Vecteur de préférences utilisateur
- Vecteur de caractéristiques produit
- Score de similarité
- Contexte temporel

**Modèle simple (future implémentation):**
- Régression logistique pour prédiction d'acceptation
- Clustering pour segmentation utilisateurs
- Collaborative filtering pour recommandations

## Tests et Validation

### Tests Unitaires

**Test des scores:**
```csharp
[Test]
public void TestNutritionalScoring()
{
    ProductData product = CreateTestProduct();
    float score = scoringSystem.CalculateNutritionalScore(product);
    Assert.IsTrue(score >= 0f && score <= 1f);
}
```

**Test des recommandations:**
```csharp
[Test]
public void TestSimilarProducts()
{
    ProductData product = GetTestProduct("PROD001");
    List<ProductData> similar = engine.GetSimilarProducts(product, 3);
    Assert.IsTrue(similar.Count <= 3);
    Assert.IsFalse(similar.Contains(product));
}
```

### Tests de Performance

**Critères:**
- Temps de génération recommandations < 100ms
- Recherche dans base de données < 50ms
- Calcul de score < 1ms par produit

### Tests de Qualité

**Métriques:**
- **Précision:** Recommandations pertinentes / Total recommandations
- **Rappel:** Recommandations pertinentes / Produits pertinents totaux
- **Diversité:** Variance des catégories recommandées
- **Nouveauté:** % de produits jamais vus

**Cibles:**
- Précision ≥ 85%
- Rappel ≥ 70%
- Diversité ≥ 60%
- Satisfaction utilisateur ≥ 80%

## Optimisations

### Performance

1. **Cache des scores:**
```csharp
private Dictionary<string, float> scoreCache;
```

2. **Lazy loading:**
- Calculer scores uniquement quand nécessaire
- Réutiliser scores déjà calculés

3. **Batch processing:**
- Scorer plusieurs produits en une passe
- Réduire accès base de données

### Mémoire

- Limiter taille historique (last 100 scans)
- Nettoyer cache périodiquement
- Utiliser structures de données efficaces

## Prochaines Étapes (Sprint 4)

Le Sprint 4 ajoutera:
- Framework de tests utilisateurs
- Analytics en temps réel
- Optimisation basée sur feedback
- A/B testing des algorithmes

## Références

### Algorithmes de Recommandation
- Content-Based Filtering
- Collaborative Filtering
- Hybrid Recommender Systems
- Context-Aware Recommendations

### Scoring et Métriques
- Nutri-Score officiel
- Éco-Score méthodologie
- Multi-Criteria Decision Making (MCDM)
- Weighted Sum Model (WSM)
