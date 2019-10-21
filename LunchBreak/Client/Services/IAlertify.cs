using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LunchBreak.Client.Services
{
    public interface IAlertify
    {
        Task Success(string message);
        Task Error(string message);
        Task Confirm(string message);
    }
}
