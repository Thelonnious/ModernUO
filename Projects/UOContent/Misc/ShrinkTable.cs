using System;
using System.IO;

namespace Server
{
    public static class ShrinkTable
    {
        public const int DefaultItemID = 0x1870; // Yellow virtue stone

        private static int[] m_Table;

        public static int Lookup(Mobile m) => Lookup(m.Body.BodyID, DefaultItemID);

        public static int Lookup(int body) => Lookup(body, DefaultItemID);

        public static int Lookup(Mobile m, int defaultValue) => Lookup(m.Body.BodyID, defaultValue);

        public static int Lookup(int body, int defaultValue)
        {
            if (m_Table == null)
            {
                Load();
            }

            var val = 0;

            if (body >= 0 && body < m_Table!.Length)
            {
                val = m_Table[body];
            }

            if (val == 0)
            {
                val = defaultValue;
            }

            return val;
        }

        private static void Load()
        {
            var path = Path.Combine(Core.BaseDirectory, "Data/shrink.cfg");

            if (!File.Exists(path))
            {
                m_Table = Array.Empty<int>();
                return;
            }

            m_Table = new int[1000];

            using var ip = new StreamReader(path);
            string line;

            while ((line = ip.ReadLine()) != null)
            {
                line = line.Trim();

                if (line.Length == 0 || line.StartsWithOrdinal("#"))
                {
                    continue;
                }

                try
                {
                    var split = line.Split('\t');

                    if (split.Length >= 2)
                    {
                        var body = Utility.ToInt32(split[0]);
                        var item = Utility.ToInt32(split[1]);

                        if (body >= 0 && body < m_Table.Length)
                        {
                            m_Table[body] = item;
                        }
                    }
                }
                catch
                {
                    // ignored
                }
            }
        }
    }
}
