# ColorLines
 
Classic [Color Lines](https://ru.wikipedia.org/wiki/Color_Lines) implemented with WPF technology.

The architecture of application is based on implementation MVVM pattern.

Thus, classes witch contains basic game logic (that are in the Model folder) can be used on any other platform. (`GameController.cs` class acts as a global helper)

The ViewModels folder contains classes that duplicate properties of the main classes of the model and change them by Commands.

Game View fully contains in the `MainWindow.xaml` file.

>Features of the game

- The game board is generated dynamically depending on board_size in settings
- Also game settings allow users to change the number of randomly balls dropped at one time and the count of balls needed to collect the "color line"
- Atomized screen resizing
- Ability to save/continue game
- Game records and a current score are determined for each board size separately
