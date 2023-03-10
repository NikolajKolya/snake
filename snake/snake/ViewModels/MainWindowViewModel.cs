using Avalonia;
using Avalonia.Input;
using ReactiveUI;
using snake.Abstract;
using snake.GameLogic.Abstract;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using System.Timers;

namespace snake.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly IGameLogic _gameLogic;

        private Dictionary<int, SquareState> _rowColumn = new Dictionary<int, SquareState>();

        private Timer _stepsTimer;

        public Dictionary<int, SquareState> RowColumn
        {
            get => _rowColumn;
            set => this.RaiseAndSetIfChanged(ref _rowColumn, value);
        }

        public MainWindowViewModel()
        {
            _gameLogic = Program.Di.GetService<IGameLogic>();

            ClearGameFiled();

            RowColumn[2 * Constants.Constants.GameFieldSize + 3] = SquareState.Aplle;

            // Starting timer
            _stepsTimer = new Timer(1000);
            _stepsTimer.AutoReset = true;
            _stepsTimer.Enabled = true;
            _stepsTimer.Elapsed += OnStepsTimer;
        }

        private void ClearGameFiled()
        {
            for (int y = 0; y < Constants.Constants.GameFieldSize; y++)
            {
                for (int x = 0; x < Constants.Constants.GameFieldSize; x++)
                {
                    RowColumn[y * Constants.Constants.GameFieldSize + x] = SquareState.Nothing;
                }
            }
        }

        private void OnStepsTimer(object? sender, ElapsedEventArgs e)
        {
            _gameLogic.NextStep();

            ClearGameFiled();

            var snakeSquares = _gameLogic
                .GetSnakeSquares();

            foreach (var snakeSquare in snakeSquares)
            {
                RowColumn[snakeSquare.Key] = snakeSquare.Value;
            }

            RowColumn[2 * Constants.Constants.GameFieldSize + 3] = SquareState.Aplle;

            RowColumn = new Dictionary<int, SquareState>(RowColumn);
        }

        public void OnKeyPress(KeyEventArgs keyEventArgs)
        {
            _gameLogic.OnKeyPress(keyEventArgs.Key);
        }
    }
}
