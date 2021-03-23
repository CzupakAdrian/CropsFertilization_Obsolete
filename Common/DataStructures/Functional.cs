using System;

namespace Common
{
    public class Functional
    {
        public class TimesActor
        {
            public int Count;

            public void Do(Action action)
            {
                for (int i = 0; i < Count; i++)
                    action();
            }
        }
        public static TimesActor Times(int times)
        {
            var instance = new TimesActor();
            instance.Count = times;
            return instance;
        }
    }
}
