# Valheim Backup Manager
Repository for VBM, a CLI tool designed to stop Steam Cloud from screwing me over.

# Installation
All you have to do to install the program is download and extract one of the release packages. You'll have to go through some steps every time you want to use the program, though. Take note of where you extracted the files because you'll need that every time you want to use VBM.

# Running VBM
1. Open your OS's terminal
    * Windows: Open the command prompt by searching "command prompt" on the task bar
    * Linux: You should know how to open the terminal. If you don't, google it.
2. Once the command prompt opens, type this: `cd "VBM-install-directory"` and press enter. (Replace VBM-install-directory with the path to where you extracted the program)
3. Now you're free to use all the VBM commands until you close the prompt window. For now, start by typing "VBM" and pressing enter. This will display the help page and teach you how to use VBM.

If you want to be able to skip step 2 every time, follow these instructions:

### Windows

### Linux (TODO)
1. Open the terminal
2. Type this and press enter: `vim ~/.bashrc`
3. Scroll to the bottom of the file and put this at the end

# Uninstalling
If you want to delete VBM for whatever reason, all you have to do is delete its containing folder. VBM doesn't modify the Windows registry or put configuration files in weird places, so there's no special cleanup that needs to be done if you want to delete it. If you want, you can also delete the backups directory:
- On Windows, it's located at %USERPROFILE%\AppData\LocalLow\ValheimBackups
- On Linux, you'll find it at ~/ValheimBackups
