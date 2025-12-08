# Smart Retail AR

Application de réalité augmentée pour améliorer l'expérience d'achat en magasin en superposant des informations détaillées sur les produits scannés.

![Unity Version](https://img.shields.io/badge/Unity-2022.3.62f3-blue)
![AR Foundation](https://img.shields.io/badge/AR%20Foundation-5.1.5-green)
![Platform](https://img.shields.io/badge/Platform-Android%20%7C%20iOS-lightgrey)

## 🎯 Vue d'ensemble

Smart Retail AR est une application mobile innovante qui utilise la réalité augmentée pour enrichir l'expérience d'achat. En pointant simplement leur smartphone vers un produit, les utilisateurs obtiennent instantanément des informations nutritionnelles, écologiques, et éthiques, ainsi que des recommandations personnalisées d'alternatives meilleures.

### Fonctionnalités Principales

- **🔍 Reconnaissance d'image AR** - Détection temps réel des produits via AR Foundation
- **📊 Informations détaillées** - Nutri-Score, Éco-Score, origine, allergènes
- **🎯 Recommandations intelligentes** - Algorithme multi-critères personnalisé
- **👤 Personnalisation** - Profil utilisateur avec préférences alimentaires
- **📈 Analytics** - Suivi des interactions et optimisation continue
- **⚡ Performance** - 30+ FPS, détection <1s, optimisé pour mobile

## 📱 Captures d'écran

*(Les captures d'écran seraient insérées ici)*

## 🏗️ Architecture

```
smart-retail-ar/
├── Assets/
│   ├── Scripts/
│   │   ├── UI/                    # Sprint 1: Interfaces utilisateur
│   │   ├── AR/                    # Sprint 2: Composants AR
│   │   ├── Recommendations/       # Sprint 3: Moteur de recommandations
│   │   ├── Testing/              # Sprint 4: Analytics et tests
│   │   ├── Data/                 # Gestion des données
│   │   └── Utils/                # Utilitaires
│   ├── Scenes/
│   │   ├── Frontend/             # Scènes UI
│   │   ├── AR/                   # Scènes AR
│   │   └── Testing/              # Scènes de test
│   ├── Resources/
│   │   └── Data/                 # Base de données produits
│   ├── AR/
│   │   ├── ReferenceImages/      # Images de référence
│   │   └── ARPrefabs/            # Prefabs AR
│   └── Prefabs/                  # Prefabs UI
├── Documentation/
│   ├── sprint1details.md         # Détails Sprint 1
│   ├── sprint2details.md         # Détails Sprint 2
│   ├── sprint3details.md         # Détails Sprint 3
│   ├── sprint4details.md         # Détails Sprint 4
│   ├── DEVELOPMENT.md            # Guide développeur
│   ├── DEPLOYMENT.md             # Guide déploiement
│   └── API.md                    # Documentation API
└── Tests/
    ├── Unit/                     # Tests unitaires
    ├── Integration/              # Tests d'intégration
    └── UserAcceptance/          # Tests utilisateur
```

## 🚀 Installation

### Prérequis

- **Unity** 2022.3.62f3 LTS
- **AR Foundation** 5.1.5
- **ARCore** (Android) ou **ARKit** (iOS)
- **TextMeshPro** (inclus)

### Configuration

1. **Cloner le repository:**
```bash
git clone https://github.com/najialaajimi/smart-retail-ar.git
cd smart-retail-ar
```

2. **Ouvrir dans Unity:**
- Lancer Unity Hub
- Cliquer sur "Add" et sélectionner le dossier du projet
- Ouvrir avec Unity 2022.3.62f3 LTS

3. **Installer les packages:**
Les packages nécessaires sont définis dans `Packages/manifest.json` et seront installés automatiquement.

4. **Configuration AR:**

**Pour Android (ARCore):**
- File > Build Settings > Android
- Player Settings > XR Plug-in Management > ARCore ✓
- Minimum API Level: 24

**Pour iOS (ARKit):**
- File > Build Settings > iOS
- Player Settings > XR Plug-in Management > ARKit ✓
- Minimum iOS Version: 11.0
- Ajouter "Camera Usage Description" dans Info.plist

## 🎮 Utilisation

### Mode Test (Sans AR)

Pour tester sans appareil AR:

1. Ouvrir `Assets/Scenes/Frontend/HomeScene`
2. Dans `ScannerController`, activer `testMode = true`
3. Play dans l'éditeur Unity

### Mode Production (Avec AR)

1. Build l'application pour Android/iOS
2. Installer sur un appareil compatible AR
3. Lancer l'application
4. Pointer la caméra vers un produit avec image de référence
5. Les informations s'affichent en overlay AR

### Navigation

- **Écran d'accueil** → Bouton Scanner → **Scanner AR**
- **Scanner AR** → Détection produit → **Détails Produit**
- **Détails Produit** → Recommandations → **Alternatives**

## 📚 Documentation Détaillée

### Sprints

- **[Sprint 1](Documentation/sprint1details.md)** - Développement des interfaces frontend
- **[Sprint 2](Documentation/sprint2details.md)** - Intégration AR avec superposition des données
- **[Sprint 3](Documentation/sprint3details.md)** - Moteur de recommandations avancé
- **[Sprint 4](Documentation/sprint4details.md)** - Tests utilisateurs et validation

### Guides

- **[Guide Développeur](Documentation/DEVELOPMENT.md)** - Setup et développement
- **[Guide Déploiement](Documentation/DEPLOYMENT.md)** - Build et publication
- **[Documentation API](Documentation/API.md)** - API et structures de données

## 🔧 Technologies Utilisées

### Core
- **Unity 2022.3.62f3** - Moteur de jeu
- **C#** - Langage de programmation
- **AR Foundation 5.1.5** - Framework AR cross-platform

### AR
- **ARCore** - AR sur Android
- **ARKit** - AR sur iOS
- **XR Plugin Management** - Gestion des plugins XR

### UI
- **Unity UI (uGUI)** - Système UI
- **TextMeshPro** - Rendu de texte avancé

### Data
- **JsonUtility** - Sérialisation JSON
- **PlayerPrefs** - Stockage local

## 📊 Système de Recommandation

### Algorithme Multi-Critères

Le moteur de recommandation évalue les produits selon 4 dimensions:

1. **Score Nutritionnel (30%)**
   - Nutri-Score
   - Protéines, fibres
   - Sucre, sodium

2. **Score Écologique (30%)**
   - Éco-Score
   - Empreinte carbone
   - Packaging, origine

3. **Score Économique (20%)**
   - Rapport qualité/prix
   - Accessibilité prix

4. **Score Éthique (20%)**
   - Commerce équitable
   - Production locale
   - Pratiques éthiques

### Personnalisation

- Préférences alimentaires (vegan, sans gluten, etc.)
- Filtres d'allergènes
- Historique d'achat
- Budget personnalisé
- Poids de scoring ajustables

## 📈 Performance & KPIs

### Cibles de Performance

| Métrique | Cible | Status |
|----------|-------|--------|
| Taux de reconnaissance | ≥ 95% | ✅ |
| Latence d'affichage | ≤ 1s | ✅ |
| Performance AR | ≥ 30 FPS | ✅ |
| Satisfaction utilisateur | ≥ 80% | ✅ |
| Précision recommandations | ≥ 85% | ✅ |
| Adoption recommandations | ≥ 60% | ✅ |

### Monitoring

L'application intègre:
- **AnalyticsManager** - Tracking des événements
- **PerformanceMonitor** - Monitoring FPS, mémoire
- Logs détaillés pour debugging
- Crash reporting

## 🗃️ Base de Données Produits

### Structure

```json
{
  "products": [
    {
      "id": "PROD001",
      "name": "Yaourt Bio Nature",
      "category": "Produits Laitiers",
      "brand": "BioFarm",
      "price": 2.99,
      "nutritionalInfo": {
        "calories": 65,
        "nutriScore": "A",
        ...
      },
      "ecologicalInfo": {
        "carbonFootprint": 1.2,
        "ecoScore": "A",
        ...
      },
      "ethicalInfo": {
        "fairTrade": true,
        "ethicalScore": "A",
        ...
      }
    }
  ]
}
```

### Catégories

- Produits Laitiers
- Boulangerie
- Boissons
- Épicerie
- Fruits et Légumes
- Confiserie

## 🧪 Tests

### Tests Unitaires

```bash
# Exécuter les tests unitaires
Unity Test Runner > PlayMode/EditMode
```

### Tests d'Intégration

- Navigation entre scènes
- Chargement de la base de données
- Intégration AR ↔ UI
- Système de recommandations

### Tests Utilisateurs

Voir [Sprint 4 Documentation](Documentation/sprint4details.md) pour le plan complet.

## 🤝 Contribution

### Workflow Git

```bash
# Créer une branche feature
git checkout -b feature/ma-fonctionnalite

# Commiter les changements
git commit -m "Add: description"

# Pousser et créer PR
git push origin feature/ma-fonctionnalite
```

### Conventions de Code

- **Naming:** PascalCase pour classes, camelCase pour variables
- **Comments:** Documenter les méthodes publiques
- **Architecture:** Singleton pour managers, Observer pour events
- **Testing:** Ajouter tests pour nouvelles fonctionnalités

## 📝 Changelog

### Version 1.0.0 (Current)

#### Sprint 1 - Frontend ✅
- Interfaces utilisateur complètes
- Navigation entre écrans
- Base de données produits
- Système de préférences

#### Sprint 2 - AR Integration ✅
- Reconnaissance d'image AR temps réel
- Superposition d'informations en AR
- Gestion session AR
- Optimisations performance

#### Sprint 3 - Recommendations ✅
- Moteur de recommandation multi-critères
- Système de scoring avancé
- Personnalisation utilisateur
- Algorithmes de similarité

#### Sprint 4 - Testing ✅
- Framework analytics
- Monitoring performance
- Tests utilisateurs
- Optimisations finales

## 📄 Licence

*(À définir selon les besoins du projet)*

## 👥 Équipe

- **Product Owner** - Vision et roadmap
- **Développeurs** - Implémentation
- **QA** - Tests et validation
- **UX Designer** - Design et expérience utilisateur

## 📧 Contact & Support

- **Issues:** [GitHub Issues](https://github.com/najialaajimi/smart-retail-ar/issues)
- **Discussions:** [GitHub Discussions](https://github.com/najialaajimi/smart-retail-ar/discussions)
- **Email:** support@smartretailar.com

## 🙏 Remerciements

- Unity Technologies pour AR Foundation
- Google pour ARCore
- Apple pour ARKit
- Communauté open-source

---

**Made with ❤️ for better shopping experiences**

## 🔗 Liens Utiles

- [Unity AR Foundation](https://unity.com/unity/features/arfoundation)
- [ARCore](https://developers.google.com/ar)
- [ARKit](https://developer.apple.com/augmented-reality/)
- [Nutri-Score](https://www.santepubliquefrance.fr/nutri-score)
- [Éco-Score](https://docs.score-environnemental.com/)