# Sound Manager
Awesome Projection Games's sound manager for Unity is a collection of tools and utilities to help you manage your audio sources and clips in your Unity project.

## Features
- **Randomization**: You can randomize the pitch, offset, clips and volume of your audio sources.
- **Fade In/Out**: You can fade in and out your audio sources.
- **Adaptive Music**: You can create adaptive music systems by providing a list of clips and the manager will play depending on the current state of your game.
- **Radio / Synced Audio**: You can create radio systems or synced audio by providing a list of audio sources and the manager will play them in sync so you can have multiple speakers playing the same audio.
- **Utility Functions**: Like custom loops, motor sounds, etc.

**Note**: This package is mainly used for randomization and fading so other feature might not be as polished.

## Installation
To install this package, you can use the Unity Package Manager. To do so, open the Unity Package Manager and click on the `+` button in the top left corner. Then select `Add package from git URL...` and enter the following URL:

```
https://github.com/AwesomeProjectionGames/SoundManager.git
```

Or you can manually to add the following line to your `manifest.json` file located in your project's `Packages` directory.

```json
{
  "dependencies": {
    ...
    "com.awesomeprojection.soundmanager": "https://github.com/AwesomeProjectionGames/SoundManager.git"
    ...
   }
}