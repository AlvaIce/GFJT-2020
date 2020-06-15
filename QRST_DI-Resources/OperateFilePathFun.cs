namespace QRST_DI_Resources
{
    public class OperateFilePathFun
    {

         
        /// <summary>
        /// 根据相对路径和绝对路径拼
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetMixedAbsPath(string absolutePath,string relatPath)
        {
            if (relatPath.StartsWith("..\\"))
            {
                int sum = substringCount(relatPath, "..\\");
                relatPath = relatPath.Replace("..\\", "");
                string[] arr = absolutePath.Split(new char[] { '\\' });
                int startIndex = absolutePath.LastIndexOf(arr[arr.Length - sum]);
                absolutePath = absolutePath.Remove(startIndex, absolutePath.Length - startIndex);
                absolutePath = string.Format("{0}{1}", absolutePath, relatPath);
            }
            else if (relatPath.StartsWith(".\\"))
            {

                relatPath = relatPath.Remove(0, 2);
                absolutePath = string.Format("{0}\\{1}", absolutePath, relatPath);

            }
            else absolutePath = relatPath;

            return absolutePath;
        }

        private static int substringCount(string str, string substring)
        {
            if (str.Contains(substring))
            {
                string strReplaced = str.Replace(substring, "");
                return (str.Length - strReplaced.Length) / substring.Length;
            }
            return 0;
        }

    }
}
