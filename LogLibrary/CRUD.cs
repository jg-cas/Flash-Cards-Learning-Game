using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogLibrary
{
    interface ICRUD
    {
        ICollection<Demographic> ReadDemographics();
        ICollection<Session_Log> ReadSession();
        decimal GetID(string name);
        decimal MaxID();
        void AddPlayer(Demographic obj);
        void AddSession(Session_Log obj);
        void DeletePlayer(Demographic obj);
        decimal LastSession();
    }
}
