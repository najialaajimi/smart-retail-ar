# Sprint 4 : Tests Utilisateurs et Validation en Conditions Réelles

## Vue d'ensemble
Sprint 4 se concentre sur les tests utilisateurs, l'analytics, le monitoring de performance, et l'optimisation basée sur les retours. Ce sprint valide l'application en conditions réelles et la prépare pour le déploiement production.

## Objectifs du Sprint
- ✅ Conduire des tests utilisateurs complets
- ✅ Implémenter analytics et monitoring
- ✅ Optimiser selon les retours utilisateurs
- ✅ Valider les KPIs en conditions réelles
- ✅ Préparer le déploiement production

## Architecture Testing

### Structure des Scripts
```
Assets/Scripts/Testing/
├── UserTestManager.cs         - Gestion tests utilisateur (future)
├── AnalyticsManager.cs        - Analytics et métriques
├── PerformanceMonitor.cs      - Monitoring performance
├── FeedbackCollector.cs       - Collecte feedback (future)
├── ABTestManager.cs           - A/B testing (future)
└── UsabilityTracker.cs        - Tracking usabilité (future)
```

## Système d'Analytics

### 1. AnalyticsManager

**Responsabilités:**
- Tracking des événements utilisateur
- Collecte de métriques d'usage
- Analyse des interactions
- Sauvegarde des données analytics

**Configuration:**
```csharp
[Header("Analytics Settings")]
[SerializeField] private bool enableAnalytics = true;
[SerializeField] private bool logToConsole = true;
```

#### Événements Trackés

**A. Événements Produits**
```csharp
TrackProductScan(string productId, float detectionTime)
```
- ID du produit scanné
- Temps de détection AR
- Timestamp

**B. Événements Recommandations**
```csharp
TrackRecommendation(string productId, string recommendedProductId, bool accepted)
```
- Produit source
- Produit recommandé
- Acceptation ou rejet
- Taux d'acceptation global

**C. Événements Navigation**
```csharp
TrackScreenView(string screenName)
```
- Nom de l'écran
- Temps passé
- Flux de navigation

**D. Événements Performance**
```csharp
TrackARPerformance(float fps, float latency)
```
- FPS en temps réel
- Latence de frame
- Moyennes mobiles

**E. Événements Erreurs**
```csharp
TrackError(string errorType, string errorMessage)
```
- Type d'erreur
- Message détaillé
- Contexte

**F. Timings**
```csharp
TrackTiming(string category, string variable, float timeMs)
```
- Catégorie d'action
- Variable mesurée
- Temps en millisecondes

#### Données Analytics

**Structure AnalyticsData:**
```csharp
[Serializable]
public class AnalyticsData
{
    public int totalEvents;
    public int totalScans;
    public int totalRecommendations;
    public int acceptedRecommendations;
    public float averageScanTime;
    public float averageFPS;
    public int totalErrors;
    public List<AnalyticsEvent> eventLog;
}
```

**Structure AnalyticsEvent:**
```csharp
[Serializable]
public class AnalyticsEvent
{
    public string eventName;
    public long timestamp;
    public Dictionary<string, object> parameters;
}
```

#### Métriques Calculées

**Session:**
- Durée de session
- Nombre d'événements
- Événements par minute

**Scans:**
- Total de scans
- Temps moyen de scan
- Succès de détection (%)

**Recommandations:**
- Total de recommandations affichées
- Recommandations acceptées
- Taux d'acceptation (%)

**Performance:**
- FPS moyen
- Latence moyenne
- Taux d'erreurs

#### Analytics Summary

```csharp
public AnalyticsSummary GetAnalyticsSummary()
{
    return new AnalyticsSummary
    {
        sessionDuration = GetSessionDuration(),
        totalEvents = analyticsData.totalEvents,
        totalScans = analyticsData.totalScans,
        recommendationAcceptanceRate = CalculateAcceptanceRate(),
        averageScanTime = analyticsData.averageScanTime,
        averageFPS = analyticsData.averageFPS,
        totalErrors = analyticsData.totalErrors
    };
}
```

### 2. PerformanceMonitor

**Responsabilités:**
- Monitoring FPS en temps réel
- Suivi de l'utilisation mémoire
- Détection des problèmes de performance
- Alertes sur dépassements de seuils

**Configuration:**
```csharp
[Header("Monitoring Settings")]
[SerializeField] private bool enableMonitoring = true;
[SerializeField] private float updateInterval = 1.0f;
[SerializeField] private bool showDebugOverlay = false;

[Header("Performance Thresholds")]
[SerializeField] private float targetFPS = 30f;
[SerializeField] private float maxMemoryMB = 1024f;
[SerializeField] private float maxLatencyMs = 1000f;
```

#### Métriques de Performance

**FPS:**
- FPS actuel
- FPS moyen
- FPS minimum
- FPS maximum
- Historique FPS (60 dernières mesures)

**Mémoire:**
- Utilisation actuelle (MB)
- Pic d'utilisation (MB)
- Garbage Collection stats

**Latence:**
- Frame time (ms)
- Latence moyenne
- Pic de latence

**Code de monitoring:**
```csharp
private void Update()
{
    if (!enableMonitoring) return;
    
    deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
    frameCount++;
    updateTimer += Time.unscaledDeltaTime;
    
    if (updateTimer >= updateInterval)
    {
        UpdateMetrics();
        updateTimer = 0f;
    }
}
```

#### Seuils de Performance

**Alertes automatiques:**
```csharp
private void CheckPerformanceThresholds()
{
    if (currentFPS < targetFPS)
        Debug.LogWarning($"FPS below target: {currentFPS:F1}");
    
    if (currentMemoryMB > maxMemoryMB)
        Debug.LogWarning($"Memory above threshold: {currentMemoryMB}MB");
    
    if (latencyMs > maxLatencyMs)
        Debug.LogWarning($"Latency exceeds threshold: {latencyMs:F1}ms");
}
```

#### Debug Overlay

**Affichage en temps réel:**
```csharp
private void OnGUI()
{
    if (!showDebugOverlay) return;
    
    string text = $"FPS: {currentFPS:F1} ({frameTimeMs:F1}ms)\n" +
                  $"Memory: {currentMemoryMB}MB";
    
    GUI.Label(rect, text, style);
}
```

#### Performance Summary

```csharp
public class PerformanceSummary
{
    public float currentFPS;
    public float averageFPS;
    public float minFPS;
    public float maxFPS;
    public long currentMemoryMB;
    public long peakMemoryMB;
    public float frameTimeMs;
    public bool meetsTargetFPS;
    public bool memoryWithinLimit;
}
```

## Plan de Tests Utilisateurs

### Phase 1: Tests Internes (Semaine 1)

**Objectifs:**
- Validation technique complète
- Correction bugs critiques
- Optimisation performances
- Tests de tous les features

**Participants:**
- Équipe de développement
- QA interne
- 5-10 testeurs internes

**Méthodes:**
- Tests de régression complets
- Tests de performance
- Tests de compatibilité
- Tests de sécurité

**Checklist:**
- [ ] Tous les écrans fonctionnent
- [ ] Navigation fluide
- [ ] Détection AR stable (≥95%)
- [ ] Recommandations pertinentes (≥85%)
- [ ] Performance cible atteinte (≥30 FPS)
- [ ] Pas de crash ni erreur bloquante

### Phase 2: Tests Contrôlés (Semaine 2)

**Objectifs:**
- Tests utilisateurs en environnement contrôlé
- Collecte de feedback qualitatif
- Validation de l'UX
- Identification problèmes usabilité

**Participants:**
- 20 testeurs externes
- Profils variés (âge, technicité)
- Nouveaux utilisateurs

**Environnement:**
- Salle de test avec produits
- Images de référence préparées
- Observation directe
- Enregistrement sessions

**Scénarios de test:**

1. **Première utilisation:**
   - Découverte de l'application
   - Premier scan produit
   - Navigation dans les informations
   - Consultation des recommandations

2. **Usage quotidien:**
   - Scan de plusieurs produits
   - Comparaison de produits
   - Personnalisation du profil
   - Consultation de l'historique

3. **Cas d'usage avancés:**
   - Scan avec occlusions
   - Scan avec mauvais éclairage
   - Scan de plusieurs produits
   - Utilisation prolongée (batterie)

**Métriques collectées:**
- Taux de succès par tâche
- Temps de complétion
- Nombre d'erreurs
- Chemins de navigation
- Satisfaction (échelle 1-5)

### Phase 3: Tests en Conditions Réelles (Semaines 3-4)

**Objectifs:**
- Validation en magasin réel
- Tests à grande échelle
- Validation des KPIs finaux
- Optimisations finales

**Participants:**
- 100+ utilisateurs réels
- Tests dans 3-5 magasins partenaires
- Durée: 2 semaines

**Méthodes:**
- Déploiement beta app
- Analytics automatiques
- Questionnaires post-utilisation
- Interviews sélectionnées

**Métriques KPIs:**

| KPI | Cible | Mesure |
|-----|-------|--------|
| Taux de reconnaissance | ≥ 95% | % scans réussis |
| Latence affichage | ≤ 1s | Temps moyen détection |
| Performance AR | ≥ 30 FPS | FPS moyen |
| Satisfaction utilisateur | ≥ 80% | Score satisfaction |
| Taux de rétention | ≥ 70% | % retour semaine suivante |
| Précision recommandations | ≥ 85% | % recommandations pertinentes |
| Adoption recommandations | ≥ 60% | % clics sur recommandations |
| Temps d'utilisation moyen | ≥ 3 min | Durée session moyenne |
| Produits scannés/session | ≥ 5 | Nombre moyen scans |

## Collecte de Feedback

### Questionnaires

**Post-première utilisation:**
```
1. L'application était-elle facile à utiliser? (1-5)
2. La détection AR a-t-elle bien fonctionné? (1-5)
3. Les informations affichées étaient-elles utiles? (1-5)
4. Les recommandations étaient-elles pertinentes? (1-5)
5. Utiliseriez-vous cette application en magasin? (Oui/Non)
6. Commentaires libres:
```

**Post-session prolongée:**
```
1. Performance globale de l'application (1-5)
2. Utilité des recommandations (1-5)
3. Facilité de comparaison des produits (1-5)
4. Impact sur vos décisions d'achat (1-5)
5. Recommanderiez-vous cette app? (NPS 0-10)
6. Fonctionnalités souhaitées:
```

### Système de Rating

**In-app ratings:**
```csharp
public class RatingSystem : MonoBehaviour
{
    public void RateRecommendation(string recommendationId, int rating)
    {
        // Rating 1-5 stars
        // Stockage et analytics
        AnalyticsManager.Instance.TrackEvent("recommendation_rated", 
            new Dictionary<string, object> {
                { "recommendation_id", recommendationId },
                { "rating", rating }
            });
    }
}
```

### Feedback Collector

**Collecte automatique:**
- Crashes et erreurs
- Temps de réponse anormaux
- Échecs de détection AR
- Parcours utilisateur complets

## A/B Testing

### Tests Planifiés

**Test 1: Layout d'informations**
- Variante A: Layout vertical
- Variante B: Layout horizontal
- Métrique: Temps de lecture, satisfaction

**Test 2: Algorithme de recommandation**
- Variante A: Similarité contenu
- Variante B: Historique utilisateur
- Métrique: Taux d'acceptation

**Test 3: Scoring weights**
- Variante A: Nutrition prioritaire (40-30-15-15)
- Variante B: Équilibré (25-25-25-25)
- Métrique: Satisfaction, pertinence

### Framework A/B Test (Future)

```csharp
public class ABTestManager : MonoBehaviour
{
    public enum Variant { A, B }
    
    public Variant GetVariant(string testName)
    {
        // Hash user ID for consistent assignment
        // 50/50 split
        return Random.value < 0.5f ? Variant.A : Variant.B;
    }
    
    public void TrackConversion(string testName, Variant variant)
    {
        AnalyticsManager.Instance.TrackEvent("ab_conversion",
            new Dictionary<string, object> {
                { "test_name", testName },
                { "variant", variant.ToString() }
            });
    }
}
```

## Optimisations Post-Tests

### Optimisations Identifiées

**Performance AR:**
- Réduction nombre images trackées simultanément
- Optimisation des overlays AR
- Amélioration du cooldown de détection
- Cache des textures

**UI/UX:**
- Simplification de la navigation
- Amélioration de la visibilité des boutons
- Réduction du nombre de clics
- Animations plus rapides

**Recommandations:**
- Ajustement des poids de scoring
- Amélioration de la pertinence
- Diversification des suggestions
- Personnalisation plus poussée

**Batterie:**
- Réduction de la fréquence de tracking
- Optimisation des rendus
- Mise en pause intelligente
- Throttling adaptatif

## Rapports de Test

### Rapport de Performance

```
=== Performance Report ===
Date: 2024-XX-XX
Duration: 2 weeks
Users: 100+
Scans: 5000+

Metrics:
- Avg FPS: 32.5 (Target: ≥30) ✓
- Avg Detection Time: 0.85s (Target: ≤1s) ✓
- Success Rate: 96.2% (Target: ≥95%) ✓
- Avg Memory: 420MB (Target: ≤512MB) ✓
- Crash Rate: 0.1% (Target: ≤1%) ✓

Issues:
- Low light detection: 88% success (needs improvement)
- Battery drain: 15%/hour (acceptable)
```

### Rapport d'Utilisabilité

```
=== Usability Report ===

Satisfaction Scores (1-5):
- Overall: 4.2/5 ✓
- AR Detection: 4.1/5 ✓
- Information Display: 4.4/5 ✓
- Recommendations: 4.0/5 ✓
- Navigation: 4.3/5 ✓

Task Success Rates:
- First scan: 92% ✓
- Product comparison: 88%
- Profile setup: 95% ✓
- Navigation: 94% ✓

Common Issues:
1. Difficulty scanning in low light (12% users)
2. Confusion about recommendation scores (8% users)
3. Profile setup too long (5% users)

Suggestions:
- Add tutorial for first use
- Improve score explanations
- Simplify profile setup
```

### Rapport de Recommandations

```
=== Recommendation System Report ===

Metrics:
- Recommendations shown: 3500
- Accepted: 2100 (60%) ✓
- Rated 4-5 stars: 85% ✓
- Diverse categories: 78% ✓

Performance by type:
- Similar products: 65% acceptance
- Better alternatives: 72% acceptance
- Trending: 55% acceptance

User feedback:
- "Very relevant": 62%
- "Somewhat relevant": 23%
- "Not relevant": 15%

Improvements needed:
- Increase diversity in similar products
- Better explanation of "better" alternatives
- More context-aware suggestions
```

## Préparation au Déploiement

### Checklist Pré-Production

**Technique:**
- [ ] Tous tests passent
- [ ] Performance validée
- [ ] Sécurité vérifiée
- [ ] Analytics configuré
- [ ] Crash reporting activé
- [ ] Logging production

**Contenu:**
- [ ] Base de données produits complète
- [ ] Images de référence optimisées
- [ ] Traductions (si applicable)
- [ ] CGU et politique de confidentialité

**Distribution:**
- [ ] Build Android signé
- [ ] Build iOS signé
- [ ] Store listings préparés
- [ ] Screenshots et vidéos
- [ ] Description store

**Support:**
- [ ] Documentation utilisateur
- [ ] FAQ
- [ ] Support contact
- [ ] Monitoring alerts

### Configuration Production

**Analytics:**
- Désactiver logs console
- Activer tracking production
- Configurer Firebase/autre service
- Set up alerting

**Performance:**
- Mode release build
- Obfuscation code
- Optimisation assets
- Compression textures

## KPIs de Production

### Métriques à Suivre

**Engagement:**
- DAU (Daily Active Users)
- MAU (Monthly Active Users)
- Session duration
- Sessions per user
- Retention rates (D1, D7, D30)

**Performance:**
- Crash rate (Target: <1%)
- ANR rate (Target: <0.5%)
- Average FPS
- API latency
- Battery drain

**Business:**
- Scans per session
- Recommendation acceptance rate
- User satisfaction (NPS)
- Store ratings
- Revenue (if applicable)

### Dashboard Monitoring

**Real-time metrics:**
- Active users
- Scans per minute
- Error rate
- Performance stats

**Daily reports:**
- Usage summary
- Top scanned products
- Recommendation performance
- Error logs

## Prochaines Itérations

### Features Futures (Post-Launch)

**V1.1:**
- Mode hors ligne
- Comparaison multi-produits
- Listes de courses
- Partage social

**V1.2:**
- Machine learning avancé
- Reconnaissance vocale
- Support multi-langues
- Integration retailers

**V2.0:**
- Gamification
- Programme de fidélité
- AR avancé (3D models)
- Recettes suggérées

## Références

### Testing & QA
- [User Testing Best Practices](https://www.nngroup.com/articles/usability-testing-101/)
- [Mobile App Analytics](https://firebase.google.com/docs/analytics)
- [A/B Testing Guide](https://www.optimizely.com/optimization-glossary/ab-testing/)

### Monitoring
- [Unity Analytics](https://docs.unity3d.com/Manual/UnityAnalytics.html)
- [Performance Profiling](https://docs.unity3d.com/Manual/Profiler.html)
- [Firebase Crashlytics](https://firebase.google.com/docs/crashlytics)
