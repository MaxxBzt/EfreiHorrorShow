
# EFREI Horror Show : Ashes Guilt

# Introduction
> *"Dans les cendres de mes souvenirs se cache une vérité que je n'ose pas regarder en face."*

**Et si votre propre culpabilité devenait votre prison ?** Vous étiez quatre amis. Un road trip, une soirée arrosée, une dispute, et l'irréparable : un fracas de métal au milieu d'une forêt sombre. Vous vous réveillez seul dans votre salon, mais l'air est différent. Les murs de votre appartement ne sont plus un refuge, ils sont les témoins silencieux d'un drame que vous tentez d'oublier. 

**Ashes Guilt** est un film d'horreur psychologique interactif en réalité augmentée. Le joueur revit le même moment encore et encore, cherchant des réponses cachées dans ses souvenirs après un tragique accident de voiture. L'application simule des boucles temporelles où l'utilisateur doit interagir avec son espace physique pour comprendre sa propre culpabilité. L'horreur n'est plus confinée à un écran : elle prend vie dans votre propre maison.

[Vidéo démo](https://youtube.com/watch?v=yeqa3PWRARc&feature=youtu.be) [![Demo video](https://drive.usercontent.google.com/download?id=1Rj-ixEcpZvJrZ20_i8Qt_o9Y_QLc8Fj8&authuser=0)](https://youtube.com/watch?v=yeqa3PWRARc&feature=youtu.be)

## Index

- [About](#about)
- [Usage](#usage)
  - [Installation](#installation)
  - [Commands](#commands)
- [Development](#development)
  - [Pre-Requisites](#pre-requisites)
  - [Development Environment](#development-environment)
  - [Build](#build)  
  - [Deployment](#deployment)  
- [Credit/Acknowledgment](#creditacknowledgment)

## About

Le projet EFREI Horror Show s'attaque à la fois aux défis technologiques et sociétaux. Il montre comment la réalité augmentée peut aller au-delà du divertissement pour explorer des sujets sensibles comme la santé mentale, les traumatismes et la mémoire. 

Contrairement à une expérience VR classique saturée d'action, notre approche repose sur une narration profonde et une horreur psychologique subtile sans jumpscares visuels. Actuellement sous forme de Proof of Concept (POC) comprenant l'introduction et la première boucle temporelle, le scénario s'adapte à l'environnement du joueur grâce à la technologie Passthrough. Le jeu cible les joueurs axés sur la narration à la recherche d'une expérience riche et immersive sur les plateformes AR.

## Usage

L'application est destinée à être utilisée avec un casque de réalité mixte (Meta Quest 3 ou Quest 3S) en configuration PCVR via l'éditeur Unity. 

### Installation

Le projet étant un prototype fonctionnel sans build `.apk` ou `.exe` pré-généré, l'exécution se fait directement depuis l'environnement de développement.

1. **Préparation de la pièce** : Avant toute chose, allumez votre casque Meta Quest et effectuez un scan complet de votre pièce (murs, meubles) via les paramètres système. C'est indispensable pour l'ancrage spatial AR.
2. **Récupération du projet** : Clonez le dépôt GitHub sur votre machine.
3. **Ouverture** : Lancez le projet via Unity Hub.
4. **Lancement** : Connectez votre casque au PC (Câble Link ou Air Link), ouvrez la scène principale et cliquez sur le bouton "Play" dans l'éditeur.

### Commands

L'expérience privilégie un gameplay "Zéro locomotion", conçu pour être joué assis ou debout sur place sans déplacement physique nécessaire.

- **Saisie d'objet (Grab)** : Gâchette latérale (Grip) des manettes pour attraper les objets tangibles liés à l'histoire.
- **Interaction** : Gâchette d'index (Trigger) pour valider ou interagir avec les éléments narratifs.
- **Déplacement** : Déplacement physique limité à votre zone de jeu (Room Scale) pour observer les éléments superposés à votre pièce.

## Development

Cette section explique comment configurer l'environnement pour développer sur le projet.

### Pre-Requisites

- Unity Hub.
- Unity Editor version **6000.0.48f1**.
- Meta XR All-in-One SDK (incluant les Building Blocks pour le Passthrough).
- Meta Quest 3 ou 3S raccordé au PC.

### Development Environment

1. **Clonage du projet** :
   ```bash
   $ git clone [https://github.com/MaxxBzt/EfreiHorrorShow/](https://github.com/MaxxBzt/EfreiHorrorShow/)
   ```
2. Configuration Unity :
3. Ouvrez Unity Hub et ajoutez le dossier du projet. Assurez-vous d'utiliser impérativement la version 6000.0.48f1 pour éviter de casser les dépendances du SDK Meta.

### Build
Bien que le projet s'exécute actuellement en Play Mode, la procédure standard pour compiler une version standalone est la suivante :
- Allez dans File > Build Settings.
- Assurez-vous que les scènes d'introduction et de boucle temporelle sont cochées dans le bon ordre.
- Sélectionnez la plateforme Android (pour un déploiement natif sur Quest) et cliquez sur `Switch Platform`.
- Cliquez sur Build pour générer un fichier `.apk.`

### Deployment
Le déploiement pour des tests internes ou des démonstrations (comme les JPO) peut se faire en direct via le bouton "Build and Run" ou en exécutant simplement le projet dans l'éditeur avec le casque branché.

Lancement de la Scène :
Ouvrez la scène principale d'introduction. Assurez-vous que les fonctionnalités Passthrough et Scene Mesh sont bien actives dans les paramètres de l'OVRCameraRig.
### Credit/Acknowledgment
Le projet a été réalisé par l'équipe suivante:
- [Maxime Bézot](https://github.com/MaxxBzt/): Chef de Projet
- [Audrey Damiba](https://github.com/audie17) : Responsable Documentation & Recherche Utilisateur 
- [Alexandre Baudin](https://github.com/Antiflex): Développeur Contenu 3D AR 
- [Roxane Braconnier](https://github.com/R0xane): Responsable Équipe Dev 
- [Yoke Ngassa](https://github.com/Rolwenx): Développeuse Scènes 3D Unity 
- [Jessica Rasoamanana](https://github.com/acrazypotterhead): Développeuse Interactions AR
