/*
=========================================================

DeadGalaxy project

All right reserved
Kabanov Kirill (Kiriller12) © 2024

Licensed under GNU General Public License v3.0
Full license text available in LICENCE file

=========================================================
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DeadGalaxy.Core.Helpers;
using Raylib_cs;

namespace DeadGalaxy.Core
{
    /// <summary>
    /// Debug console
    /// </summary>
    internal class Console
    {
        private static readonly Color Background = new(0, 0, 0, 200);
        private static readonly Color Input = new(50, 50, 50, 200);
        private static readonly Color InputHover = new(75, 75, 75, 200);
        private static readonly Color InputActive = new(100, 100, 100, 200);

        private readonly List<string> _output = [];
        private readonly List<string> _history = [];
        private int _historyIndex;

        private bool _isInputActive;
        private bool _isInputHover;

        private float _timer = 1.0f;
        private bool _isTimerTick;

        private string _inputText = string.Empty;

        /// <summary>
        /// Debug console
        /// </summary>
        private Console()
        {
            DumpLines([
                string.Empty,
                "=====================================================================",
                $" DeadGalaxy - logs - {DateTime.Now:G}",
                "====================================================================="
            ]);
            
            RaylibHelpers.SetTraceLogCallback(LogMessage);
        }

        /// <summary>
        /// Debug console instance
        /// </summary>
        public static Console? Instance { get; private set; }

        /// <summary>
        /// Is debug console displayed on screen
        /// </summary>
        public bool Shown { get; private set; }

        /// <summary>
        /// Initializes debug console
        /// </summary>
        public static void Init()
        {
            if (Instance != null)
            {
                Raylib.TraceLog(TraceLogLevel.Warning, "[Console]: Couldn't create debug console because it is already created!");

                return;
            }

            Instance = new Console();
        }

        /// <summary>
        /// Updates debug console
        /// </summary>
        public void Update()
        {
            // Updating timer value
            var dt = Raylib.GetFrameTime();

            _timer -= dt;
            if (_timer < 0)
            {
                // Updating timer-dependent values
                _isTimerTick = !_isTimerTick;

                _timer = 0.75f; // Restoring 1-second timer
            }

            // Checking if "tilda" key was pressed
            if (Raylib.IsKeyPressed(KeyboardKey.F1))
            {
                Shown = !Shown;

                _isInputActive = true;
                _isInputHover = false;
                _historyIndex = 0;

                GuiHelpers.SetGuiMode(Shown);
            }

            if (!Shown)
            {
                return;
            }

            // Processing input field states
            var width = Raylib.GetScreenWidth();
            var height = Raylib.GetScreenHeight();
            var mousePosition = Raylib.GetMousePosition();

            _isInputHover = Raylib.CheckCollisionPointRec(mousePosition,
                new Rectangle(8, height / 4.0f - 32 - 16, width - 16, 32));

            if (_isInputActive)
            {
                if (!_isInputHover && Raylib.IsMouseButtonPressed(MouseButton.Left))
                {
                    _isInputActive = false;

                    return;
                }

                // Processing history up
                if (Raylib.IsKeyPressed(KeyboardKey.Up))
                {
                    if (!_history.Any())
                    {
                        return;
                    }

                    _historyIndex++;
                    if (_historyIndex > _history.Count)
                    {
                        _historyIndex = _history.Count;
                    }

                    _inputText = _history[^_historyIndex];

                    return;
                }

                // Processing history down
                if (Raylib.IsKeyPressed(KeyboardKey.Down))
                {
                    if (!_history.Any())
                    {
                        return;
                    }

                    _historyIndex--;
                    if (_historyIndex < 1)
                    {
                        _historyIndex = 1;
                    }

                    _inputText = _history[^_historyIndex];

                    return;
                }

                // Processing input text remove
                if (Raylib.IsKeyPressed(KeyboardKey.Backspace))
                {
                    if (_inputText.Length > 0)
                    {
                        _inputText = _inputText.Remove(_inputText.Length - 1);

                        return;
                    }

                    return;
                }

                // Processing input text command
                if (Raylib.IsKeyPressed(KeyboardKey.Enter))
                {
                    if (!string.IsNullOrWhiteSpace(_inputText))
                    {
                        ProcessCommand(_inputText.Trim());
                    }
                    
                    // Clearing input
                    _inputText = string.Empty;

                    return;
                }

                // Processing input text enter
                var symbol = (char)Raylib.GetCharPressed();
                if (char.IsAsciiLetterOrDigit(symbol) || char.IsWhiteSpace(symbol) || symbol == ':' || symbol == '.')
                {
                    _inputText += symbol;
                }
            }
            else
            {
                _isInputActive = _isInputHover && Raylib.IsMouseButtonPressed(MouseButton.Left);
            }
        }

        /// <summary>
        /// Renders debug console
        /// </summary>
        public void Render()
        {
            var width = Raylib.GetScreenWidth();
            var height = Raylib.GetScreenHeight();

            Raylib.DrawRectangle(0, 0, width, 270, Background);

            var inputBackColor = _isInputActive 
                ? InputActive 
                : _isInputHover 
                    ? InputHover 
                    : Input;

            Raylib.DrawRectangle(8, height / 4 - 32 - 16, width - 16, 32, inputBackColor);

            // Trimming text start if it is too long
            var inputText = _inputText;
            var inputTextWidth = Raylib.MeasureText(inputText, 20);

            while (inputTextWidth > width - 32)
            {
                inputText = $"...{inputText[4..]}";

                inputTextWidth = Raylib.MeasureText(inputText, 20);
            }

            // Adding input caret symbol
            if (_isTimerTick)
            {
                inputText = $"{inputText}|";
            }

            Raylib.DrawText(inputText, 16, height / 4 - 32 - 10, 20, Color.White);

            var logText = _output.TakeLast(8).ToList();
            for (var i = 0; i < logText.Count; i++)
            {
                Raylib.DrawText(logText[i], 16, 16 + i * 24, 20, Color.White);
            }
        }

        /// <summary>
        /// Processes debug console command
        /// </summary>
        /// <param name="command">Command text</param>
        private void ProcessCommand(string command)
        {
            // Parsing and processing command
            var commandArgs = command.Split(' ');
            switch (commandArgs[0].ToLower())
            {
                case "help":
                    Raylib.TraceLog(TraceLogLevel.Info, "[Console]: Available commands: ");
                    Raylib.TraceLog(TraceLogLevel.Info, "[Console]: load <scene> - loads scene by its name");
                    Raylib.TraceLog(TraceLogLevel.Info, "[Console]: set <setting> <value> - sets setting value");

                    break;

                case "load":
                    if (commandArgs.Length < 2)
                    {
                        Raylib.TraceLog(TraceLogLevel.Warning, "[Console]: Wrong command syntax! Use \"load <scene>\"");

                        break;
                    }

                    Scene.Load(commandArgs[1]);

                    break;

                case "set":
                    if (commandArgs.Length < 3)
                    {
                        Raylib.TraceLog(TraceLogLevel.Warning, "[Console]: Wrong command syntax! Use \"set <setting> <value>\"");

                        break;
                    }

                    Configuration.Set(commandArgs[1], commandArgs[2]);
                    Configuration.Update(commandArgs[1]);

                    Raylib.TraceLog(TraceLogLevel.Info, $"[Console]: {commandArgs[1]} = {commandArgs[2]}");

                    break;

                default:
                    Raylib.TraceLog(TraceLogLevel.Warning, $"[Console]: Command not found \"{command}\"!");

                    break;
            }

            // Saving command to history
            _history.Remove(command);
            _history.Add(command);

            _historyIndex = 0;
        }

        /// <summary>
        /// Message log callback
        /// </summary>
        private void LogMessage(TraceLogLevel logLevel, string? message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                return;
            }

            var logLevelPrefix = logLevel switch
            {
                TraceLogLevel.Trace => "[Trace] ",
                TraceLogLevel.Debug => "[Debug] ",
                TraceLogLevel.Warning => "[Warning] ",
                TraceLogLevel.Error => "[Error] ",
                TraceLogLevel.Fatal => "[Fatal] ",
                _ => string.Empty
            };

            DumpLines([ $"[{DateTime.Now:G}] {logLevelPrefix}{message}" ]);
        }

        /// <summary>
        /// Dumps log lines to output and file
        /// </summary>
        /// <param name="lines">Log lines</param>
        private void DumpLines(string[] lines)
        {
            // Saving logs in memory for debug console
            _output.AddRange(lines);

            // Creating directory and writing logs to file
            Directory.CreateDirectory("Logs");
            File.AppendAllLines($"Logs/log-{DateTime.Now:dd.MM.yyyy}.log", lines);
        }
    }
}
