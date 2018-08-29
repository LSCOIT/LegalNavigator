using System.Collections.Generic;

namespace Access2Justice.Shared.A2JAuthor
{
    public class LogicalAndComparer : IEqualityComparer<KeyValuePair<string, string>>
    {
        public bool Equals(KeyValuePair<string, string> varDic1, KeyValuePair<string, string> varDic2)
        {
            return (varDic1.Key == varDic2.Key) && (varDic1.Value == varDic2.Value);
        }

        public int GetHashCode(KeyValuePair<string, string> obj)
        {
            return obj.GetHashCode();
        }
    }
}
