using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Common.Common
{
   public interface IStorageService
    {
        public string GetFileUrl(string fileName);

        public Task SaveFileAsync(Stream mediaBinaryStream, String fileName);

        public Task DeleteFileAsync(string fileName);

    }
}
