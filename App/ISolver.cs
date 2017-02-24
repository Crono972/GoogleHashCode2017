using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App
{
    public interface ISolver
    {
        List<string> GenerateOutput(List<EndPoint> endpoints, List<Request> requests, List<int> videoSize, int cachesCapacity, int numberOfCacheServers);
    }
}
