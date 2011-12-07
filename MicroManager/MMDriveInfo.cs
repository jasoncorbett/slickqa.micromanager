/*
 * Created by SharpDevelop.
 * User: jcorbett
 * Date: 12/7/2011
 * Time: 11:27 AM
 */
using System;
using System.IO;

namespace MicroManager
{
	/// <summary>
	/// Description of MMDriveInfo.
	/// </summary>
	public class MMDriveInfo
	{
		public String Name { get; set; }
		public String VolumeLabel { get; set; }
		public String RootDirectory { get; set; }
		public String DriveFormat { get; set; }
		public String DriveType { get; set; } 
		public long Size { get; set; }
		public long AvailableFreeSpace { get; set; }
		public long TotalFreeSpace { get; set; }
		
		public MMDriveInfo()
		{			
		}
		
		public MMDriveInfo(DriveInfo info)
		{
			if(info.IsReady)
			{
				Name = info.Name;
				VolumeLabel = info.VolumeLabel;
				RootDirectory = info.RootDirectory.FullName;
				DriveFormat = info.DriveFormat;
				DriveType = info.DriveType.ToString();
				Size = info.TotalSize;
				AvailableFreeSpace = info.AvailableFreeSpace;
				TotalFreeSpace = info.TotalFreeSpace;
			}
		}
	}
}
