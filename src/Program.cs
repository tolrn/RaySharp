﻿using System.IO;
using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;
using Raytracer.Scenes;

namespace Raytracer
{
    public class Program
    {
        static void Main(string[] args)
        {
            Raytracer raytracer = new(imageWidth: 1200,
                                      aspectRatio: 1,
                                      samples: 1,
                                      maxDepth: 100,
                                      shouldDenoise: true,
                                      denoiserPath: Path.GetFullPath(@"..\Denoiser\"),
                                      outputName: "output.png",
                                      outputFolder: Path.GetFullPath(@"..\Renders"),
                                      printProgress: true);

            raytracer.LoadScene(Scene.LightScene);

            var nativeWindowSettings = new NativeWindowSettings()
            {
                Size = new Vector2i(raytracer.ImageWidth, raytracer.ImageHeight),
                WindowBorder = OpenTK.Windowing.Common.WindowBorder.Fixed,
            };

            using (var window = new Window(GameWindowSettings.Default, nativeWindowSettings, raytracer))
            {
                window.Run();
            }
        }
    }
}
