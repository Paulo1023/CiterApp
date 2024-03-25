using System;
using System.Collections.Generic;
using System.Text;

namespace CITER.Interfaces
{
    public interface IFilesService
    {
        string StorageDirectory { get; }
        void OpenFile(string path);

    }
}
