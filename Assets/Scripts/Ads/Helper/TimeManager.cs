
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager
{
    public static System.DateTime GetCurrentRealTime()
    {
        //Test
        return System.DateTime.Now;
    }

    public static bool IsPassTheDay(System.DateTime oldTime, System.DateTime currentTime)
    {
        System.DateTime replaceOldTime = new System.DateTime(oldTime.Year, oldTime.Month, oldTime.Day);
        System.DateTime replaceCurrentTime = new System.DateTime(currentTime.Year, currentTime.Month, currentTime.Day);

        if (replaceCurrentTime > replaceOldTime)
            return true;

        return false;
    }


    //86400 quy ve
    /// <summary>
    /// Chuyển đổi thời gian hiện tại về thời gian đầu ngày 00:00:00 day/mouth/year 
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    public static System.DateTime ParseTimeStartDay(System.DateTime time)
    {
        return new System.DateTime(time.Year, time.Month, time.Day);
    }

    /// <summary>
    /// Chuyển đổi thời gian hiện tại về thời gian đầu tuần
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    public static System.DateTime ParseTimeStartWeek(System.DateTime time)
    {
        time = ParseTimeStartDay(time);
        int timePass = (int)time.DayOfWeek - 1;
        Debug.Log("time.DayOfWeek " + time.DayOfWeek);
        System.DateTime a = time.AddDays(-timePass);
        return new System.DateTime(a.Year, a.Month, a.Day);
    }

    /// <summary>
    ///Lấy ra thời điểm cuối tháng
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    public static System.DateTime GetLastDayOfMonth(System.DateTime time)
    {

        int daysInMonth = System.DateTime.DaysInMonth(time.Year, time.Month);
        System.DateTime start = new System.DateTime(time.Year, time.Month, 1);
        System.DateTime last = start.AddDays(daysInMonth - 1);
        return last;
    }


    public static long TimeLeftPassTheDay(System.DateTime currentTime)
    {

        System.DateTime replaceCurrentTime = new System.DateTime(currentTime.Year, currentTime.Month, currentTime.Day);
        System.DateTime nextTime = replaceCurrentTime.AddDays(1);



        return CaculateTime(currentTime, nextTime);
    }

    public static long CaculateTime(System.DateTime oldTime, System.DateTime newTime)
    {
        System.TimeSpan diff2 = newTime - oldTime;
        long result = diff2.Days * 24 * 60 * 60 + diff2.Hours * 60 * 60 + diff2.Minutes * 60 + diff2.Seconds;
        if (result >= 0)
            return result;
        else
            return -result;
    }
    public static long CaculateTime2(System.DateTime oldTime, System.DateTime newTime)
    {
        System.TimeSpan diff2 = newTime - oldTime;
        long result = diff2.Days * 24 * 60 * 60 + diff2.Hours * 60 * 60 + diff2.Minutes * 60 + diff2.Seconds;
        return result;
    }
    public static long CaculatePing(System.DateTime oldTime, System.DateTime newTime)
    {
        System.TimeSpan diff2 = newTime - oldTime;

        return diff2.Milliseconds;
    }

    //public UnityEngine.UI.Text remainingTime_Text;
    public static void ShowTime(ref UnityEngine.UI.Text remainingTime_Text, long remainingTime)
    {
        if (remainingTime < 60)
            remainingTime_Text.text = remainingTime.ToString();
        else if (remainingTime >= 60 && remainingTime < 3600)
        {
            long remainingMinuter = remainingTime / 60;
            long remainingSecons = remainingTime - remainingMinuter * 60;
            if (remainingSecons >= 10)
                remainingTime_Text.text = remainingMinuter.ToString() + ":" + remainingSecons.ToString();
            else
                remainingTime_Text.text = remainingMinuter.ToString() + ":" + "0" + remainingSecons.ToString();
        }
        else if (remainingTime >= 3600 && remainingTime < 86400)
        {
            long remainingHours = remainingTime / 3600;
            long remainingMinuter = (remainingTime - remainingHours * 3600) / 60;
            long remainingSecons = remainingTime - remainingHours * 3600 - remainingMinuter * 60;

            if (remainingMinuter == 0)
            {
                if (remainingSecons >= 10)
                    remainingTime_Text.text = remainingHours.ToString() + ":" + remainingSecons.ToString();
                else
                    remainingTime_Text.text = remainingHours.ToString() + ":" + "0" + remainingSecons.ToString();

            }
            else if (remainingMinuter < 10)
            {
                if (remainingSecons >= 10)
                    remainingTime_Text.text = remainingHours.ToString() + ":" + "0" + remainingMinuter.ToString() + ":" + remainingSecons.ToString();
                else
                    remainingTime_Text.text = remainingHours.ToString() + ":" + "0" + remainingMinuter.ToString() + ":" + "0" + remainingSecons.ToString();
            }
            else
            {
                if (remainingSecons >= 10)
                    remainingTime_Text.text = remainingHours.ToString() + ":" + remainingMinuter.ToString() + ":" + remainingSecons.ToString();
                else
                    remainingTime_Text.text = remainingHours.ToString() + ":" + remainingMinuter.ToString() + ":" + "0" + remainingSecons.ToString();
            }
        }

    }

    public static string ShowMinutesTime(long remainingTime)
    {

        string result = "00:00";
        if (remainingTime < 60)
        {
            if (remainingTime >= 10)
                result = "00:" + remainingTime.ToString();
            else
                result = "00:0" + remainingTime.ToString();
        }

        return result;
    }

    public static string ShowTime(long remainingTime)
    {
        string result = "00:00:00";
        if (remainingTime < 60)
        {
            if (remainingTime >= 10)
                result = "00:00:" + remainingTime.ToString();
            else
                result = "00:00:0" + remainingTime.ToString();
        }
        else if (remainingTime >= 60 && remainingTime < 3600)
        {
            long remainingMinuter = remainingTime / 60;
            long remainingSecons = remainingTime - remainingMinuter * 60;

            if (remainingMinuter >= 10)
            {
                if (remainingSecons >= 10)
                    result = "00:" + remainingMinuter.ToString() + ":" + remainingSecons.ToString();
                else
                    result = "00:" + remainingMinuter.ToString() + ":" + "0" + remainingSecons.ToString();
            }
            else
            {
                if (remainingSecons >= 10)
                    result = "00:0" + remainingMinuter.ToString() + ":" + remainingSecons.ToString();
                else
                    result = "00:0" + remainingMinuter.ToString() + ":" + "0" + remainingSecons.ToString();
            }
        }

        else if (remainingTime >= 3600 && remainingTime < 86400)


        {
            long remainingHours = remainingTime / 3600;
            long remainingMinuter = (remainingTime - remainingHours * 3600) / 60;
            long remainingSecons = remainingTime - remainingHours * 3600 - remainingMinuter * 60;

            if (remainingMinuter == 0)
            {
                if (remainingSecons >= 10)
                    result = remainingHours.ToString() + ":00:" + remainingSecons.ToString();
                else
                    result = remainingHours.ToString() + ":00:0" + remainingSecons.ToString();

            }
            else if (remainingMinuter < 10)
            {
                if (remainingSecons >= 10)
                    result = remainingHours.ToString() + ":" + "0" + remainingMinuter.ToString() + ":" + remainingSecons.ToString();
                else
                    result = remainingHours.ToString() + ":" + "0" + remainingMinuter.ToString() + ":" + "0" + remainingSecons.ToString();
            }
            else
            {
                if (remainingSecons >= 10)
                    result = remainingHours.ToString() + ":" + remainingMinuter.ToString() + ":" + remainingSecons.ToString();
                else
                    result = remainingHours.ToString() + ":" + remainingMinuter.ToString() + ":" + "0" + remainingSecons.ToString();
            }
        }

        else if (remainingTime >= 86400)
        {
            long remainingDay = remainingTime / 86400;
            long remainingHours = (remainingTime - remainingDay * 86400) / 3600;

            result = remainingDay.ToString() + " Day " + remainingHours.ToString() + " Hour";
        }

        return result;
    }

    public static string ShowTime2(long remainingTime)
    {
        return ShowTime5(remainingTime);
    }
    public static string ShowTime3(long remainingTime)
    {
        return ShowTime5(remainingTime);
    }

    public static string ShowTime4(long remainingTime)
    {
        string result = "00:00:00";
        if (remainingTime < 60)
        {
            result = remainingTime + "s";
        }
        else if (remainingTime >= 60 && remainingTime < 3600)
        {
            result = (remainingTime / 60) + "m";
        }
        else if (remainingTime >= 3600 && remainingTime < 86400)
        {
            result = (remainingTime / 3600) + "h";
        }    
        else if (remainingTime >= 86400)
        {
            result = (remainingTime / 86400) + "d";
        }

        return result;
    }

    public static string ShowTime5(long remainingTime)
    {
        string result = "00:00:00";
        if (remainingTime < 60)
        {
            if (remainingTime >= 10)
                result = "00:" + remainingTime.ToString();
            else
                result = "00:0" + remainingTime.ToString();
        }
        else if (remainingTime >= 60 && remainingTime < 3600)
        {
            long remainingMinuter = remainingTime / 60;
            long remainingSecons = remainingTime - remainingMinuter * 60;

            if (remainingMinuter >= 10)
            {
                if (remainingSecons >= 10)
                    result = remainingMinuter.ToString() + ":" + remainingSecons.ToString();
                else
                    result = remainingMinuter.ToString() + ":" + "0" + remainingSecons.ToString();
            }
            else
            {
                if (remainingSecons >= 10)
                    result = "0" + remainingMinuter.ToString() + ":" + remainingSecons.ToString();
                else
                    result = "0" + remainingMinuter.ToString() + ":" + "0" + remainingSecons.ToString();
            }
        }

        else if (remainingTime >= 3600 && remainingTime < 86400)
        {
            long remainingHours = remainingTime / 3600;
            long remainingMinuter = (remainingTime - remainingHours * 3600) / 60;
            long remainingSecons = remainingTime - remainingHours * 3600 - remainingMinuter * 60;

            if (remainingMinuter == 0)
            {
                    result = remainingHours.ToString() + "h:00";

            }
            else if (remainingMinuter < 10)
            {
                if (remainingSecons >= 10)
                    result = remainingHours.ToString() + "h:" + "0" + remainingMinuter.ToString();
                else
                    result = remainingHours.ToString() + "h:" + "0" + remainingMinuter.ToString();
            }
            else
            {
                    result = remainingHours.ToString() + "h:" + remainingMinuter.ToString();
            }
        }

        else if (remainingTime >= 86400)
        {
            long remainingDay = remainingTime / 86400;

            result = remainingDay.ToString() + "d";
        }

        return result;
    }

    public static string ShowTime6(long remainingTime)
    {
        string result = "00:00:00";
        if (remainingTime < 60)
        {
            if (remainingTime >= 10)
                result = "00:00:" + remainingTime.ToString();
            else
                result = "00:00:0" + remainingTime.ToString();
        }
        else if (remainingTime >= 60 && remainingTime < 3600)
        {
            long remainingMinuter = remainingTime / 60;
            long remainingSecons = remainingTime - remainingMinuter * 60;

            if (remainingMinuter >= 10)
            {
                if (remainingSecons >= 10)
                    result = "00:" + remainingMinuter.ToString() + ":" + remainingSecons.ToString();
                else
                    result = "00:" + remainingMinuter.ToString() + ":" + "0" + remainingSecons.ToString();
            }
            else
            {
                if (remainingSecons >= 10)
                    result = "00:0" + remainingMinuter.ToString() + ":" + remainingSecons.ToString();
                else
                    result = "00:0" + remainingMinuter.ToString() + ":" + "0" + remainingSecons.ToString();
            }
        }

        else if (remainingTime >= 3600 && remainingTime < 86400)


        {
            long remainingHours = remainingTime / 3600;
            long remainingMinuter = (remainingTime - remainingHours * 3600) / 60;
            long remainingSecons = remainingTime - remainingHours * 3600 - remainingMinuter * 60;

            if (remainingMinuter == 0)
            {
                if (remainingSecons >= 10)
                    result = remainingHours.ToString() + ":00:" + remainingSecons.ToString();
                else
                    result = remainingHours.ToString() + ":00:0" + remainingSecons.ToString();

            }
            else if (remainingMinuter < 10)
            {
                if (remainingSecons >= 10)
                    result = remainingHours.ToString() + ":" + "0" + remainingMinuter.ToString() + ":" + remainingSecons.ToString();
                else
                    result = remainingHours.ToString() + ":" + "0" + remainingMinuter.ToString() + ":" + "0" + remainingSecons.ToString();
            }
            else
            {
                if (remainingSecons >= 10)
                    result = remainingHours.ToString() + ":" + remainingMinuter.ToString() + ":" + remainingSecons.ToString();
                else
                    result = remainingHours.ToString() + ":" + remainingMinuter.ToString() + ":" + "0" + remainingSecons.ToString();
            }
        }

        else if (remainingTime >= 86400)
        {
            long remainingDay = remainingTime / 86400;
            long remainingHours = (remainingTime - remainingDay * 86400) / 3600;
            long remainingMinuter = (remainingTime - remainingDay * 86400 - remainingHours * 3600) / 60;

            result = remainingDay.ToString() + "d " + remainingHours.ToString() + "h " + remainingMinuter.ToString() + "m";
        }

        return result;
    }

    public static void ShowTimeOfText(ref UnityEngine.UI.Text remainingTime_Text, long remainingTime)
    {
        //if (remainingTime < 60)
        //    remainingTime_Text.text = remainingTime.ToString() + Localization.Get("lb_s");
        //else if (remainingTime >= 60 && remainingTime < 3600)
        //{
        //    long remainingMinuter = remainingTime / 60;
        //    long remainingSecons = remainingTime - remainingMinuter * 60;
        //    if (remainingSecons >= 10)
        //        remainingTime_Text.text = remainingMinuter.ToString() + Localization.Get("lb_m") + remainingSecons.ToString() + Localization.Get("lb_s");
        //    else
        //        remainingTime_Text.text = remainingMinuter.ToString() + Localization.Get("lb_m") + "0" + remainingSecons.ToString() + Localization.Get("lb_s");
        //}
        //else if (remainingTime >= 3600 && remainingTime < 86400)
        //{
        //    long remainingHours = remainingTime / 3600;
        //    long remainingMinuter = (remainingTime - remainingHours * 3600) / 60;
        //    long remainingSecons = remainingTime - remainingHours * 3600 - remainingMinuter * 60;

        //    if (remainingMinuter == 0)
        //    {
        //        if (remainingSecons >= 10)
        //            remainingTime_Text.text = remainingHours.ToString() + Localization.Get("lb_h") + remainingSecons.ToString() + Localization.Get("lb_s");
        //        else
        //            remainingTime_Text.text = remainingHours.ToString() + Localization.Get("lb_h") + "0" + remainingSecons.ToString() + Localization.Get("lb_s");

        //    }
        //    else if (remainingMinuter < 10)
        //    {
        //        if (remainingSecons >= 10)
        //            remainingTime_Text.text = remainingHours.ToString() + Localization.Get("lb_h") + "0" + remainingMinuter.ToString() + Localization.Get("lb_m") + remainingSecons.ToString() + Localization.Get("lb_s");
        //        else
        //            remainingTime_Text.text = remainingHours.ToString() + Localization.Get("lb_h") + "0" + remainingMinuter.ToString() + Localization.Get("lb_m") + "0" + remainingSecons.ToString() + Localization.Get("lb_s");
        //    }
        //    else
        //    {
        //        if (remainingSecons >= 10)
        //            remainingTime_Text.text = remainingHours.ToString() + Localization.Get("lb_h") + remainingMinuter.ToString() + Localization.Get("lb_m") + remainingSecons.ToString() + Localization.Get("lb_s");
        //        else
        //            remainingTime_Text.text = remainingHours.ToString() + Localization.Get("lb_h") + remainingMinuter.ToString() + Localization.Get("lb_m") + "0" + remainingSecons.ToString() + Localization.Get("lb_s");
        //    }
        //}

    }


    public static void ShowTimeTextOptimal(ref float remainingTime, ref float timer, UnityEngine.UI.Text textShow)
    {
        remainingTime -= Time.unscaledDeltaTime;
        timer += Time.unscaledDeltaTime;

        if (remainingTime > 86400)
        {
            if (timer >= 3600)//1H đếm 1 lần
            {
                textShow.text = ShowTime((long)remainingTime);
                timer = 0;
            }
        }
        else
        {
            if (timer >= 1)//1s đếm 1 lần
            {
                textShow.text = ShowTime((long)remainingTime);
                timer = 0;
            }
        }
    }


    /// <summary>
    /// return datetime co dang 8h:25m:40s
    /// </summary>
    /// <param name="seconds"></param>
    /// <returns></returns>
    public static string ToDateTimeString(long seconds)
    {
        long num = seconds / 3600;
        long num2 = seconds % 3600 / 60;
        long num3 = seconds % 60;
        if (num > 0)
        {
            return string.Format("{0}h:{1:D2}m:{2:D2}s", num, num2, num3);
        }
        if (num2 > 0)
        {
            return num2.ToString() + "m:" + num3.ToString("00") + "s";
            //eturn num2.ToString("00") + ":" + num3.ToString("00") + "s";
        }
        return num3.ToString() + " s";
        //return num3.ToString("00") + "s";
    }

    public static System.DateTime EndOfDay(System.DateTime param)
    {
        return new System.DateTime(param.Year, param.Month, param.Day).AddDays(1).Subtract(new System.TimeSpan(0, 0, 0, 0, 1));
    }
    public static int GetWeekNumberOfMonth(System.DateTime date)
    {
        date = date.Date;
        System.DateTime firstMonthDay = new System.DateTime(date.Year, date.Month, 1);
        System.DateTime firstMonthMonday = firstMonthDay.AddDays((System.DayOfWeek.Monday + 7 - firstMonthDay.DayOfWeek) % 7);
        if (firstMonthMonday > date)
        {
            firstMonthDay = firstMonthDay.AddMonths(-1);
            firstMonthMonday = firstMonthDay.AddDays((System.DayOfWeek.Monday + 7 - firstMonthDay.DayOfWeek) % 7);
        }
        return (date - firstMonthMonday).Days / 7 + 1;
    }
}
