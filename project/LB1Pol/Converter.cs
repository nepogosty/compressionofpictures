using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.IO;

using System.Drawing;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;

namespace LB1Pol
{
	public class Converter
	{

		public Converter()
		{
		}

		public byte[] imageToByteArray(System.Drawing.Image imageIn)
		{
			byte[] arr;
			using (MemoryStream ms = new MemoryStream())
			{
				imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
				return arr = ms.ToArray();
			}
		}
		public ImageCodecInfo GetEncoder(ImageFormat format)
		{
			ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
			foreach (ImageCodecInfo codec in codecs)
			{
				if (codec.FormatID == format.Guid)
				{
					return codec;
				}
			}
			return null;
		}
		public  static Bitmap BytesToBitmap(byte[] data)
		{
			Size size = new System.Drawing.Size(512, 512);
			GCHandle handle = GCHandle.Alloc(data, GCHandleType.Pinned);
			Bitmap bmp = new Bitmap(size.Width, size.Height, size.Width * 3, PixelFormat.Format24bppRgb, handle.AddrOfPinnedObject());
			handle.Free();
			return bmp;
		}
		static IntPtr Iptr = IntPtr.Zero;
		static BitmapData bitmapData = null;
		static public byte[] Pixels { get; set; }
		static public int Depth { get; private set; }
		static public int Width { get; private set; }
		static public int Height { get; private set; }
		

		static unsafe public void LockBits(Bitmap source)

		{
            // Get width and height of bitmap
            Width = source.Width;
            Height = source.Height;
   //         Width = 3840;
			//Height = 2400;

			// get total locked pixels count
			int PixelCount = Width * Height;

			// Create rectangle to lock
			Rectangle rect = new Rectangle(0, 0, Width, Height);

			// get source bitmap pixel format size
			Depth = System.Drawing.Bitmap.GetPixelFormatSize(source.PixelFormat);


			// Lock bitmap and return bitmap data
			bitmapData = source.LockBits(rect, ImageLockMode.ReadWrite,
									 source.PixelFormat);
			// create byte array to copy pixel values

			int step = Depth/8 ;
			Pixels = new byte[PixelCount * step];
			Iptr = bitmapData.Scan0;

            // Copy data from pointer to array

            Marshal.Copy(Iptr, Pixels, 0, Pixels.Length);
        }

		public Image byteArrayToImage(byte[] byteArrayIn)
		{
			MemoryStream ms = new MemoryStream(byteArrayIn);
			Image returnImage = Image.FromStream(ms);
			return returnImage;

		}
		public static byte[] Encode(byte[] source)
		{
			List<byte> dest = new List<byte>();
			byte runLength;

			for (int i = 0; i < source.Length; i++)
			{
				runLength = 1;
				while (runLength < byte.MaxValue
				&& i + 1 < source.Length
				&& source[i] == source[i + 1])
				{
					runLength++;
					i++;
				}
				dest.Add(runLength);
				dest.Add(source[i]);
			}
			return dest.ToArray();
		}
		public static byte[] RLEDecode(byte[] source)
		{
			List<byte> dest = new List<byte>();
			byte runLength;

			for (int i = 1; i < source.Length; i += 2)
			{
				runLength = source[i - 1];

				while (runLength > 0)
				{
					dest.Add(source[i]);
					runLength--;
				}
			}
			return dest.ToArray();
		}

	}
	
}
public class ConverterLZW
	{

	public byte[] imageToByteArray(System.Drawing.Image imageIn)
	{
		byte[] arr;
		using (MemoryStream ms = new MemoryStream())
		{
			imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
			return arr = ms.ToArray();
		}
	}
	public Bitmap StringToImage(string inputString)
	{
		byte[] imageBytes = Encoding.Unicode.GetBytes(inputString);
		using (MemoryStream ms = new MemoryStream(imageBytes))
		{
			return new Bitmap(ms);
		}
	}
	public int[] compressLZW(byte[] uncompressed)
	{
		int dictSize = 256;
		Dictionary<string, int> dictionary = new Dictionary<string, int>();
		for (int i = 0; i < dictSize; i++)
			dictionary.Add(i.ToString(), i);

		string w = "";
		List<int> result = new List<int>();

		for (int i = 0; i < uncompressed.Length; i++)
		{
			var c = uncompressed[i];
			var wc = w + c;
			if (dictionary.ContainsKey(wc))
				w = wc;
			else
			{
				result.Add(dictionary[w]);
				dictionary.Add(wc, dictSize++);
				w = "" + c;
			}
		}
		if (w != "")
			result.Add(dictionary[w]);
		return result.ToArray();
	}
	public string DecompressLZW(int[] compressed)
	{
		Dictionary<int, string> dictionary = new Dictionary<int, string>();
		for (int i = 0; i < 256; i++)
			dictionary.Add(i, i.ToString());

		string w = dictionary[compressed[0]];
		StringBuilder decompressed = new StringBuilder(w);

		for (int i = 1; i < compressed.Length; i++)
		{
			string entry = null;
			if (dictionary.ContainsKey(compressed[i]))
				entry = dictionary[compressed[i]];
			else if (compressed[i] == dictionary.Count)
				entry = w + w[0];
			decompressed.Append(entry);
			dictionary.Add(dictionary.Count, w + entry[0]);
			w = entry;
		}
		return decompressed.ToString();
	}

	
}


