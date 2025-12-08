# Guide des Assets Visuels - Smart Retail AR

## Structure des Assets

```
Assets/Resources/
├── Images/
│   ├── Products/          # Images produits
│   ├── Icons/             # Icônes UI
│   ├── Logos/             # Logos et marques
│   └── Backgrounds/       # Arrière-plans
├── QRCodes/              # QR codes générés
└── Materials/            # Materials AR
```

---

## Images Produits

### Spécifications

**Format:** JPG ou PNG  
**Résolution:** 512x512 pixels (recommandé)  
**Ratio:** 1:1 (carré)  
**Taille max:** 500 KB par image  
**Nommage:** `prod001.jpg`, `prod002.jpg`, etc.  
**Fond:** Blanc ou transparent (PNG)

### Organisation

```
Assets/Resources/Images/Products/
├── prod001.jpg    # Organic Dairy Product 1
├── prod002.jpg    # Fresh Fruits Product 2
├── prod003.jpg    # Natural Vegetables Product 3
└── ...
```

### Création d'Images

#### Option 1: Photos Réelles
```bash
# Prendre des photos de produits réels
# Nettoyer l'arrière-plan
# Recadrer en carré
# Exporter en 512x512
```

#### Option 2: Images de Stock
Sites recommandés:
- Unsplash (gratuit)
- Pexels (gratuit)
- Pixabay (gratuit)

#### Option 3: Génération Placeholder

Pour le développement, créer des placeholders:

```python
# generate_placeholder_images.py
from PIL import Image, ImageDraw, ImageFont
import os

def create_placeholder(product_id, category, size=512):
    # Créer image avec couleur de fond
    colors = {
        'Dairy': '#FFF8DC',
        'Fruits': '#FFE4B5',
        'Vegetables': '#90EE90',
        'Beverages': '#87CEEB',
        'Snacks': '#FFD700',
        'Bakery': '#F4A460',
        'Meat': '#CD5C5C',
        'Seafood': '#4682B4',
        'Frozen': '#E0FFFF',
        'Pantry': '#D2B48C'
    }
    
    color = colors.get(category, '#FFFFFF')
    img = Image.new('RGB', (size, size), color)
    draw = ImageDraw.Draw(img)
    
    # Ajouter texte
    font = ImageFont.load_default()
    text = f"{product_id}\n{category}"
    bbox = draw.textbbox((0, 0), text, font=font)
    text_width = bbox[2] - bbox[0]
    text_height = bbox[3] - bbox[1]
    position = ((size - text_width) // 2, (size - text_height) // 2)
    draw.text(position, text, fill='black', font=font)
    
    return img

# Générer pour tous les produits
output_dir = "Assets/Resources/Images/Products"
os.makedirs(output_dir, exist_ok=True)

categories = ["Dairy", "Fruits", "Vegetables", "Beverages", "Snacks"]
for i in range(1, 51):
    product_id = f"PROD{i:03d}"
    category = categories[(i-1) % len(categories)]
    img = create_placeholder(product_id, category)
    img.save(f"{output_dir}/prod{i:03d}.jpg", quality=85)
    print(f"Created {product_id}.jpg")
```

### Optimisation dans Unity

```
1. Sélectionner l'image dans Project
2. Inspector > Texture Type: Sprite (2D and UI)
3. Max Size: 512 ou 1024
4. Format: 
   - iOS: ASTC 6x6
   - Android: ETC2
5. Compression: High Quality
6. Generate Mip Maps: ✓
7. Apply
```

---

## Icônes UI

### Icônes Nécessaires

#### Navigation (32x32 px)
- `icon_home.png` - Accueil
- `icon_scan.png` - Scanner
- `icon_ar.png` - Vue AR
- `icon_list.png` - Liste
- `icon_profile.png` - Profil
- `icon_settings.png` - Paramètres

#### Actions (32x32 px)
- `icon_back.png` - Retour
- `icon_close.png` - Fermer
- `icon_check.png` - Valider
- `icon_info.png` - Information
- `icon_favorite.png` - Favori
- `icon_share.png` - Partager
- `icon_filter.png` - Filtrer

#### Tags (24x24 px)
- `tag_bio.png` - Bio
- `tag_local.png` - Local
- `tag_vegan.png` - Vegan
- `tag_gluten_free.png` - Sans gluten
- `tag_fair_trade.png` - Commerce équitable

#### Scores (48x48 px)
- `score_eco.png` - Score écologique
- `score_health.png` - Score santé
- `score_quality.png` - Score qualité

### Sources d'Icônes

**Gratuites:**
- Material Icons (Google)
- Font Awesome
- Ionicons
- Feather Icons

**Premium:**
- Nucleo
- Streamline Icons

### Format

**Format:** PNG avec transparence  
**Résolution:** @1x, @2x, @3x pour différents DPI  
**Couleurs:** Monochrome ou couleur selon usage

---

## Logos et Marques

### Logo Principal

```
Assets/Resources/Images/Logos/
├── logo_main.png          # 512x512 - Logo principal
├── logo_horizontal.png    # 1024x256 - Version horizontale
├── logo_icon.png          # 192x192 - Icône app
└── logo_splash.png        # 1080x1920 - Écran splash
```

### Spécifications Logo

**Logo Principal:**
- Format: PNG transparent
- Taille: 512x512 px
- Padding: 64px de chaque côté
- Usage: Écran accueil, header

**Logo Horizontal:**
- Format: PNG transparent
- Taille: 1024x256 px
- Usage: Header, branding

**Icône App:**
- Format: PNG
- Taille: 192x192 px (Android)
- Taille: 1024x1024 px (iOS)
- Usage: Icône application

---

## QR Codes

### Génération

Utiliser le script `generate_qrcodes.py`:

```bash
python3 generate_qrcodes.py
```

### Spécifications

**Format:** PNG  
**Taille:** 300x300 px minimum  
**Version QR:** 1 (21x21 modules)  
**Error Correction:** L (Low, 7%)  
**Couleur:** Noir sur blanc  
**Border:** 4 modules (quiet zone)

### Nommage

```
Assets/Resources/QRCodes/
├── PROD001.png
├── PROD002.png
├── PROD003.png
└── ...
```

### Impression

Pour imprimer les QR codes:

1. Ouvrir `printable_qr_codes.html`
2. Format A4, portrait
3. Taille minimum: 3x3 cm
4. Papier autocollant recommandé
5. Résolution: 300 DPI

---

## Materials AR

### Materials Nécessaires

```
Assets/Materials/
├── AROverlay.mat          # Material overlay AR
├── ARPlane.mat           # Material plans détectés
├── ARHighlight.mat       # Material surbrillance
└── ARPointer.mat         # Material pointeur
```

### Configuration AROverlay

```
Shader: UI/Default ou Custom AR Shader
Rendering Mode: Transparent
Color: Blanc avec alpha
Properties:
  - Main Texture: UI texture
  - Alpha Cutoff: 0
  - Billboard: Enabled
```

---

## Textures

### Textures UI

```
Assets/Resources/Images/Backgrounds/
├── bg_main.jpg           # Arrière-plan principal
├── bg_scanner.jpg        # Arrière-plan scanner
├── bg_gradient.png       # Gradient pour overlays
└── pattern_grid.png      # Motif grille
```

### Spécifications

**Format:** JPG (photos) ou PNG (avec transparence)  
**Taille:** 1920x1080 maximum  
**Compression:** Medium à High  
**Tiling:** Power of 2 si répétée (256, 512, 1024)

---

## Animations

### Sprites Animés

Pour les animations UI:

```
Assets/Resources/Images/Animations/
├── loading_001.png
├── loading_002.png
├── loading_003.png
└── ...
```

### Format

**Format:** PNG transparent  
**FPS:** 12-24 images/seconde  
**Taille:** 128x128 px (icônes) ou 256x256 px (éléments plus grands)

---

## Checklist Assets

### Images Produits
- [ ] 50 images produits créées
- [ ] Format 512x512 px
- [ ] Optimisées < 500 KB
- [ ] Nommage correct (prod001.jpg, etc.)

### Icônes
- [ ] Icônes navigation (6)
- [ ] Icônes actions (7)
- [ ] Icônes tags (5)
- [ ] Icônes scores (3)

### Logos
- [ ] Logo principal
- [ ] Logo horizontal
- [ ] Icône app
- [ ] Écran splash

### QR Codes
- [ ] 50 QR codes générés
- [ ] Fichier HTML imprimable
- [ ] Testés avec scanner

### Materials
- [ ] AROverlay material
- [ ] ARPlane material
- [ ] Materials configurés

---

## Outils Recommandés

### Édition Images
- **GIMP** (gratuit) - Édition avancée
- **Paint.NET** (gratuit, Windows) - Édition simple
- **Photopea** (web, gratuit) - Alternative Photoshop
- **Adobe Photoshop** (payant) - Professionnel

### Compression
- **TinyPNG** (web) - Compression PNG
- **JPEGmini** (web/app) - Compression JPEG
- **ImageOptim** (Mac, gratuit) - Optimisation batch

### Génération
- **Canva** (web, gratuit/payant) - Design graphique
- **Figma** (web, gratuit/payant) - UI/UX design
- **Blender** (gratuit) - 3D et rendering

---

## Workflow Création Assets

1. **Planification**
   - Lister tous les assets nécessaires
   - Définir spécifications
   - Créer style guide

2. **Création**
   - Designer ou photographier
   - Respecter spécifications
   - Nommer correctement

3. **Optimisation**
   - Compresser images
   - Vérifier tailles
   - Tester qualité

4. **Import Unity**
   - Placer dans bons dossiers
   - Configurer import settings
   - Créer materials si nécessaire

5. **Tests**
   - Vérifier affichage
   - Tester sur différents devices
   - Ajuster si nécessaire

---

**Document maintenu par:** Équipe Design  
**Dernière mise à jour:** Décembre 2024
