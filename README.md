# HedgeModManager - Pirate-Friendly Version
A patched version of the program for managing mods for Sonic Generations, Sonic Lost World and Sonic Forces on PC.

![https://raw.githubusercontent.com/marcussacana/HedgeModManager/master/CallingCard.png](https://raw.githubusercontent.com/marcussacana/HedgeModManager/master/CallingCard.png)

## So how do I use this?
Its simple, just grab the [latest version from GitHub](https://github.com/marcussacana/HedgeModManager/releases/latest) (or grab the latest compile from [AppVeyor](https://ci.appveyor.com/project/marcussacana/hedgemodmanager) then extract all the files anywhere and run HedgeModManager.exe.

## How do I install mods?
There are multiple ways of installing mods, one of the easy ways of installing mods is by dragging its zip/7z/rar/folder into the mod list along with also being able to drag and drop multiple files and/or folders.

Once your done, you can start checking the checkbox(es) of the mods and codes you want to play and click "Save and Play".

## How do I release mods for this?
**The following section is for mod developers only. If all you want to do is play with some mods made by others, simply follow the above steps.**

Mods designed for HedgeModManager needs to come in the form of folders that contain the following:

- A "mod.ini" file (a file which describes your mod, as well as all it's various details).
- A "disk" folder
  - A "bb" folder (for Sonic Generations)
    - All your modified files/folders from the root of Sonic Generations' bb.cpk in their raw form.
  - A "bb2" folder (for Sonic Generations)
    - All your modified files/folders from the root of Sonic Generations' bb2.cpk in their raw form.
  - A "bb3" folder (for Sonic Generations)
    - All your modified files/folders from the root of Sonic Generations' bb3.cpk in their raw form.
  - A "sonic2013_patch_0" folder (for Sonic Lost World)
    - All your modified files/folders from the root of Sonic Lost World's sonic2013_0.cpk in their raw form.
  - A "wars_patch" folder (for Sonic Forces)
    - All your modified files/folders from the root of Sonic Forces' wars_0.cpk and wars_1.cpk in their raw form.

So long as the structure of your mod remains in this way, virtually any file in the game can be modified and released as part of your mod.

As an example, the extremely basic "Tanic the Hedgehog" recolor mod for Sonic Generations, Sonic Lost World and Sonic Forces has a file/folder structure that goes like so:
- A "mod.ini" file
- A "disk" folder
  - A "bb3" folder (Sonic Generations)
    - Sonic.ar.00
    - Sonic.arl
  - A "sonic2013_patch_0" folder (Sonic Lost World)
    - Sonic.pac
    - Sonic.pac.00
  - A "wars_patch" folder (Sonic Forces)
    - A "character" folder
      - Sonic.pac
      - Sonic.pac.000
      - SonicClassic.pac
      - SonicClassic.pac.000

Wereas the "MLG Speedrun Zone 1" mod (which modifies certain files not on the root of the .cpk) has a file/folder structure that goes like so:

- A "mod.ini" file
- A "disk" folder
  - A "sonic2013_patch_0" folder
    - A "set" folder
      - w1a01_obj_00.orc
      - w1a01_obj_01.orc
      - w1a01_obj_02.orc
      - w1a01_obj_03.orc
    - actstgmission.lua

### The mod.ini file
The mod.ini file is a mod configuration file that details all the information about your mod, as well as how CPKREDIR should load the mod.

The version of the format used in the HedgeModManager is a variation on the format used in SLWModLoader and SonicGMI, with some minor changes/additions here and there.

Here's an example of a mod.ini file for Sonic Lost World:
```
[Main]
IncludeDir0="."
IncludeDirCount=1
UpdateServer="https://dl.dropboxusercontent.com/s/xkapkbby1vu4snk/Sonic06UpdateFile.txt"

[Desc]
Title="SLW '06 Project"
Description="What if Sonic '06 stages were fully playable in an engine that not only perfectly fit it's level design/gameplay, but was actually GOOD? \n\nIntroducing the SLW '06 Project! A mega mod for Lost World that ports as much of Sonic '06 as humanly possible without the glitches and unattractive visuals!  \n\nCredits:\n UltimateDarkman for making the wonderful '06-esque animations.\n Death for his wonderful beta-testing work. \n Gotta Play Fast and Slash for porting the Sonic '06 player models to Lost World. \n Radfordhound for ripping the stages, rendering GIA/generating lightfields, drawing vertex colors, and porting the HUD/music. \n Radfordhound, Gotta Play Fast, Slash, and Beatz for porting the stages and doing object placement. "
Version="Alpha 1.0"
Date="4/20/16"
Author="Radfordhound & Beatz & GPF & Death & UltimateDarkman"
AuthorURL="https://www.youtube.com/user/Radfordhound & https://www.youtube.com/channel/UCEjwges-3BTaWsMwOGJDoGQ & https://www.youtube.com/channel/UCZfOGBkXRKICFozWU5bE0Xg & https://www.youtube.com/user/DeathwolvesProjects & https://www.youtube.com/user/UltimateDarkman2010"
URL="https://onedrive.live.com/redir?resid=A0D011638C5973B3!5011&authkey=!AJXTG3vsMq0OXFc&ithint=folder%2c"
```

The following is a list of the most important values that can be used in a mod.ini file:

### Main

**IncludeDir?** Specifies which folders will be included with your mod, allowing you to modify the default file/folder structure mentioned above. You can also set this to `../{Another Mod Name}` which should load files from another mod, But this is **NOT RECOMMEND**. (`?` is a Zero-based Number)

**IncludeDirCount** Specifies how many folders will be included with your mod.

**UpdateServer** A modification of the existing SonicGMI value that specifies the link to an XML file containing file infomations like its URL and name, I recommend linking a empty XML file or a folder in a http server, as it will allow you to release auto-downloading updates to your mods. The old GMI way of updating still works.

### Desc

**Title** The name of your mod as shown in the manager.

**Description** A description of your mod that is shown in the description window in the manager.
Typing a "\n" in this value will indicate a new line within the manager, **which should be done to keep your mods loadable!**

**Date** The date the mod was originally created as shown in the manager.

**Author** The author(s) of the mod. **You can include multiple authors in this value!** Simply seperate the authors via a space, followed by an ampersand, and another space. (Like this: "Radfordhound & Gotta Play Fast") They will be loaded as seperate authors within the manager, allowing you to link to them seperately.

**AuthorURL** The URL(s) to the author(s) of the mod. (Such as websites, YouTube channels, and social media accounts.) **You can include multiple authors in this value!** Simply seperate the authors' URLs via a space, followed by an ampersand, and another space. (Like this: "https://www.youtube.com/user/Radfordhound & https://www.youtube.com/channel/UCZfOGBkXRKICFozWU5bE0Xg") They will be loaded as seperate URLs within the manager and automatically linked with the data contained in the "Author" value, allowing you to link to them seperately.

**URL** The URL of the mod (aka mod homepages/threads/release videos).

There are many other values that can be used in a mod.ini file, many of which are already being used in several mods. So keep an eye out for them in other released mods!
