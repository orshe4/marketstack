using System.ComponentModel;
using System.Reflection;

namespace Marketstack.Entities.Enums
{
    public enum Interval
    {
        [Description("1min")]
        _1min,
        [Description("5min")]
        _5min,
        [Description("10min")]
        _10min,
        [Description("15min")]
        _15min,
        [Description("30min")]
        _30min,
        [Description("1hour")]
        _1hour,
        [Description("3hour")]
        _3hour,
        [Description("6hour")]
        _6hour,
        [Description("12hour")]
        _12hour,
        [Description("24hour")]
        _24hour,
    }

    public static class IntervalExtention
    {
        public static string GetDescription(this Interval interval)
        {
            FieldInfo fi = interval.GetType().GetField(interval.ToString());
            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes.Length > 0)
                return attributes[0].Description;
            else
                return interval.ToString();
        }
    }
}