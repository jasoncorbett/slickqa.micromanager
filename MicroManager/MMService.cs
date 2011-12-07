/*
 * Created by SharpDevelop.
 * User: jcorbett
 * Date: 11/9/2011
 * Time: 12:24 PM
 * 
 */
using System;
using System.Collections.Generic;
using System.Net;
using System.IO;
using System.ServiceModel;
using System.Diagnostics;
using System.ServiceModel.Web;

namespace MicroManager
{
	/// <summary>
	/// Description of ProcessService.
	/// </summary>
	[ServiceContract]
	public class MMService
	{
		public MMService()
		{
		}
		
		[WebGet(UriTemplate = "processes?name={name}", ResponseFormat = WebMessageFormat.Json)]
		[OperationContract]
		public List<MMProcess> getProcessesByName(String name)
		{
			Process[] processes = Process.GetProcessesByName(name);
			List<MMProcess> retval = new List<MMProcess>();
			foreach(Process process in processes)
			{
				retval.Add(new MMProcess(process));
			}
			return retval;
		}
		
		[WebGet(UriTemplate = "processes/{pid}", ResponseFormat = WebMessageFormat.Json)]
		[OperationContract]
		public MMProcess getProcessById(String pid)
		{
			int ppid = 0;
			try
			{
				ppid = Int32.Parse(pid);
			} catch(Exception e)
			{
				WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.NotFound;
				return null;
			}
			Process p = null;
			try
			{
				p = Process.GetProcessById(ppid);
			} catch(ArgumentException e)
			{
				WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.NotFound;
				return null;
			}
			return new MMProcess(p);
		}
				
		[WebInvoke(UriTemplate = "processes/{pid}?force={force}", ResponseFormat = WebMessageFormat.Json, Method = "DELETE")]
		[OperationContract]
		public MMProcess killProcess(String pid, String force)
		{
			int ppid = 0;
			try
			{
				ppid = Int32.Parse(pid);
			} catch(Exception e)
			{
				WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.NotFound;
				return null;
			}
			bool bforce = false;
			Boolean.TryParse(force, out bforce);
			Process proc = null;
			try
			{
				proc = Process.GetProcessById(ppid);
			} catch(ArgumentException e)
			{
				WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.NotFound;
				return null;
			}
			
			MMProcess retval = new MMProcess(proc);
			
			if(bforce)
			{
				proc.Kill();
				proc.WaitForExit(2000);
			} else
			{
				proc.CloseMainWindow();
				proc.WaitForExit(2000);
			}
			
			retval.HasExited = proc.HasExited;
			
			return retval;
		}

		[WebInvoke(UriTemplate = "processes/{pid}", ResponseFormat = WebMessageFormat.Json, Method = "DELETE")]
		[OperationContract]
		public MMProcess killProcessNicely(String pid)
		{
			return killProcess(pid, "false");
		}
		
		[WebInvoke(UriTemplate = "processes", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, Method = "POST")]
		[OperationContract]
		public MMProcess startNewProcess(ProcessStartInfo info)
		{
			Process proc = Process.Start(info);
			return new MMProcess(proc);
		}
		
		[WebInvoke(Method = "DELETE", UriTemplate = "files", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
		[OperationContract]
		public MMPath deletePath(MMPath path)
		{
			path.initialize();
			if(File.Exists(path.Path))
			{
				File.Delete(path.Path);
			} else if(Directory.Exists(path.Path))
			{
				Directory.Delete(path.Path, true);
			} else
			{
				WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.NotFound;
			}
			return path.update();
		}
		
		[WebInvoke(Method="PUT", UriTemplate = "files/content", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
		[OperationContract]
		public MMPathWithContent getFileContents(MMPath path)
		{
			MMPathWithContent retval = new MMPathWithContent(path.Path);
			retval.initialize();
			retval.loadContent();
			return retval;
		}
		
		[WebInvoke(Method = "POST", UriTemplate = "files/content", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
		[OperationContract]
		public MMPathWithContent saveFileContents(MMPathWithContent content)
		{
			content.saveContent();
			MMPathWithContent retval = new MMPathWithContent(content.Path);
			retval.initialize();
			retval.loadContent();
			return retval;			
		}
		
		[WebInvoke(Method = "POST", UriTemplate = "files/mkdir", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
		[OperationContract]
		public MMPath createDirectory(MMPath dir)
		{
			dir.initialize();
			if(!dir.Exists)
				Directory.CreateDirectory(dir.Path);
			dir.initialize();
			return dir;
		}
		
		[WebGet(UriTemplate = "env", ResponseFormat = WebMessageFormat.Json)]
		[OperationContract]
		public System.Collections.IDictionary getEnvironment()
		{
			return System.Environment.GetEnvironmentVariables();
		}

		[WebGet(UriTemplate = "disks", ResponseFormat = WebMessageFormat.Json)]
		[OperationContract]
		public List<MMDriveInfo> getDrives()
		{
			List<MMDriveInfo> retval = new List<MMDriveInfo>();
			foreach(DriveInfo info in DriveInfo.GetDrives())
			{
				if(info.IsReady)
				{
					retval.Add(new MMDriveInfo(info));
				}
			}
			return retval;
		}

	}
}
