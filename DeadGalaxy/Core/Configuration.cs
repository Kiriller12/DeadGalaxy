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
using System.IO;
using DeadGalaxy.Core.Helpers;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Raylib_cs;

namespace DeadGalaxy.Core
{
    /// <summary>
    /// Game configuration
    /// </summary>
    internal static class Configuration
    {
        private const string ConfigFilePath = "settings.json";

        private static IConfiguration? _instance;

        /// <summary>
        /// Initializes game configuration
        /// </summary>
        /// <param name="args">Command line arguments</param>
        public static void Init(string[] args)
        {
            if (_instance != null)
            {
                Raylib.TraceLog(TraceLogLevel.Warning, "[Configuration]: Couldn't create game configuration because it is already created!");

                return;
            }

            _instance = new ConfigurationBuilder()
                .AddJsonFile(ConfigFilePath)
                .AddCommandLine(args)
                .Build();
        }

        /// <summary>
        /// Returns configuration value by given path
        /// </summary>
        /// <typeparam name="T">Value type</typeparam>
        /// <param name="path">Configuration path</param>
        /// <returns>Configuration value of given type or default</returns>
        public static T? Get<T>(string path)
        {
            if (_instance == null)
            {
                return default;
            }

            try
            {
                return _instance.GetValue<T>(path);
            }
            catch
            {
                return default;
            }
        }

        /// <summary>
        /// Sets configuration value by given path
        /// </summary>
        /// <param name="path">Configuration path</param>
        /// <param name="value">Configuration value</param>
        public static void Set(string path, string value)
        {
            if (_instance == null)
            {
                return;
            }

            _instance[path] = value;

            // Updating data in file
            UpdateFile(path, value);
        }

        /// <summary>
        /// Applies configuration update for setting by its path
        /// </summary>
        /// <param name="path">Configuration path</param>
        public static void Update(string path)
        {
            switch (path)
            {
                case "graphic:screen:width":
                case "graphic:screen:height":
                    ApplyScreenSize();

                    break;
                case "graphic:fullscreen":
                    ApplyFullscreen();

                    break;
                case "graphic:vsync":
                    ApplyVsync();

                    break;
                case "graphic:fpsLock":
                    ApplyFpsLock();

                    break;
                case "graphic:filtering":
                case "graphic:mipmaps":
                    ApplyFiltering();

                    break;
                case "sound:master":
                    ApplyMasterVolume();

                    break;
                case "sound:music":
                    ApplyMusicVolume();

                    break;
                case "sound:effects":
                    ApplyEffectsVolume();

                    break;
            }
        }

        /// <summary>
        /// Checks is configuration initialized
        /// </summary>
        public static void CheckInitialization()
        {
            if (_instance != null)
            {
                return;
            }

            throw new EngineException("Couldn't load game configuration!");
        }

        /// <summary>
        /// Applies screen size config
        /// </summary>
        public static void ApplyScreenSize()
        {
            if (_instance == null)
            {
                return;
            }

            // Retrieving screen width and height from config
            var width = _instance.GetValue<int>("graphic:screen:width");
            var height = _instance.GetValue<int>("graphic:screen:height");

            // Retrieving screen width and height of current monitor
            var monitor = Raylib.GetCurrentMonitor();
            var monitorWidth = Raylib.GetMonitorWidth(monitor);
            var monitorHeight = Raylib.GetMonitorHeight(monitor);

            // If config values not defined
            if (width < 1 || height < 1)
            {
                // Calculating default values based on monitor screen size
                width = (int)(monitorWidth * 0.8f);
                height = (int)(monitorHeight * 0.8f);
            }

            // Calculating window offset
            var xOffset = (monitorWidth - width) / 2;
            var yOffset = (monitorHeight - height) / 2;

            // Applying settings
            Raylib.SetWindowSize(width, height);
            Raylib.SetWindowPosition(xOffset, yOffset);
        }

        /// <summary>
        /// Applies fullscreen config
        /// </summary>
        public static void ApplyFullscreen()
        {
            if (_instance == null)
            {
                return;
            }

            // If current fullscreen state needs to be changed
            var fullscreen = _instance.GetValue<bool>("graphic:fullscreen");
            if (Raylib.IsWindowFullscreen() == fullscreen)
            {
                return;
            }

            Raylib.ToggleFullscreen();
        }

        /// <summary>
        /// Applies vsync config
        /// </summary>
        public static void ApplyVsync()
        {
            if (_instance == null)
            {
                return;
            }

            if (_instance.GetValue<bool>("graphic:vsync"))
            {
                Raylib.SetWindowState(ConfigFlags.VSyncHint);
            }
            else
            {
                Raylib.ClearWindowState(ConfigFlags.VSyncHint);
            }
        }

        /// <summary>
        /// Applies FPS lock config
        /// </summary>
        public static void ApplyFpsLock()
        {
            if (_instance == null)
            {
                return;
            }

            // Setting FPS lock
            var fpsLock = _instance.GetValue<int>("graphic:fpsLock");
            if (fpsLock >= 30)
            {
                Raylib.SetTargetFPS(fpsLock);

                return;
            }

            // Reset FPS lock
            Raylib.SetTargetFPS(0);
        }

        /// <summary>
        /// Applies texture filtering config
        /// </summary>
        public static void ApplyFiltering()
        {
            if (_instance == null || Scene.Main == null)
            {
                return;
            }

            // Setting scene textures filtering and mipmaps
            var mipmaps = _instance.GetValue<bool>("graphic:mipmaps");
            var filter = _instance.GetValue<TextureFilter>("graphic:filtering");

            foreach (var (_, texture) in Scene.Main.TextureResources)
            {
                texture.SetMipmaps(mipmaps);
                texture.SetFiltering(filter);
            }
        }

        /// <summary>
        /// Applies master volume
        /// </summary>
        public static void ApplyMasterVolume()
        {
            if (_instance == null)
            {
                return;
            }

            var masterVolume = _instance.GetValue<float>("sound:master");
            Raylib.SetMasterVolume(masterVolume);
        }

        /// <summary>
        /// Applies music volume
        /// </summary>
        public static void ApplyMusicVolume()
        {
            if (_instance == null)
            {
                return;
            }

            //var musicVolume = _instance.GetValue<float>("sound:music");
            //Raylib.SetMusicVolume(music, musicVolume);
            // TODO implement music system and apply setting
        }

        /// <summary>
        /// Applies effects volume
        /// </summary>
        public static void ApplyEffectsVolume()
        {
            if (_instance == null)
            {
                return;
            }

            //var effectsVolume = _instance.GetValue<float>("sound:effects");
            //Raylib.SetSoundVolume(sound, effectsVolume);
            // TODO implement sound system and apply setting
        }

        /// <summary>
        /// Updating configuration value in file
        /// </summary>
        /// <param name="path">Configuration path</param>
        /// <param name="value">Configuration value</param>
        private static void UpdateFile(string path, string value)
        {
            if (_instance == null)
            {
                return;
            }

            lock (_instance)
            {
                // Loading config file data
                using var configFile = File.OpenText(ConfigFilePath);
                var serializer = new JsonSerializer
                {
                    Formatting = Formatting.Indented
                };

                if (serializer.Deserialize(configFile, typeof(object)) is not JObject configData)
                {
                    return;
                }

                configFile.Close();

                // Trying to get config value
                var configPath = path.Replace(':', '.');
                var configToken = configData.SelectToken(configPath);
                if (configToken == null)
                {
                    return;
                }

                // Updating config value
                var newTokenValue = configToken.Type switch
                {
                    JTokenType.Integer => value.Contains('.')
                        ? JToken.FromObject(value.Convert<float>())
                        : JToken.FromObject(value.Convert<int>()),

                    JTokenType.Float => JToken.FromObject(value.Convert<float>()),
                    JTokenType.Boolean => JToken.FromObject(value.Convert<bool>()),
                    JTokenType.Date => JToken.FromObject(value.Convert<DateTime>()),
                    JTokenType.TimeSpan => JToken.FromObject(value.Convert<TimeSpan>()),

                    _ => value == "null"
                        ? JValue.CreateNull()
                        : JValue.CreateString(value)
                };

                configToken.Replace(newTokenValue);

                // Writing updated data to file
                using var configFileToSave = File.CreateText(ConfigFilePath);
                serializer.Serialize(configFileToSave, configData);
            }
        }
    }
}
