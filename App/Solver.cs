using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App
{
    public class Solver : ISolver
    {
        public List<string> GenerateOutput(List<EndPoint> endpoints, List<Request> requests, List<int> videoSize, int cachesCapacity, int numberOfCacheServers)
        {
            var cacheOutputs = new List<CacheOutput>();

            var usedCacheIds = new HashSet<int>();

            for (int i = 0; i < numberOfCacheServers; i++)
            {
                cacheOutputs.Add(new CacheOutput { Id = i, Capacity = cachesCapacity, videos = new HashSet<int>() });
            }

            var orderedRequest = requests.OrderBy(r => r.NumberOfRequest);

            foreach (var request in orderedRequest)
            {
                var concernedEndpoint = endpoints[request.endPoint];

                var alreadyUsedCacheForThisEndPointId = concernedEndpoint.caches.Where(c => usedCacheIds.Contains(c.number)).Select(s => s.number);
                if(cacheOutputs.Where(c=> alreadyUsedCacheForThisEndPointId.Contains(c.Id)).SelectMany(s=>s.videos).Contains(request.videoNumber))
                {
                    continue;
                }
                
                var caches = concernedEndpoint.caches.Where(c => c.latency < concernedEndpoint.latency);
                if (caches.Any())
                {
                    var sizeOfRequestedVideo = videoSize[request.videoNumber];
                    var cacheIds = caches.OrderBy(c => c.latency).Select(c => c.number).ToList();
                    var cachesWithEnoughtSpace = cacheOutputs.FirstOrDefault(c => c.Capacity >= sizeOfRequestedVideo && cacheIds.Contains(c.Id));

                    if (cachesWithEnoughtSpace != null)
                    {
                        cachesWithEnoughtSpace.videos.Add(request.videoNumber);
                        cachesWithEnoughtSpace.Capacity -= sizeOfRequestedVideo;
                        usedCacheIds.Add(cachesWithEnoughtSpace.Id);
                    }
                }
            }

            var output = new List<string>();
            output.Add(usedCacheIds.Count.ToString());
            var orderedCacheUsedId = usedCacheIds.OrderBy(c => c).ToList();
            foreach (int cacheUsedNumber in orderedCacheUsedId)
            {
                CacheOutput cache = cacheOutputs.Single(c => c.Id == cacheUsedNumber);
                string listOfCachedVideo = string.Join(" ", cache.videos);
                output.Add(string.Format("{0} {1}", cacheUsedNumber, listOfCachedVideo));
            }
            return output;
        }
    }

    public class CacheOutput
    {
        public int Id;
        public int Capacity;
        public HashSet<int> videos;
    }
}
