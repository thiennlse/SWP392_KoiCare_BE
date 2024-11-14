using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.ResponseModel
{
    public class FishFoodCalculateResponModel
    {
       public double DailyFood { get; set; } 
       public double WeeklyFood { get; set; }
       public int FeedDay { get; set; }
       public double PerFeedingDay { get; set; }
    }
}
