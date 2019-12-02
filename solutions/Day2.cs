namespace solutions 
{
    public class Day2
    {
        public int[] ProcessIntCode(int[] input)
        {
            int iteration = 0;

            while(true)
            {
                int opCodeStart = (iteration * 4);
                int actionId = input[opCodeStart];
                if (actionId == 99)
                    break;

                int a = input[opCodeStart + 1];
                int b = input[opCodeStart + 2];
                int store = input[opCodeStart + 3];

                if (actionId == 1)
                {
                    input[store] = input[a] + input[b];    
                }

                if (actionId == 2)
                {
                    input[store] = input[a] * input[b];
                }

                iteration++;
            }

            return input;
        }
    }
}