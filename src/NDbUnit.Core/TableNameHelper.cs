/*
 * NDbUnit2
 * https://github.com/savornicesei/NDbUnit2
 * This source code is released under the Apache 2.0 License; see the accompanying license file.
 *
 */
using System.Text;

namespace NDbUnit.Core
{
    public static class TableNameHelper
    {
        public static string FormatTableName(string declaredTableName, string quotePrefix, string quoteSuffix)
        {
            StringBuilder result = new StringBuilder();

            var tableNameElements = declaredTableName.Split(".".ToCharArray());

            bool firstElement = true;

            foreach (string s in tableNameElements)
            {
                StringBuilder temp = new StringBuilder();
                if (s != string.Empty)
                {
                    if (!s.StartsWith(quotePrefix))
                        temp.Append(quotePrefix);

                    temp.Append(s);

                    if (!s.EndsWith(quoteSuffix))
                        temp.Append(quoteSuffix);

                    if (firstElement)
                        result.Append(temp.ToString());
                    else
                        result.Append("." + temp);

                    firstElement = false;
                }
            }

            return result.ToString();
        }
    }
}
