// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("YJVBdXuvWURY2POMhdCYlPZ2uOb/SPBx47fV2xNcE38rRpf5LqPbisEfetzt9hJ4Qq/xE9fxXY/z5BMXCZY2KTjAV3ogh8e1fidmwcrnirpreAYvpTCI53Tvon8mqFRXUBY++JVcklPHCB2IANr6sdF/rLk/ktT+jOi9Uh21t5bxsstaUSH/mvJN6bywTeoP8xtpB0pObnm5E3+RdNFHHhFAUvxR3vfR7HnklQ9a/wF0vHuXdfb498d19v31dfb292TRweN1kmTluxXm60LSDazdBeTJ1f4fC5phxsd19tXH+vH+3XG/cQD69vb28vf01BF8+ReQtQNBfSaeGBCaI5y22fQgq9y7dK/L7AnGBNtjix3lAss8uGVqk2i5Hn5PbvX09vf2");
        private static int[] order = new int[] { 4,3,6,10,12,13,11,13,8,10,13,12,12,13,14 };
        private static int key = 247;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
