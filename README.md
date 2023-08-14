# CustomToolbar
![Unity 2019.4+](https://img.shields.io/badge/unity-unity%202019.4%2B-blue)
![License: MIT](https://img.shields.io/badge/License-MIT-brightgreen.svg)

based on this [marijnz unity-toolbar-extender](https://github.com/marijnz/unity-toolbar-extender).

![image]("https://github.com/Herb95/CustomToolbar/blob/main/Documentation~/Image/mainView.jpg?raw=true")

### Why you should use the CustomToolbar?
This custom tool helps you to test and develop your game easily

## Installation
### (For Unity 2018.3 or later) Using OpenUPM  
This package is available on [OpenUPM](https://openupm.com).  
You can install it via [openupm-cli](https://github.com/openupm/openupm-cli).  
```
openupm add com.smkplus.customtoolbar
```

### (For Unity 2019.2 or later) Through Unity Package Manager
 * MenuItem - Window - Package Manager
 * Add package from git url
 * paste ```https://github.com/Herb95/CustomToolbar.git#main```

### (For Unity 2018.3 or later) Using Git
Find the manifest.json file in the Packages folder of your project and add a line to `dependencies` field.
`"com.smkplus.customtoolbar": "https://github.com/Herb95/CustomToolbar.git#main"`
Or, use [UpmGitExtension](https://github.com/mob-sakai/UpmGitExtension) to install and update the package.

### For Unity 2018.2 or earlier
1. Download a source code zip this page
2. Extract it
3. Import it into the following directory in your Unity project
   - `Packages` (It works as an embedded package. For Unity 2018.1 or later)
   - `Assets` (Legacy way. For Unity 2017.1 or later)

## Sample scenes to test  
You can import sample scenes from package manager. 

![image](https://github.com/Herb95/CustomToolbar/blob/main/Documentation~/Image/Package-Manager.png)
____________
Scene selection dropdown to open scene in editor. Scenes in build have unity icon while selected and appear above splitter in list

![image](https://github.com/Herb95/CustomToolbar/blob/main/Documentation~/Image/SceneSelect.jpg)
____________

when you want to clear all playerprefs you have to follow 3 step:

![image](https://github.com/Herb95/CustomToolbar/blob/main/Documentation~/Image/clear_all_playerprefs.png)

but you can easily Clear them by clicking on this button:

![image](https://github.com/Herb95/CustomToolbar/blob/main/Documentation~/Image/btnClearPrefs.jpg)
____________

another button relevant to saving is this button that prevents saving during the gameplay. because sometimes you have to Clear All playerprefs after each test so you can enable this toggle:

Enable Playerprefs:

![image](https://github.com/Herb95/CustomToolbar/blob/main/Documentation~/Image/btnDisablePrefs.jpg)

Disable Playerprefs:

![image](https://github.com/Herb95/CustomToolbar/blob/main/Documentation~/Image/btnDisablePrefsInactive.jpg)
____________

you can restart the active scene by this button:

![image](https://github.com/Herb95/CustomToolbar/blob/main/Documentation~/Image/btnRestartScene.jpg)
____________

then you should start the game:

![image](https://user-images.githubusercontent.com/16706911/100723264-cd945380-33d6-11eb-9611-b1fe470dbd0b.png)

this button is shortcut to start the game from scene 1:

![image](https://github.com/Herb95/CustomToolbar/blob/main/Documentation~/Image/btnFirstScene.jpg)
____________

I usually test my games by changing timescale.

![image](https://github.com/Herb95/CustomToolbar/blob/main/Documentation~/Image/timescale.jpg)
____________

Also it usefull to test your game with different framerates, to be sure that it is framerate-independent.

![image](https://github.com/Herb95/CustomToolbar/blob/main/Documentation~/Image/FPS.jpg)
____________

Button to recompile scripts. Usefull when you working on splitting code into .asmdef

![image](https://github.com/Herb95/CustomToolbar/blob/main/Documentation~/Image/btnRecompile.jpg)
____________

Force reserialize selected(in project window) assets. What it does - https://docs.unity3d.com/ScriptReference/AssetDatabase.ForceReserializeAssets.html

![image](https://github.com/Herb95/CustomToolbar/blob/main/Documentation~/Image/btnReserializeSelected.jpg)
____________

Force reserialize all assets. Same as previous, but for all assets and takes some time. Use this after adding new asset or updating unity version in order to not spam git history with unwanted changes.

![image](https://github.com/Herb95/CustomToolbar/blob/main/Documentation~/Image/btnReserializeAll.jpg)
____________
  
You can customize the toolbar on Project Setting

![Images~/ProjectSetting-CustomToolbar.png](https://github.com/Herb95/CustomToolbar/blob/main/Documentation~/Image/ProjectSetting-CustomToolbar.png)





