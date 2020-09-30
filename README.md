# Orpheus

## Starting the project

1. In Microsoft Visual Studio 2017 or newer, open the file `Orpheus.sln`.

2. To configure whether or not you will create a Debug build or a Release build click the dropdown labeled `Debug` and select your requisite option.

3. To build the project, go to Build -> Build Solution, or press F7.

4. To run your build, you can go to Debug -> Start Debugging, or press F5. The application will start automatically.

5. If you want to run the application outside of Visual Studio, you can open your project directory, by right-clicking `Orpheus` in the Solution Explorer in the right pane of Visual Studio, and then click Open Folder in File Explorer. From here, you can find your application's build in the `bin/Debug` or `bin/Release` folders. Your produced application executable will be in this directory. If you copy this folder somewhere else on your computer, you can run this build independently from Visual Studio. The only requirement is building the solution before attempting to copy the folder to a different directory.

## Feature Development Timeline

### Feature 1

#### Added

- Added project template.

- Added dependencies, including `FontAwesome.WPF`, `Microsoft.Xaml.Behaviors.Wpf`, `ModernWpfUI`, `NAudio`, and `Newtonsoft.Json`.

- Added `MainWindow.xaml` layout.

- Added stub functions to the file menu.

- Added hard-coded fake data for layouting purposes.

#### Changed

- Changed README.md file to reflect these changes.