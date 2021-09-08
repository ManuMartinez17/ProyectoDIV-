using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace ProyectoDIV1.Helpers
{
    public static class ArchivoAStream
    {
        public static Stream GetStreamFromUrl(string url)
        {
            byte[] imageData = null;
            MemoryStream ms;

            ms = null;

            try
            {
                using (var wc = new System.Net.WebClient())
                {
                    imageData = wc.DownloadData(url);
                }
                ms = new MemoryStream(imageData);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return ms;
        }
    }
}
