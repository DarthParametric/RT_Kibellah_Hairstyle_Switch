# Kibellah Hairstyle Switch
A mod for Owlcat's Rogue Trader cRPG. Adds an option to swap the bowl cut for something less terrible.

## Overview
This mod adds a selection of alternative hairstyles for everyone's favourite goth girlfriend. Inspired by a Reddit meme by [TequilaBaugette51](https://old.reddit.com/r/RogueTraderCRPG/comments/1p43nbf/all_that_cutting_and_kibellah_cant_get_a_decent), reproduced here:

<p align="center"><img src="img/Lets_Keep_This_On.jpg?raw=true" width="512" height="512"/></p>

## Installation
This is an Owlmod, made using the Unity template supplied by Owlcat. In order to properly load custom assets, you ***must*** install the Unity Mod Manager-based mod [MicroPatches](https://github.com/microsoftenator2022/MicroPatches/releases) by microsoftenator2022. You can install it manually or via [ModFinder RT](https://www.nexusmods.com/warhammer40kroguetrader/mods/146).

Use [ModFinder RT](https://www.nexusmods.com/warhammer40kroguetrader/mods/146) to install this mod automagically.

Alternatively, to install the mod manually, first make sure you have run the game at least once. Download the mod from the [releases section](https://github.com/DarthParametric/RT_Kibellah_Hairstyle_Switch/releases/latest) or [Nexus](https://www.nexusmods.com/warhammer40kroguetrader/mods/XXXXXXX) and extract it into:

`%userprofile%\AppData\LocalLow\Owlcat Games\Warhammer 40000 Rogue Trader\Modifications\`

Each Owlmod needs to be in its own sub-folder in the Modifications folder.

Afterwards, you need to edit OwlcatModificationManagerSettings.json in the base Warhammer 40000 Rogue Trader folder in a text editor (Notepad++ recommended). It should look something like this:

```
{
"$id": "1",
"SourceDirectories": [],
"EnabledModifications": ["DPKibblesHairSwitch"],
"ActiveModifications": ["DPKibblesHairSwitch"],
"DisabledModifications": []
}
```

If you have other mods, list them in quotes separated by commas. For example:

```
"EnabledModifications": ["DPKibblesHairSwitch", "ModName2", "ModName3"],
"ActiveModifications": ["DPKibblesHairSwitch", "ModName2", "ModName3"],
```

You can move individual mods from the ActiveModifications section to the DisabledModifications section if you want to disable them without physically removing them.

## User Settings
You can configure the mod's settings via the Owlcat Mod Manager (Shift-F10 by default). Click on the Settings button next to the mod's name in the list, which will open its settings page. Here you can pick which of the available hairstyles you want to swap to. 

Your settings will be saved in a `DPKibblesHairSwitch_Data.json` file in the Modifications folder. The settings apply universally across all saves.

## Known Issues
The mod works when using a controller, but the in-game settings window is not available for the controller layout.

Some of the options may still look a little brownish in colour rather than black like Kibellah's vanilla hair colour. I may tweak this in future.

## Acknowledgements
Many thanks to the modders on the Owlcat Discord, but particularly microsoftenator2022, Kurufinve, and ADDB for helping to coach me through my ineptitude in order to get the mod working.

