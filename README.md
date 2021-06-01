# Overview

PS4CheaterNeo is a program to find game cheat codes, and it is based on [`ps4debug`](https://github.com/jogolden/ps4debug) and `.Net Framework 4.8`.

Currently in `beta version 0.9.0.0`

## Table of Contents
- [Building](#building)
- [Description](#description)
  * [SendPayload](#sendpayload)
  * [ps4debug](#ps4debug)
  * [Cheat window](#cheat-window)
  * [Add Address](#add-address)
  * [Query window](#query-window)
  * [Group ScanType](#group-scantype)
  * [Hex Editor](#hex-editor)
  * [Pointer finder](#pointer-finder)
- [Reference](#reference)
https://ecotrust-canada.github.io/markdown-toc/

## Building

- Open `PS4CheaterNeo.sln` with Visual Studio([Community](https://visualstudio.microsoft.com/vs/community/) also available) and built with .Net Framework 4.8.


## Description

- User interface re-layout and design to `dark mode`.
- The `cheat` window and the `query` window are separated.
- `Hex Editor` can be opened from the `cheat` or `query` window.
- `Pointer finder` can be executed from the `cheat` or `query` lists.


### SendPayload

- Opening the `PS4CheaterNeo` program will automatically detect whether `ps4debug` is enabled.
- If not enabled, `SendPayload` will be executed to enable `ps4debug`.
- You must specify the ps4 connection `IP` in SendPayload.
- `SendPayload` requires the `ps4debug.bin` file that conforms to the `FW` version.

![sendpayload](assets/sendpayload.jpg)


### ps4debug

- You must manually copy `ps4debug.bin` to the `same path as PS4CheaterNeo.exe`\payloads\[FW version]\` directory.

> path\PS4CheaterNeo\bin\Debug\payloads\[FW version]\ps4debug.bin  
> path\PS4CheaterNeo\bin\Release\payloads\[FW version]\ps4debug.bin  

- It can be downloaded at the following URL(`Only ps4debug 6.72 has been tested`).

> [ps4debug 5.05](https://github.com/jogolden/ps4debug/releases)  
> [ps4debug 6.72](https://github.com/GiantPluto/ps4debug/releases)  
> [ps4debug 7.02](https://github.com/ChendoChap/ps4debug/tags)  
> [ps4debug 7.55](https://github.com/Joonie86/ps4debug/releases)  
> [ps4debug 9.00](https://www.reddit.com/r/ps4homebrew/comments/rimeyi/900fw_ported_ps4debug_and_webrte_payloads/)  


### Cheat window

- The cheat list can be loaded with cheats file, and the cheat value can be `edited` and `locked`.
- The cheat list has a group expandable/collapsable mechanism, and the cheat description with the same beginning will be set to the same group.
- You can add the address to the `Cheat List` from the `Query window` or `Hex Editor`, and can also be added manually.

![cheat_1](assets/cheat_1.jpg)
![cheat_2](assets/cheat_2.jpg)
![cheat_3](assets/cheat_3.jpg)
![cheat_4](assets/cheat_4.jpg)


### Add Address

- You can manually add addresses to the `Cheat List`.

![add](assets/add.jpg)


### Query window

- Opening the query window will automatically refresh processes list, if eboot.bin already exists it will be selected.
- Support query multiple targets, Multiple query windows can be opened at the same time.
- In addition to query types such as `Byte, Float, Double, Hex`, etc., it also supports `Group` types.
- Make the `section` of the suspected target more obvious.
- The query value will skip the filtered section list when the filter checkbox is clicked.
- The preset section filter rules is "`libSce, libc.prx, SceShell, SceLib, SceNp, SceVoice, SceFios, libkernel, SceVdec`", These rules can also be customized.

![query_1](assets/query_1.jpg)
![query_2](assets/query_2.jpg)
![query_3](assets/query_3.jpg)


### Group ScanType

- Use `group search` when you already know the data structure of the query target.
- Input format: [`ValueType`1:]`ValueNumber`1 [,] [`ValueType`2:]`ValueNumber`2 [,] [`ValueType`3:]`ValueNumber`3...
- The `ValueType` can be `1`(Byte), `2`(2 Bytes), `4`(4 Bytes), `8`(8 Bytes), `F`(Float), `D`(Double), `H`(Hex) or not specified.
- The `ValueType` is preset to 4 bytes when the value type is not specified.
- The `ValueNumber` can be specified as an asterisk(`*`) or question mark(`?`) when the value is unknown.
- The delimiter can be comma(`,`) or space(` `).

> Example:  
> Assuming the target structure is `63 00` `E7 03 00 00` `AB CD 00 00` `00 01`  
> Group scan can be entered as `2:99 999 ? 2:256`  


### Hex Editor

- Display the detailed information values of the address value of the current cursor position.
- Make address values greater than zero more obvious.
- You can add the address to the `Cheat List` from the current cursor position.

![hexeditor](assets/hexeditor.jpg)

### Pointer finder

- Make the `base address` of the pointer be in the `executable section` when `FastScan` is clicked.
- If there is no result, you can try to click `NegativeOffset`.
- The finder will skip the filtered section list when the `filter` checkbox is clicked.
- The preset section filter rules is "`libSce, libc.prx, SceShell, SceLib, SceNp, SceVoice, SceFios, libkernel, SceVdec`", These rules can also be customized.


![pointerfinder_1](assets/pointerfinder_1.jpg)
![pointerfinder_2](assets/pointerfinder_2.jpg)


## Reference

[ps4debug](https://github.com/jogolden/ps4debug)
[PS4_Cheater](https://github.com/hurrican6/PS4_Cheater)
