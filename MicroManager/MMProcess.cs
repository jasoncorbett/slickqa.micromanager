/*
 * Created by SharpDevelop.
 * User: jcorbett
 * Date: 11/9/2011
 * Time: 12:20 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Diagnostics;

namespace MicroManager
{
	/// <summary>
	/// Description of MMProcess.
	/// </summary>
	public class MMProcess
	{
		public MMProcess()
		{
		}
		
		public MMProcess(Process p)
		{
			this.PID = p.Id;
			this.Name = p.ProcessName;
			this.HasExited = p.HasExited;
		}
		
		public int PID { get; set; }
		
		public String Name { get; set; }
		
		public bool HasExited { get; set; }
	}}
