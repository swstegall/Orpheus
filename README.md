# Orpheus

## Starting the project

1. In Microsoft Visual Studio 2017 or newer, open the file `Orpheus.sln`.

2. To configure whether or not you will create a Debug build or a Release build click the dropdown labeled `Debug` and select your requisite option.

3. To build the project, go to Build -> Build Solution, or press `F7`. Visual Studio should attempt to pull the project dependencies for this application automatically. If this does not happen, then you can go into the application's file manually and run `dotnet restore` to restore application dependencies.

4. To run your build, you can go to Debug -> Start Debugging, or press `F5`. The application will start automatically.

5. If you want to run the application outside of Visual Studio, you can open your project directory, by right-clicking `Orpheus` in the Solution Explorer in the right pane of Visual Studio, and then click Open Folder in File Explorer. From here, you can find your application's build in the `bin/Debug` or `bin/Release` folders. Your produced application executable will be in this directory. If you copy this folder somewhere else on your computer, you can run this build independently from Visual Studio. The only requirement is building the solution before attempting to copy the folder to a different directory.

## Feature Development Timeline

### SWE I

#### Feature 1

##### Added

- Added project template.

- Added dependencies, including `FontAwesome.WPF`, `Microsoft.Xaml.Behaviors.Wpf`, `ModernWpfUI`, `NAudio`, and `Newtonsoft.Json`.

- Added `MainWindow.xaml` layout.

- Added stub functions to the file menu.

- Added hard-coded fake data for layouting purposes.

##### Changed

- Changed README.md file to reflect these changes.

#### Feature 2

##### Added

- JSON schema file: `music_storage_schema.json`

- JSON file `music_storage.json`

- Class `SongLocation` to store the title and path of a `SongLocation`. This is used when deserializing `music_storage.json` and reflects a JSON object.

- Class `SongList` to store a list of song. This is used when deserializing `music_storage.json` and is the root level of the file.

- Class `JSONHandler` which will handle all interactions with `music_storage.json`. The method in it is `ReadJsonFile()` which reads `music_storage.json` is found and returns the list of songs in a `SongList` object.

##### Changed

- Creation of a `JSONHandler` object in `MainWindow.xaml.cs` `MainWindow()` method and a call to `ReadJsonFile()` to retrieve the stored songs.

- Changed `README.md` file to reflect these additions and changes.

#### Feature 3

##### Added

- Added file menu callback to process JSON.

- Added file items to the datagrid.

##### Changed

- JSON schema to reflect the Error object.

#### Feature 4

##### Added

- Added Core NAudio functionality.

- Added music playing on double-click event.

##### Removed

- Removed Add Folder to Library.

#### Feature 5 (1.0)

##### Added

- Added metadata parsing.

- Added Play and Stop button functionality.

##### Changed

- Datagrid column order.

- Updated `README.md` to reflect these changes.

##### Removed

- Removed incomplete player controls.

### SWE II

#### Milestone 1

##### Added

- Add folder to library (#10).

- Resizable player up to full screen (#11).

- Add the icon for the application (#12).

- Setup a right-click context menu (#13).

##### Changed

- Update `README.md` (#26).

- Add File and Add Folder buttons are broken (#27).

#### Milestone 2

##### Added

- File menu option to select a theme (#16).

- Prune invalid files from JSON (#14).

##### Changed

- Fix broken song by remapping its location with right-click context menu action (#15).

- Fix Shuffle (#17).

- Fix the Seekbar (#18).

- Extract Playlist Refresh to its own function (#37).

- Update `README.md` (#39).

#### Milestone 3

##### Added

- Eradicate duplicates from the JSON and frontend (#19).

- Double-click guard for broken song JSON (#20).

- Setup proper theme initialization (#21).

##### Changed

- Fix Fast-Forward (#22).

#### Milestone 4

##### Added

- Save theme choice in JSON (#23).

- Custom User-Created Themes (#24).

##### Changed

- Fix Row Item Styling (#25).
