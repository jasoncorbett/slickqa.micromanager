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
using System.Drawing;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Security;
using System.Threading;
using System.Windows.Forms;

namespace MicroManager
{
	public sealed class NotificationIcon
	{
		private NotifyIcon notifyIcon;
		private ContextMenu notificationMenu;
		
		#region Initialize icon and menu
		public NotificationIcon()
		{
			notifyIcon = new NotifyIcon();
			notificationMenu = new ContextMenu(InitializeMenu());
			
			notifyIcon.DoubleClick += IconDoubleClick;
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NotificationIcon));
			notifyIcon.Icon = (Icon)resources.GetObject("$this.Icon");
			notifyIcon.ContextMenu = notificationMenu;
		}
		
		private MenuItem[] InitializeMenu()
		{
			MenuItem[] menu = new MenuItem[] {
				new MenuItem("About", menuAboutClick),
				new MenuItem("Exit", menuExitClick)
			};
			return menu;
		}
		#endregion
		
		#region Main - Program entry point
		/// <summary>Program entry point.</summary>
		/// <param name="args">Command Line Arguments</param>
		//s[STAThread]
		public static void Main(string[] args)
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			
			bool isFirstInstance;
			// Please use a unique name for the mutex to prevent conflicts with other programs
			using (Mutex mtx = new Mutex(true, "MicroManager", out isFirstInstance)) {
				if (isFirstInstance) {
					NotificationIcon notificationIcon = new NotificationIcon();
					notificationIcon.notifyIcon.Visible = true;
					ServiceHost host = new ServiceHost(typeof(MMService), new Uri[] {new Uri("http://0.0.0.0:4321/api")});
					ServiceDebugBehavior debugbehavior = host.Description.Behaviors.Find<ServiceDebugBehavior>();
					if(debugbehavior == null)
					{
						debugbehavior = new ServiceDebugBehavior();
						debugbehavior.IncludeExceptionDetailInFaults = true;
						host.Description.Behaviors.Add(debugbehavior);
					} else
					{
						debugbehavior.IncludeExceptionDetailInFaults = true;
					}
					ServiceMetadataBehavior metabehavior = host.Description.Behaviors.Find<ServiceMetadataBehavior>();
					if(metabehavior == null)
					{
						metabehavior = new ServiceMetadataBehavior();
						metabehavior.HttpGetEnabled = true;
						host.Description.Behaviors.Add(metabehavior);
					} else
					{
						metabehavior.HttpGetEnabled = true;
					}
					ServiceCredentials creds = host.Description.Behaviors.Find<ServiceCredentials>();
					if(creds == null)
					{
						creds = new ServiceCredentials();
						creds.UserNameAuthentication.UserNamePasswordValidationMode = UserNamePasswordValidationMode.Custom;
						creds.UserNameAuthentication.CustomUserNamePasswordValidator = new CustomUsernamePasswordValidator();
						host.Description.Behaviors.Add(creds);
					} else
					{
						creds.UserNameAuthentication.UserNamePasswordValidationMode = UserNamePasswordValidationMode.Custom;
						creds.UserNameAuthentication.CustomUserNamePasswordValidator = new CustomUsernamePasswordValidator();
					}
					WebHttpEndpoint mmservendpt = new WebHttpEndpoint(ContractDescription.GetContract(typeof(MMService)), new EndpointAddress("http://0.0.0.0:4321/api"));
					mmservendpt.Security.Mode = WebHttpSecurityMode.TransportCredentialOnly;
					mmservendpt.Security.Transport.ClientCredentialType = HttpClientCredentialType.Basic;
					
					host.AddServiceEndpoint(mmservendpt);
					host.Open();
					Application.Run();
					host.Close();
					notificationIcon.notifyIcon.Dispose();
				} else {
					// The application is already running
					// TODO: Display message box or change focus to existing application instance
				}
			} // releases the Mutex
		}
		#endregion
		
		#region Event Handlers
		private void menuAboutClick(object sender, EventArgs e)
		{
			MessageBox.Show("Micro Manager remote management service 1.0 (by Jason Corbett)");
		}
		
		private void menuExitClick(object sender, EventArgs e)
		{
			Application.Exit();
		}
		
		private void IconDoubleClick(object sender, EventArgs e)
		{
			MessageBox.Show("Micro Manager remote management service 1.0 (by Jason Corbett)");
		}
		#endregion
	}
}
