# File patterns
This doc will document the patterns of each of the files required to translate the game **text** files, theres also a bunch of files with scripts that allow you to see the pattern whith [ImHex](https://github.com/WerWolv/ImHex). This patterns have only been tested with the english release of the game.

## SCEDIC.eng
| Data type| Given Name | Description |
|----------|----------|----------|
| `u32`   | `numEntries`    | Number of text entries - 1    |
| `u32*[numEntries + 1]`    | `pointers`    | Absolute offset pointers to the strings    |
| `string[numEntries + 1]`    | `texts`    | Strings containing the text    |

## itemdata.eng
| Data type| Given Name | Description |
|----------|----------|----------|
|`u32`|`numEntries`|Number of item entries (Name + Description)|
|`u32`|`textSize`|Size in bytes of `texts`|
|`u16*[numEntries * 2]`|`pointers`|Relative offset pointers from the start of `texts` to the strings|
| `string[numEntries * 2]`|`texts`|Strings containing the text|

## DataDictionary.eng
| Data type| Given Name | Description |
|----------|----------|----------|
|`u32`|`numEntries`|Number of dictionary entries / 11 |
|`u32*[numEntries * 11 + 1]`|`pointers`|Absolute offset pointers to the strings|
| `string[numEntries * 11 + 1]`|`texts`|Strings containing the text. The first `string` of each dictionary entry is a fixed size `string` (`62 bytes`)|

## monsdata_e.bin
| Data type| Given Name | Description |
|----------|----------|----------|
|`u32`|`numEntries`|Number of monster entries |
|`padding[4]`|`-`||
|`monsterEntry[numEntries]`|`monsters`| Monster data info|
### `monsterEntry`
| Data type| Given Name | Description |
|----------|----------|----------|
|`u16`|`entryId`| Id of this monster entry |
|`u16`|`entryId2`| Duplicate of this monster Id |
|`byte[0x120]`|`metadata`| Metadata info |
|`string`|`name`| Name of the monster, fixed size `string` (`0x18 bytes`)|

## UIMess.bin
Do not confuse with `UIMess.arc`.
| Data type| Given Name | Description |
|----------|----------|----------|
|`u16*[288]`|`pointers`|Absolute `offset / 2` pointers to the strings|
|`byte[]`|`metadata`||
|`string[288]`|`text`|Strings containing the text|

## arm9.bin
`ar.9.bin` pointers always have an offset based on ram positions, and `arm9.bin` is always loaded on ram position `0x02000000`, meaning that when analyzing the file by its own all pointers must subtract said value to get the real position they point to.
| Data type| Offset |
|----------|----------|
|`BattleBlock`|`0x0ED054`|
|`StatusBlock`|`0x0ED25C`|
|`OutskirtsBlock`|`0x0F12D0`|
|`LocationsBlock`|`0x0F76BC`|
|`AbilitiesBlock`|`0x0113644`|
|`SpellsBlock`|`0x0113D9C`|
|`EnemySkillsBlock`|`0x01152DC`|
|`PlayerSkillsBlock`|`0x0116B94`|
|`BattleDialogsBlock`|`0x011A1B0`|

# `BattleBlock`
| Data type| Given Name | Description |
|----------|----------|----------|
|`BattleEntry[58]`|`entries`||
# `BattleEntry`
| Data type| Given Name | Description |
|----------|----------|----------|
|`byte[4]`|`metadata`||
|`u32*`|`pointer`|Absolute offset pointers to the strings|

# `StatusBlock`
| Data type| Given Name | Description |
|----------|----------|----------|
|`StatusEntry[47]`|`entries`||
# `StatusEntry`
| Data type| Given Name | Description |
|----------|----------|----------|
|`byte[6]`|`metadata`||
|`string`|`text`|Fixed size `0x18` string|

# `OutskirtsBlock`
| Data type| Given Name | Description |
|----------|----------|----------|
|`OutskirtsEntry[19]`|`entries`||
# `OutskirtsEntry`
| Data type| Given Name | Description |
|----------|----------|----------|
|`byte[4]`|`metadata`||
|`string`|`text`|Fixed size `0x20` string|

# `LocationsBlock`
| Data type| Given Name | Description |
|----------|----------|----------|
|`LocationsEntry[275]`|`entries`||
# `LocationsEntry`
| Data type| Given Name | Description |
|----------|----------|----------|
|`byte[0x4]`|`metadata`||
|`string`|`text`|Fixed size `0x20` string|

# `AbilitiesBlock`
| Data type| Given Name | Description |
|----------|----------|----------|
|`AbilitiesEntry[47]`|`entries`||
# `AbilitiesEntry`
| Data type| Given Name | Description |
|----------|----------|----------|
|`byte[0x10]`|`metadata`||
|`string`|`name`|Name of the ability. Fixed size `0x14` string|
|`u32*`|`descriptionPointer`|Pointer to the description of the ability|

# `SpellsBlock`
| Data type| Given Name | Description |
|----------|----------|----------|
|`SpellsEntry[80]`|`entries`||
# `SpellsEntry`
| Data type| Given Name | Description |
|----------|----------|----------|
|`string`|`name`|Name of the spell. Fixed size `0x10` string|
|`byte[0x30]`|`metadata`||
|`u32*`|`descriptionPointer`|Pointer to the description of the spell|

# `EnemySkillsBlock`
| Data type| Given Name | Description |
|----------|----------|----------|
|`EnemySkillEntry[113]`|`entries`||
# `EnemySkillEntry`
| Data type| Given Name | Description |
|----------|----------|----------|
|`string`|`name`|Name of the skill. Fixed size `0x10` string|
|`byte[0x24]`|`metadata`||
|`u32*`|`descriptionPointer`|Pointer to the description of the skill (ignored due to not being used ingame)|

# `PlayerSkillsBlock`
| Data type| Given Name | Description |
|----------|----------|----------|
|`PlayerSkillEntry[173]`|`entries`||
# `PlayerSkillEntry`
| Data type| Given Name | Description |
|----------|----------|----------|
|`byte[0x14]`|`metadata`||
|`string`|`name`|Name of the skill. Fixed size `0x14` string|
|`u32*`|`descriptionPointer`|Pointer to the description of the skill|
|`byte[0x24]`|`metadata`||

# `BattleDialogsBlock`
| Data type| Given Name | Description |
|----------|----------|----------|
|`u32*[931]`|`pointers`|Pointer to the text|

## \*.arc (Version 3)
| Data type| Given Name | Description |
|----------|----------|----------|
|`string`|`magic`|Fixed size `0x04` string|
|`u32`|`headerSize`|Size in bytes of the header|
|`u16`|`version`|Version of the .arc format|
|`u16`|`numFiles`|Number of files in the container|
|`u16`|`numFilePointers`|Number of `FileData` in the container|
|`padding[2]`|`-`||
|`string`|`magic2`|Fixed size `0x10` string|
|`FileData[numFilePointers]`|`fileData`||
|`Files[numFiles]`|`files`||

# `FileData`
| Data type| Given Name | Description |
|----------|----------|----------|
|`u32`|`fileOffset`|Absolute offset of the file, can be `0x00` (no file)|
|`u32`|`fileSize`|Size of the file, can be `0x00` (no file)|
