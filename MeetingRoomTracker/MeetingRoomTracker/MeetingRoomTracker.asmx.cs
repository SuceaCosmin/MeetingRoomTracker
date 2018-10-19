using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace MeetingRoomTracker
{
    /// <summary>
    /// Summary description for MeetingRoomTracker
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class MeetingRoomTracker : System.Web.Services.WebService
    {

        public static readonly string Na = "NA";
        private readonly MeetingRoomTrackerEntities _manager = new MeetingRoomTrackerEntities();

        [WebMethod]
        public string TestConnection()
        {
            return "Connection is working.. Hello World from Meeting room tracker!";
        }

        [WebMethod]
        public bool AddData(string teamId, string cardId, string numberOfParticipants, string temperature)
        {

            var currentTeam = _manager.Teams.FirstOrDefault(team => team.Id.ToString().Equals(teamId));

            if (currentTeam == null)
            {
                throw new Exception("Provided TeamId(" + teamId + ") does not belong to any team! Please check your assigned id and try again!");
            }




            int? attendnats = null;
            if (numberOfParticipants != null && !numberOfParticipants.ToUpper().Equals(Na))
            {
                try
                {
                    attendnats = int.Parse(numberOfParticipants);
                }
                catch (Exception)
                {
                    throw new FormatException("Provided number(" + numberOfParticipants + ") of attendants value is not a valid integer number! Provided entry will be ignored.");
                }
            }

            double? temp = null;
            if (temperature != null && !temperature.ToUpper().Equals(Na))
            {
                try
                {
                    temp = double.Parse(temperature);
                }
                catch (Exception)
                {
                    throw new FormatException("Provided temperature(" + temperature + ") is not a valid number! Provided entry will be ignored.");
                }
            }

            User currentUser = null;
            if (cardId != null && !cardId.ToUpper().Equals(Na))
            {
                currentUser = _manager.Users.FirstOrDefault(user => user.CardId.Equals(cardId));

                if (currentUser == null)
                {
                    throw new Exception("Provided id (" + cardId + ") is unregistered and does  not have any right to book a meeting room! Provided entry will be ignored.");
                }
            }


            MeetingRoomEntry entry = new MeetingRoomEntry()
            {
                date = DateTime.Now,
                teamId = int.Parse(teamId),
                User = currentUser,



            };

            if (attendnats != null)
            {
                entry.numberOfParticipants = attendnats;
            }

            if (temp != null)
            {
                entry.temperature = (int)temp;
            }



            _manager.MeetingRoomEntries.Add(entry);
            _manager.SaveChanges();

            return true;

        }

        [WebMethod]
        public bool ValidateUser(string cardId)
        {
            try
            {
                return _manager.Users.Any(user => user.CardId.Equals(cardId));
            }
            catch (Exception)
            {
                return false;
            }

        }
    }
}
