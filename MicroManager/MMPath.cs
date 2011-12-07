/*
 * Created by SharpDevelop.
 * User: jcorbett
 * Date: 11/9/2011
 * Time: 11:47 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.IO;

namespace MicroManager
{
	/// <summary>
	/// Description of MMPath.
	/// </summary>
	public class MMPath
	{
		public String Path { get; set; }
		public String Type { get; set; }
		public Boolean Exists { get; set; }
		
		public MMPath()
		{
		}
		
		public MMPath(String path)
		{
			Path = path;
			initialize();
		}
				
		public void initialize()
		{
			if(File.Exists(Path))
			{
				Type = "FILE";
				Exists = true;
			} else if(Directory.Exists(Path))
			{
				Type = "DIRECTORY";
				Exists = true;
			} else
			{
				Type = "?";
				Exists = false;
			}
			
			
			
			
			
		}
		
		public MMPath update()
		{
			if(Type.Equals("FILE"))
				Exists = File.Exists(Path);
			else if(Type.Equals("DIRECTORY"))
				Exists = Directory.Exists(Path);
			return this;
		}
	}
	
	public class MMPathWithContent : MMPath
	{
		public byte[] Content { get; set; }
		
		public MMPathWithContent() :
			   base()
		{
		}
		
		public MMPathWithContent(String path) :
			   base(path)
		{
		}
		
		public void loadContent()
		{
			if(File.Exists(Path))
			{
				Content = File.ReadAllBytes(Path);
			}
		}
		
		public void saveContent()
		{
			File.WriteAllBytes(Path, Content);
		}
	}
}
