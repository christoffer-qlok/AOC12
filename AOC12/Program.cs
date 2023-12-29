namespace AOC12
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var lines = File.ReadAllLines("input.txt");
            var rows = new List<SpringRow>();
            foreach (var line in lines)
            {
                var parts = line.Split(' ');
                var config = parts[1].Split(',').Select(int.Parse).ToArray();
                rows.Add(new SpringRow() { Springs = parts[0], Configuration = config });
            }

            int sum = 0;
            foreach (var row in rows)
            {
                var valid = new HashSet<string>();
                GetValidConfigurations(row.Springs, row.Configuration, "", valid);
                sum += valid.Count();
            }
            Console.WriteLine(sum);
        }

        static void GetValidConfigurations(string springs, int[] config, string cur, HashSet<string> valid)
        {
            if(config.Count() == 0)
            {
                if(!springs.Contains('#'))
                {
                    valid.Add(cur + new string('.', springs.Length));
                }
                return;
            }

            if(springs.Length == 0)
            {
                if(config.Count() == 0  || (config.Count() == 1 && config[0] == 0))
                {
                    valid.Add(cur);
                }
                return;
            }

            switch (springs[0])
            {
                case '#':
                    if (config[0] == 0)
                        return;
                    config[0] -= 1;
                    GetValidConfigurations(springs.Remove(0, 1), config, cur + '#', valid);
                    return;
                case '.':
                    if (config[0] == 0)
                    {
                        GetValidConfigurations(springs.Remove(0, 1), config.Skip(1).ToArray(), cur + '.', valid);
                        return;
                    }
                    if(cur.Length == 0 || cur[cur.Length - 1] != '#' || config[0] == 0)
                    {
                        GetValidConfigurations(springs.Remove(0, 1), config, cur + '.', valid);
                    }
                    return;
                case '?':
                    GetValidConfigurations('#' + springs.Remove(0, 1), (int[])config.Clone(), cur, valid);
                    GetValidConfigurations('.' + springs.Remove(0, 1), (int[])config.Clone(), cur, valid);
                    return;

            }
        }
    }
}