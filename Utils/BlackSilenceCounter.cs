
namespace Furioso.Utils
{
    public class BlackSilenceCounter
    {
        public enum Attack
        {
            Ranga,
            OldBoys,
            Zelkova,
            Logic,
            Durandal,
            Mook,
            Allas,
            Crystal,
            Wheel,
        }

        private bool[] _attacks = new bool[9];

        public BlackSilenceCounter()
        {
            for (int i = 0; i < 9; i++)
            {
                _attacks[i] = false;
            }
        }

        public void AddCounter(Attack info)
        {
            _attacks[(int)info] = true;
        }

        public bool IsUsed(Attack info)
        {
            return _attacks[(int)info];
        }

        public void AddCounter(int info)
        {
            _attacks[info] = true;
        }

        public bool IsUsed(int info)
        {
            return _attacks[info];
        }

        public int GetCounter()
        {
            int num = 0;
            for (int i = 0; i < 9; i++)
            {
                if (_attacks[i])
                {
                    num++;
                }
            }
            return num;
        }

        public void Clear()
        {
            for (int i = 0; i < 9; i++)
            {
                _attacks[i] = false;
            }
        }
    }
}
