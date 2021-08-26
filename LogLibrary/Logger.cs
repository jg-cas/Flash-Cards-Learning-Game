using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogLibrary
{
    public class Logger : ICRUD
    {
        //Global Variable
        FlashCardsGameEntities Entities;

        //Constructor
        public Logger()
        {
            Entities = new FlashCardsGameEntities();
        }

        public void AddPlayer(Demographic obj)
        {
            Entities.Demographics.Add(obj);
            Entities.SaveChanges();
        }

        public void AddSession(Session_Log obj)
        {
            Entities.Session_Log.Add(obj);
            Entities.SaveChanges();
        }

        public void DeletePlayer(Demographic obj)
        {
            Entities.Demographics.Remove(obj);
            Entities.SaveChanges();
        }

        public decimal LastSession()
        {
            try
            {
                var sessionNumber = Entities.Session_Log.Max(s => s.SessionNumber);
                return sessionNumber;
            }
            catch (Exception)
            {
                return 0;
            }
            
        }
        public decimal MaxID()
        {
            //This Fixes the Creation of the First ID in which the Return would be Null
            try
            {
                decimal id =Entities.Demographics.Max(m => m.ID);
                return id;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public ICollection<Demographic> ReadDemographics()
        {
            return Entities.Demographics.ToList();
        }

        public ICollection<Session_Log> ReadSession()
        {
            return Entities.Session_Log.ToList();
        }

        public decimal GetID(string name)
        {
            var id = Entities.Demographics.FirstOrDefault(n => n.Name == name);
            return id.ID;

        }
        private Session_Log FindSession(decimal sessionNumber)
        {
            //Find Session using Players Name
            return Entities.Session_Log.Find(sessionNumber);
        }
        public decimal CorrectsQuery(decimal sessionNumber)
        {
            var session = FindSession(sessionNumber);
            return session.Correct;
        }

        public decimal SkipsQuery(decimal sessionNumber)
        {
            var session = FindSession(sessionNumber);
            return session.Skips;
        }

        public decimal NumberOfCardsQuery(decimal sessionNumber)
        {
            var session = FindSession(sessionNumber);
            return session.Cards_Used;
        }
    }
}
