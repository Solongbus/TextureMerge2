# TextureMerge 2

[![GitHub latest release version](https://img.shields.io/github/v/release/Solongbus/TextureMerge2.svg?style=flat)](https://github.com/Solongbus/TextureMerge2/releases/latest)
[![Github All Releases download count](https://img.shields.io/github/downloads/Solongbus/TextureMerge2/total.svg?style=flat)](https://github.com/Solongbus/TextureMerge2/releases/latest)

[![Main](https://github.com/Solongbus/TextureMerge2/actions/workflows/main.yml/badge.svg)](https://github.com/Solongbus/TextureMerge2/actions/workflows/main.yml)
[![CodeQL](https://github.com/Solongbus/TextureMerge2/actions/workflows/codeql.yml/badge.svg)](https://github.com/Solongbus/TextureMerge2/actions/workflows/codeql.yml)

This project is based on the original source: [fidifis/TextureMerge](https://github.com/fidifis/TextureMerge)

### New Features in TextureMerge 2:
- **Project Configuration**: Save and load your project settings as `.tmproj` files for quick access.
- **CLI Mode**: Fully functional command-line interface for automated workflows.
- **Copy CLI Command**: Easily copy the command for your current project setup to the clipboard.

## Software to merge or pack textures into image channels, producing one image with up to four textures.

**TextureMerge 2** is ideal if you want to pack individual grayscale textures in one image.

![image](https://github.com/Solongbus/TextureMerge2/raw/master/Tutorial-images/img1.jpg)

## Requirements
To run this program you need [.NET Framework 4.8](https://dotnet.microsoft.com/en-us/download/dotnet-framework/net48) (installed by default on most windows computers)

## Usage
1. Download the software from [releases](https://github.com/Solongbus/TextureMerge2/releases) \
*Install it using setup or extract the zip folder*
2. Open `TextureMerge.exe`
3. Click **Load** under the color channel into which you want to insert texture.\
**You can also *drag and drop* files into the slots.**

![image](https://github.com/Solongbus/TextureMerge2/raw/master/Tutorial-images/img2.jpg)

4. The Windows dialog appears. Select the texture and click open.

![image](https://github.com/Solongbus/TextureMerge2/raw/master/Tutorial-images/img3.jpg)

5. Repeat for other textures. Empty channels will be black or you can change it to white (the alpha channel will not be added if no image provided).
6. When you are done click **Merge**.

![image](https://github.com/Solongbus/TextureMerge2/raw/master/Tutorial-images/img4.jpg)

7. The Windows dialog appears. Enter a file name and click save.

![image](https://github.com/Solongbus/TextureMerge2/raw/master/Tutorial-images/img5.jpg)

## Command Line Interface (CLI)
You can use **TextureMerge 2** from the command line for automation.

### Arguments:
- `-r, --red <path>`: Path to the texture for the **Red** channel.
- `-g, --green <path>`: Path to the texture for the **Green** channel.
- `-b, --blue <path>`: Path to the texture for the **Blue** channel.
- `-a, --alpha <path>`: Path to the texture for the **Alpha** channel.
- `-o, --output <path>`: Path to the **Output** image file (**Required**).
- `-c, --color <hex>`: Default color for empty channels (default: `#000000`).
- `-d, --depth <bits>`: Bit depth for the output image.

### Example:
```bash
TextureMerge.exe -r "base.png" -g "roughness.png" -b "metalness.png" -o "packed.png" -c "#FFFFFF"
```

## Example of use in Unreal Engine
![image](https://github.com/Solongbus/TextureMerge2/raw/master/Tutorial-images/img7.jpg)

## Questions
If you have any questions, suggestions or something don't work, create [issue](https://github.com/Solongbus/TextureMerge2/issues).
