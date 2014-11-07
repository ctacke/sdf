using System;

namespace OpenNETCF.Net.NetworkInformation
{
    /*
    typedef struct _MIB_IPSTATS {
      DWORD dwForwarding; 
      DWORD dwDefaultTTL; 
      DWORD dwInReceives; 
      DWORD dwInHdrErrors; 
      DWORD dwInAddrErrors; 
      DWORD dwForwDatagrams; 
      DWORD dwInUnknownProtos; 
      DWORD dwInDiscards; 
      DWORD dwInDelivers; 
      DWORD dwOutRequests; 
      DWORD dwRoutingDiscards; 
      DWORD dwOutDiscards; 
      DWORD dwOutNoRoutes; 
      DWORD dwReasmTimeout; 
      DWORD dwReasmReqds; 
      DWORD dwReasmOks; 
      DWORD dwReasmFails; 
      DWORD dwFragOks; 
      DWORD dwFragFails; 
      DWORD dwFragCreates; 
      DWORD dwNumIf; 
      DWORD dwNumAddr; 
      DWORD dwNumRoutes; 
    } MIB_IPSTATS, *PMIB_IPSTATS;
    */

    internal struct MibIpStats
    {
        public int dwForwarding;
        public int dwDefaultTTL;
        public int dwInReceives;
        public int dwInHdrErrors;
        public int dwInAddrErrors;
        public int dwForwDatagrams;
        public int dwInUnknownProtos;
        public int dwInDiscards;
        public int dwInDelivers;
        public int dwOutRequests;
        public int dwRoutingDiscards;
        public int dwOutDiscards;
        public int dwOutNoRoutes;
        public int dwReasmTimeout;
        public int dwReasmReqds;
        public int dwReasmOks;
        public int dwReasmFails;
        public int dwFragOks;
        public int dwFragFails;
        public int dwFragCreates;
        public int dwNumIf;
        public int dwNumAddr;
        public int dwNumRoutes;
    }
}
