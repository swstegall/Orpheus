# Orpheus

## Starting the project

1. In Microsoft Visual Studio 2017 or newer, open the file `Orpheus.sln`.

2. To configure whether or not you will create a Debug build or a Release build click the dropdown labeled `Debug` and select your requisite option.

3. To build the project, go to Build -> Build Solution, or press F7. Visual Studio should attempt to pull the project dependencies for this application automatically. If this does not happen, then you can go into the application's file manually and run `dotnet restore` to restore application dependencies.

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

### Feature 2

#### Added

- JSON schema file: music_storage_schema.json

-JSON file music_storage.json

-Class SongLocation to store the title and path of a SongLocation. This is used when deserializing music_storage.json and reflects a json object.

-Class SongList to store a list of song. This is used when deserializing music_storage.json and is the root level of the file.

-Class JSONHandler which will handle alll interactions with music_storage.json. The method in it is ReadJsonFile() which reads music_storage.json is found and returns the list of songs in a SongList object.

#### Changed

-Creation of a JSONHandler object in MainWindow.xaml.cs MainWindow() method and a call to ReadJsonFile() to retrieve the stored songs.

- Changed README.md file to reflect these additions and changes.