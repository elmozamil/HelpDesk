using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace HelpDesk
{
    public class PageHitsCounter : Hub
    {
        static int _hitCount = 0;
        public void RecordHit()
        {
            _hitCount++;
            Clients.All.OnRecordHit(_hitCount);
        }

        public override System.Threading.Tasks.Task OnDisconnected(bool stopCalled)
        {
            _hitCount--;
            Clients.All.OnRecordHit(_hitCount);
            return base.OnDisconnected(stopCalled);
        }
    }
}