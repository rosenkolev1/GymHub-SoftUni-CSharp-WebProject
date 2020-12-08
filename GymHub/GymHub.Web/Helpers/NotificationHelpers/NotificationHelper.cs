using GymHub.Common;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GymHub.Web.Helpers.NotificationHelpers
{
    public static class NotificationHelper
    {
        public static void SetNotification(ITempDataDictionary tempData, NotificationType type, string text)
        {
            tempData[GlobalConstants.NotificationType] = type.ToString();
            tempData[GlobalConstants.NotificationText] = text;
        }
    }
}
