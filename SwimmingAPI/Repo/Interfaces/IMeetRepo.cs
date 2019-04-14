using System;
using System.Collections;
using System.Collections.Generic;
using SwimmingAPI.Models;

namespace SwimmingAPI.Repo.Interfaces
{
    public interface IMeetRepo
    {
        bool AddMeet(AddMeetModel model);
        List<Meet> GetAllMeets();
        bool UpdateMeet(Meet model);
        List<Meet> GetMeets(DateTime startDate, DateTime endDate);
        List<Meet> GetMeets(DateTime date);
        List<Meet> GetMeets(string venue);
    }
}