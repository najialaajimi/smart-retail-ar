# Guide d'Installation et Configuration pour Ubuntu 24.04

## Problème: Écran Bleu dans Unity

Si vous voyez seulement un écran bleu, cela peut être causé par plusieurs facteurs sur Ubuntu. Voici les étapes complètes pour résoudre ce problème.

---

## Prérequis Système

### Configuration Minimale
- **OS**: Ubuntu 24.04 LTS
- **RAM**: 8 GB minimum (16 GB recommandé)
- **Espace Disque**: 20 GB libres
- **GPU**: Carte graphique compatible OpenGL 3.2+ ou Vulkan

---

## Étape 1: Installation de Unity Hub

### 1.1 Télécharger Unity Hub
```bash
# Créer un dossier pour Unity Hub
mkdir -p ~/Unity

# Télécharger Unity Hub (version officielle pour Linux)
cd ~/Unity
wget https://public-cdn.cloud.unity3d.com/hub/prod/UnityHubSetup.AppImage

# Rendre le fichier exécutable
chmod +x UnityHubSetup.AppImage
```

### 1.2 Installer les Dépendances Système
```bash
# Mettre à jour le système
sudo apt update
sudo apt upgrade -y

# Installer les dépendances essentielles
sudo apt install -y libgconf-2-4 libcanberra-gtk-module libcanberra-gtk3-module

# Installer les bibliothèques graphiques
sudo apt install -y libgl1-mesa-glx libgl1-mesa-dri mesa-utils

# Installer les codecs et bibliothèques multimédia
sudo apt install -y libgtk-3-0 libgbm1 libasound2

# Installer FUSE (nécessaire pour AppImage)
sudo apt install -y fuse libfuse2
```

### 1.3 Lancer Unity Hub
```bash
cd ~/Unity
./UnityHubSetup.AppImage
```

**Note**: Si Unity Hub ne démarre pas, essayez:
```bash
./UnityHubSetup.AppImage --no-sandbox
```

---

## Étape 2: Installation de Unity 2022.3.62f3 (VERSION EXACTE REQUISE)

### 2.1 Activer Unity Hub
1. Ouvrir Unity Hub
2. Cliquer sur l'icône utilisateur (coin supérieur droit)
3. Se connecter avec votre compte Unity (créer un compte si nécessaire sur https://id.unity.com/)
4. Obtenir une licence gratuite:
   - Cliquer sur "Manage Licenses"
   - Cliquer sur "Add"
   - Sélectionner "Get a free personal license"
   - Activer "Unity Personal"

### 2.2 Installer Unity 2022.3.62f3
1. Dans Unity Hub, aller dans l'onglet **"Installs"**
2. Cliquer sur **"Install Editor"**
3. Dans la liste, chercher **"2022.3.62f3"** (version LTS)
   - Si cette version n'apparaît pas, cliquer sur "Archive" en haut à droite
   - Aller sur https://unity.com/releases/editor/archive
   - Chercher Unity 2022.3.62f3
   - Cliquer sur "Unity Hub" pour l'installer

4. **Sélectionner les Modules** (IMPORTANT):
   - ✅ **Linux Build Support (Mono)**
   - ✅ **Android Build Support** (cocher toutes les sous-options)
     - Android SDK & NDK Tools
     - OpenJDK
   - ✅ **Documentation** (optionnel mais recommandé)
   - ✅ **Language Packs** (Français si désiré)

5. Cliquer sur "Continue" puis "Install"
   - L'installation prendra 15-30 minutes

---

## Étape 3: Configuration Graphique pour Éviter l'Écran Bleu

### 3.1 Vérifier le Support OpenGL
```bash
# Vérifier la version OpenGL
glxinfo | grep "OpenGL version"

# Devrait afficher au moins OpenGL 3.2
# Si la commande n'existe pas, installer:
sudo apt install -y mesa-utils

# Vérifier le pilote graphique
glxinfo | grep "OpenGL renderer"
```

### 3.2 Installer les Pilotes Graphiques Appropriés

**Pour NVIDIA:**
```bash
# Vérifier si vous avez une carte NVIDIA
lspci | grep -i nvidia

# Installer les pilotes propriétaires NVIDIA
sudo ubuntu-drivers devices
sudo ubuntu-drivers autoinstall

# OU installer manuellement une version spécifique
sudo apt install -y nvidia-driver-535

# Redémarrer après installation
sudo reboot
```

**Pour AMD:**
```bash
# Les pilotes AMD sont généralement inclus dans le kernel
# Installer les outils supplémentaires
sudo apt install -y mesa-vulkan-drivers mesa-vdpau-drivers
```

**Pour Intel:**
```bash
# Installer les pilotes Intel
sudo apt install -y intel-media-va-driver i965-va-driver
```

### 3.3 Configuration Vulkan (Recommandé pour Unity sur Linux)
```bash
# Installer Vulkan
sudo apt install -y vulkan-tools libvulkan1 vulkan-validationlayers

# Vérifier Vulkan
vulkaninfo | grep "deviceName"
```

---

## Étape 4: Cloner et Ouvrir le Projet

### 4.1 Installer Git (si pas déjà installé)
```bash
sudo apt install -y git
```

### 4.2 Cloner le Projet
```bash
# Créer un dossier pour vos projets Unity
mkdir -p ~/UnityProjects
cd ~/UnityProjects

# Cloner le dépôt
git clone https://github.com/najialaajimi/smart-retail-ar.git
cd smart-retail-ar
```

### 4.3 Ouvrir le Projet dans Unity Hub
1. Dans Unity Hub, aller dans l'onglet **"Projects"**
2. Cliquer sur **"Add"** → **"Add project from disk"**
3. Naviguer vers `~/UnityProjects/smart-retail-ar`
4. Sélectionner le dossier et cliquer "Add Project"

⚠️ **IMPORTANT**: Vérifier que la version Unity affichée est **"2022.3.62f3"**

---

## Étape 5: Premier Lancement du Projet

### 5.1 Ouvrir le Projet
1. Dans Unity Hub, cliquer sur le projet "smart-retail-ar"
2. Unity va s'ouvrir et commencer l'import des packages
3. **C'est normal si cela prend 5-15 minutes** la première fois

### 5.2 Résoudre l'Écran Bleu

**Si vous voyez toujours un écran bleu:**

#### Solution 1: Changer le Mode de Rendu
1. Une fois Unity ouvert, aller dans **Edit** → **Project Settings**
2. Sélectionner **Graphics** dans le panneau gauche
3. Changer **"Scriptable Render Pipeline Settings"** à **"None"**
4. Sauvegarder et redémarrer Unity

#### Solution 2: Désactiver l'Accélération Matérielle
```bash
# Lancer Unity avec OpenGL (au lieu de Vulkan)
cd ~/Unity/Hub/Editor/2022.3.62f3/Editor
./Unity -force-glcore
```

#### Solution 3: Lancer Unity en Mode Sûr
```bash
# Ouvrir Unity avec les paramètres par défaut
cd ~/Unity/Hub/Editor/2022.3.62f3/Editor
./Unity -force-d3d11 -force-glcore42
```

#### Solution 4: Configurer les Préférences Unity
1. Avant d'ouvrir le projet, dans Unity Hub
2. Cliquer sur les 3 points à côté du projet
3. Sélectionner "Open with" → "2022.3.62f3"
4. Cocher "Safe Mode"

---

## Étape 6: Tester le Projet

### 6.1 Ouvrir la Scène Principale
1. Dans Unity, aller dans **Project** (panneau en bas)
2. Naviguer vers **Assets** → **Scenes**
3. Double-cliquer sur **HomeScene.unity**

### 6.2 Lancer le Projet
1. Cliquer sur le bouton **Play** (▶️) en haut de l'éditeur
2. Vous devriez voir l'interface de l'application avec:
   - Message de bienvenue
   - Bouton "Scanner Produit"
   - Bouton "Mon Profil"

### 6.3 Vérifier la Console
1. Aller dans **Window** → **General** → **Console** (ou Ctrl+Shift+C)
2. Vérifier qu'il n'y a pas d'erreurs rouges
3. Vous devriez voir: `"Loaded 52 products from database"`

---

## Étape 7: Dépannage Avancé

### Problème: "Failed to load window layout"
```bash
# Supprimer les préférences Unity corrompues
rm -rf ~/.config/unity3d/Unity/Editor-5.x/
```

### Problème: "Assembly has reference to non-existent assembly"
1. Dans Unity: **Assets** → **Reimport All**
2. Attendre la fin de la réimportation

### Problème: Performances Faibles
```bash
# Augmenter la priorité de Unity
sudo apt install -y cpulimit
# Lancer Unity normalement puis:
pidof Unity | xargs renice -n -10 -p
```

### Problème: Textures ou UI Manquantes
1. **Edit** → **Project Settings** → **Quality**
2. Réduire le niveau de qualité à "Medium" ou "Low"
3. **Edit** → **Preferences** → **GI Cache**
4. Cliquer sur "Clean Cache"

---

## Étape 8: Configuration Optimale pour Ubuntu

### 8.1 Fichier de Configuration Unity (Optionnel)
Créer un fichier de lancement personnalisé:

```bash
# Créer un script de lancement
nano ~/launch-unity-smart-retail.sh
```

Ajouter ce contenu:
```bash
#!/bin/bash
export UNITY_EDITOR_PATH=~/Unity/Hub/Editor/2022.3.62f3/Editor/Unity
export PROJECT_PATH=~/UnityProjects/smart-retail-ar

# Lancer avec OpenGL Core
$UNITY_EDITOR_PATH -projectPath "$PROJECT_PATH" -force-glcore -force-glcore42

# Alternative avec Vulkan (si supporté)
# $UNITY_EDITOR_PATH -projectPath "$PROJECT_PATH" -force-vulkan
```

Rendre exécutable:
```bash
chmod +x ~/launch-unity-smart-retail.sh
```

Lancer:
```bash
~/launch-unity-smart-retail.sh
```

### 8.2 Améliorer les Performances
```bash
# Augmenter les limites de fichiers ouverts
echo "fs.inotify.max_user_watches=524288" | sudo tee -a /etc/sysctl.conf
sudo sysctl -p
```

---

## Étape 9: Vérification Finale

### Liste de Vérification ✅
- [ ] Unity Hub installé et lancé
- [ ] Unity 2022.3.62f3 installé avec modules Android
- [ ] Pilotes graphiques à jour
- [ ] Projet cloné depuis GitHub
- [ ] Projet ouvert sans erreurs
- [ ] HomeScene.unity s'ouvre correctement
- [ ] Mode Play fonctionne (bouton ▶️)
- [ ] Console affiche "Loaded 52 products from database"
- [ ] Interface utilisateur visible (pas d'écran bleu)

---

## Logs et Support

### Localisation des Logs Unity
```bash
# Logs de l'éditeur
~/.config/unity3d/Editor.log

# Voir les logs en temps réel
tail -f ~/.config/unity3d/Editor.log
```

### Commandes de Diagnostic
```bash
# Informations système
uname -a
lsb_release -a

# Informations graphiques
glxinfo | head -20
vulkaninfo | head -30

# Processus Unity
ps aux | grep Unity
```

### Capture d'Écran pour Aide
Si le problème persiste, faire une capture d'écran:
```bash
# Installer outil de capture
sudo apt install -y flameshot

# Prendre une capture
flameshot gui
```

---

## Résumé des Commandes Rapides

```bash
# Installation complète en une fois
sudo apt update && sudo apt upgrade -y
sudo apt install -y libgconf-2-4 libcanberra-gtk-module libcanberra-gtk3-module \
    libgl1-mesa-glx libgl1-mesa-dri mesa-utils libgtk-3-0 libgbm1 libasound2 \
    fuse libfuse2 vulkan-tools libvulkan1 git

# Cloner le projet
mkdir -p ~/UnityProjects
cd ~/UnityProjects
git clone https://github.com/najialaajimi/smart-retail-ar.git

# Augmenter limite fichiers
echo "fs.inotify.max_user_watches=524288" | sudo tee -a /etc/sysctl.conf
sudo sysctl -p
```

---

## Contacts et Ressources

### Documentation du Projet
- `README.md` - Vue d'ensemble
- `TECHNICAL_DOC.md` - Architecture technique
- `SETUP_GUIDE.md` - Guide d'installation général

### Ressources Unity Linux
- Unity Forums Linux: https://forum.unity.com/forums/linux-editor-support.93/
- Unity Documentation: https://docs.unity3d.com/2022.3/Documentation/Manual/

---

**Version**: 1.0.0 - Sprint 1
**Dernière mise à jour**: Décembre 2024
**Testé sur**: Ubuntu 24.04 LTS
