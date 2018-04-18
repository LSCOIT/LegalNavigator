namespace ContentsExtractionApi.Utilities
{
    /// <summary>
    /// 
    /// </summary>
    public class StateToConnectionStringMapper
    {
        // private const string prefix = "ContentsDb";//"CrowledContentsDb";//
        /// <summary>
        /// Provides connection string name
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="state"></param>
        /// <param name="translateTo"></param>
        /// <returns></returns>
        public static string ToConnectionString(string prefix, string state, string translateTo = null)
        {
            //Todo: adjust the suffix later
            if (!string.IsNullOrEmpty(state))
            {
                if(string.IsNullOrEmpty(translateTo))
                  return string.Format("{0}_{1}", prefix, state.Substring(0, 2).ToUpper());
                else
                {
                    return string.Format("{0}_{1}_{2}", prefix, state.Substring(0, 2).ToUpper(),translateTo.ToUpper());
                }
            }
            return null;
        }
    }
}