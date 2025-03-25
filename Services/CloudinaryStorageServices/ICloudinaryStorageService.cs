﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.CloudinaryStorageServices
{
    public interface ICloudinaryStorageService
    {
        Task<string> UploadFileAsync(Stream fileStream, string originalFileName);
    }
}
