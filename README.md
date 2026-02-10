# Inventory Combat Alert 🎒⚔️

**Stop missing loot because your bags are full!**

Inventory Combat Alert is a lightweight plugin for Final Fantasy XIV that automatically checks your inventory space every time you enter combat. If you're running low on space (or completely full), it warns you immediately so you can clear space *before* the boss dies.

## ✨ Features

* **Automatic Scanning:** Checks your 4 main inventory bags instantly upon entering combat.
* **Visual Alerts:**
    * **CRITICAL:** If bags are 100% full, you get a large error on screen and an "New Duty Unlocked" sound.
    * **WARNING:** If bags are low (e.g., < 3 slots), you get a gentle "Quest Complete" toast and sound.
* **Customizable Threshold:** You decide when to be warned (1 to 10 slots left).
* **Sound Options:** Choose between "Happy Ding," "Angry Buzz," "Silent," or your own **Custom Sound File**.
* **Chat Log Integration:** Prints a clear warning in your chat log so you don't miss it.

## 📥 Installation

1.  Open the **Dalamud Settings** (type `/xlsettings` in game).
2.  Go to the **Experimental** tab.
3.  Paste the following URL into the **Custom Plugin Repositories** section:
    ```
    https://raw.githubusercontent.com/UnbirthdayHatter/Inventory-Combat-Alert/main/pluginmaster.json
    ```
4.  Click the **+** button to add the repo.
5.  Click **Save and Close**.
6.  Open the **Plugin Installer** (type `/xlplugins`).
7.  Search for **Inventory Combat Alert** and click **Install**.

## ⚙️ Usage

The plugin works automatically in the background.

* To open the settings menu, type:
    ```
    /alertconfig
    ```

### 🎵 How to use Custom Sounds

1.  Open the settings menu (`/alertconfig`).
2.  Select **"Custom: alert.wav"** from the dropdown menu.
3.  Place a `.wav` file named exactly `alert.wav` into the plugin's installation folder.
    * *You can find this folder path listed at the bottom of the settings window.**
    * *There is already a sound that comes with, but you can change it to whatever you like!*
4.  Click **Test Sound** to verify it works!

---

**Created by UnbirthdayHatter**
