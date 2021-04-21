# Valheim Backup Manager
Repository for VBM, a CLI tool designed to stop Steam Cloud from screwing me over.

# Installation
All you have to do to install the program is download and extract one of the release packages. Take note of where you extracted the files because you'll need that in a minute, so try to make it someplace simple. Linux users have an extra step, though: you'll need to allow the downloaded program to be executed, because Linux won't allow it to run by default. There are two ways you can do this, depending on your preference: with the terminal or with the Files app. Both of these methods are explained below.

#### Terminal
1. Open the terminal and run this command: `cd "VBM-install-directory"`
2. Run this command: `chmod +x VBM`
3. You're good to go and can close the terminal now

#### Files app
1. Open the files app and navigate to where you installed VBM
2. Right-click on the file called "VBM", then select Properties
3. Select the Permissions tab, and check the box labeled "Allow this file to run like a program" (or something similar)
4. You're all set and you can close everything you just opened

# Running VBM
1. Open your OS's command line interface:
    * Windows: Open the command prompt by searching "command prompt" on the task bar
    * Linux: As a Linux user, you should know how to open the terminal. If you don't, google it.
2. Once the CLI opens, type this: `cd "VBM-install-directory"` and press enter. (Replace VBM-install-directory with the path to where you extracted the program)
3. Now you're free to use all the VBM commands until you close the prompt window. For now, start by typing "VBM" and pressing enter. This will display the help page and teach you how to use VBM. Linux users, please note that you'll need to type "./VBM" every time unless you follow the instructions below.

If you want to be able to skip step 2 every time, follow these instructions:

#### Windows
1. Search "environment variables" in the taskbar and select "Edit the system environment variables"
2. Click the button at the bottom of the window labeled "Environment Variables"
3. Select the "Path" environment variable from the list labeled "User variables for `your-username`". If for whatever reason there isn't already an environment variable named Path, click "New..." to create one and set its value to the place where you extracted VBM. Once you click OK to close all the screens you just opened, you're all set and can stop reading the instructions now.
4. After you've selected the "Path" user environment variable, click Edit.
5. Click New and type in the name of the directory where you extracted VBM
6. Click OK to close all the menus, and you're done! Now you can access VBM commands from any command prompt.

#### Linux
1. Open the terminal
2. Type this and press enter: `vim ~/.bashrc` This opens the bash configuration file with Vim, a terminal text editor.
3. Press the "i" key. This allows you to start changing the contents of the file.
4. Scroll to the bottom of the file and put this at the end
```bash
myDirectory="PUT YOUR EXTRACT DIRECTORY HERE INSIDE THE QUOTES"

if [ -d "$myDirectory" ]; then
   export PATH=$PATH:$myDirectory
fi
```
Essentially, this code adds the install directory to your PATH.

5. Save the file by pressing escape, then type this exactly (without the quotes): ":wq"

# Uninstalling
If you want to delete VBM for whatever reason, all you have to do is delete its containing folder. VBM doesn't modify the Windows registry or put configuration files in weird places, so there's no special cleanup that needs to be done if you want to delete it. If you want, you can also delete the backups directory:
- On Windows, it's located at %USERPROFILE%\AppData\LocalLow\ValheimBackups
- On Linux, you'll find it at ~/.ValheimBackups
