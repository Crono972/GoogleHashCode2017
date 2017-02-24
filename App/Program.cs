using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App
{
    class Program
    {
        static void Main(string[] args)
        {
            var inputfolder = @"..\..\..\Input";
            var inputFile = "kittens.in";
            var path = Path.GetFullPath(string.Format(@"{0}\{1}", inputfolder, inputFile));
            var lines = File.ReadAllLines(path);

            var firstLine = lines.First();
            var data = firstLine.Split(' ').Select(int.Parse).ToArray();
            //var numberOfVideos = data[0];
            var numberOfEndPoint = data[1];
            //var numberOfRequest = data[2];
            var numberOfCacheServers = data[3];
            var capacityOfEachCache = data[4];

            var secondLine = lines.Skip(1).First();
            var sizeOfVideos = secondLine.Split(' ').Select(int.Parse).ToList();

            var restOfData = lines.Skip(2);
            var endPoints = new List<EndPoint>();

            var endpointNumber = 0;
            while (endpointNumber < numberOfEndPoint)
            {
                var endPointData = restOfData.First().Split(' ');
                var endpoint = new EndPoint { number = endpointNumber, latency = int.Parse(endPointData[0]), caches = new List<Cache>() };
                var numberofCacheForThisEndPoint = int.Parse(endPointData[1]);
                var lineToSkip = 1 + numberofCacheForThisEndPoint;
                var cacheData = restOfData.Skip(1).Take(numberofCacheForThisEndPoint);
                foreach (var rawCache in cacheData)
                {
                    var tmpCache = rawCache.Split(' ').Select(int.Parse).ToArray();
                    endpoint.caches.Add(new Cache { number = tmpCache[0], latency = tmpCache[1], capacity = capacityOfEachCache });

                }

                endpointNumber++;
                restOfData = restOfData.Skip(lineToSkip);
                endPoints.Add(endpoint);
            }

            var requests = new List<Request>();
            foreach (var line in restOfData)
            {
                var dataLine = line.Split(' ').Select(int.Parse).ToArray();
                requests.Add(new Request { videoNumber = dataLine[0], endPoint = dataLine[1], NumberOfRequest = dataLine[2] });
            }

            ISolver solver = new Solver();

            var output = solver.GenerateOutput(endPoints, requests, sizeOfVideos, capacityOfEachCache, numberOfCacheServers);

            var outputFolder = Path.GetFullPath(@"..\..\..\output");
            if(!Directory.Exists(outputFolder))
            {
                Directory.CreateDirectory(outputFolder);
            }
            var fileName = Path.GetFileNameWithoutExtension(path);

            File.WriteAllLines(string.Format(@"{0}\{1}{2}", outputFolder, fileName, ".out"), output);
            
        }
    }
}
