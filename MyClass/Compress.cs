using System;
using System.IO;
using System.IO.Compression;

namespace Tally.MyClass {
    public class Compress {
        /// <summary>
        /// 壓縮字串
        /// </summary>
        /// <param name="unCompressedString"></param>
        /// <returns></returns>
        public byte[] CompressString (string unCompressedString) {
            byte[] bytData = System.Text.Encoding.UTF8.GetBytes (unCompressedString);
            return CompressBytes (bytData);
        }

        private byte[] CompressBytes (byte[] data) {
            MemoryStream output = new MemoryStream ();
            GZipStream gzip = new GZipStream (output, CompressionMode.Compress, true);
            gzip.Write (data, 0, data.Length);
            gzip.Close ();

            return output.ToArray ();
        }

        /// <summary>
        /// 解壓縮字串
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public string DecompressString (byte[] data) {
            if (data != null) {
                return System.Text.Encoding.UTF8.GetString (DecompressBytes (data));
            } else {
                return "";
            }
        }

        private byte[] DecompressBytes (byte[] data) {
            MemoryStream Input = new MemoryStream ();
            Input.Write (data, 0, data.Length);
            Input.Position = 0;
            GZipStream gzip = new GZipStream (Input, CompressionMode.Decompress, true);
            MemoryStream output = new MemoryStream ();
            byte[] buff = new byte[4097];
            int read = -1;

            read = gzip.Read (buff, 0, buff.Length);

            while ((read > 0)) {
                output.Write (buff, 0, read);

                read = gzip.Read (buff, 0, buff.Length);
            }

            gzip.Close ();

            return output.ToArray ();
        }

        // 64進位編碼
        public string CodeBase64 (byte[] data) {
            string codedata = null;

            try {
                codedata = Convert.ToBase64String (data);
            } catch { }

            return codedata;
        }

        // 64進位解碼
        public byte[] DecodeBase64 (string data) {
            byte[] decodedata = null;

            try {
                decodedata = Convert.FromBase64String (data);
            } catch { }

            return decodedata;
        }
    }
}