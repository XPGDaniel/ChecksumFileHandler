using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HashListComparer.Class
{
    public class ListComparer : IEqualityComparer<FileStruct>
    {
        #region IEqualityComparer<YourClass> Members

        public bool Equals(FileStruct x, FileStruct y)
        {
            return x.hash.Equals(y.hash);// && x.Name.Equals(y.Name);
        }

        public int GetHashCode(FileStruct obj)
        {
            int hCode = obj.hash.GetHashCode() ^ obj.hash.GetHashCode();
            return hCode.GetHashCode();
        }

        #endregion
    }


}
