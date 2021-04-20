# Valheim Backup Manager
Repository for VBM, a CLI tool designed to stop Steam Cloud from screwing me over.

# Installation
All you have to do to install the program is download and extract one of the release packages. You'll have to go through some steps every time you want to use the program, though. Take note of where you extracted the files because you'll need that every time you want to use VBM, so try to make it someplace simple.

# Running VBM
1. Open your OS's command line interface:
    * Windows: Open the command prompt by searching "command prompt" on the task bar
    * Linux: As a Linux user, you should know how to open the terminal. If you don't, google it.
2. Once the CLI opens, type this: `cd "VBM-install-directory"` and press enter. (Replace VBM-install-directory with the path to where you extracted the program)
3. Now you're free to use all the VBM commands until you close the prompt window. For now, start by typing "VBM" and pressing enter. This will display the help page and teach you how to use VBM.

If you want to be able to skip step 2 every time, follow these instructions:

### Windows
1. Search "environment variables" in the taskbar and select "Edit the system environment variables"
2. Click the button at the bottom of the window labeled "Environment Variables"
3. Select the "Path" environment variable from the list labeled "User variables for `your-username`". If for whatever reason there isn't already an environment variable named Path, click "New..." to create one and set its value to the place where you extracted VBM. Once you click OK to close all the screens you just opened, you're all set and can stop reading the instructions now.
4. After you've selected the "Path" user environment variable, click Edit.
5. Click New and type in the name of the directory where you extracted VBM
6. Click OK to close all the menus, and you're done! Now you can access VBM commands from any command prompt.

### Linux
1. Open the terminal
2. Type this and press enter: `vim ~/.bashrc`
3. Press the "i" key
4. Scroll to the bottom of the file and put this at the end
```bash
myDirectory="PUT YOUR EXTRACT DIRECTORY HERE INSIDE THE QUOTES"

if [ -d "$myDirectory" ]; then
   export PATH=$PATH:$myDirectory
fi
```
5. Save the file by pressing escape, then type this exactly: ":wq"

# Uninstalling
If you want to delete VBM for whatever reason, all you have to do is delete its containing folder. VBM doesn't modify the Windows registry or put configuration files in weird places, so there's no special cleanup that needs to be done if you want to delete it. If you want, you can also delete the backups directory:
- On Windows, it's located at %USERPROFILE%\AppData\LocalLow\ValheimBackups
- On Linux, you'll find it at ~/.ValheimBackups
