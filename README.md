# StickyWindows
A WPF library that lets you have WinAmp style windows snapping features.

## Usage
For any number of windows

```
_mainWindow = new MainWindow();
_secondaryWindow = new SecondaryWindow();
```

Use the StickyWindowsManager.Register method

```
StickyWindowsManager.Register(_mainWindow);
StickyWindowsManager.Register(_secondaryWindow);
```

That's all! Enjoy WinAmp style windows snapping.
