using SourceBaseBE.Database.Models.RequestModels;
using System.Text;
using System.Web;

namespace PRPO.Database.Helpers
{
    public static class StringToDictionaryHelper
    {

        public static Dictionary<string, string> ToStringString(string str, bool isDecodeValue = false)
        {
            if (string.IsNullOrEmpty(str))
            {
                return new Dictionary<string, string>();
            }

            string[] pairs = str.Split(',');

            Dictionary<string, string> dictionary = new Dictionary<string, string>();

            foreach (string pair in pairs)
            {
                string[] keyValue = pair.Split(":");

                if (isDecodeValue)
                {
                    //* encode base64
                    byte[] data = Convert.FromBase64String(keyValue[1]);
                    string decodedText = Encoding.UTF8.GetString(data);

                    dictionary.Add(keyValue[0].ToLower(), decodedText);
                }
                else
                {
                    dictionary.Add(keyValue[0].ToLower(), keyValue[1]);
                }

            }

            return dictionary;
        }
        public static SearchModel ToDicOrString(string str, bool isDecodeValue = false)
        {
            var ret = new SearchModel();

            if (string.IsNullOrEmpty(str))
            {
                return ret;
            }
            string[] pairs = str.Split(',');
            string[] keyValue = null;
            if (pairs.Length <= 1)
            {
                keyValue = pairs[0].Split(":");
                if (keyValue.Length <= 1)
                {
                    if (isDecodeValue)
                    {
                        var rightValue = ValidateBase64EncodedString(keyValue[0]);
                        var data = Convert.FromBase64String(rightValue);
                        keyValue[0] = Encoding.UTF8.GetString(data);
                    }
                    ret.SearchStr = keyValue[0];
                    //* encode base64
                    ret.SearchStr = ret.SearchStr.ToLower();
                    return ret;
                }

            }
            Dictionary<string, string> dictionary = new Dictionary<string, string>();

            foreach (string pair in pairs)
            {
                keyValue = pair.Split(":");
                if (isDecodeValue)
                {
                    var rightValue = ValidateBase64EncodedString(keyValue[1]);
                    var data = Convert.FromBase64String(rightValue);
                    keyValue[1] = Encoding.UTF8.GetString(data);
                }
                dictionary.Add(keyValue[0].ToLower(), keyValue[1].ToLower());

            }
            ret.DicSearch = dictionary;
            return ret;
        }
        private static string ValidateBase64EncodedString(string inputText)
        {
            string stringToValidate = inputText;
            stringToValidate = stringToValidate.Replace(" ", "+");
            stringToValidate = stringToValidate.Replace('-', '+'); // 62nd char of encoding
            stringToValidate = stringToValidate.Replace('_', '/'); // 63rd char of encoding
            switch (stringToValidate.Length % 4) // Pad with trailing '='s
            {
                case 0: break; // No pad chars in this case
                case 2: stringToValidate += "=="; break; // Two pad chars
                case 3: stringToValidate += "="; break; // One pad char
                default:
                    throw new System.Exception(
             "Illegal base64url string!");
            }

            return stringToValidate;
        }

        public static Dictionary<string, object> ToStringAndObj(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return new Dictionary<string, object>();
            }

            string[] pairs = str.Split(',');

            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            foreach (string pair in pairs)
            {
                long num;
                string[] keyValue = pair.Split(":");

                if (long.TryParse(keyValue[1], out num))
                {
                    if (!dictionary.ContainsKey(keyValue[0]))
                    {
                        dictionary.Add(keyValue[0].ToLower().Trim(), num);
                    }
                    else
                    {
                        dictionary[keyValue[0].ToLower().Trim()] += $",{num}";
                    }
                }
                else
                {
                    if (!dictionary.ContainsKey(keyValue[0]))
                    {
                        dictionary.Add(keyValue[0].ToLower().Trim(), keyValue[1].ToLower().Trim());
                    }
                    else
                    {
                        dictionary[keyValue[0].ToLower().Trim()] += $",{keyValue[1].ToLower().Trim()}";
                    }

                }
            }

            return dictionary;
        }

        //test
        public static Dictionary<string, long> ToStringLongTest(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return new Dictionary<string, long>();
            }

            string[] pairs = str.Split(',');

            Dictionary<string, long> dictionary = new Dictionary<string, long>();

            foreach (string pair in pairs)
            {
                long num;
                string[] keyValue = pair.Split(":");

                if (long.TryParse(keyValue[1], out num))
                {
                    dictionary.Add(keyValue[0].ToLower(), num);
                }
            }

            return dictionary;
        }
    }
}