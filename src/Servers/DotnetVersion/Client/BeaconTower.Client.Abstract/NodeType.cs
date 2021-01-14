using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeaconTower.Client.Abstract
{
    public enum NodeType
    {
        WebServer = 0,
        Mqconsumer = 1,
        Gateway = 2,
        AuthCentral = 3,
        Unset = 0xff,
    }
}
