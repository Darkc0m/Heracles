# File patterns
This doc will document the patterns of each of the files required to translate the game **text** files, theres also a [folder](https://github.com/Darkc0m/Heracles/tree/Docs/Docs/ImHex%20Scripts) with scripts that allow you to see the pattern whith [ImHex](https://github.com/WerWolv/ImHex). This patterns have only been tested with the english release of the game.

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
