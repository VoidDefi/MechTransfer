# MechTransfer

MechTransfer is a small mod, that adds various wire controlled devices to interact with items and move them between containers.

The mod contains the following features:
- Transferring items from one chest to another
- Dropping and picking up items
- Item sorting and detection
- Automatic crafting

For more information visit the Terraria Community Forums: [MechTransfer - Item translocation and more...](https://forums.terraria.org/index.php?threads/mechtransfer-item-translocation-and-more.60571/)

# Cross Mod Support

MechTransfer supports all standard chests from any mod. If your mod has a custom item storage solution, you can use MechTransfer's Mod.Call() based API to set up an adapter for it.
The complete API documentation is available [here](https://github.com/DRKV333/MechTransfer/wiki/Adapter-extensibility).

Currently supported mods:
- [Portable Storage](https://forums.terraria.org/index.php?threads/portable-storage.65161/)
- [Magic Storage](https://forums.terraria.org/index.php?threads/magic-storage.56294/) (Support built into MechTransfer)

# Special Thanks

- [@Flithor](https://github.com/Flithor) for the Chinese translation
- [@Vedomir](https://forums.terraria.org/index.php?members/vedomir.130490/) for the Russian translation
- [@RonRicoSuave](https://github.com/RonRicoSuave) for help with spriting
- [@speeder](https://github.com/speeder) for the TModLoader 0.11.5 port
- [@yut23](https://github.com/yut23) for the Assembler bugfix
- [@Ahbahl](https://github.com/Ahbahl) for help with MagicStorageExtra support
- [@MistressNebula](https://github.com/435THz) and [@Kadeanon](https://github.com/Kadeanon) for the Terraria 1.4 port

# About this fork
At the moment i'm working on the appearance of the filters, because in my opinion they look a bit illogical and not quite suitable for Terraria (although my version, probably, too). I use post drawing to draw the category on top of one image of the lab filter. However, this method has a number of disadvantages: the category is not displayed in the Heros mod Item Browser, but in the Cheat panel everything is fine, as well as in the weapon rack and when hovering over the frame with the filter. So far nothing is resolved if you drop an item on the ground, but this is relatively easy to fix. 

![Screenshot of a comment on a GitHub issue showing an image, added in the Markdown, of an Octocat smiling and raising a tentacle.](https://github.com/VoidDefi/MechTransfer/blob/master/screen1.png)
