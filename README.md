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
I'm currently working on the look of the filters, as I think they look a bit illogical and don't quite fit Terraria (though my version probably does too). I use post draw to draw the category over the lab filter image (funnel) or, in case of rarities, color the filter in its color (if the rarity changes color, then the filter too). But this method has a drawback: in the weapon rack or in the Item Browser in Hero's mod, the filters looked the same (in the Cheat Sheet everything is fine with this). To somehow solve this problem, I left all the old textures and moved them to the Items/OldFilters folder. Here is an image showing how filters look in the inventory and in the Hero's Mod item browser:

(I also added a filter for Fiery Red rarity, I also translated it into all 4 languages, but I don't speak Chinese and Italian, so I used a translator (I don't know if it's right). And these textures on top of the filter are items, for the categories "Equipment", "Tools" and "Weapons" I created separate items so as not to build unnecessary systems, they cannot be obtained honestly and they are worthless)

![Screenshot of a comment on a GitHub issue showing an image, added in the Markdown, of an Octocat smiling and raising a tentacle.](https://github.com/VoidDefi/MechTransfer/blob/master/screen1.png)
