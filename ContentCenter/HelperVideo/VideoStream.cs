﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace ContentCenter.HelperVideo
{
    public class VideoStream
    {
        private readonly string _filename;
        public VideoStream(string filename)
        {
            _filename = filename;
        }
        public void WriteToStream(Stream outputStream, HttpContent content, TransportContext context)
        {            
            try
            {
                var buffer = new byte[65536];

                using (var video = File.Open(_filename, FileMode.Open, FileAccess.Read))
                {
                    var length = (int)video.Length;
                    var bytesRead = 1;

                    while (length > 0 && bytesRead > 0)
                    {
                        bytesRead = video.Read(buffer, 0, Math.Min(length, buffer.Length));                                                
                        outputStream.Write(buffer, 0, bytesRead);

                        length -= bytesRead;
                    }
                }
            }
            catch (Exception ex)
            {
                
            }
            finally
            {
                outputStream.Close();
                outputStream.Dispose();
            }
        }
    }
}
