using System;
using System.Collections.ObjectModel;
using System.Security.Cryptography;

namespace Orpheus
{
    public static class Extensions
    {
        public static T NextItem<T>(this ObservableCollection<T> collection, T currentItem)
        {
            var currentIndex = collection.IndexOf(currentItem);
            if (currentIndex < collection.Count - 1)
            {
                return collection[currentIndex + 1];
            }
            return collection[0];
        }

        public static ObservableCollection<T> Shuffle<T>(this ObservableCollection<T> input)
        {
            var provider = new RNGCryptoServiceProvider();
            var n = input.Count;
            while (n > 1)
            {
                var box = new byte[1];
                do provider.GetBytes(box);
                while (!(box[0] < n * (Byte.MaxValue / n)));
                var k = (box[0] % n);
                n--;
                var value = input[k];
                input[k] = input[n];
                input[n] = value;
            }
            return input;
        }
    }
}
