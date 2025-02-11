﻿using System;
using System.Linq;
using System.IO;
using GeoCoordinatePortable;

namespace LoggingKata
{
    class Program
    {
        static readonly ILog logger = new TacoLogger();
        const string csvPath = "TacoBell-US-AL.csv";

        static void Main(string[] args)
        {
            logger.LogInfo("Log initialized");

            var lines = File.ReadAllLines(csvPath);

            if (lines.Length == 0)
            {
                logger.LogError("file has no input");
            }
            if (lines.Length == 1)
            {
                logger.LogError("file only has one input");
            }

            var parser = new TacoParser();

            var locations = lines.Select(x => parser.Parse(x)).ToArray();

            ITrackable tacoBell1 = null;
            ITrackable tacoBell2 = null;
            double distance = 0;

            for (int i = 0; i < locations.Length; i++)
            {
                var locA = locations[i];
                GeoCoordinate corA = new GeoCoordinate(locA.Location.Latitude, locA.Location.Longitude);
               

                for (int j = 0; j < locations.Length; j++)
                {
                    var locB = locations[j];
                    GeoCoordinate corB = new GeoCoordinate(locB.Location.Latitude, locB.Location.Longitude);
                   

                    if (corA.GetDistanceTo(corB) > distance)
                    {
                        distance = corA.GetDistanceTo(corB) ;
                        tacoBell1 = locA;
                        tacoBell2 = locB;
                    }
                   
                }
            }

            distance = Math.Round(distance / 1609.344,2);

            logger.LogInfo($"{tacoBell1.Name} and {tacoBell2.Name} are the farthest apart with a distance of {distance} Miles separating them.");

        }
    }
}
