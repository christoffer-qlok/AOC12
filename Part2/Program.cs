namespace Part2
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
                var configStr = string.Join(',', Enumerable.Repeat(parts[1], 5));
                var config = configStr.Split(',').Select(int.Parse).ToArray();
                var springs = string.Join('?', Enumerable.Repeat(parts[0], 5));
                rows.Add(new SpringRow() { Springs = springs, Configuration = config });
            }

            long sum = 0;
            var startTime = DateTime.Now;
            foreach (var row in rows)
            {
                sum += GetValidConfigurations(row.Springs, row.Configuration);
            }
            var runTime = DateTime.Now - startTime;

            Console.WriteLine($"Answer: {sum}");
            Console.WriteLine($"With thousands separators: {sum:n0}");
            Console.WriteLine($"Runtime: {runTime}");
        }

        static long GetValidConfigurations(string springs, int[] configs)
        {
            springs = '.' + springs.TrimEnd('.');
            var cSprings = springs.ToCharArray();
            var arrangements = new long[configs.Length + 1][];
            arrangements[0] = new long[cSprings.Length + 1];
            arrangements[0][0] = 1;

            for (int i = 0; i < arrangements[0].Length - 1; i++)
            {
                if (cSprings[i] == '#')
                    break;
                arrangements[0][i + 1] = 1;
            }
            for (int i = 0; i < configs.Length; i++)
            {
                arrangements[i+1] = new long[cSprings.Length + 1];
                long blockSize = 0;

                for (int j = 0; j < cSprings.Length; j++)
                {
                    var c = cSprings[j];

                    if (c == '.')
                        blockSize = 0;
                    else
                        blockSize++;

                    if (c != '#')
                        arrangements[i + 1][j + 1] = arrangements[i+1][j];

                    if (blockSize >= configs[i] && cSprings[j - configs[i]] != '#')
                        arrangements[i+1][j + 1] += arrangements[i][j - configs[i]];
                }
            }

            return arrangements.Last().Last();
        }
    }
}